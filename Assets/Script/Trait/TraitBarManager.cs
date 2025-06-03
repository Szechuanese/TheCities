using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class TraitBarManager : MonoBehaviour
{
    public TraitSystem traitSystem;
    public CharacterSystem characterSystem;

    public GameObject traitCardPrefab;           // TraitBar 预制体
    public GameObject characterCardPrefab; // CharacterCard 预制体
    public Transform traitBar;            // TraitPanel → Layout Group容器

    public GameObject moneyCardPrefab;  //MoneyCard预制体

    private GameObject moneyCard;


    private Dictionary<string, TraitCardController> traitCards = new Dictionary<string, TraitCardController>();
    private Dictionary<string, CharacterCardController> characterCards = new Dictionary<string, CharacterCardController>();

    void Start()
    {
        moneyCard = Instantiate(moneyCardPrefab, traitBar);//生成MoneyCard

        //生成 TraitBar（带 Slider）
        foreach (var trait in traitSystem.traits)
        {
            GameObject bar = Instantiate(traitCardPrefab, traitBar);
            TraitCardController controller = bar.GetComponent<TraitCardController>();
            controller.SetTrait(trait.id, trait.value, trait.displayName); //使用 trait.id，而非 displayName
            traitCards[trait.id] = controller;
        }

        //紧接着生成 CharacterCard（不带 Slider）
        foreach (var character in characterSystem.characters)
        {
            GameObject card = Instantiate(characterCardPrefab, traitBar);
            CharacterCardController controller = card.GetComponent<CharacterCardController>();
            controller.SetCharacter(character.id, character.value); //使用 ID！
            characterCards[character.id] = controller;
        }
    }

    void Update()
    {
        foreach (var kvp in traitCards)
        {
            string id = kvp.Key;
            var controller = kvp.Value;
            float value = traitSystem.GetTrait(id);
            string displayName = traitSystem.traits.Find(t => t.id == id)?.displayName ?? "";

            controller.SetTrait(id, value, displayName);


            bool shouldBeActive = value >= 1f;
            if (controller.gameObject.activeSelf != shouldBeActive)
            {
                controller.gameObject.SetActive(shouldBeActive);
            }
        }

        foreach (var kvp in characterCards)
        {
            string id = kvp.Key;
            var controller = kvp.Value;
            float value = characterSystem.GetCharacterRaw(id);
            string displayName = characterSystem.characters.Find(c => c.id == id)?.displayName ?? id;

            controller.SetCharacter(id, value);

            bool shouldBeActive = value >= 1f;
            if (controller.gameObject.activeSelf != shouldBeActive)
            {
                controller.gameObject.SetActive(shouldBeActive);
            }
        }
    }
}