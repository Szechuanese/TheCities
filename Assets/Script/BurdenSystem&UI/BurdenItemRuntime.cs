using System;
using UnityEngine;

[Serializable]
public class BurdenItemRuntime
{
    public string id;
    public BurdenItemDefinition definition;

    [Min(0)]
    public int amount = 1;

    public bool isEquipped = false;

    public BurdenSlot Slot => definition != null ? definition.slot : BurdenSlot.None;
    public bool Equipable => definition != null && definition.equipable;
    public bool Stackable => definition != null && definition.stackable;
}