using UnityEngine;
using TMPro;

public class MoneyBoxController : MonoBehaviour
{
    public TMP_Text current_Money;

    public ValueSystem valueSystem;

    private void Start()
    {

        if (valueSystem != null)
        {
            //订阅数值变化事件
            valueSystem.OnValueChanged += RefreshMoneyText;
            //进来先刷新一次
            RefreshMoneyText();
        }
    }

    private void OnDestroy()
    {
        // 记得取消订阅，避免事件泄漏
        if (valueSystem != null)
        {
            valueSystem.OnValueChanged -= RefreshMoneyText;
        }
    }

    //真正更新UI的函数
    private void RefreshMoneyText()
    {
        if (current_Money == null || valueSystem == null) return;

        //从 ValueSystem 取 money
        float moneyValue = valueSystem.GetValue("money");  
        int moneyInt = Mathf.RoundToInt(moneyValue);

        //想加单位就自己拼，比如 "₵" 或 "Cr" 或 "¥"
        //current_Money.text = moneyInt.ToString();
        current_Money.text = $"{moneyInt} ₵";
    }
}
