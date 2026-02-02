using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    [Header("绑定的页面")]
    public GameObject storyPanel;
    public GameObject filePanel;
    public GameObject authorizPanel;
    public GameObject burdenPanel;

    [Header("需求组件")]
    public List<TabButtonController> tabButtons; // 所有 Tab 按钮控制器
    private TabButtonController currentTab;       // 当前激活的按钮


    [Header("各种Rect")]
    public RectTransform fileContentRectTransform;
    public RectTransform authorizContentRectTransform;
    public RectTransform burdenContentRectTransform;

    //出于偷懒我只绑定了一个Scrollbar，authriz和burden还有file界面共用一个滚动条
    [Header("ScrollBar")]
    public Scrollbar filePanelScrollBar;


    void Start()
    {
        //默认显示 Story 页面
        ShowStoryPanel(tabButtons[0]);
    }

    [Header("各种ScrollRect")]
    public ScrollRect storyScrollRect;
    public ScrollRect fileScrollRect;
    public ScrollRect authorizScrollRect;
    public ScrollRect burdenScrollRect;

    //这下面的代码与UImanager联动
    public void ShowStoryPanel(TabButtonController sender)
    {
        AudioManager.Instance.PlaySFX("Tabs_Click"); // 播放点击音效
        storyPanel.SetActive(true);
        filePanel.SetActive(false);
        authorizPanel.SetActive(false);
        burdenPanel.SetActive(false);


        UIManager.Instance.ScrollPanelToTop(storyScrollRect);
        SetActiveTab(sender);
    }

    public void ShowFilePanel(TabButtonController sender)
    {
        AudioManager.Instance.PlaySFX("Tabs_Click");
        storyPanel.SetActive(false);
        filePanel.SetActive(true);
        authorizPanel.SetActive(false);
        burdenPanel.SetActive(false);


        //告诉程序，ScrollRect换content
        fileScrollRect.content = fileContentRectTransform;
        //强制刷新Layout确保高度计算正确
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(fileContentRectTransform);



        //置顶
        UIManager.Instance.ScrollPanelToTop(fileScrollRect);
        filePanelScrollBar.value = 1f;
        SetActiveTab(sender);
    }

    public void ShowAuthorizPanel(TabButtonController sender)
    {
        AudioManager.Instance.PlaySFX("Tabs_Click");
        storyPanel.SetActive(false);
        filePanel.SetActive(false);
        authorizPanel.SetActive(true);
        burdenPanel.SetActive(false);

        //告诉程序，ScrollRect换content
        fileScrollRect.content = authorizContentRectTransform;
        //强制刷新Layout确保高度计算正确
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(authorizContentRectTransform);


        UIManager.Instance.ScrollPanelToTop(fileScrollRect);
        filePanelScrollBar.value = 1f;
        SetActiveTab(sender);
    }

    public void ShowBurdenPanel(TabButtonController sender)
    {
        AudioManager.Instance.PlaySFX("Tabs_Click");
        storyPanel.SetActive(false);
        filePanel.SetActive(false);
        authorizPanel.SetActive(false);
        burdenPanel.SetActive(true);


        //告诉程序，ScrollRect换content
        fileScrollRect.content = burdenContentRectTransform;
        //强制刷新Layout确保高度计算正确
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(burdenContentRectTransform);



        UIManager.Instance.ScrollPanelToTop(fileScrollRect);
        filePanelScrollBar.value = 1f;
        SetActiveTab(sender);
    }

    void SetActiveTab(TabButtonController sender)
    {
        AudioManager.Instance.PlaySFX("Tabs_Click");
        //恢复上一个按钮样式
        if (currentTab != null && currentTab != sender)
            currentTab.SetInactiveStyle();

        currentTab = sender;
        currentTab.SetActiveStyle();
    }
}