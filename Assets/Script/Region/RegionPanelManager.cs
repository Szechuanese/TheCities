// ✅ RegionPanelManager.cs
// 控制点击大地图区域后，显示区域内的事件卡片（StoryCard）列表

using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RegionPanelManager : MonoBehaviour
{
    [Header("UI 组件")]
    public GameObject regionPanel; // 主面板（显示/隐藏）
    public TMP_Text regionTitleText; // 区域名称
    public TMP_Text regionDescriptionText;
    public Transform contentParent; // 卡片容器（RegionContent）
    public GameObject storyCardPrefab; // 复用你已有的 StoryCard
    public EventManager eventManager; // 事件管理器

    [Header("依赖组件")]
    public GameObject worldMapPanel; // 地图面板

    private List<GameObject> currentCards = new List<GameObject>();

    // 调用这个函数来显示区域事件列表
    public void ShowRegion(RegionData regionData)
    {
        if (regionData == null) return;

        // ✅ 隐藏旧事件界面，并回收所有旧卡片
        eventManager.eventUIManager.ClearStoryCards();

        // ✅ 显示当前区域面板
        regionPanel.SetActive(true);
        regionTitleText.text = regionData.regionDisplayName;
        regionDescriptionText.text = regionData.regionDescription;

        // ✅ 清除旧区域卡片
        ClearCards();

        // ✅ 动态生成卡片（不绑定跳转）
        foreach (var e in regionData.regionEvents)
        {
            if (e == null) continue;

            GameObject card = Instantiate(storyCardPrefab, contentParent);
            EventCardController controller = card.GetComponent<EventCardController>();
            if (controller != null)
            {
                controller.eventUIManager = eventManager.eventUIManager;
                controller.LoadEvent(e, eventManager, isPreview: true); // 只用于展示，不触发事件
            }
            currentCards.Add(card);
        }
    }

    // 关闭面板 + 清空卡片
    public void CloseRegionPanel()
    {
        regionPanel.SetActive(false);
        ClearCards();
    }

    private void ClearCards()
    {
        foreach (var card in currentCards)
        {
            Destroy(card);
        }
        currentCards.Clear();
    }

    // 从地图按钮中调用此方法：ShowRegion(regionInfo.regionData);
}

