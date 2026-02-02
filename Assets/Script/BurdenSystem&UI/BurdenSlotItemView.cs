using UnityEngine;
using UnityEngine.UI;

public class BurdenSlotItemView : MonoBehaviour
{
    [Header("Bind")]
    public BurdenSystem burdenSystem;     // 不填也行：运行时自动找
    public BurdenSlot slot = BurdenSlot.Hat;

    [Header("UI")]
    public Image burdenItemImage;         // 绑定你新建的 BurdenItemImage

    [Header("Behavior")]
    public bool hideWhenEmpty = true;

    private void Awake()
    {
        if (burdenSystem == null)
            burdenSystem = FindFirstObjectByType<BurdenSystem>();
    }

    private void OnEnable()
    {
        if (burdenSystem != null)
            burdenSystem.OnBurdenChanged += Refresh;

        Refresh();
    }

    private void OnDisable()
    {
        if (burdenSystem != null)
            burdenSystem.OnBurdenChanged -= Refresh;
    }

    public void Refresh()
    {
        if (burdenItemImage == null || burdenSystem == null)
            return;

        string equippedId = burdenSystem.GetEquippedItemId(slot);

        if (string.IsNullOrEmpty(equippedId))
        {
            if (hideWhenEmpty)
                burdenItemImage.enabled = false;
            else
            {
                burdenItemImage.enabled = true;
                burdenItemImage.sprite = null;
            }
            return;
        }

        var item = burdenSystem.GetItem(equippedId);
        var icon = item?.definition != null ? item.definition.icon : null;

        burdenItemImage.sprite = icon;
        burdenItemImage.enabled = (icon != null) || !hideWhenEmpty;
    }
}
