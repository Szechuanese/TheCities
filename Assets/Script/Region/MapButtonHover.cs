using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MapButtonHover : MonoBehaviour
{
    [Header("依赖")]
    public Canvas canvas;
    public RectTransform targetRectTransform;
    public Image targetImage;

    [HideInInspector] public RegionInfo regionInfo;

    [Header("高亮颜色设置")]//按钮高亮，以后可以去除
    public Color normalColor = new Color32(62, 70, 103, 255);
    public Color highlightColor = new Color32(91, 88, 156, 255);

    [Header("事件")]
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;

    private bool isHovering = false;

    private void Start()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        if (targetImage != null)
            targetImage.color = normalColor;
    }

    void Update()
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
            if (targetImage != null)
                targetImage.color = highlightColor;

            //显示地图Tooltip
            if (regionInfo != null && regionInfo.regionData != null)
                MapTooltipManager.instance.ShowTooltip(regionInfo.displayName, regionInfo.description);

            onHoverEnter.Invoke();
        }
        else if (!isInside && isHovering)
        {
            isHovering = false;
            if (targetImage != null)
                targetImage.color = normalColor;

            MapTooltipManager.instance?.HideTooltip();
            onHoverExit.Invoke();
        }
    }
}