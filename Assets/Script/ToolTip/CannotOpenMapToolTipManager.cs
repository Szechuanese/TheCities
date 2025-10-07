using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannotOpenMapToolTipManager : MonoBehaviour
{
    public static CannotOpenMapToolTipManager Instance;
    public GameObject cannotOpenMapToolTip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Update()
    {
        // ToolTip 跟随鼠标
        if (cannotOpenMapToolTip != null && cannotOpenMapToolTip.activeSelf)
        {
            RectTransform canvasRect = cannotOpenMapToolTip.transform.parent as RectTransform;
            RectTransform tooltipRect = cannotOpenMapToolTip.GetComponent<RectTransform>();

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                Input.mousePosition,
                null,
                out localPoint);
            //可调整偏移
            tooltipRect.anchoredPosition = localPoint + new Vector2(0, -100f);
        }
    }

    public void ShowToolTip()
    {
        if (cannotOpenMapToolTip != null)
            cannotOpenMapToolTip.SetActive(true);
    }
    public void HideToolTip()
    {
        if (cannotOpenMapToolTip != null)
            cannotOpenMapToolTip.SetActive(false);
    }

}
