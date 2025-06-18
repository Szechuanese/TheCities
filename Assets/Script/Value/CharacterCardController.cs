using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CharacterCardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text titleText;
    public TMP_Text valueText;
    public Image characterImage;
    public IconDatabase iconDatabase;
    private string characterId;

    public void SetData(string id, string displayName, float value)
    {
        characterId = id;
        if (titleText != null)
            titleText.text = displayName;
        if (valueText != null)
            valueText.text = Mathf.RoundToInt(value).ToString();

        if (iconDatabase != null && characterImage != null)
        {
            var icon = iconDatabase.GetIcon(id);
            if (icon != null)
            {
                characterImage.sprite = icon;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(characterId))
            TraitToolTipManager.instance?.ShowById(characterId);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TraitToolTipManager.instance?.HideTooltip();
    }
}