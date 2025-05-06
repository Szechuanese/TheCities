using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapManager : MonoBehaviour
{
    [Header("地图区域列表")]
    public List<RegionInfo> regionInfos = new List<RegionInfo>();

    [Header("提示窗口")]
    public GameObject tooltipPanel;
    public TMP_Text tooltipTitle;
    public TMP_Text tooltipDescription;

    [Header("依赖")]
    public EventManager eventManager;
    [Header("地图总面板")]
    public GameObject worldMapPanel; // 拖入 WorldMapPanel 对象

    public RegionPanelManager regionPanelManager; // 拖入组件
    public void ShowMap()
    {
        worldMapPanel.SetActive(true);
    }

    private void Start()
    {
        worldMapPanel.SetActive(false);
        foreach (var region in regionInfos)
        {
            if (region.regionButton == null) continue;

            region.regionButton.interactable = region.isUnlocked;

            region.regionButton.onClick.AddListener(() =>
            {
                if (region.regionData != null)
                {
                    worldMapPanel.SetActive(false);
                    regionPanelManager.ShowRegion(region.regionData); // 跳转地点事件链
                }
            });

            // 悬停提示
            EventTriggerListener.Get(region.regionButton.gameObject).onEnter = () =>
            {
                tooltipPanel.SetActive(true);
                tooltipTitle.text = region.displayName;
                tooltipDescription.text = region.description;
            };

            EventTriggerListener.Get(region.regionButton.gameObject).onExit = () =>
            {
                tooltipPanel.SetActive(false);
            };
        }

        tooltipPanel.SetActive(false);
    }
}
