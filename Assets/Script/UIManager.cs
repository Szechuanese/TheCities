using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    //UI״̬��������������л���
    public static UIManager Instance { get; private set; }


    [Header("����Canvas��Panel")]
    public GameObject storyPanel;
    public GameObject filePanel;
    public Canvas stockMarketCanvas;
    public GameObject regionPanel;
    public GameObject worldPanel;
    public GameObject settingPanel;
    public GameObject mainCanvas;
    // �ɼ�����������������


    [Header("����ScrollRect")]
    public ScrollRect storyScrollRect;
    public ScrollRect fileScrollRect;
    public ScrollRect authorizScrollRect;
    public ScrollRect burdenScrollRect;

    public enum UIState
    {
        MainMenu,
        //Setting,
        Region,
        Story,
        StockMarket,
        File,
        WorldMap,
        None
    }

    private UIState currentState = UIState.Story;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SwitchState(UIState nextState)
    {
        if (currentState == nextState) return;

        // ͳһ�ر��������
        storyPanel.gameObject.SetActive(false);
        filePanel.SetActive(false);
        stockMarketCanvas.gameObject.SetActive(false);
        regionPanel.SetActive(false);
        worldPanel.SetActive(false);
        settingPanel.gameObject.SetActive(false);
        //���Լ�������������

        // ��Ŀ�����
        switch (nextState)
        {
            case UIState.Story:
                storyPanel.gameObject.SetActive(true);
                break;
            case UIState.Region:
                regionPanel.SetActive(true);
                break;
            case UIState.StockMarket:
                stockMarketCanvas.gameObject.SetActive(true);
                break;
            case UIState.File:
                filePanel.SetActive(true);
                break;
            case UIState.WorldMap:
                worldPanel.SetActive(true);
                regionPanel.SetActive(true);
                break;
            //case UIState.Setting:
            //    settingPanel.gameObject.SetActive(true);
            //    mainCanvas.SetActive(true);
            //    break;
            case UIState.MainMenu:
                // �����˵��߼�
                break;
            case UIState.None:
            default:
                // �������ȫ��
                break;
        }
        currentState = nextState;
    }

    public UIState GetCurrentState()
    {
        return currentState;
    }

    #region �����ö�
    public void ScrollPanelToTop(ScrollRect scrollRect)
    {
        StartCoroutine(ScrollToTopAfterFrame(scrollRect));
    }
    private IEnumerator ScrollToTopAfterFrame(ScrollRect scrollRect)
    {
        yield return null;
        if (scrollRect != null)
            scrollRect.verticalNormalizedPosition = 1f;
    }
    #endregion
}
