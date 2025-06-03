using UnityEngine;
using TMPro;
using System.Collections.Generic;
//地图按钮对应的提示管理器
public class MapTooltipManager : MonoBehaviour
{
    public static MapTooltipManager instance;
    public GameObject MapToolTipPanel;
    public TMP_Text MapToolTipTitle;      //标题文本
    public TMP_Text MapToolTipText;       //正文

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }


    private Dictionary<string, string> regionDict = new Dictionary<string, string>();

    private void Update()
    {
        if (MapToolTipPanel != null && MapToolTipPanel.activeSelf)
        {
            RectTransform canvasRect = MapToolTipPanel.transform.parent as RectTransform;
            RectTransform tooltipRect = MapToolTipPanel.GetComponent<RectTransform>();

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                Input.mousePosition,
                null,
                out localPoint);

            tooltipRect.anchoredPosition = localPoint + new Vector2(200f, -150f);
        }
    }

    public void RegisterRegion(string id, string description)
    {
        if (!regionDict.ContainsKey(id))
            regionDict.Add(id, description);
    }

    //title
    public void ShowTooltip(string title, string description)
    {
        if (MapToolTipTitle != null) MapToolTipTitle.text = title;
        if (MapToolTipText != null) MapToolTipText.text = description;

        MapToolTipPanel?.SetActive(true);
    }

    public void HideTooltip()
    {
        MapToolTipPanel?.SetActive(false);
    }
}