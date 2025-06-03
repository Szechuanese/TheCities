using System.Collections.Generic;
using UnityEngine;

public class StockMarketUIController : MonoBehaviour
{
    public Transform companyListContainer;
    public GameObject companyButtonPrefab;
    public StockDetailPanel stockDetailPanel;
    public BuySellPanelController buySellPanel;

    private RuntimeStockData currentStock;
    private float refreshTimer = 0f;
    public float refreshInterval = 5f;

    void Start()
    {
        FindObjectOfType<StockSaveManager>()?.LoadStocks();
        var stockDataManager = FindObjectOfType<StockDataManager>();
        if (stockDataManager == null)
        {
            Debug.LogError("❌ 找不到 StockDataManager！");
            return;
        }

        var allStocks = stockDataManager.GetAllStockData();
        foreach (var stock in allStocks)
        {
            GameObject btn = Instantiate(companyButtonPrefab, companyListContainer);
            var controller = btn.GetComponent<CompanyButtonController>();
            controller.Init(stock, this);
        }

        if (allStocks.Count > 0)
            ShowCompany(allStocks[0]);
    }

    void Update()
    {
        refreshTimer += Time.deltaTime;
        if (refreshTimer >= refreshInterval && currentStock != null)
        {
            refreshTimer = 0f;
            stockDetailPanel.ShowCompany(currentStock);
        }
    }

    public void ShowCompany(RuntimeStockData stock)
    {
        currentStock = stock;
        stockDetailPanel.ShowCompany(stock);
        buySellPanel.ShowStock(stock);
    }
}
