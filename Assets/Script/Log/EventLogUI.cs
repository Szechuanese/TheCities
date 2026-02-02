using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class EventLogUI : MonoBehaviour
{
    [Header("绑定组件")]
    public TMP_Text logText;
    public GameObject LogRectWithImg;
    public GameObject logCloseButton;
    public GameObject aPageButton;
    public Transform logCardScrollController; // Content容器
    public GameObject logCardPrefab; // LogCard预制体
    public ScrollRect scrollRect; // 日志ScrollView

    public GameObject ClickCatchcerLog;
    public GameObject LogBroad;

    private List<GameObject> activeLogCards = new List<GameObject>();
    public void ShowLogPanel()
    {
        AudioManager.Instance.PlaySFX("Log_OpenAndClose");
        LogBroad.SetActive(true);
        RefreshLogs();
    }
    public void hideLogPanel()
    {
        AudioManager.Instance.PlaySFX("Log_OpenAndClose");
        LogBroad.SetActive(false);
    }
    public void hideLogPanelClickCatcher()
    {
        AudioManager.Instance.PlaySFX("Log_OpenAndClose");
        LogBroad.SetActive(false);
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