using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
//股东提示悬浮窗管理器
public class ShareholderTooltipManager : MonoBehaviour
{
    public static ShareholderTooltipManager instance;
    public GameObject shareHolderToolTipPanel;
    public TMP_Text shareHolderToolTipTitle;
    public TMP_Text shareHolderToolTipText;


    [Header("偏移设置")]
    private Vector2 offset = new Vector2(150f, -100f); //根据鼠标位置偏移

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    void Update()
    {
        if (shareHolderToolTipPanel != null && shareHolderToolTipPanel.activeSelf)
        {
            UpdateTooltipPosition();
        }
    }
    private void UpdateTooltipPosition()
    {
        RectTransform canvasRect = shareHolderToolTipPanel.transform.parent as RectTransform;
        RectTransform tooltipRect = shareHolderToolTipPanel.GetComponent<RectTransform>();

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            null,
            out localPoint);

        // 防止 tooltip 超出屏幕边界
        Vector2 targetPosition = localPoint + offset;

        // 简单的边界检查
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;
        float tooltipWidth = tooltipRect.rect.width;
        float tooltipHeight = tooltipRect.rect.height;

        // 调整 X 坐标，防止超出右边界
        //if (targetPosition.x + tooltipWidth > canvasWidth / 2)
        //{
        //    targetPosition.x = localPoint.x - offset.x - tooltipWidth;
        //}

        //// 调整 Y 坐标，防止超出下边界
        //if (targetPosition.y - tooltipHeight < -canvasHeight / 2)
        //{
        //    targetPosition.y = localPoint.y - offset.y + tooltipHeight;
        //}

        tooltipRect.anchoredPosition = targetPosition;
    }

    public void ShowTooltip(string title, string description)
    {
        StartCoroutine(RefreshLayoutNextFrame());
        //Debug.Log("ShowTooltip 被触发：{title}");

        if (shareHolderToolTipTitle != null)
            shareHolderToolTipTitle.text = title;

        if (shareHolderToolTipText != null)
            shareHolderToolTipText.text = description;

        if (shareHolderToolTipPanel != null)
        {
            shareHolderToolTipPanel.SetActive(true);
            // 确保 tooltip 在激活后立即更新位置
            UpdateTooltipPosition();
        }
        StartCoroutine(RefreshLayoutNextFrame());
    }

    public void HideTooltip()
    {
        //Debug.Log("HideTooltip 被触发");
        shareHolderToolTipPanel?.SetActive(false);
    }

    private IEnumerator RefreshLayoutNextFrame()
    {
        yield return null;

        //布局刷新方法
        LayoutRebuilder.ForceRebuildLayoutImmediate(shareHolderToolTipPanel.GetComponent<RectTransform>());
    }
}