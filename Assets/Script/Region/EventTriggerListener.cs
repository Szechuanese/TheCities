using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class EventTriggerListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler//悬停工具处理
{
    public Action onEnter;
    public Action onExit;

    public static EventTriggerListener Get(GameObject obj)
    {
        var listener = obj.GetComponent<EventTriggerListener>();
        if (listener == null) listener = obj.AddComponent<EventTriggerListener>();
        return listener;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onExit?.Invoke();
    }
}
