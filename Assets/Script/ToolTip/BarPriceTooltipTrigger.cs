using UnityEngine;
using UnityEngine.EventSystems;
//StockMarket鼠标悬停时显示价格的Tooltip触发器
public class PriceTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float price; //当前柱子价格

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShareholderTooltipManager.instance?.ShowTooltip("历史价格", $"价格：{price:F2} 元");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShareholderTooltipManager.instance?.HideTooltip();
    }
}
