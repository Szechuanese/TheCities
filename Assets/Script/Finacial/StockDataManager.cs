using System.Collections.Generic;
using UnityEngine;

public class StockDataManager : MonoBehaviour
{
    //股票数据管理器
    public List<CompanyStock> stockTemplates; // 在 Inspector 中绑定所有原始股票模板
    private Dictionary<string, RuntimeStockData> runtimeData = new Dictionary<string, RuntimeStockData>();
    private Dictionary<string, CompanyStock> templateLookup = new Dictionary<string, CompanyStock>();

    void Awake()
    {
        templateLookup.Clear();
        foreach (var template in stockTemplates)
        {
            templateLookup[template.stockId] = template;
            runtimeData[template.stockId] = new RuntimeStockData(
                template.stockId,
                template.currentPrice
            );
        }
    }

    public CompanyStock GetCompanyTemplate(string stockId)
    {
        templateLookup.TryGetValue(stockId, out var template);
        return template;
    }

    public RuntimeStockData GetStockData(string stockId)
    {
        return runtimeData.TryGetValue(stockId, out var data) ? data : null;
    }

    public List<RuntimeStockData> GetAllStockData()
    {
        return new List<RuntimeStockData>(runtimeData.Values);
    }

    public void ResetAll()
    {
        runtimeData.Clear();
        foreach (var template in stockTemplates)
        {
            runtimeData[template.stockId] = new RuntimeStockData(
                template.stockId,
                template.currentPrice
            );
        }
    }
    public void LoadFromExternal(List<RuntimeStockData> saveData)
    {
        runtimeData.Clear();
        foreach (var saved in saveData)
        {
            runtimeData[saved.stockId] = saved;
        }
    }
}
