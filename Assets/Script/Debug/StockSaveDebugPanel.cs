using UnityEngine;
using UnityEngine.UI;
using System.IO;
//StockMarketDebug按钮管理器
public class StockSaveDebugPanel : MonoBehaviour
{
    public StockSaveManager saveManager;
    public Button saveButton;
    public Button loadButton;
    public Button openFolderButton;
    public Button printJsonButton;

    private string saveFilePath => Path.Combine(Application.persistentDataPath, "stock_save.json");

    void Start()
    {
        if (saveButton != null) saveButton.onClick.AddListener(() => saveManager.SaveStocks());
        if (loadButton != null) loadButton.onClick.AddListener(() => saveManager.LoadStocks());

        if (openFolderButton != null) openFolderButton.onClick.AddListener(() =>
        {
            string folder = Path.GetDirectoryName(saveFilePath);
            Application.OpenURL("file://" + folder);
        });

        if (printJsonButton != null) printJsonButton.onClick.AddListener(() =>
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                Debug.Log($"📝 当前存档内容：\n{json}");
            }
            else
            {
                Debug.LogWarning("⚠️ 没有找到存档文件！");
            }
        });
    }
}
