using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    [Header("地图区域列表")]
    public List<RegionInfo> regionInfos = new List<RegionInfo>();

    [Header("依赖")]
    public EventManager eventManager;

    [Header("地图总面板")]
    public GameObject worldMapPanel;

    public RegionPanelManager regionPanelManager;

    public void ShowMap()
    {
        UIManager.Instance.SwitchState(UIManager.UIState.WorldMap);

        var controller = MapInteractionController.GetOrCreateInstance();

        if (eventManager.lastRegion != null &&
            eventManager.lastRegion.regionButton != null &&
            controller != null &&
            controller.mapZoomContainerTransform != null)
        {
            RegionInfo targetRegion = eventManager.lastRegion;
            RectTransform targetButton = targetRegion.regionButton.GetComponent<RectTransform>();
            if (targetButton != null)
            {
                Vector3 targetPos = targetButton.position;
                Vector3 mapCenter = worldMapPanel.GetComponent<RectTransform>().position;
                Vector3 offset = mapCenter - targetPos;
                Vector3 targetContainerPos = controller.mapZoomContainerTransform.position + offset;

                controller.MoveToPosition(targetContainerPos);

                Debug.Log($"✅ 地图已聚焦到区域：{targetRegion.regionData.regionDisplayName}");
            }
            else
            {
                Debug.LogWarning("❗ targetRegion.regionButton 无法获取 RectTransform");
            }
        }
        else
        {
            Debug.LogWarning("❗ lastRegion或MapInteractionController未初始化，地图无法聚焦");
        }
    }

    private void Start()
    {
        UIManager.Instance.SwitchState(UIManager.UIState.WorldMap);
        foreach (var region in regionInfos)
        {
            if (region.regionButton == null) continue;

            if (!string.IsNullOrEmpty(region.requiredTraitId))
            {
                float traitValue = eventManager.valueSystem.GetValue(region.requiredTraitId);
                region.isUnlocked = traitValue >= region.requiredTraitValue;
            }

            region.regionButton.interactable = region.isUnlocked;

            MapButtonHover detector = region.regionButton.GetComponent<MapButtonHover>();
            if (detector != null)
            {
                detector.regionInfo = region;

                //同步注册Tooltip
                if (MapTooltipManager.instance != null &&
                    region.regionData != null &&
                    !string.IsNullOrEmpty(region.regionData.regionId))
                {
                    MapTooltipManager.instance.RegisterRegion(region.regionData.regionId, region.regionData.regionDescription);
                }
            }

            region.regionButton.onClick.AddListener(() =>
            {
                if (region.regionData != null)
                {
                    UIManager.Instance.SwitchState(UIManager.UIState.Region);//点击相应区域的按钮切换位置
                    eventManager.regionHistory.Clear();
                    regionPanelManager.ShowRegion(region, disableHistoryPush: true);
                }
            });
        }
    }
}