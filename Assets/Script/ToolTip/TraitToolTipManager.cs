using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
//Trait悬浮提示管理器,位于时钟左边。
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

    [Header("需求组件")]
    public GameObject traitToolTipPanel;
    public TMP_Text traitToolTipDescription;
    public TMP_Text traitToolTipHeader;
    public TMP_Text numerical_value;

    [Header("进度条相关")]
    public Image progressBar; 
    private Coroutine fillRoutine;
    public float duration = 1f;

    [System.Serializable]
    public class TooltipEntry
    {
        public string id;
        [TextArea]
        public string header;
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

            tooltipRect.anchoredPosition = localPoint + new Vector2(100f, -120f); //鼠标相对位置偏移
        }
    }

    public void ShowById(string id)
    {
        //Debug.Log($"✨ 试图显示 ID：{id}");
        //Debug.Log($"TooltipManager 激活了 tooltipPanel: {traitToolTipPanel != null}");

        var entry = entries.Find(e => e.id == id);
        if (entry != null)
        {
            //标题和描述
            traitToolTipHeader.text = entry.header;
            traitToolTipDescription.text = entry.description;
            //显示动态数值
            var vs = FindObjectOfType<ValueSystem>();
            float currentVal = vs != null ? vs.GetValue(id) : 0f;
            numerical_value.text = Mathf.RoundToInt(currentVal).ToString();

            traitToolTipPanel?.SetActive(true);
            //启动进度条协程
            if (fillRoutine != null) StopCoroutine(fillRoutine);
            fillRoutine = StartCoroutine(FillProgressBar(duration));

            //恢复CanvasGroup逻辑
            var canvasGroup = traitToolTipPanel.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.blocksRaycasts = false;
            }
        }
        else
        {
            traitToolTipDescription.text = $"未知ID：{id}";
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
        //关闭进度条
        if (fillRoutine != null) StopCoroutine(fillRoutine);
        if (progressBar != null) progressBar.fillAmount = 0f;
    }
    //进度条协程
    private IEnumerator FillProgressBar(float duration)
    {
        if (progressBar == null) yield break;

        progressBar.fillAmount = 0f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            progressBar.fillAmount = Mathf.Clamp01(t / duration);
            yield return null;
        }

        progressBar.fillAmount = 1f;
    }
}