using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FilePanelManager : MonoBehaviour
{
    [Header("各种box")]
    public Transform fileTraitBox;         //File_TraitBox
    public Transform fileCharacterBox;     //File_CharacterBox
    public Transform fileRelationBox;    //File_RelationBox



    public GameObject fileContent;         //FileContent
    public ScrollRect fileScrollRect;       //FileScrollRect
    public Scrollbar filePanelScrollBar;
    

    [Header("预制体")]
    public GameObject valueItemPrefab;     //ValueItem 预制体

    [Header("系统引用")]
    public ValueSystem valueSystem;        //ValueSystem
    public IconDatabase iconDatabase;      //图标数据库

    private void Start()
    {
        GenerateFileValues();

        if (valueSystem != null)
            valueSystem.OnValueChanged += GenerateFileValues;
    }

    private void OnDisable()
    {
        if (valueSystem != null)
            valueSystem.OnValueChanged -= GenerateFileValues;
    }

    /// <summary>
    /// 统一生成 Trait 与 Character ValueItems
    /// </summary>
    public void GenerateFileValues()
    {
        // 清空旧Item
        foreach (Transform child in fileTraitBox) Destroy(child.gameObject);
        foreach (Transform child in fileCharacterBox) Destroy(child.gameObject);
        foreach (Transform child in fileRelationBox) Destroy(child.gameObject);

        // Trait 类型
        foreach (var val in valueSystem.GetValuesByType(ValueType.Trait))
        {
            if (val.value >= 1f)
            {
                GameObject item = Instantiate(valueItemPrefab, fileTraitBox);
                ValueItemController controller = item.GetComponent<ValueItemController>();
                controller.iconDatabase = iconDatabase;
                controller.SetData(val.id, val.displayName, val.value, GetDescriptionById(val.id));
            }
        }

        // Character 类型
        foreach (var val in valueSystem.GetValuesByType(ValueType.Character))
        {
            if (val.value >= 1f)
            {
                GameObject item = Instantiate(valueItemPrefab, fileCharacterBox);
                ValueItemController controller = item.GetComponent<ValueItemController>();
                controller.iconDatabase = iconDatabase;
                controller.SetData(val.id, val.displayName, val.value, GetDescriptionById(val.id));
            }
        }
        // Relation 类型
        foreach (var val in valueSystem.GetValuesByType(ValueType.Relation))
        {
            if (val.value >= 1f)
            {
                GameObject item = Instantiate(valueItemPrefab, fileRelationBox);
                ValueItemController controller = item.GetComponent<ValueItemController>();
                controller.iconDatabase = iconDatabase;
                controller.SetData(val.id, val.displayName, val.value, GetDescriptionById(val.id));
            }
        }
    }

    /// <summary>
    /// 获取描述文本，目前只用于FilePanel的ValueBox
    /// </summary>
    private string GetDescriptionById(string id)
    {
        //没有Bear类型的Value，Bear类型的Value在FileCarriesBox里
        switch (id)
        {
            //Trait 类型
            case "Fang":
                return "咧出笑容，轻露利齿，人会不自觉地被感染,模仿你的口吻。";
            case "Talon":
                return "暴力是一种控制的方式，但要注意，不要被这种方式所控制。";
            case "Pelt":
                return "学士们证明，古狈涪尔阏人有更长的毛发，用于抵御严寒，现在则没有。";
            case "Thigmata":
                return "风拂过你的肌肤，传来一股腐烂的味道。";
            case "Wing":
                return "我遥远的祖先啊，你们是怎么穿过无边的海洋，来到柱良帛的呢？——慕古絮";
            case "Tail":
                return "某些狈涪尔阏和亘尔阏人的脊椎会分出一条新的骨骼，具有这种特征的人精力会额外旺盛；没人知道为什么。";
            case "Spore":
                return "每年夏天，失温河都会解冻，雨会停止，河谷里的空气也会浮起成片黑色的绒毛。";
            case "Rhizome":
                return "“大树下有一百具骸骨，我的祖母就在其中”——洛温民谣”";
            case "Rifle":
                return"酒馆里依旧有人在低声吟唱XXX(战争英雄)的歌谣，人们口口相传他手持步枪就义的那天。";
            //Relation类型
            case "Relation_JinSi":
                return "今司是你同在孤儿院长大的朋友，因为能流畅背诵缄节的自述全文，现在当上了学徒，还给你找了个房子。在这个世道，" +
                    "你应该好好保护这段友谊。";
            case "Relation_WeiTao":
                return "未陶先生是个严肃的人，但总能逗乐你的姨妈，他一出现，她的脸上就有了笑容。" +
                    "姨妈去世后，他负责打理姨妈的后事，这其中也包括你，你应该好好听他的话。";
            case "Relation_HoHo":
                return "。";
            case "Relation_LittleWorm":
                return "你常常回忆起和小蠕虫一起住在下水管道的日子，你们在污水与废气中奔跑，听着地面上人们的脚步声入眠。你们许久未见，往日的火花不知是否还在燃烧。";
            //Character类型

            //Effort类型


            //Favor类型

            //Parts类型
        }
        StartCoroutine(RefreshLayoutNextFrame());
        // TODO：你可以接入自定义描述表或switch语句
        return "这是一个Value项";
    }
    #region 刷新布局相关
    public IEnumerator RefreshLayoutNextFrame() 
    {
        yield return null; // 等待下一帧，确保布局更新
        LayoutRebuilder.ForceRebuildLayoutImmediate(fileContent.GetComponent<RectTransform>());
    }
    #endregion
}