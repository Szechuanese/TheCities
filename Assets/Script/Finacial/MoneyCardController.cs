using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoneyCardController : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text valueText;
    public Image iconImage;

    private string traitId = "money";
    private TraitSystem traitSystem;

    void Start()
    {
        traitSystem = GameObject.FindObjectOfType<TraitSystem>();
        nameText.text = "资金"; // 这个才能显示出来，
    }

    void Update()
    {
        if (traitSystem != null)
        {
            float moneyValue = traitSystem.GetTrait(traitId);
            valueText.text = $"${Mathf.RoundToInt(moneyValue)}";
        }
    }
}
