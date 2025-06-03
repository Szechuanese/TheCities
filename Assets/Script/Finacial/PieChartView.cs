using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PieChartView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image piePrefab;
    public RectTransform chartArea;
    public int maxSlices = 8;

    private List<Image> slices = new List<Image>();
    private Dictionary<string, float> currentShareholders;
    private bool isMouseOver = false;

    void Awake()
    {
        //预生成固定数量的切片
        for (int i = 0; i < maxSlices; i++)
        {
            var obj = Instantiate(piePrefab, chartArea);
            obj.gameObject.SetActive(false); //初始隐藏
            slices.Add(obj);
        }
    }

    public void DrawPie(Dictionary<string, float> shareholders)
    {
        Debug.Log($"📊 PieChart 更新，股东数 = {shareholders?.Count}");

        currentShareholders = shareholders; //保存当前股东数据

        if (shareholders == null || shareholders.Count == 0)
        {
            //隐藏所有切片
            for (int i = 0; i < slices.Count; i++)
            {
                slices[i].gameObject.SetActive(false);
            }
            return;
        }

        float total = 0f;
        foreach (var kv in shareholders)
            total += kv.Value;

        if (total <= 0f) return;

        float fillAmountSoFar = 0f;
        int sliceIndex = 0;

        foreach (var kv in shareholders)
        {
            if (sliceIndex >= maxSlices) break;

            float ratio = kv.Value / total;
            if (ratio <= 0f) continue;

            var slice = slices[sliceIndex++];
            slice.gameObject.SetActive(true);

            slice.type = Image.Type.Filled;
            slice.fillMethod = Image.FillMethod.Radial360;
            slice.fillOrigin = 0;
            slice.fillAmount = ratio;
            slice.fillClockwise = true;
            slice.color = GetColorByName(kv.Key);

            var rt = slice.rectTransform;
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.localRotation = Quaternion.Euler(0f, 0f, -fillAmountSoFar * 360f);

            fillAmountSoFar += ratio;
        }

        //隐藏多余的切片
        for (int i = sliceIndex; i < slices.Count; i++)
        {
            slices[i].gameObject.SetActive(false);
        }
    }

    //鼠标进入圆盘区域
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        ShowTooltip();
        Debug.Log("🎯 鼠标进入圆盘图区域");
    }

    // 鼠标离开圆盘区域
    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        HideTooltip();
        Debug.Log("🎯 鼠标离开圆盘图区域");
    }

    private void ShowTooltip()
    {
        if (currentShareholders == null || currentShareholders.Count == 0)
            return;

        float total = 0f;
        foreach (var kv in currentShareholders)
            total += kv.Value;

        if (total <= 0f) return;

        string tooltipSummary = "";
        foreach (var kv in currentShareholders)
        {
            float ratio = kv.Value / total;
            tooltipSummary += $"{kv.Key}：{ratio * 100f:F1}%\n";
        }

        ShareholderTooltipManager.instance?.ShowTooltip("股东列表", tooltipSummary.TrimEnd());
    }

    private void HideTooltip()
    {
        ShareholderTooltipManager.instance?.HideTooltip();
    }

    private Color GetColorByName(string id)
    {
        switch (id)
        {
            //case "X": return Color.magenta; //玩家
            case "cedar_corp":
            case "雪松公司": return new Color(0.4f, 0.8f, 0.4f); //灰绿色
            case "north_shipping_corp":
            case "北方航运": return new Color(0.4f, 0.4f, 0.8f); //暗蓝色
            case "north_industry_corp":
            case "北方工业": return Color.blue;
            case "hive_chemical_corp":
            case "蜂巢日化": return Color.yellow;
            case "city_security_corp":
            case "城市安全": return Color.black;
            case "united_food_corp":
            case "联合食品": return new Color(0.6f, 0.3f, 0.8f); //暗紫色
            case "refining_materials_corp":
            case "精炼材料": return new Color(0.8f, 0.4f, 0.4f); //赭石
            case "public_shareholder":
            case "大众股东": return Color.white;
            case "security_committee":
            case "治安委员会": return Color.black;
            case "always_asking_relief_society":
            case "常诘救济会": return Color.red;
            case "shipping_committee":
            case "航运委员会": return new Color(0.4f, 0.4f, 0.8f);
            default: return Color.gray;
        }
    }
}

