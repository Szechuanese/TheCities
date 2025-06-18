using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCacherStock : MonoBehaviour
{
    //股票页面关闭
    [Header("地图总面板（点击后会关闭它）")]
    public GameObject stockMarketPanel;
    public Canvas stockMarketCanvas;


    public void OnClickCloseStock()
    {
        //关闭股票页面时，打开StoryCanvas
        if (stockMarketCanvas != null)
        {
            stockMarketCanvas.gameObject.SetActive(false);
            FindObjectOfType<StockMarketManager>()?.SetCanvasOpen(false);
            FindObjectOfType<EventTagHandler>()?.ReturnToRegion();

            //清空pendingEnterStockMarket状态
            var eventManager = FindObjectOfType<EventManager>();
            if (eventManager != null)
                eventManager.SetPendingStockMarket(false);
        }
    }
}
