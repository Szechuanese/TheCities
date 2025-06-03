using UnityEngine;
using UnityEngine.EventSystems;
//没几把用的脚本，但是我删除会报错，所以它还留在这里。，但我不希望它引起任何错误。
public class ToolTipManagerTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string id; // 如 "trait_wisdom", "char_insight"

    public void OnPointerEnter(PointerEventData eventData)
    {
        //TraitToolTipManager.instance?.ShowMapTip(id);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //TraitToolTipManager.instance?.HideMapTip();
    }
}