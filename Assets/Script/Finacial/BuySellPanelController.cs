using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuySellPanelController : MonoBehaviour
{
    //这个脚本包含买卖股票的逻辑。以及可视化文本的语句

    public TMP_Text currentPriceText; //当前价格
    public TMP_InputField quantityInput;// 输入买入或卖出的数量
    public Button buyButton;
    public Button sellButton;
    public TMP_Text holdingText; //持有信息

    public ValueSystem valueSystem;
    public PlayerPortfolio playerPortfolio;

    private RuntimeStockData currentStock;
    public TMP_Text currentMoney;

    public void ShowStock(RuntimeStockData stock)
    {
        currentStock = stock;
        currentPriceText.text = $"当前价格：{stock.currentPrice:F2}";
        currentMoney.text = $"当前资金：${valueSystem.GetValue("money"):F2}";

        float held = playerPortfolio.GetHoldingQuantity(stock.stockId);
        float cost = playerPortfolio.GetHoldingCost(stock.stockId);
        holdingText.text = $"持有：{held} 股 @ 均价 ${cost:F2}";
    }

    void Start()
    {
        buyButton.onClick.AddListener(() =>
        {
            if (currentStock == null) return;

            if (float.TryParse(quantityInput.text, out float qty) && qty > 0)
            {
                float cost = currentStock.currentPrice * qty;
                float money = valueSystem.GetValue("money");

                if (money >= cost)
                {
                    valueSystem.ModifyValue("money", -cost);
                    playerPortfolio.Buy(currentStock.stockId, qty, currentStock.currentPrice);
                    ShowStock(currentStock);
                }
                else
                {
                    Debug.Log("❌ 资金不足");
                }
            }
        });

        sellButton.onClick.AddListener(() =>
        {
            if (currentStock == null) return;

            if (float.TryParse(quantityInput.text, out float qty) && qty > 0)
            {
                float owned = playerPortfolio.GetHoldingQuantity(currentStock.stockId);
                if (owned >= qty)
                {
                    float gain = currentStock.currentPrice * qty;
                    valueSystem.ModifyValue("money", gain);
                    playerPortfolio.Sell(currentStock.stockId, qty);
                    ShowStock(currentStock);
                }
                else
                {
                    Debug.Log("❌ 持股不足");
                }
            }
        });
    }
}