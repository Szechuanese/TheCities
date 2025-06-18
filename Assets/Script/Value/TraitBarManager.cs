using System.Collections.Generic;
using UnityEngine;

public class TraitBarManager : MonoBehaviour
{
    public ValueSystem valueSystem;
    public GameObject traitCardPrefab;
    public GameObject characterCardPrefab;
    public Transform traitBar;

    private Dictionary<string, TraitCardController> traitCards = new Dictionary<string, TraitCardController>();
    private Dictionary<string, CharacterCardController> characterCards = new Dictionary<string, CharacterCardController>();

    void Start()
    {
        GenerateTraitCards();
        GenerateCharacterCards();

        if (valueSystem != null)
            valueSystem.OnValueChanged += Refresh;
    }

    void OnDisable()
    {
        if (valueSystem != null)
            valueSystem.OnValueChanged -= Refresh;
    }

    void Refresh()
    {
        GenerateTraitCards();
        GenerateCharacterCards();
    }

    void GenerateTraitCards()
    {
        //清楚旧的卡片
        foreach (Transform child in traitBar)
        {
            Destroy(child.gameObject);
        }
        traitCards.Clear();
        //得到新卡片
        foreach (var value in valueSystem.GetValuesByType(ValueType.Trait))
        {
            if (value.value >= 1f)
            {
                GameObject card = Instantiate(traitCardPrefab, traitBar);
                TraitCardController controller = card.GetComponent<TraitCardController>();
                controller.SetData(value.id, value.displayName, value.value);
                traitCards[value.id] = controller;
            }
        }
    }

    void GenerateCharacterCards()
    {
        foreach (var value in valueSystem.GetValuesByType(ValueType.Character))
        {
            if (value.value >= 1f)
            {
                GameObject card = Instantiate(characterCardPrefab, traitBar);
                CharacterCardController controller = card.GetComponent<CharacterCardController>();
                controller.SetData(value.id, value.displayName, value.value);
                characterCards[value.id] = controller;
            }
        }
    }
}
