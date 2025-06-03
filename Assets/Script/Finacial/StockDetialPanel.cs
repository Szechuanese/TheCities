using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StockDetailPanel : MonoBehaviour
{
    //股票信息放在这个脚本里。

    //股东盘子
    public BarChartView chartView;
    public PieChartView pieView;

    private string lastStockId = "";
    private float lastDrawTime = 0f;
    private float minDrawInterval = 0.5f;

    public PlayerPortfolio playerPortfolio;

    //玩家将不会显示在股东列表中
    //[SerializeField, Tooltip("玩家股份视觉权重（倍率）")]
    //private float playerVisualWeight = 100f;
    public TMP_Text marketCapText;  //总市值
    public TMP_Text totalSharesText; //总股本
    public void ShowCompany(RuntimeStockData stock)
    {
        if (stock == null) return;

        if (stock.stockId == lastStockId && Time.realtimeSinceStartup - lastDrawTime < minDrawInterval)
            return;

        lastStockId = stock.stockId;
        lastDrawTime = Time.realtimeSinceStartup;

        StartCoroutine(DrawChartNextFrame(stock.historicalPrices));// 曲线图

        var template = FindObjectOfType<StockDataManager>()?.GetCompanyTemplate(stock.stockId);
        if (template != null)
        {
            // 显示总市值和总股本（单位：亿）
            double capBillion = template.marketCap / 1e8f;
            marketCapText.text = $"总市值：{capBillion:F2}亿";

            double sharesBillion = template.totalShares / 1e8f;
            totalSharesText.text = $"总股本：{sharesBillion:F2}亿股";

            // 合成股东数据
            long totalShares = template.totalShares;
            Dictionary<string, float> shareholderDict = new();

            foreach (var entry in template.defaultShareholders)
            {
                float shares = entry.percentage * totalShares;
                if (shareholderDict.ContainsKey(entry.name))
                    shareholderDict[entry.name] += shares;
                else
                    shareholderDict[entry.name] = shares;
            }

            pieView.DrawPie(shareholderDict);
        }
    }
    private IEnumerator DrawChartNextFrame(List<float> prices)
    {
        yield return null; // 等待一帧，确保 Canvas 正确布局
        chartView.DrawChart(prices);
    }
}
