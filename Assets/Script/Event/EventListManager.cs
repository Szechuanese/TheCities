using System.Collections.Generic;
using UnityEngine;

public class EventListManager : MonoBehaviour
{
    public Transform cardContainer; // 指向 StoryBroad（放卡片的容器）
    public GameObject cardPrefab;   // 事件卡片预制体（带 EventCardController）
    public EventManager eventManager;

    public List<NarrativeEvent> allEvents; // 所有事件资源

    void Start()
    {
        GenerateCardList();

    }

    public void GenerateCardList()
    {
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var e in allEvents)
        {
            // ✅ 跳过已经被触发的当前事件（避免重复显示）
            if (eventManager.CurrentEvent != null && e.eventId == eventManager.CurrentEvent.eventId)
                continue;

            if (e.singleUse && eventManager.HasTriggered(e.eventId))
                continue;

            GameObject card = Instantiate(cardPrefab, cardContainer);
            var controller = card.GetComponent<EventCardController>();
            controller.LoadEvent(e, eventManager);

        }
    }
}