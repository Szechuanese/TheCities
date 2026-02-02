using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class MapButtonHover : MonoBehaviour
{
    [Header("依赖")]
    public Canvas canvas;
    public RectTransform targetRectTransform;
    public Image targetImage;

    [HideInInspector] public RegionInfo regionInfo;

    [Header("放大动画设置")]
    public bool enableScaleOnHover = true;
    public float hoverScale = 1.075f;
    public float tweenDuration = 0.075f;
    public Ease tweenEase = Ease.InOutQuad;

    [Header("事件")]
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;

    private bool isHovering = false;

    //记录原始缩放（防止为 0）
    private Vector3 originalScale = Vector3.one;
    private bool hasRecordedScale = false;

    private void Start()
    {
        if (targetRectTransform == null)
            targetRectTransform = GetComponent<RectTransform>();

        if (targetImage == null)
            targetImage = GetComponent<Image>();

        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        //这里先不动 scale，只是准备好引用
        //originalScale 我们等第一次 Hover 再记录
    }

    private void Update()
    {
        if (canvas == null || targetRectTransform == null)
            return;

        bool isInside = RectTransformUtility.RectangleContainsScreenPoint(
            targetRectTransform,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera
        );

        if (isInside && !isHovering)
        {
            isHovering = true;

            //第一次真正用到时，再记录原始 scale，避免 Awake/Start 时是 0 或奇怪数值
            if (!hasRecordedScale)
            {
                originalScale = targetRectTransform.localScale;
                //防止原始值正好是 0 被直接吃没
                if (Mathf.Approximately(originalScale.x, 0f) &&
                    Mathf.Approximately(originalScale.y, 0f))
                {
                    originalScale = Vector3.one;
                }
                hasRecordedScale = true;
            }

            if (regionInfo != null && regionInfo.regionData != null)
            {
                MapTooltipManager.instance.ShowTooltip(
                    regionInfo.displayName,
                    regionInfo.description
                );
            }

            if (enableScaleOnHover)
            {
                targetRectTransform.DOKill();
                targetRectTransform
                    .DOScale(originalScale * hoverScale, tweenDuration)
                    .SetEase(tweenEase);
            }

            onHoverEnter?.Invoke();
        }
        else if (!isInside && isHovering)
        {
            isHovering = false;

            MapTooltipManager.instance?.HideTooltip();

            if (enableScaleOnHover && hasRecordedScale)
            {
                targetRectTransform.DOKill();
                targetRectTransform
                    .DOScale(originalScale, tweenDuration)
                    .SetEase(tweenEase);
            }

            onHoverExit?.Invoke();
        }
    }
}


