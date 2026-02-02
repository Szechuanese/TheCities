using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ValueChange
{
    public string id;              //Value的唯一标识（Trait或Character）
    public float changeAmount;     //改变量
}

[System.Serializable]
public class EventChoice
{
    public string text;           //选项标题
    public string description;    //选项描述
    public string nextEventId;    //下一事件 ID（可为空)

    public bool isChallenge;  //是否为挑战选项
    public string challengeTraitId;   //用哪个 Trait 判定
    public float successChancePerPoint; //每点 Value 对应的成功率（如10%）
    public string nextEventIdSuccess;
    public string nextEventIdFailure;

    //多个角色状态判定
    [System.Serializable]
    public class TraitRequirement
    {
        public string traitId;    
        public int requiredValue;
    }

    public List<TraitRequirement> traitRequirements = new List<TraitRequirement>();

    //点击卡片通用Value变化（Trait、Character）
    public List<ValueChange> valueChanges = new List<ValueChange>();


    [System.Serializable]
    public class BurdenItemChange
    {
        public string itemId;
        public int amount; //正数=获得；负数=失去
    }
    //通用的物品变化
    public List<BurdenItemChange> burdenItemChanges = new List<BurdenItemChange>();
    //挑战系统成功/失败的物品变化（仅当isChallenge=true时使用）
    public List<BurdenItemChange> burdenItemChangesSuccess = new List<BurdenItemChange>();
    public List<BurdenItemChange> burdenItemChangesFailure = new List<BurdenItemChange>();

    //标签操作变化
    [System.Serializable]
    public class TagChange
    {
        public string tagName;
        public bool add;   //true = AddTag, false = RemoveTag
    }

    public List<TagChange> tagChanges = new List<TagChange>();



    public ChoicePriority priority = ChoicePriority.Secondary;
    //这个是为勒选项设置优先级的枚举，主要用于在UI中排序显示。

    /// <summary>
    /// 选项优先级枚举
    /// </summary>
    public enum ChoicePriority // 选项优先级
    {
        /// <summary>
        /// 主要选项，放在第一位
        /// </summary>
        Primary,
        /// <summary>
        /// 次要选项，放在主要选项Primary之后
        /// </summary>
        Secondary,
        /// <summary>
        /// 隐藏选项，为满足条件不在UI中显示（尚未完成，目前只能通过手动）
        /// </summary>
        Hidden,
    }

    /// <summary>
    /// 卡片样式枚举
    /// </summary>
    // 枚举控制卡片样式（颜色）,将这个方法放置在这里，让我可以直接在Unity中设置事件卡片样式。
    public enum StoryCardStyle
    {
        /// <summary>
        /// 默认样式
        /// </summary>
        Normal, 
        /// <summary>
        /// 战斗样式，只会改变卡片颜色
        /// </summary>
        Combat,  
        /// <summary>
        /// 重要样式，改变卡片颜色
        /// </summary>
        Important, 
        /// <summary>
        /// 可重复样式，改变卡片颜色
        /// </summary>
        Repeatable,  
    }

    public StoryCardStyle cardStyle = StoryCardStyle.Normal;
}

[CreateAssetMenu(fileName = "NewEvent", menuName = "Narrative/Event")]
public class NarrativeEvent : ScriptableObject
{
    // 事件 ID
    public string eventId;
    // 事件标题
    public string title;
    // HeaderCard事件描述
    [TextArea] public string description;

    public bool singleUse;
    public bool isImportant;

    // 事件标签
    public List<EventTag> tags = new List<EventTag>();       
    public List<EventChoice> choices = new List<EventChoice>();

    #region 标签操作函数
    // 判断是否包含标签
    public bool HasTag(EventTag tag)
    {
        return tags.Contains(tag);
    }

    //添加标签
    public void AddTag(EventTag tag)
    {
        if (!tags.Contains(tag))
            tags.Add(tag);
    }

    //移除标签
    public void RemoveTag(EventTag tag)
    {
        if (tags.Contains(tag))
            tags.Remove(tag);
    }
    #endregion

    /// <summary>
    /// 编辑器校验：确保挑战选项和非挑战选项的字段填写符合逻辑
    /// </summary>
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (choices == null) return;

        for (int i = 0; i < choices.Count; i++)
        {
            var c = choices[i];
            if (c == null) continue;

            //规则1：挑战选项不应该使用“通用物品变化”
            if (c.isChallenge)
            {
                if (c.burdenItemChanges != null && c.burdenItemChanges.Count > 0)
                {
                    Debug.LogWarning($"[NarrativeEvent:{eventId}] Choice[{i}] 是挑战选项，但填写了 burdenItemChanges（通用）。建议改填 Success/Failure 列表。已自动清空通用列表。", this);
                    c.burdenItemChanges.Clear();
                }

                //规则2：挑战选项必须填 challengeTraitId 或 successChancePerPoint 才有意义
                if (string.IsNullOrWhiteSpace(c.challengeTraitId))
                {
                    Debug.LogWarning($"[NarrativeEvent:{eventId}] Choice[{i}] 是挑战选项，但 challengeTraitId 为空。", this);
                }
            }
            else
            {
                //规则3：非挑战选项不应该填写成功/失败物品变化（避免误会）
                if (c.burdenItemChangesSuccess != null && c.burdenItemChangesSuccess.Count > 0)
                {
                    Debug.LogWarning($"[NarrativeEvent:{eventId}] Choice[{i}] 非挑战选项，却填写了 burdenItemChangesSuccess。已自动清空。", this);
                    c.burdenItemChangesSuccess.Clear();
                }
                if (c.burdenItemChangesFailure != null && c.burdenItemChangesFailure.Count > 0)
                {
                    Debug.LogWarning($"[NarrativeEvent:{eventId}] Choice[{i}] 非挑战选项，却填写了 burdenItemChangesFailure。已自动清空。", this);
                    c.burdenItemChangesFailure.Clear();
                }
            }
        }
    }
#endif
}