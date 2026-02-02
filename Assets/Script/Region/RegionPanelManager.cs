using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
//区域起始事件显示
public class RegionPanelManager : MonoBehaviour
{
    [Header("UI 组件")]
    public GameObject regionPanel;
    public TMP_Text regionTitleText;
    public TMP_Text regionDescriptionText;
    public Transform contentParent;
    public GameObject storyCardPrefab;
    public EventManager eventManager;
    public GameObject regionContent; //只用于精准定位刷新用，绑定的RegionStory_Content
    public RectTransform regionContentTransform;//用来告诉unity
    public Transform regionStoryBroad;
    public ScrollRect regionPanelScroll;


    [Header("依赖组件")]
    public GameObject worldMapPanel;
    private List<GameObject> currentCards = new List<GameObject>();

    public void ShowRegion(RegionInfo regionInfo, bool disableHistoryPush = false)
    {
        #region 起手动作，保证UI正确刷新
        //告诉Unity,ScrollRect换content
        regionPanelScroll.content = regionContentTransform;
        //强制刷新Layout确保高度计算正确
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(regionContentTransform);
        //等一帧再刷新
        StartCoroutine(RefreshLayoutNextFrame());
        #endregion

        //进入区域时关闭按钮。
        MapTooltipManager.instance?.HideTooltip();


        if (regionInfo == null) return;

        eventManager.eventUIManager.ClearStoryCards();

        //切换到Region状态
        UIManager.Instance.SwitchState(UIManager.UIState.Region);
        //设置RegionPanel的标题和描述
        regionTitleText.text = regionInfo.regionData.regionDisplayName;
        regionDescriptionText.text = regionInfo.regionData.regionDescription;
        //滚动条切换（具体绑定在UIManager）
        if (eventManager.eventUIManager.storyPanelScrollbar != null)
            eventManager.eventUIManager.storyPanelScrollbar.SetActive(false);

        if (eventManager.eventUIManager.regionPanelScrollbar != null)
            eventManager.eventUIManager.regionPanelScrollbar.SetActive(true);
        //刷新页面，清楚卡片
        StartCoroutine(RefreshLayoutNextFrame());
        ClearCards();

        //遍历区域信息，区域数据，区域事件
        foreach (var e in regionInfo.regionData.regionEvents)
        {
            if (e == null) continue;

            GameObject card = Instantiate(storyCardPrefab, contentParent);
            EventCardController controller = card.GetComponent<EventCardController>();
            if (controller != null)
            {
                controller.eventUIManager = eventManager.eventUIManager;
                controller.LoadEvent(e, eventManager, isPreview: true);
            }
            Animators.cardEntrancePlay(card, regionStoryBroad, type: 3);//卡片进入动画调用CardEntranceAnimator.cs
            currentCards.Add(card);
        }
        //再次刷新页面
        StartCoroutine(RefreshLayoutNextFrame());
        //
        if (!disableHistoryPush && eventManager.lastRegion != null)
            eventManager.regionHistory.Push(eventManager.lastRegion);

        eventManager.lastRegion = regionInfo;
        eventManager.exploredRegionIds.Add(regionInfo.regionData.regionId);
        EventLogManager.instance?.AddLog($"你来到了【{regionInfo.regionData.regionDisplayName}】");
        //调用UIManager方法让滚动条向上
        UIManager.Instance.ScrollPanelToTop(regionPanelScroll);
        //再次刷新页面
        StartCoroutine(RefreshLayoutNextFrame());
    }

    public void CloseRegionPanel()
    {
        //调用UIManager切换状态到Story
        UIManager.Instance.SwitchState(UIManager.UIState.Story);
        ClearCards();
        //关闭RegionPanel时候，关闭RegionPanel的滚动条
        if (eventManager.eventUIManager.regionPanelScrollbar != null)
            eventManager.eventUIManager.regionPanelScrollbar.SetActive(false);
    }

    private void ClearCards()
    {
        foreach (var card in currentCards)
        {
            Destroy(card);
        }
        currentCards.Clear();
    }
    #region 刷新布局相关
    private IEnumerator RefreshLayoutNextFrame()
    {
        yield return null;

        //目前的布局刷新方法，暂时只需要RegionStoryBroad
        LayoutRebuilder.ForceRebuildLayoutImmediate(regionPanel.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(regionStoryBroad.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(regionContent.GetComponent<RectTransform>());
    }
    #endregion
}

