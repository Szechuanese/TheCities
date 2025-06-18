using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoneyCardController : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text valueText;
    public Image iconImage;

    private string traitId = "money";
    private ValueSystem valueSystem;

    void Start()
    {
        valueSystem = GameObject.FindObjectOfType<ValueSystem>();
        nameText.text = "资金"; // 这个才能显示出来，
    }

    void Update()
    {
        if (valueSystem != null)
        {
            float moneyValue = valueSystem.GetValue(traitId);
            valueText.text = $"${Mathf.RoundToInt(moneyValue)}";
        }
    }
}
