using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class BarChartView : MonoBehaviour
{
    public RectTransform chartArea;
    public GameObject barPrefab;
    public float barSpacing = 2f;

    private List<RectTransform> barPool = new List<RectTransform>();
    private int maxBars = 60;

    public RectTransform yAxisContainer;  // UI容器（垂直方向）
    public GameObject yLabelPrefab;       // 标签预制体
    public int yAxisSteps = 5;            // 分为几段
    private List<GameObject> yLabelPool = new List<GameObject>();

    public float waveStepDelay = 0.02f;//波浪延迟

    public void DrawChart(List<float> prices)
    {
        if (prices == null || prices.Count < 1) return;

        float width = chartArea.rect.width;
        float height = chartArea.rect.height;

        float min = Mathf.Min(prices.ToArray());
        float max = Mathf.Max(prices.ToArray());

        int count = Mathf.Min(prices.Count, maxBars);
        int startIndex = Mathf.Max(0, prices.Count - count);

        EnsureBarPool(count); // 确保有足够柱子

        float barWidth = (width - barSpacing * count) / count;

        for (int i = 0; i < count; i++)
        {
            float value = prices[startIndex + i];
            float normalizedHeight = Mathf.InverseLerp(min, max, value);
            float barHeight = normalizedHeight * height;

            RectTransform bar = barPool[i];
            bar.gameObject.SetActive(true);
            bar.sizeDelta = new Vector2(barWidth, 0f); //初始高度为零，方便拉伸
            bar.anchoredPosition = new Vector2(i * (barWidth + barSpacing), 0);

            //开始动画拉伸
            float delay = i * 0.02f;
            StartCoroutine(AnimateBarHeight(bar, 0f, barHeight, 0.3f, delay));

            //柱子颜色逻辑
            Image barImage = bar.GetComponent<Image>();
            if (i == 0)
            {
                barImage.color = new Color(0.831f, 0.216f, 0.435f); //首根柱子默认色
            }
            else
            {
                float prevValue = prices[startIndex + i - 1];
                if (value > prevValue)
                    barImage.color = new Color(0.831f, 0.216f, 0.435f);//涨幅用默认色
                else if (value < prevValue)
                    barImage.color = new Color(0.4f, 0.9f, 0.7f);//跌幅柔绿色
                else
                    barImage.color = new Color(0.831f, 0.216f, 0.435f); // 不变时默认
            }

            // 添加价格提示触发器
            var trigger = bar.GetComponent<PriceTooltipTrigger>();
            if (trigger == null)
                trigger = bar.gameObject.AddComponent<PriceTooltipTrigger>();
            trigger.price = value;
        }

        // 隐藏多余的柱子
        for (int i = count; i < barPool.Count; i++)
            barPool[i].gameObject.SetActive(false);

        //添加坐标轴绘制
        DrawYAxisLabels(min, max);
    }

    private IEnumerator AnimateBarHeight(RectTransform bar, float startHeight, float endHeight, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);// 等待波浪延迟

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float height = Mathf.Lerp(startHeight, endHeight, t);
            bar.sizeDelta = new Vector2(bar.sizeDelta.x, height);
            yield return null;
        }
        // 确保最后精确到目标高度
        bar.sizeDelta = new Vector2(bar.sizeDelta.x, endHeight);
    }


    private void EnsureBarPool(int count)
    {
        while (barPool.Count < count)
        {
            GameObject bar = Instantiate(barPrefab, chartArea);
            RectTransform rt = bar.GetComponent<RectTransform>();
            barPool.Add(rt);
        }
    }
    private void DrawYAxisLabels(float min, float max)
    {
        float height = chartArea.rect.height;
        EnsureYLabelPool();

        for (int i = 0; i < yAxisSteps; i++)
        {
            float t = (float)i / (yAxisSteps - 1); // 0 → 1
            float value = Mathf.Lerp(min, max, t);
            float y = t * height;

            var labelObj = yLabelPool[i];
            labelObj.SetActive(true);

            var rt = labelObj.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, y); //左侧对齐

            var text = labelObj.GetComponent<TMPro.TMP_Text>();
            text.text = $"{value:F0}";
        }

        for (int i = yAxisSteps; i < yLabelPool.Count; i++)
            yLabelPool[i].SetActive(false);
    }

    private void EnsureYLabelPool()
    {
        while (yLabelPool.Count < yAxisSteps)
        {
            var obj = Instantiate(yLabelPrefab, yAxisContainer);
            yLabelPool.Add(obj);
        }
    }
}