using UnityEngine;
using DG.Tweening;
//我第一个使用Dotween的脚本，我觉得十分有纪念意义
//丢拉近拉远的动画
public class MapInteractionController : MonoBehaviour
{//地图动画
    public static MapInteractionController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public static MapInteractionController GetOrCreateInstance()
    {
        if (Instance == null)
            Instance = FindObjectOfType<MapInteractionController>();

        return Instance;
    }

    [Header("地图设置")]
    public RectTransform mapZoomContainerTransform;
    public RectTransform mapContentTransform;

    [Header("缩放设置")]
    public float zoomSpeed = 0.5f;
    public float minZoom = 1.0f;
    public float maxZoom = 3.0f;

    [Header("拖动设置")]
    public float dragSpeed = 1.0f;

    private Vector3 lastMousePosition;
    private static bool draggingOrZooming = false;

    public static bool IsDraggingOrZooming() => draggingOrZooming;

    void Update()
    {
        draggingOrZooming = HandleZoom() || HandleDrag();
    }

    bool HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            Vector3 scale = mapZoomContainerTransform.localScale;
            float newScale = Mathf.Clamp(scale.x + scroll * zoomSpeed, minZoom, maxZoom);
            mapZoomContainerTransform.localScale = new Vector3(newScale, newScale, 1f);
            ClampMapPosition();
            return true;
        }
        return false;
    }

    bool HandleDrag()
    {
        if (Input.GetMouseButtonDown(0))
            lastMousePosition = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            mapZoomContainerTransform.position += delta * dragSpeed;
            lastMousePosition = Input.mousePosition;
            ClampMapPosition();
            return true;
        }
        return false;
    }

    void ClampMapPosition()
    {
        Rect contentRect = GetWorldRect(mapContentTransform);
        Rect zoomRect = GetWorldRect(mapZoomContainerTransform);
        Vector3 offset = Vector3.zero;

        if (zoomRect.width <= contentRect.width)
            offset.x = contentRect.center.x - zoomRect.center.x;
        else
        {
            if (zoomRect.xMin > contentRect.xMin) offset.x = contentRect.xMin - zoomRect.xMin;
            if (zoomRect.xMax < contentRect.xMax) offset.x = contentRect.xMax - zoomRect.xMax;
        }

        if (zoomRect.height <= contentRect.height)
            offset.y = contentRect.center.y - zoomRect.center.y;
        else
        {
            if (zoomRect.yMin > contentRect.yMin) offset.y = contentRect.yMin - zoomRect.yMin;
            if (zoomRect.yMax < contentRect.yMax) offset.y = contentRect.yMax - zoomRect.yMax;
        }

        mapZoomContainerTransform.position += offset;
    }

    Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        return new Rect(corners[0].x, corners[0].y,
                        corners[2].x - corners[0].x,
                        corners[2].y - corners[0].y);
    }

    public void MoveToPosition(Vector3 targetPosition, float targetScale = 2.5f, float duration = 0.5f)
    {
        if (mapZoomContainerTransform != null)
        {
            //同时放大和位移
            mapZoomContainerTransform.DOScale(new Vector3(targetScale, targetScale, 10f), duration);
            mapZoomContainerTransform.DOMove(targetPosition, duration);
        }
        else
        {
            Debug.LogWarning("❗ mapZoomContainerTransform 未绑定！");
        }
    }
    public void FocusOnButton(RectTransform targetButton,
                          float baseScale = 2.0f,
                          float duration = 0.5f)
    {
        if (mapZoomContainerTransform == null || mapContentTransform == null || targetButton == null)
        {
            Debug.LogWarning("FocusOnButton参数未绑定完整");
            return;
        }

        // 1）计算按钮在地图本地坐标下的“靠边程度”
        //    按钮越靠近 mapZoomContainer 的边缘，edgeFactor 越接近 1
        Vector2 localPosInMap = mapZoomContainerTransform.InverseTransformPoint(targetButton.position);
        float halfW = mapZoomContainerTransform.rect.width * 0.5f;
        float halfH = mapZoomContainerTransform.rect.height * 0.5f;

        //归一化，0 表示在中心，1 表示在边缘
        float edgeX = Mathf.InverseLerp(0f, halfW, Mathf.Abs(localPosInMap.x));
        float edgeY = Mathf.InverseLerp(0f, halfH, Mathf.Abs(localPosInMap.y));
        float edgeFactor = Mathf.Clamp01(Mathf.Max(edgeX, edgeY));

        // 2）根据靠边程度，决定目标缩放倍数
        //    靠中心：接近 baseScale；靠边：接近 maxZoom
        float targetScale = Mathf.Lerp(baseScale, maxZoom, edgeFactor);

        // 3）在“目标缩放”下计算真正的目标位置
        Vector3 originalScale = mapZoomContainerTransform.localScale;
        Vector3 originalPos = mapZoomContainerTransform.position;

        // 先临时应用缩放，利用变换矩阵正确换算世界坐标
        mapZoomContainerTransform.localScale = new Vector3(targetScale, targetScale, 1f);

        // 视口中心（MapViewportContainer 的几何中心 → 世界坐标）
        Vector3 viewportCenterWorld =
            mapContentTransform.TransformPoint(mapContentTransform.rect.center);

        //按钮的几何中心（不是 pivot，而是 rect.center → 世界坐标）
        Vector3 buttonCenterWorld =
            targetButton.TransformPoint(targetButton.rect.center);

        //需要把按钮中心搬到视口中心 → offset
        Vector3 offset = viewportCenterWorld - buttonCenterWorld;
        Vector3 targetPos = mapZoomContainerTransform.position + offset;

        //把临时改动恢复回来，真正的动画交给 DOTween
        mapZoomContainerTransform.localScale = originalScale;
        mapZoomContainerTransform.position = originalPos;

        //4）播放缩放 + 位移动画
        mapZoomContainerTransform
            .DOScale(new Vector3(targetScale, targetScale, 1f), duration);

        mapZoomContainerTransform
            .DOMove(targetPos, duration)
            .OnComplete(() =>
            {
                // 动画结束后再夹一次位置，防止极端情况把地图“拉出边界”
                ClampMapPosition();
            });

        Debug.Log($"FocusOnButton -> edgeFactor={edgeFactor:F2}, targetScale={targetScale:F2}");
    }
    }
