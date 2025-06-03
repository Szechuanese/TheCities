using System.Collections.Generic;

[System.Serializable]
public class RuntimeStockData
{
    public string stockId;
    public float currentPrice;
    public List<float> historicalPrices = new List<float>();

    public RuntimeStockData(string stockId, float initialPrice)
    {
        this.stockId = stockId;
        this.currentPrice = initialPrice;
        this.historicalPrices = new List<float> { initialPrice };
    }
}
