using System.Collections.Generic;
using UnityEngine;
using static BurdenCategory;

[CreateAssetMenu(fileName = "BurdenItem", menuName = "Burden/Item Definition")]
public class BurdenItemDefinition : ScriptableObject
{
    [Header("Category")]
    public E_BurdenCategory category;
    [Header("Main Paraments")]
    public string id;                 // 唯一ID（事件/存档/查找用）
    public string displayName;        // 显示名
    [TextArea] public string description;

    [Header("Visual")]
    public Sprite icon;

    [Header("Rules")]
    [Tooltip("是否可叠加")]
    public bool stackable = false;    // 可叠加（数量）
    [Tooltip("是否可装备")]
    public bool equipable = false;    // 可装备
    [Tooltip("装备槽位")]
    public BurdenSlot slot = BurdenSlot.None; // 装备槽位


    [Header("Value Modifiers")]
    public List<ValueModifier> valueModifiers = new List<ValueModifier>();

    //结构体，用于装备物品时的属性修改
    [System.Serializable]
    public class ValueModifier
    {
        public string valueId;   // 对应 ValueSystem 中的 Value.id
        public float delta;      // 装备时 +delta，卸下时 -delta
    }

}
