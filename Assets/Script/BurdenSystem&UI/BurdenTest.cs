using UnityEngine;

public class BurdenTest : MonoBehaviour
{
    public BurdenSystem bd;
    public BurdenItemDefinition myMushroom;
    public BurdenItemDefinition myShip;
    public BurdenItemDefinition laoyo;

    void Start()
    {
        if (bd == null) bd = FindFirstObjectByType<BurdenSystem>();

        bd.AddItem(myMushroom, 1, false); // 自动装备
        //bd.AddItem(myShip, 1, false); // 自动装备
        //bd.AddItem(laoyo, 1, false);

        Debug.Log("HasItem=" + bd.HasItem(myMushroom.id));
        Debug.Log("Amount=" + bd.GetAmount(myMushroom.id));
        Debug.Log("TryEquip=" + bd.TryEquip(myMushroom.id)); // 再强制装备一次，看是否成功
        Debug.Log("equippedId(Hat)=" + bd.GetEquippedItemId(BurdenSlot.Hat));
    }
}
