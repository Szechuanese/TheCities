using UnityEngine;
using System.Collections.Generic;

public class StockMarketManager : MonoBehaviour
{
    //股票交易管理器
    public StockDataManager stockDataManager;

    public float updateIntervalCanvasOpen = 30f; //刷新事件按
    public float updateIntervalCanvasClosed = 300f;//刷新时间非内部

    private float lastUpdateTime;  //最后更新时间
    private bool isCanvasOpen = false;

    void Update()
    {
        float interval = isCanvasOpen ? updateIntervalCanvasOpen : updateIntervalCanvasClosed;
        if (Time.realtimeSinceStartup - lastUpdateTime > interval)
        {
            ApplyFluctuations();
            lastUpdateTime = Time.realtimeSinceStartup;
        }
    }

    public void SetCanvasOpen(bool open)
    {
        isCanvasOpen = open;
    }

    private void ApplyFluctuations()
    {
        //涨跌逻辑
        var stockTemplates = stockDataManager.stockTemplates;
        var runtimeStocks = stockDataManager.GetAllStockData();

        for (int i = 0; i < runtimeStocks.Count; i++)
        {
            var template = stockTemplates[i];
            var stock = runtimeStocks[i];

            float direction = Random.value < template.dropBias ? -1f : 1f;//涨跌方向
            float delta = stock.currentPrice * Random.Range(0.01f, template.volatility);//波动范围
            float newPrice = Mathf.Max(1f, stock.currentPrice + direction * delta); //不能超过前高。

            // 限制反弹不能超过前一跌幅前价格
            if (direction > 0 && stock.historicalPrices.Count > 0)
            {
                float lastDropPrice = stock.historicalPrices[stock.historicalPrices.Count - 1];
                newPrice = Mathf.Min(newPrice, lastDropPrice);
            }

            stock.currentPrice = Mathf.Round(newPrice * 100f) / 100f;//保留两位小数


            stock.historicalPrices.Add(stock.currentPrice);
                if (stock.historicalPrices.Count > 720)
                    stock.historicalPrices.RemoveAt(0);
            

            Debug.Log($"📉 [波动] {template.displayName} 当前价格：{stock.currentPrice}");
        }
    }
}
