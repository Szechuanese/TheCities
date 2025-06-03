using UnityEngine;
using DG.Tweening;
//我第一个使用Dotween的脚本，我觉得十分有纪念意义
public class MapInteractionController : MonoBehaviour
{
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
}
