using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StockSaveManager : MonoBehaviour
{
    /// 股票存档管理器
    public StockDataManager stockDataManager;

    public PlayerPortfolio playerPortfolio;

    private string saveFilePath => Path.Combine(Application.persistentDataPath, "stock_save.json");

    [System.Serializable]
    public class StockSaveWrapper
    {
        public int version = 1;
        public List<RuntimeStockData> stocks = new List<RuntimeStockData>();
        public List<Holding> holdings = new List<Holding>();
    }

    public void SaveStocks()
    {
        var data = new StockSaveWrapper
        {
            stocks = stockDataManager.GetAllStockData(),
            holdings = playerPortfolio.GetAllHoldings()
        };

        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log($"股票和持股存档保存成功！路径：{saveFilePath}");
    }

    public void LoadStocks()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("⚠️ 未找到存档文件，跳过加载");
            return;
        }

        string json = File.ReadAllText(saveFilePath);
        var data = JsonUtility.FromJson<StockSaveWrapper>(json);

        if (data == null)
        {
            Debug.LogWarning("⚠️ 存档解析失败");
            return;
        }

        if (data.version != 1)
        {
            Debug.LogWarning($"⚠️ 存档版本不兼容，当前为 {data.version}");
            return;
        }

        stockDataManager.LoadFromExternal(data.stocks);
        playerPortfolio.LoadHoldings(data.holdings);
        Debug.Log($"📂 股票和持股存档读取完成，股票数：{data.stocks.Count}，持股数：{data.holdings.Count}");
    }
    void Start()
    {
        LoadStocks(); // 游戏启动时自动读取存档
    }

    void OnApplicationQuit()
    {
        SaveStocks(); // 游戏关闭时自动保存存档
    }
}
