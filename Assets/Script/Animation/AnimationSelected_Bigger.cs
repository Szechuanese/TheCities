using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

    //这个脚本用于处理StoryCard按钮的选中和取消选中效果，控制卡片/按钮的选中时的放大动画
public class AnimationSelected_Bigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        //格式为(XXf大小, XXf时间).SetEase(Ease.InOutQuad运动曲线)
        transform.DOScale(1.075f, 0.075f).SetEase(Ease.InOutQuad); // 选中时放大

    }
    public void OnDeselect(BaseEventData eventData)
    {
        //格式为(XXf大小, XXf时间).SetEase(Ease.InOutQuad运动曲线)
        transform.DOScale(1f, 0.075f);//取消选中时缩小回原大小
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
