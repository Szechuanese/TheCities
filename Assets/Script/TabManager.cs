using UnityEngine;
using System.Collections.Generic;

public class TabManager : MonoBehaviour
{
    public GameObject storyPanel;
    public GameObject filePanel;
    public GameObject authorizPanel;
    public GameObject burdenPanel;

    public List<TabButtonController> tabButtons; // ���� Tab ��ť������
    private TabButtonController currentTab;       // ��ǰ����İ�ť

    void Start()
    {
        // Ĭ����ʾ Story ҳ��
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
        // �ָ���һ����ť��ʽ
        if (currentTab != null && currentTab != sender)
            currentTab.SetInactiveStyle();

        currentTab = sender;
        currentTab.SetActiveStyle();
    }
}