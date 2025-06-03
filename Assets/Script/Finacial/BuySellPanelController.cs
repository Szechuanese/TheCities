using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuySellPanelController : MonoBehaviour
{
    public TMP_Text currentPriceText;
    public TMP_InputField quantityInput;
    public Button buyButton;
    public Button sellButton;
    public TMP_Text holdingText;

    public TraitSystem traitSystem;
    public PlayerPortfolio playerPortfolio;

    private RuntimeStockData currentStock;

    public void ShowStock(RuntimeStockData stock)
    {
        currentStock = stock;
        currentPriceText.text = $"当前价格：{stock.currentPrice:F2}";

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
                float money = traitSystem.GetTrait("money");

                if (money >= cost)
                {
                    traitSystem.ModifyTrait("money", -cost);
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
                    traitSystem.ModifyTrait("money", gain);
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