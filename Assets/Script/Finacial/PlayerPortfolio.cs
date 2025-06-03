using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Holding
{
    public string stockId;
    public float quantity;    // 持股数量
    public float averageCost; // 成本均价（用于盈亏计算）
}

public class PlayerPortfolio : MonoBehaviour
{
    public List<Holding> holdings = new List<Holding>();

    // 买入逻辑：加权平均法
    public void Buy(string stockId, float amount, float price)
    {
        Debug.Log($"📦 Buy() 进入, stock={stockId}, amount={amount}, price={price}");
        var h = holdings.Find(h => h.stockId == stockId);
        if (h == null)
        {
            h = new Holding { stockId = stockId, quantity = amount, averageCost = price };
            holdings.Add(h);
        }
        else
        {
            float totalCost = h.quantity * h.averageCost + amount * price;
            h.quantity += amount;
            h.averageCost = totalCost / h.quantity;
            Debug.Log($"📦 Buy() 退出，quantity = {h.quantity}");

        }
    }

    // 卖出逻辑：减少持股
    public bool Sell(string stockId, float amount)
    {
        var h = holdings.Find(h => h.stockId == stockId);
        if (h == null || h.quantity < amount)
            return false;

        h.quantity -= amount;
        if (h.quantity <= 0f)
            holdings.Remove(h);

        return true;
    }

    public float GetHoldingQuantity(string stockId)
    {
        var h = holdings.Find(h => h.stockId == stockId);
        return h != null ? h.quantity : 0f;
    }

    public float GetHoldingCost(string stockId)
    {
        var h = holdings.Find(h => h.stockId == stockId);
        return h != null ? h.averageCost : 0f;
    }
    public List<Holding> GetAllHoldings()
    {
        return new List<Holding>(holdings);
    }

    // 从外部载入（用于读取存档）
    public void LoadHoldings(List<Holding> savedHoldings)
    {
        holdings.Clear();
        foreach (var h in savedHoldings)
        {
            holdings.Add(new Holding
            {
                stockId = h.stockId,
                quantity = h.quantity,
                averageCost = h.averageCost
            });
        }
    }
}
