using UnityEngine;

public class EventTagHandler : MonoBehaviour
{
    [Header("UI 面板绑定")]
    public GameObject middleContent;
    public GameObject stockMarketPanel;
    public GameObject regionPanel;

    public RegionPanelManager regionPanelManager;

    private RegionInfo cachedLastRegion;
    [Header("Canvas 控制")]
    public Canvas storyCanvas;
    public Canvas stockMarketCanvas;


    public void Handle(EventTag tag, EventManager manager)
    {
        switch (tag)
        {
            case EventTag.StockMarketEntry:
                Debug.Log("📈 StockMarketEntry 触发");
                manager.SetPendingStockMarket(true);
                break;
        }
    }
    public void ReturnToRegion()
    {
        //打开 StoryCanvas
        if (storyCanvas != null) storyCanvas.gameObject.SetActive(true);

        //同时打开 RegionPanel，并返回区域
        if (regionPanel != null) regionPanel.SetActive(true);

        if (cachedLastRegion != null && regionPanelManager != null)
        {
            regionPanelManager.ShowRegion(cachedLastRegion, disableHistoryPush: true);
        }
    }

    public void ExecuteStockMarketTransition(RegionInfo lastRegion)
    {

        //控制股票面板和区域面板的切换
        cachedLastRegion = lastRegion;

        // 完整关闭 StoryCanvas
        if (storyCanvas != null) storyCanvas.gameObject.SetActive(false);

        if (regionPanel != null) regionPanel.SetActive(false);
        if (stockMarketCanvas != null)
        {
            stockMarketCanvas.gameObject.SetActive(true);
            FindObjectOfType<StockMarketManager>()?.SetCanvasOpen(true);
        }
    }
}