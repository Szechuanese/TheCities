using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;

public class EventLogUI : MonoBehaviour
{
    public TMP_Text logText;
    public GameObject logPanel;
    public GameObject logCloseButton;
    public GameObject aPageButton;
    public Transform logCardScrollController; // Content容器
    public GameObject logCardPrefab; // LogCard预制体
    public ScrollRect scrollRect; // 日志ScrollView

    private List<GameObject> activeLogCards = new List<GameObject>();
    public void Start()
    {
        logPanel.SetActive(false);
    }
    public void showLogPanel()
    {
        logPanel.SetActive(true);
        RefreshLogs();
    }
    public void hideLogPanel()
    {
        logPanel.SetActive(false);
    }
    public void RefreshLogs()
    {
        if (EventLogManager.instance == null) return;

        ClearOldLogCards();

        List<string> logs = EventLogManager.instance.GetLogs();
        foreach (string log in logs)
        {
            CreateLogCard(log);
        }

        // 最后刷新布局并滚动到底部
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
    public void AddNewLogCard(string text)
    {
        CreateLogCard(text);
        ScrollToBottom();
    }

    private void CreateLogCard(string logText)
    {
        GameObject newCard = Instantiate(logCardPrefab, logCardScrollController);
        TMP_Text textComp = newCard.GetComponentInChildren<TMP_Text>();
        if (textComp != null)
        {
            textComp.text = logText;
        }
        activeLogCards.Add(newCard);
    }

    private void ClearOldLogCards()
    {
        foreach (var card in activeLogCards)
        {
            Destroy(card);
        }
        activeLogCards.Clear();
    }
    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}