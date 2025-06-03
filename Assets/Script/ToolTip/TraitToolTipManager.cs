using System.Collections.Generic;
using UnityEngine;
using TMPro;
//特质悬浮提示管理器
public class TraitToolTipManager : MonoBehaviour
{
    public static TraitToolTipManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    [Header("Tooltip设置")]
    public GameObject traitToolTipPanel;
    public TMP_Text traitToolTipText;

    [System.Serializable]
    public class TooltipEntry
    {
        public string id;
        [TextArea]
        public string description;
    }

    [Header("Tooltip数据")]
    public List<TooltipEntry> entries = new List<TooltipEntry>();


    private void Update()
    {
        if (traitToolTipPanel != null && traitToolTipPanel.activeSelf)
        {
            RectTransform canvasRect = traitToolTipPanel.transform.parent as RectTransform;
            RectTransform tooltipRect = traitToolTipPanel.GetComponent<RectTransform>();

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                Input.mousePosition,
                null,
                out localPoint);

            tooltipRect.anchoredPosition = localPoint + new Vector2(200f, -120f); //鼠标相对位置偏移
        }
    }

    public void ShowById(string id)
    {
        //Debug.Log($"✨ 试图显示 ID：{id}");
        //Debug.Log($"TooltipManager 激活了 tooltipPanel: {traitToolTipPanel != null}");

        var entry = entries.Find(e => e.id == id);
        if (entry != null)
        {
            traitToolTipText.text = entry.description;
            traitToolTipPanel?.SetActive(true);

            //恢复 CanvasGroup逻辑
            var canvasGroup = traitToolTipPanel.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.blocksRaycasts = false;
            }
        }
        else
        {
            traitToolTipText.text = $"未知ID：{id}";
            traitToolTipPanel?.SetActive(true);

            var canvasGroup = traitToolTipPanel.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.blocksRaycasts = false;
            }
        }
    }

    public void HideTooltip()
    {
        traitToolTipPanel?.SetActive(false);
    }
}