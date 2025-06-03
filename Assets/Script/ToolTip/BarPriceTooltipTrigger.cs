using UnityEngine;
using UnityEngine.EventSystems;
//StockMarket�����ͣʱ��ʾ�۸��Tooltip������
public class PriceTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float price; //��ǰ���Ӽ۸�

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShareholderTooltipManager.instance?.ShowTooltip("��ʷ�۸�", $"�۸�{price:F2} Ԫ");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShareholderTooltipManager.instance?.HideTooltip();
    }
}
