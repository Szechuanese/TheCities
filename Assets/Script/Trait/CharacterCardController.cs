using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//与TraitCardController类同
public class CharacterCardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text nameText;
    public TMP_Text valueText;

    private string characterId;
    public Image characterImage; // 在 Inspector 绑定 CharacterImageCard → CharacterImage
    public IconDatabase iconDatabase;

    public void SetCharacter(string id, float value)
    {
        characterId = id;

        string displayName = id;
        var character = GameObject.FindObjectOfType<CharacterSystem>()?.characters.Find(c => c.id == id);
        if (character != null)
            displayName = character.displayName;

        nameText.text = displayName;
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