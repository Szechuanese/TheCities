using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TraitCardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text titleText;
    public TMP_Text valueText;
    public Slider traitSlider;
    public Image traitImage;
    public IconDatabase iconDatabase;
    private string traitId;

    public void SetData(string id, string displayName, float value)
    {
        traitId = id;
        if (titleText != null)
            titleText.text = displayName;
        if (valueText != null)
            valueText.text = Mathf.RoundToInt(value).ToString();
        if (traitSlider != null)
            traitSlider.value = value;

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
            TraitToolTipManager.instance?.ShowById(traitId);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TraitToolTipManager.instance?.HideTooltip();
    }
}
