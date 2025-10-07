using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ValueChange
{
    public string id;              // Value的唯一标识（Trait或Character）
    public float changeAmount;     // 改变量
}

[System.Serializable]
public class EventChoice
{
    public string text;           // 选项标题
    public string description;    // 选项描述
    public string nextEventId;    // 下一事件 ID（可为空)

    public bool isChallenge;  // 是否为挑战选项
    public string challengeTraitId;   // 用哪个 Trait 判定
    public float successChancePerPoint; // 每点 Trait 对应的成功率（如 10%）
    public string nextEventIdSuccess;
    public string nextEventIdFailure;

    // 多个角色状态判定
    [System.Serializable]
    public class TraitRequirement
    {
        public string traitId;    
        public int requiredValue;
    }

    public List<TraitRequirement> traitRequirements = new List<TraitRequirement>();

    //通用Value变化（Trait、Character）
    public List<ValueChange> valueChanges = new List<ValueChange>();

    // 标签操作变化
    [System.Serializable]
    public class TagChange
    {
        public string tagName;
        public bool add;   // true = AddTag, false = RemoveTag
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
        Normal,      // 默认
        /// <summary>
        /// 战斗样式，只会改变卡片颜色
        /// </summary>
        Combat,      // 战斗/挑战
        /// <summary>
        /// 重要样式，改变卡片颜色
        /// </summary>
        Important,   // 重要
        /// <summary>
        /// 可重复样式，改变卡片颜色
        /// </summary>
        Repeatable,  // 可重复
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

    // 标签操作函数

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
}