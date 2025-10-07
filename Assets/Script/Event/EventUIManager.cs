using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static EventChoice;
using DG.Tweening;

public class EventUIManager : MonoBehaviour
{

    //初始化与页面绑定
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
        //返回按钮包裹
        returnButton.onClick.AddListener(() =>
        {
            //返回按钮点击音效
            AudioManager.Instance.PlaySFX("ReturnButton_Click");
            //检测当前事件是否具有 Returnable 标签，（返回标签）
            if (eventManager.CurrentEvent.HasTag(EventTag.Returnable))
            {
                //收回卡片
                eventManager.eventUIManager.ClearStoryCards();
                //如果当前事件具有 Returnable 标签，则返回到区域面板
                UIManager.Instance.SwitchState(UIManager.UIState.Region);

                //优先按历史记录返回
                if (eventManager.regionHistory.Count > 0)
                {
                    RegionInfo previous = eventManager.regionHistory.Pop();
                    eventManager.lastRegion = previous;
                    eventManager.regionPanelManager.ShowRegion(previous, disableHistoryPush: true);
                }
                //如果历史为空，回到 lastRegion
                else if (eventManager.lastRegion != null)
                {
                    eventManager.regionPanelManager.ShowRegion(eventManager.lastRegion);
                }
                return;
            }
            //
            if (isReturnBlocked)
            {
                Debug.Log("返回按钮已被阻止");
                return;
            }

            //返回区域面板
            UIManager.Instance.SwitchState(UIManager.UIState.Region);
            RefreshLayout();
            RefreshLayout();
        });
    }


    //显示当前Event方法
    public void ShowEvent(NarrativeEvent currentEvent)
    {
        //如果当前事件为空，则返回错误信息
        if (currentEvent == null)
        {
            Debug.LogError("当前事件为空！");
            return;
        }
        //切换至StoryPanel
        UIManager.Instance.SwitchState(UIManager.UIState.Story);
        //装载描述文本
        headerEventTitle.text = currentEvent.title;
        headerEventDescription.text = currentEvent.description;

        // 清空旧卡片
        foreach (Transform child in storyBroad)
        {
            cardPoolManager.Release(child.gameObject);
        }
        storyBroad.DetachChildren();
        //遍历当前事件的选择项，生成卡片
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

                AudioManager.Instance.PlaySFX("GoButton_Click");//播放选择音效

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

        //如果拥有Returnable标签，则可以返回
        if (currentEvent.HasTag(EventTag.Returnable))
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
        //否则则无法返回，并修改按钮（卡片整体，因为按钮就是整张卡片）样式
        else
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
    //生成需要的Value值得文本
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
    //检查选择项的要求是否满足
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
    //清除所有故事卡片方法
    public void ClearStoryCards()
    {
        if (cardPoolManager != null)
        {
            cardPoolManager.ReclaimAllCards(storyPanel.transform);
        }

        UIManager.Instance.SwitchState(UIManager.UIState.Region);
    }
}