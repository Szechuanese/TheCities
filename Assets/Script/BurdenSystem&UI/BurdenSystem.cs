using System.Collections.Generic;
using UnityEngine;

public class BurdenSystem : MonoBehaviour
{
    public ValueSystem valueSystem;

    public delegate void BurdenChangedHandler();
    public event BurdenChangedHandler OnBurdenChanged;

    [Header("Runtime Data (Inspector 可见)")]
    public List<BurdenItemRuntime> items = new List<BurdenItemRuntime>();

    private Dictionary<string, BurdenItemRuntime> itemDict = new Dictionary<string, BurdenItemRuntime>();

    //记录每个槽位当前装备的物品 id
    private Dictionary<BurdenSlot, string> equippedBySlot = new Dictionary<BurdenSlot, string>();

    private void Awake()
    {
        RebuildIndex();
        if(valueSystem== null )
        {
            valueSystem=FindAnyObjectByType<ValueSystem>();
        }
        ApplyInitialEquippedModifiers();

    }
    /// <summary>
    /// 开局应用已装备物品的属性修正
    /// </summary>
    private void ApplyInitialEquippedModifiers()
    {
        if (valueSystem == null) return;

        // 先把当前“已装备”的修正全部应用一次
        foreach (var it in items)
        {
            if (it == null || it.definition == null) continue;
            if (!it.isEquipped) continue;
            if (!it.Equipable || it.Slot == BurdenSlot.None) continue;

            ApplyValueModifiers(it.definition);
        }
    }

    ///<summary>
    ///重新构建 Dictionary 索引（在Inspector手动改items时可用）
    ///</summary>
    public void RebuildIndex()
    {
        itemDict.Clear();
        equippedBySlot.Clear();

        foreach (var it in items)
        {
            //允许 id 为空但 definition 不为空
            if (it == null || it.definition == null)
                continue;

            if (string.IsNullOrEmpty(it.id))
                it.id = it.definition.id;

            if (string.IsNullOrEmpty(it.id))
                continue;



            if (!itemDict.ContainsKey(it.id))
                itemDict[it.id] = it;

            // 同步装备表
            if (it.isEquipped && it.Equipable && it.Slot != BurdenSlot.None)
                equippedBySlot[it.Slot] = it.id;

        }
    }

    // =========================
    // 查询 API
    // =========================
    public bool HasItem(string id) => itemDict.ContainsKey(id);

    public BurdenItemRuntime GetItem(string id)
        => itemDict.TryGetValue(id, out var it) ? it : null;

    public int GetAmount(string id)
        => itemDict.TryGetValue(id, out var it) ? it.amount : 0;

    public string GetEquippedItemId(BurdenSlot slot)
        => equippedBySlot.TryGetValue(slot, out var id) ? id : null;

    // =========================
    // 增删物品
    // =========================
    public void AddItem(BurdenItemDefinition def, int amount = 1, bool autoEquipIfPossible = false)
    {
        if (def == null || string.IsNullOrEmpty(def.id) || amount <= 0)
            return;

        if (itemDict.TryGetValue(def.id, out var existing))
        {
            //可叠加：加数量；不可叠加：也允许加数量（你以后想做“唯一物品”再限制）
            existing.amount += amount;
        }
        else
        {
            var runtime = new BurdenItemRuntime
            {
                id = def.id,
                definition = def,
                amount = amount,
                isEquipped = false
            };
            items.Add(runtime);
            itemDict[def.id] = runtime;
        }

        if (autoEquipIfPossible && def.equipable && def.slot != BurdenSlot.None)
        {
            TryEquip(def.id);
        }

        OnBurdenChanged?.Invoke();
    }

    public bool RemoveItem(string id, int amount = 1, bool autoUnequipIfRemoved = true)
    {
        if (string.IsNullOrEmpty(id) || amount <= 0)
            return false;

        if (!itemDict.TryGetValue(id, out var it))
            return false;

        //如果要移除的数量 >= 持有数量：直接移除该物品
        if (amount >= it.amount)
        {
            if (autoUnequipIfRemoved && it.isEquipped)
                UnequipById(id);

            items.Remove(it);
            itemDict.Remove(id);
            OnBurdenChanged?.Invoke();
            return true;
        }

        it.amount -= amount;
        OnBurdenChanged?.Invoke();
        return true;
    }
    /// <summary>
    ///装备物品时应用其属性修改
    /// </summary>
    private void ApplyValueModifiers(BurdenItemDefinition def)
    {
        if (def == null || valueSystem == null) return;
        if (def.valueModifiers == null) return;

        foreach (var mod in def.valueModifiers)
        {
            if (string.IsNullOrEmpty(mod.valueId)) continue;
            valueSystem.ModifyValue(mod.valueId, mod.delta);
        }
    }

    /// <summary>
    ///脱下物品时应用属性还原
    /// </summary>
    private void RevertValueModifiers(BurdenItemDefinition def)
    {
        if (def == null || valueSystem == null) return;
        if (def.valueModifiers == null) return;

        foreach (var mod in def.valueModifiers)
        {
            if (string.IsNullOrEmpty(mod.valueId)) continue;
            valueSystem.ModifyValue(mod.valueId, -mod.delta);
        }
    }

    //=========================
    //装备/卸下/换装
    //=========================

    ///<summary>
    ///尝试装备某个物品。若该槽位已有装备，会自动卸下旧装备（不移除物品，只改 isEquipped）。
    ///</summary>
    public bool TryEquip(string id)
    {
        
        if (!itemDict.TryGetValue(id, out var it))
            return false;

        if (!it.Equipable || it.Slot == BurdenSlot.None)
            return false;

        // 必须拥有数量 > 0
        if (it.amount <= 0)
            return false;

        // 若槽位已有装备，先卸下旧的
        if (equippedBySlot.TryGetValue(it.Slot, out var oldId) && !string.IsNullOrEmpty(oldId) && oldId != id)
        {
            UnequipById(oldId);
        }

        it.isEquipped = true;
        equippedBySlot[it.Slot] = id;
        ApplyValueModifiers(it.definition);
        

        OnBurdenChanged?.Invoke();
        Debug.Log("[BurdenSystem] TryEquip apply " + it.id);
        return true;

    }

    ///<summary>
    ///按槽位卸下装备
    ///</summary>
    public bool UnequipSlot(BurdenSlot slot)
    {
        if (slot == BurdenSlot.None)
            return false;

        if (!equippedBySlot.TryGetValue(slot, out var id) || string.IsNullOrEmpty(id))
            return false;

        return UnequipById(id);
    }

    /// <summary>
    /// 按物品ID卸下装备
    /// </summary>
    public bool UnequipById(string id)
    {
        if (!itemDict.TryGetValue(id, out var it))
            return false;

        if (!it.isEquipped)
            return false;

        // 先撤销装备带来的数值影响
        RevertValueModifiers(it.definition);

        it.isEquipped = false;

        if (it.Slot != BurdenSlot.None && equippedBySlot.TryGetValue(it.Slot, out var equippedId) && equippedId == id)
            equippedBySlot.Remove(it.Slot);

        OnBurdenChanged?.Invoke();
        return true;
    }

    /// <summary>
    /// 便捷:切换装备状态（已装备则卸下，未装备则装备）
    /// </summary>
    public bool ToggleEquip(string id)
    {
        var it = GetItem(id);
        if (it == null) return false;

        return it.isEquipped ? UnequipById(id) : TryEquip(id);
    }
    /// <summary>
    /// 按槽位筛选可装备物品
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public List<BurdenItemRuntime> GetEquipableItemsForSlot(BurdenSlot slot)
    {
        var result = new List<BurdenItemRuntime>();
        foreach (var it in items)
        {
            if (it == null || it.definition == null) continue;
            if (it.amount <= 0) continue;
            if (!it.Equipable) continue;
            if (it.Slot != slot) continue;
            result.Add(it);
        }
        return result;
    }
}
