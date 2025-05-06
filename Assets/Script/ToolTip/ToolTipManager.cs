using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager instance;

    [Header("UI")]
    public RectTransform tooltipTransform;
    public TMP_Text tooltipText;
    public CanvasGroup canvasGroup;

    [Header("数据")]
    public List<TooltipEntry> entries;

    private Dictionary<string, string> tooltipDict = new Dictionary<string, string>();

    private void Awake()
    {
        instance = this;

        foreach (var entry in entries)
        {
            if (!tooltipDict.ContainsKey(entry.id))
                tooltipDict.Add(entry.id, entry.description);
        }

        Hide();
    }

    public void ShowById(string id)
    {
        Debug.Log($"✨ 试图显示 ID：{id}");
        if (tooltipDict.TryGetValue(id, out string desc))
        {
            tooltipText.text = desc;
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(true);
        }
        else 
        {
            Debug.LogWarning($"⚠️ 未找到 ID：{id} 的描述！");
        }
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (canvasGroup.alpha <= 0f) return;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            Input.mousePosition,
            null,
            out pos);
        tooltipTransform.anchoredPosition = pos + new Vector2(200f, -120f);
    }
    public static string GetDescriptionById(string id)
    {
        if (instance == null) return "";
        if (instance.tooltipDict.TryGetValue(id, out string desc))
            return desc;
        return "(无描述)";
    }
}

[System.Serializable]
public class TooltipEntry
{
    public string id; // 如 "trait_perception", "char_sanity"
    [TextArea]
    public string description;
}