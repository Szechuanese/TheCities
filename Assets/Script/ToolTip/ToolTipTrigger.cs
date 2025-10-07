using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//与 ToolTipSystem 相关的脚本,目前同样没有实现具体功能
public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTipSystem.Show();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipSystem.Hide();
    }
}
