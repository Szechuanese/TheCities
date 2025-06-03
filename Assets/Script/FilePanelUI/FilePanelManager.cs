using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilePanelManager : MonoBehaviour
{
    [Header("Value绑定")]
    public Transform fileTraitBox;         // File_TraitBox
    public Transform fileCharacterBox;     // File_CharacterBox

    [Header("预制体")]
    public GameObject valueItemPrefab;     //ValueItem专属预制体

    [Header("系统引用")]
    public TraitSystem traitSystem;
    public CharacterSystem characterSystem;
    public IconDatabase iconDatabase;      //TraitIconDatabase
    [Header("UI界面相关")]
    public ScrollRect filePanelScrollRect;
    public Transform filePanelContent;

    [System.Serializable]
    public class ValueDescriptionEntry
    {
        public string id;
        [TextArea]
        public string description;
    }

    [Header("Value 描述配置")]
    public List<ValueDescriptionEntry> valueDescriptions = new List<ValueDescriptionEntry>();

    private void Start()
    {
        StartCoroutine(RefreshLayoutNextFrame());
        GenerateFileTraits();
        GenerateFileCharacters();

        if (filePanelScrollRect != null)
        {
            StartCoroutine(RefreshLayoutNextFrame());
            Canvas.ForceUpdateCanvases(); // 确保内容已经生成完毕
            filePanelScrollRect.verticalNormalizedPosition = 1f;
            Debug.Log("FileScrollRect 已自动滚动到顶部");

        }
    }


    private void OnEnable()
    {
        if (traitSystem != null)
            traitSystem.OnTraitChanged += GenerateFileTraits;
        if (characterSystem != null)
            characterSystem.OnCharacterChanged += GenerateFileCharacters;
    }

    private void OnDisable()
    {
        if (traitSystem != null)
            traitSystem.OnTraitChanged -= GenerateFileTraits;
        if (characterSystem != null)
            characterSystem.OnCharacterChanged -= GenerateFileCharacters;
    }


    private string GetDescriptionById(string id, string defaultDescription)
    {
        var entry = valueDescriptions.Find(e => e.id == id);
        return entry != null ? entry.description : defaultDescription;
    }

    /// <summary>
    /// 动态生成所有 Trait ValueItems
    /// </summary>
    public void GenerateFileTraits()
    {
        // 清空旧物体
        foreach (Transform child in fileTraitBox)
            Destroy(child.gameObject);

        // 遍历特质系统
        foreach (var trait in traitSystem.traits)
        {
            if (trait.value >= 1f)
            {
                GameObject item = Instantiate(valueItemPrefab, fileTraitBox);
                ValueItemController controller = item.GetComponent<ValueItemController>();
                controller.iconDatabase = iconDatabase;
                controller.SetData(
                trait.id,
                trait.displayName,
                trait.value,
                GetDescriptionById(trait.id, "这是一个特质")
            );
            }
        }
    }

    /// <summary>
    /// 动态生成所有 Character ValueItems
    /// </summary>
    public void GenerateFileCharacters()
    {
        // 清空旧物体
        foreach (Transform child in fileCharacterBox)
            Destroy(child.gameObject);

        // 遍历角色系统
        foreach (var character in characterSystem.characters)
        {
            if (character.value >= 1f)
            {
                GameObject item = Instantiate(valueItemPrefab, fileCharacterBox);
                ValueItemController controller = item.GetComponent<ValueItemController>();
                controller.iconDatabase = iconDatabase;
                controller.SetData(
                character.id,
                character.displayName,
                character.value,
                GetDescriptionById(character.id, "这是一个个性")
            );
            }
        }
    }
    private IEnumerator RefreshLayoutNextFrame()
    {
        yield return null;

        LayoutRebuilder.ForceRebuildLayoutImmediate(filePanelContent.GetComponent<RectTransform>());
    }
}