using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static EventChoice;
using DG.Tweening;

public class EventUIManager : MonoBehaviour
{
    [Header("返回控制")]
    public bool isReturnBlocked = false;
    public void BlockReturn(bool block) => isReturnBlocked = block;

    [Header("UI 绑定")]
    public GameObject storyPanel;
    public GameObject HeaderCard;
    public TMP_Text headerEventDescription;
    public TMP_Text headerEventTitle;
    public Button returnButton;
    public GameObject storyCardPrefab;
    public Transform storyBroad;
    public GameObject regionPanel;
    public ScrollRect storyPanelScrollRect;

    [Header("依赖")]
    public EventManager eventManager;

    [Header("对象池")]
    public CardPoolManager cardPoolManager;

    [Header("滚动条绑定")]
    public GameObject storyPanelScrollbar;
    public GameObject regionPanelScrollbar;

    void Start()
    {
        returnButton.onClick.AddListener(() =>
        {
            if (eventManager.CurrentEvent.HasTag(EventTag.Returnable))
            {
                eventManager.eventUIManager.ClearStoryCards();
                //eventManager.eventUIManager.storyPanel.SetActive(false);
                //eventManager.eventUIManager.HeaderCard.SetActive(false);
                UIManager.Instance.SwitchState(UIManager.UIState.Region);

                //优先按历史记录返回
                if (eventManager.regionHistory.Count > 0)
                {
                    RegionInfo previous = eventManager.regionHistory.Pop();
                    eventManager.lastRegion = previous;
                    eventManager.regionPanelManager.ShowRegion(previous, disableHistoryPush: true);
                }
                else if (eventManager.lastRegion != null)
                {
                    //如果历史为空，回到 lastRegion
                    eventManager.regionPanelManager.ShowRegion(eventManager.lastRegion);
                }
                return;
            }

            if (isReturnBlocked)
            {
                Debug.Log("返回按钮已被阻止");
                return;
            }

            //storyPanel.SetActive(false);
            UIManager.Instance.SwitchState(UIManager.UIState.Region);
            RefreshLayout();
            //HeaderCard.SetActive(true);
            RefreshLayout();
        });
    }

    public void ShowEvent(NarrativeEvent currentEvent)
    {
        if (currentEvent == null)
        {
            Debug.LogError("当前事件为空！");
            return;
        }

        //storyPanel.SetActive(true);
        //HeaderCard.SetActive(true);
        UIManager.Instance.SwitchState(UIManager.UIState.Story);

        headerEventTitle.text = currentEvent.title;
        headerEventDescription.text = currentEvent.description;

        // 清空旧卡片
        foreach (Transform child in storyBroad)
        {
            cardPoolManager.Release(child.gameObject);
        }
        storyBroad.DetachChildren();

        foreach (var choice in currentEvent.choices)
        {
            GameObject card = cardPoolManager.GetCard();
            card.SetActive(true);
            card.transform.SetParent(storyBroad, false);

            Animators.cardEntrancePlay(card, storyBroad, type: 3);//卡片进入动画调用CardEntranceAnimator.cs

            EventCardController controller = card.GetComponent<EventCardController>();

            string title = choice.text;
            string descriptionText = choice.description;
            string tag = currentEvent.tags != null && currentEvent.tags.Count > 0
                ? string.Join(", ", currentEvent.tags)
                : "";

            string requireText = GenerateRequirementText(choice);

            bool available = CheckRequirements(choice);

            controller.eventUIManager = this;
            controller.SetDataFromChoice(title, requireText, descriptionText, tag, available, choice);

            var localChoice = choice;

            controller.goButton.onClick.RemoveAllListeners();
            controller.goButton.onClick.AddListener(() =>
            {
                if (CheckRequirements(localChoice))
                {
                    eventManager.SelectChoiceDirect(localChoice);
                }
                else
                {
                    Debug.Log("不满足进入条件！");
                }
            });
        }

        returnButton.gameObject.SetActive(true);

        if (currentEvent.HasTag(EventTag.Returnable))//如果拥有Returnable标签，则可以返回
        {
            isReturnBlocked = false;
            returnButton.interactable = true;

            ColorBlock colors = returnButton.colors;
            colors.normalColor = new Color(57f / 255f, 54f / 255f, 111f / 255f, 1f);
            colors.highlightedColor = new Color(91f / 255f, 88f / 255f, 156f / 255f, 1f);
            colors.pressedColor = new Color(45f / 255f, 42f / 255f, 84f / 255f, 1f);
            colors.selectedColor = new Color(91f / 255f, 88f / 255f, 156f / 255f, 1f);
            returnButton.colors = colors;
        }
        else  //否则相反
        {
            isReturnBlocked = true;
            returnButton.interactable = false;

            ColorBlock colors = returnButton.colors;
            colors.normalColor = new Color(0.6f, 0.6f, 0.6f, 0.5f);
            colors.highlightedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            returnButton.colors = colors;
        }
        //滚动条设定函数，切换页面切换不同的滚动条
        if (storyPanelScrollbar != null) storyPanelScrollbar.SetActive(true);
        if (regionPanelScrollbar != null) regionPanelScrollbar.SetActive(false);
        //刷新布局函数
        StartCoroutine(RefreshLayoutDelayed());
        UIManager.Instance.ScrollPanelToTop(storyPanelScrollRect);
    }

    string GenerateRequirementText(EventChoice choice)
    {
        if (choice.traitRequirements == null || choice.traitRequirements.Count == 0)
            return "";

        List<string> requirements = new List<string>();
        foreach (var req in choice.traitRequirements)
        {
            if (req != null)
                requirements.Add($"需要 {req.traitId} ≥ {req.requiredValue}");
        }
        return string.Join("，", requirements);
    }

    bool CheckRequirements(EventChoice choice)
    {
        foreach (var req in choice.traitRequirements)
        {
            if (req != null && eventManager.valueSystem.GetValue(req.traitId) < req.requiredValue)
                return false;
        }
        return true;
    }
    #region 刷新布局相关
    void RefreshLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(storyBroad.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(storyPanel.GetComponent<RectTransform>());
    }

    public IEnumerator RefreshLayoutDelayed()
    {
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(storyPanel.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(storyBroad.GetComponent<RectTransform>());
    }
    #endregion
    public void ClearStoryCards()
    {
        if (cardPoolManager != null)
        {
            cardPoolManager.ReclaimAllCards(storyPanel.transform);
        }

        UIManager.Instance.SwitchState(UIManager.UIState.Region);
    }
}