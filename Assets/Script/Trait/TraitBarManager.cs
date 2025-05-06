using UnityEngine;
using System.Collections.Generic;

public class TraitBarManager : MonoBehaviour
{
    public TraitSystem traitSystem;
    public CharacterSystem characterSystem;

    public GameObject traitCardPrefab;           // TraitBar 预制体
    public GameObject characterCardPrefab; // CharacterCard 预制体
    public Transform traitBar;            // TraitPanel → Layout Group容器

    private Dictionary<string, TraitCardController> traitCards = new Dictionary<string, TraitCardController>();
    private Dictionary<string, CharacterCardController> characterCards = new Dictionary<string, CharacterCardController>();

    void Start()
    {
        // ✅ 生成 TraitBar（带 Slider）
        foreach (var trait in traitSystem.traits)
        {
            GameObject bar = Instantiate(traitCardPrefab, traitBar);
            TraitCardController controller = bar.GetComponent<TraitCardController>();
            controller.SetTrait(trait.id, trait.value); // ✅ 使用 trait.id，而非 displayName
            traitCards[trait.id] = controller;
        }

        // ✅ 紧接着生成 CharacterCard（不带 Slider）
        foreach (var character in characterSystem.characters)
        {
            GameObject card = Instantiate(characterCardPrefab, traitBar);
            CharacterCardController controller = card.GetComponent<CharacterCardController>();
            controller.SetCharacter(character.id, character.value); // ✅ 使用 ID！
            characterCards[character.id] = controller;
        }
    }

    void Update()
    {
        // ✅ 更新 TraitBar 的值
        foreach (var kvp in traitCards)
        {
            string id = kvp.Key;
            var controller = kvp.Value;
            float value = traitSystem.GetTrait(id);
            string displayName = traitSystem.traits.Find(t => t.id == id).displayName;
            controller.SetTrait(id, value);

            // ✅ 根据值控制是否显示
            controller.gameObject.SetActive(value >= 1f);
        }

        // ✅ 更新 CharacterCard 的整数值
        foreach (var kvp in characterCards)
        {
            string id = kvp.Key;
            var controller = kvp.Value;

            float value = characterSystem.GetCharacterRaw(id);
            string displayName = characterSystem.characters.Find(c => c.id == id).displayName;

            controller.SetCharacter(id, value);

            // ✅ 根据值控制是否显示
            controller.gameObject.SetActive(value >= 1f);
        }
    }
}