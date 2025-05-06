using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static EventChoice;

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
    public Transform storyCardContainer;
    public GameObject regionPanel;

    [Header("依赖")]
    public EventManager eventManager;
    public ScrollResetToTop scrollResetter;

    [Header("对象池")]
    public CardPoolManager cardPoolManager;

    void Start()
    {
        returnButton.onClick.AddListener(() =>
        {
            if (isReturnBlocked)
            {
                Debug.Log("返回按钮已被阻止");
                return;
            }

            storyPanel.SetActive(false);
            HeaderCard.SetActive(true);
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

        storyPanel.SetActive(true);
        HeaderCard.SetActive(true);

        headerEventTitle.text = currentEvent.title;
        headerEventDescription.text = currentEvent.description;

        // 清空旧卡片
        foreach (Transform child in storyCardContainer)
        {
            cardPoolManager.Release(child.gameObject);
        }
        storyCardContainer.DetachChildren();

        foreach (var choice in currentEvent.choices)
        {
            GameObject card = cardPoolManager.GetCard();
            card.SetActive(true);
            card.transform.SetParent(storyCardContainer, false);

            EventCardController controller = card.GetComponent<EventCardController>();

            string title = choice.text;
            string descriptionText = choice.description;
            string tag = "";

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

        returnButton.gameObject.SetActive(!currentEvent.isImportant);
        isReturnBlocked = currentEvent.isImportant;

        StartCoroutine(RefreshLayoutDelayed());
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
            if (req != null && eventManager.traitSystem.GetTrait(req.traitId) < req.requiredValue)
                return false;
        }
        return true;
    }

    void RefreshLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(storyCardContainer.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(storyPanel.GetComponent<RectTransform>());
    }

    public IEnumerator RefreshLayoutDelayed()
    {
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(storyPanel.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(storyCardContainer.GetComponent<RectTransform>());
    }
    public void ClearStoryCards()
    {
        if (cardPoolManager != null)
        {
            cardPoolManager.ReclaimAllCards(storyPanel.transform); // 回收所有使用中的卡片到池中
        }

        storyPanel.SetActive(false);   // 隐藏事件区域
    }
}