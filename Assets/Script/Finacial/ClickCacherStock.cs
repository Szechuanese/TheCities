using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCacherStock : MonoBehaviour
{
    //��Ʊҳ��ر�
    [Header("��ͼ����壨������ر�����")]
    public GameObject stockMarketPanel;
    public Canvas stockMarketCanvas;


    public void OnClickCloseStock()
    {
        //�رչ�Ʊҳ��ʱ����StoryCanvas
        if (stockMarketCanvas != null)
        {
            stockMarketCanvas.gameObject.SetActive(false);
            FindObjectOfType<StockMarketManager>()?.SetCanvasOpen(false);
            FindObjectOfType<EventTagHandler>()?.ReturnToRegion();

            //���pendingEnterStockMarket״̬
            var eventManager = FindObjectOfType<EventManager>();
            if (eventManager != null)
                eventManager.SetPendingStockMarket(false);
        }
    }
}
