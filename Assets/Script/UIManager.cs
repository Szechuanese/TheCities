using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    //UI状态机控制所有面板切换，
    public static UIManager Instance { get; private set; }


    [Header("各种Canvas和Panel")]
    public GameObject storyPanel;
    public GameObject filePanel;
    public Canvas stockMarketCanvas;
    public GameObject regionPanel;
    public GameObject worldPanel;
    public GameObject settingPanel;
    public GameObject mainCanvas;
    // 可继续添加其它面板引用


    [Header("各种ScrollRect")]
    public ScrollRect storyScrollRect;
    public ScrollRect fileScrollRect;
    public ScrollRect authorizScrollRect;
    public ScrollRect burdenScrollRect;

    public enum UIState
    {
        MainMenu,
        //Setting,
        Region,
        Story,
        StockMarket,
        File,
        WorldMap,
        None
    }

    private UIState currentState = UIState.Story;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SwitchState(UIState nextState)
    {
        if (currentState == nextState) return;

        // 统一关闭所有面板
        storyPanel.gameObject.SetActive(false);
        filePanel.SetActive(false);
        stockMarketCanvas.gameObject.SetActive(false);
        regionPanel.SetActive(false);
        worldPanel.SetActive(false);
        settingPanel.gameObject.SetActive(false);
        //可以继续添加其它面板

        // 打开目标面板
        switch (nextState)
        {
            case UIState.Story:
                storyPanel.gameObject.SetActive(true);
                break;
            case UIState.Region:
                regionPanel.SetActive(true);
                break;
            case UIState.StockMarket:
                stockMarketCanvas.gameObject.SetActive(true);
                break;
            case UIState.File:
                filePanel.SetActive(true);
                break;
            case UIState.WorldMap:
                worldPanel.SetActive(true);
                regionPanel.SetActive(true);
                break;
            //case UIState.Setting:
            //    settingPanel.gameObject.SetActive(true);
            //    mainCanvas.SetActive(true);
            //    break;
            case UIState.MainMenu:
                // 加主菜单逻辑
                break;
            case UIState.None:
            default:
                // 所有面板全关
                break;
        }
        currentState = nextState;
    }

    public UIState GetCurrentState()
    {
        return currentState;
    }

    #region 滚动置顶
    public void ScrollPanelToTop(ScrollRect scrollRect)
    {
        StartCoroutine(ScrollToTopAfterFrame(scrollRect));
    }
    private IEnumerator ScrollToTopAfterFrame(ScrollRect scrollRect)
    {
        yield return null;
        if (scrollRect != null)
            scrollRect.verticalNormalizedPosition = 1f;
    }
    #endregion
}
