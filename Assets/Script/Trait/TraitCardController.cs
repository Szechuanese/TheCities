using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TraitCardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text nameText;
    public TMP_Text valueText;
    public Slider traitSlider;
    public Image traitImage; // 在 Inspector 绑定 TraitImageCard → TraitImage
    public IconDatabase iconDatabase; // 在 Inspector 绑定 ScriptableObject 资源

    private string traitId;

    public void SetTrait(string id, float value)
    {
        traitId = id;

        // 从 traitSystem 中查找中文 displayName
        string displayName = id;
        var trait = GameObject.FindObjectOfType<TraitSystem>()?.traits.Find(t => t.id == id);
        if (trait != null)
            displayName = trait.displayName;

        nameText.text = displayName; // ✅ UI 显示中文
        traitSlider.value = value;
        valueText.text = Mathf.RoundToInt(value).ToString();

        if (iconDatabase != null && traitImage != null)
        {
            var icon = iconDatabase.GetIcon(id);
            if (icon != null)
            {
                traitImage.sprite = icon;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(traitId))
        {
            TooltipManager.instance?.ShowById(traitId);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.instance?.Hide();
    }
}