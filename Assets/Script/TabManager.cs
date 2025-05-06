using UnityEngine;
using System.Collections.Generic;

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

    public void ShowStoryPanel(TabButtonController sender)
    {
        storyPanel.SetActive(true);
        filePanel.SetActive(false);
        authorizPanel.SetActive(false);
        burdenPanel.SetActive(false);
        SetActiveTab(sender);
    }

    public void ShowFilePanel(TabButtonController sender)
    {
        storyPanel.SetActive(false);
        filePanel.SetActive(true);
        authorizPanel.SetActive(false);
        burdenPanel.SetActive(false);
        SetActiveTab(sender);
    }

    public void ShowAuthorizPanel(TabButtonController sender)
    {
        storyPanel.SetActive(false);
        filePanel.SetActive(false);
        authorizPanel.SetActive(true);
        burdenPanel.SetActive(false);
        SetActiveTab(sender);
    }

    public void ShowBurdenPanel(TabButtonController sender)
    {
        storyPanel.SetActive(false);
        filePanel.SetActive(false);
        authorizPanel.SetActive(false);
        burdenPanel.SetActive(true);
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