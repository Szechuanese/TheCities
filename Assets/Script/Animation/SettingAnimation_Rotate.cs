using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

// 这个脚本用于处理“设置按钮”的选中旋转效果，只旋转指定的目标 RectTransform
public class SettingAnimation_Rotate : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [Header("组件")]
    public RectTransform upperGear;
    public RectTransform lowerGear;

    [Header("旋转圈数")]
    public float upperGearRotateLoops = 0.2f;
    public float lowerGearRotateLoops = 1f;

    [Header("完成一整个旋转的时间")]
    public float upperDuration = 7f;
    public float lowerDuration = 10f;

    private Tween upperRotateTween;
    private Tween lowerRotateTween;
    private Vector3 originalRotationUpperGear;
    private Vector3 originalRotationLowerGear;

    private void Awake()
    {
        if (upperGear == null)
            upperGear = transform as RectTransform;   //没拖就默认自己

        originalRotationUpperGear = upperGear.localEulerAngles;
        originalRotationLowerGear = lowerGear.localEulerAngles;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (upperGear == null||lowerGear==null) return;
        //停止之前的旋转动画
        upperRotateTween?.Kill();
        lowerRotateTween?.Kill();

        //时间*旋转圈数=总旋转角度
        float upperGearTotalAngle = 360f * upperGearRotateLoops;
        float lowerGearTotalAngle = -360f * lowerGearRotateLoops;

        upperRotateTween = upperGear
            .DOLocalRotate(originalRotationUpperGear + new Vector3(0, 0, upperGearTotalAngle), upperDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
        lowerRotateTween = lowerGear
            .DOLocalRotate(originalRotationLowerGear + new Vector3(0, 0, lowerGearTotalAngle), lowerDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (upperGear == null||lowerGear==null) return;

        upperRotateTween?.Kill();
        lowerRotateTween?.Kill();

        //回正
        upperGear
            .DOLocalRotate(originalRotationUpperGear, 0.2f)
            .SetEase(Ease.OutQuad);
        lowerGear
            .DOLocalRotate(originalRotationLowerGear, 0.7f)
            .SetEase(Ease.OutQuad);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = null;
    }

}
