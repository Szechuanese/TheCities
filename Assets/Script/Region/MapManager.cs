using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class MapManager : MonoBehaviour
{
   //初始化与绑定
    public static MapManager Instance { get; private set; }
    public UnityEngine.UI.Button openMapButton;

    
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
        // 播放地图打开音效
        AudioManager.Instance.PlaySFX("Map_Open"); 

        //切换至地图
        UIManager.Instance.SwitchState(UIManager.UIState.WorldMap);
        //调用动画
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
    //调用无法打开地图时的提示方法——CannotOpenMapToolTipManager.cs;我为什么一开始会把这个方法添加在MapManager里呢。我不明白。
    //我明白了，因为StoryPanel里的按钮是通过MapManager来控制的，所以我把这个方法放在这里。
    public void CanNotShowMap() 
    {
        //透明度降低
        var colors = openMapButton.colors;
        colors.normalColor = new Color(0.5f, 0.5f, 0.5f);
        colors.highlightedColor = new Color(0.5f, 0.5f, 0.5f);
        openMapButton.colors = colors;

        // 注册鼠标事件（只注册一次，避免重复添加）
        var trigger = openMapButton.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = openMapButton.gameObject.AddComponent<EventTrigger>();
        if (trigger.triggers == null)
            trigger.triggers = new List<EventTrigger.Entry>();
        else
            trigger.triggers.Clear();

        // 鼠标移入显示
        var entryEnter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        entryEnter.callback.AddListener((e) => {
            CannotOpenMapToolTipManager.Instance.ShowToolTip();
        });
        trigger.triggers.Add(entryEnter);

        // 鼠标移出隐藏
        var entryExit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        entryExit.callback.AddListener((e) => {
            CannotOpenMapToolTipManager.Instance.HideToolTip();
        });
        trigger.triggers.Add(entryExit);

        //先隐藏
        CannotOpenMapToolTipManager.Instance.HideToolTip();
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