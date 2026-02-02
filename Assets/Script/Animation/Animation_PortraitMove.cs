using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Animation_PortraitMove : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [Header("移动对象")]
    public RectTransform portrait;
    public RectTransform backGround;

    [Header("移动距离(像素)")]
    public float moveUpDistance = 5f;
    public float moveLeftDistance = 5f;

    public float animTime = 0.12f;

    //记录初始位置
    private Vector2 originalUpPos;
    private Vector2 originalLeftPos;

    private Tween portraitTween;
    private Tween backGroundTween;

    private void Awake()
    {
        if (portrait != null)
            originalUpPos = portrait.anchoredPosition;

        if (backGround != null)
            originalLeftPos = backGround.anchoredPosition;
    }

    //鼠标选中
    private void PlayEnterAnim()
    {
        if (portrait != null)
        {
            portraitTween?.Kill();
            portraitTween = portrait.DOAnchorPos(
                    originalUpPos + new Vector2(0, moveUpDistance),
                    animTime
                ).SetEase(Ease.OutQuad);
        }

        if (backGround != null)
        {
            backGroundTween?.Kill();
            backGroundTween = backGround.DOAnchorPos(
                    originalLeftPos + new Vector2(-moveLeftDistance, 0),
                    animTime
                ).SetEase(Ease.OutQuad);
        }
    }

    //鼠标离开
    private void PlayExitAnim()
    {
        if (portrait != null)
        {
            portraitTween?.Kill();
            portraitTween = portrait.DOAnchorPos(originalUpPos, animTime)
                .SetEase(Ease.OutQuad);
        }

        if (backGround != null)
        {
            backGroundTween?.Kill();
            backGroundTween = backGround.DOAnchorPos(originalLeftPos, animTime)
                .SetEase(Ease.OutQuad);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData != null)
            eventData.selectedObject = gameObject;

        //鼠标移入就移上去，并保持这个状态
        PlayEnterAnim();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData != null && eventData.selectedObject == gameObject)
            eventData.selectedObject = null;

        //鼠标移出才回到原位
        PlayExitAnim();
    }

    //适配手柄
    public void OnSelect(BaseEventData eventData)
    {
        PlayEnterAnim();
    }

    // 取消选中时：这里不再立即复位，让“点击后仍保持悬停效果”
    public void OnDeselect(BaseEventData eventData)
    {
        //如果键盘切换选中时立刻回位，可以在这里调用 PlayExitAnim();
        //对鼠标来说，一般只在 PointerExit 时复位就够了，我暂时没有做键盘和手柄，所以这里不处理
    }
}
