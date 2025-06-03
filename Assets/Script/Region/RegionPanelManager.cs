using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
//区域事件显示
public class RegionPanelManager : MonoBehaviour
{
    [Header("UI 组件")]
    public GameObject regionPanel;
    public TMP_Text regionTitleText;
    public TMP_Text regionDescriptionText;
    public Transform contentParent;
    public GameObject storyCardPrefab;
    public EventManager eventManager;
    public GameObject regionContent; //只用于精准定位刷新用
    public Transform regionStoryBroad;


    [Header("依赖组件")]
    public GameObject worldMapPanel;

    private List<GameObject> currentCards = new List<GameObject>();

    public void ShowRegion(RegionInfo regionInfo, bool disableHistoryPush = false)
    {
        MapTooltipManager.instance?.HideTooltip();//进入区域时关闭按钮。


        if (regionInfo == null) return;

        //StartCoroutine(RefreshLayoutNextFrame());
        eventManager.eventUIManager.ClearStoryCards();

        //StartCoroutine(RefreshLayoutNextFrame());
        regionPanel.SetActive(true);
        regionTitleText.text = regionInfo.regionData.regionDisplayName;
        regionDescriptionText.text = regionInfo.regionData.regionDescription;
        //滚动条切换（具体绑定在UIManager）
        if (eventManager.eventUIManager.storyPanelScrollbar != null)
            eventManager.eventUIManager.storyPanelScrollbar.SetActive(false);

        if (eventManager.eventUIManager.regionPanelScrollbar != null)
            eventManager.eventUIManager.regionPanelScrollbar.SetActive(true);
        StartCoroutine(RefreshLayoutNextFrame());
        ClearCards();

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
            CardEntranceAnimator.Play(card, regionStoryBroad, type: 3);//卡片进入动画调用CardEntranceAnimator.cs
            currentCards.Add(card);
        }

        StartCoroutine(RefreshLayoutNextFrame());
        if (!disableHistoryPush && eventManager.lastRegion != null)
            eventManager.regionHistory.Push(eventManager.lastRegion);

        eventManager.lastRegion = regionInfo;
        eventManager.exploredRegionIds.Add(regionInfo.regionData.regionId);
        EventLogManager.instance?.AddLog($"你来到了【{regionInfo.regionData.regionDisplayName}】");

        //StartCoroutine(RefreshLayoutNextFrame());
    }

    public void CloseRegionPanel()
    {
        regionPanel.SetActive(false);
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

    private IEnumerator RefreshLayoutNextFrame()
    {
        yield return null;

        //目前的布局刷新方法，暂时只需要RegionStoryBroad
        //LayoutRebuilder.ForceRebuildLayoutImmediate(regionPanel.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(regionStoryBroad.GetComponent<RectTransform>());
        //LayoutRebuilder.ForceRebuildLayoutImmediate(regionContent.GetComponent<RectTransform>());
    }
}

