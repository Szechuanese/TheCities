using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ValueItemController : MonoBehaviour
{
    [Header("组件绑定")]
    public Image itemIcon;                      // ItemIcon
    public TMP_Text itemTitle;                  // ItemTitle
    public TMP_Text itemValue;                  // ItemValue
    public TMP_Text itemDescribe;               // ItemDescribe

    [Header("数据")]
    public IconDatabase iconDatabase;           // TraitIconDatabase

    public void SetData(string id, string displayName, float value, string description)
    {
        // 设置Icon
        if (iconDatabase != null && itemIcon != null)
        {
            var icon = iconDatabase.GetIcon(id);
            if (icon != null)
                itemIcon.sprite = icon;
        }

        // 设置文字
        if (itemTitle != null)
            itemTitle.text = displayName;

        if (itemValue != null)
            itemValue.text = Mathf.RoundToInt(value).ToString();

        if (itemDescribe != null)
            itemDescribe.text = description;
    }
}