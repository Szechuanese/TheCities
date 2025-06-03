using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CompanyButtonController : MonoBehaviour
{
    public TMP_Text companyNameText;
    public Image companyLogo;
    private CompanyStock companyStock;

    private StockMarketUIController uiController;
    private RuntimeStockData stockData;

    public void Init(RuntimeStockData stock, StockMarketUIController controller)
    {
        stockData = stock;
        uiController = controller;

        var template = FindObjectOfType<StockDataManager>()?.GetCompanyTemplate(stock.stockId);
        if (template != null)
        {
            companyNameText.text = template.displayName;
            if (template.logo != null)
                companyLogo.sprite = template.logo;
        }
        else
        {
            companyNameText.text = stock.stockId; // ¶µµ×·½°¸
        }

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        uiController.ShowCompany(stockData);
    }
}
