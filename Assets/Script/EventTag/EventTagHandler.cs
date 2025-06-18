using UnityEngine;
using static UIManager;

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
                Debug.Log("StockMarketEntry 触发");
                manager.SetPendingStockMarket(true);
                break;
        }
    }
    public void ReturnToRegion()
    {
        UIManager.Instance.SwitchState(UIState.Region);
    }

    public void ExecuteStockMarketTransition(RegionInfo lastRegion)
    {

        //控制股票面板和区域面板的切换
        cachedLastRegion = lastRegion;

        // 完整关闭 StoryCanvas
        UIManager.Instance.SwitchState(UIState.StockMarket);
        FindObjectOfType<StockMarketManager>()?.SetCanvasOpen(true);
    }
}