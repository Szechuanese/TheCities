using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipManagerTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string id; // 如 "trait_wisdom", "char_insight"

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.instance?.ShowById(id);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.instance?.Hide();
    }
}