using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class EventStoryCardButtonSelected : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    //����ű����ڴ���StoryCard��ť��ѡ�к�ȡ��ѡ��Ч�������ƿ�Ƭ/��ť��ѡ��ʱ�ķŴ󶯻�
    public void OnSelect(BaseEventData eventData)
    {
        //��ʽΪ(XXf��С, XXfʱ��).SetEase(Ease.InOutQuad�˶�����)
        transform.DOScale(1.075f, 0.075f).SetEase(Ease.InOutQuad); // ѡ��ʱ�Ŵ�

    }
    public void OnDeselect(BaseEventData eventData)
    {
        //��ʽΪ(XXf��С, XXfʱ��).SetEase(Ease.InOutQuad�˶�����)
        transform.DOScale(1f, 0.075f);// ȡ��ѡ��ʱ��С��ԭ��С
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject; // ����ѡ�ж���Ϊ��ǰ��ť
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = null; // ���ѡ�ж���
    }

}
