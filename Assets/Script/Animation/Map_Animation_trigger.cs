using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//鼠标移动到地图按钮切换开启子对象，子对象挂在行走动画开始行走
public class Map_Animation_trigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public GameObject map_Walk;
    public void OnSelect(BaseEventData eventData)
    {
        this.GetComponent<Image>().enabled = false;
        map_Walk.SetActive(true);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        map_Walk.SetActive(false);
        this.GetComponent<Image>().enabled = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //设置选中对象为当前按钮
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //清除选中对象
        eventData.selectedObject = null;
    }
}
