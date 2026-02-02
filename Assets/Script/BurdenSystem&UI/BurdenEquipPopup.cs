using System.Collections.Generic;
using UnityEngine;

public class BurdenEquipPopup : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelRoot;      //弹窗面板根物体（用 SetActive 控制显示/隐藏）
    public Transform content;         //列表容器（Vertical Layout Group）
    public BurdenEquipOptionView optionPrefab; //单条选项预制体

    private BurdenSystem burdenSystem;
    private BurdenSlot currentSlot;

    public void Open(BurdenSlot slot, BurdenSystem system)
    {
        currentSlot = slot;
        burdenSystem = system;

        if (panelRoot != null) panelRoot.SetActive(true);
        RebuildList();
    }

    public void Close()
    {
        if (panelRoot != null) panelRoot.SetActive(false);
    }

    private void RebuildList()
    {
        foreach (Transform child in content) Destroy(child.gameObject);

        List<BurdenItemRuntime> list = burdenSystem.GetEquipableItemsForSlot(currentSlot);

        foreach (var it in list)
        {
            var row = Instantiate(optionPrefab, content);
            row.Bind(it, OnChoose);
        }
    }

    private void OnChoose(BurdenItemRuntime item)
    {
        if (item == null) return;

        // 装备（会自动顶掉同槽位旧装备）
        burdenSystem.TryEquip(item.id);
        Close();
    }
}
