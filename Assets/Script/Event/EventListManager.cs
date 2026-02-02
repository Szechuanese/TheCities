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
            //如果目前事件为空，或者事件ID与当前事件ID相同，则跳过
            if (eventManager.CurrentEvent != null && e.eventId == eventManager.CurrentEvent.eventId)
                continue;
            //如果事件挂有 singleUse 标签，且事件已被触发，则跳过
            if (e.singleUse && eventManager.HasTriggered(e.eventId))
                continue;
            GameObject card = Instantiate(cardPrefab, cardContainer);
            var controller = card.GetComponent<EventCardController>();
            controller.LoadEvent(e, eventManager);

        }
    }
}