using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public GameObject storyPanel;
    public GameObject filePanel;
    public GameObject authorizPanel;
    public GameObject burdenPanel;

    public List<TabButtonController> tabButtons; // 所有 Tab 按钮控制器
    private TabButtonController currentTab;       // 当前激活的按钮

    void Start()
    {
        // 默认显示 Story 页面
        ShowStoryPanel(tabButtons[0]);
    }

    public ScrollRect storyScrollRect;
    public ScrollRect fileScrollRect;
    public ScrollRect authorizScrollRect;
    public ScrollRect burdenScrollRect;

    public void ShowStoryPanel(TabButtonController sender)
    {
        storyPanel.SetActive(true);
        filePanel.SetActive(false);
        authorizPanel.SetActive(false);
        burdenPanel.SetActive(false);

        // ✅ 重置ScrollBar
        if (storyScrollRect != null)
            storyScrollRect.verticalNormalizedPosition = 1f;

        SetActiveTab(sender);
    }

    public void ShowFilePanel(TabButtonController sender)
    {
        storyPanel.SetActive(false);
        filePanel.SetActive(true);
        authorizPanel.SetActive(false);
        burdenPanel.SetActive(false);

        if (fileScrollRect != null)
            fileScrollRect.verticalNormalizedPosition = 1f;

        SetActiveTab(sender);
    }

    public void ShowAuthorizPanel(TabButtonController sender)
    {
        storyPanel.SetActive(false);
        filePanel.SetActive(false);
        authorizPanel.SetActive(true);
        burdenPanel.SetActive(false);

        if (authorizScrollRect != null)
            authorizScrollRect.verticalNormalizedPosition = 1f;

        SetActiveTab(sender);
    }

    public void ShowBurdenPanel(TabButtonController sender)
    {
        storyPanel.SetActive(false);
        filePanel.SetActive(false);
        authorizPanel.SetActive(false);
        burdenPanel.SetActive(true);

        if (burdenScrollRect != null)
            burdenScrollRect.verticalNormalizedPosition = 1f;

        SetActiveTab(sender);
    }

    void SetActiveTab(TabButtonController sender)
    {
        // 恢复上一个按钮样式
        if (currentTab != null && currentTab != sender)
            currentTab.SetInactiveStyle();

        currentTab = sender;
        currentTab.SetActiveStyle();
    }
}