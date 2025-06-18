using System.Collections.Generic;
using UnityEngine;

//通用Value变化结构（Trait / Character）
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

    public enum ChoicePriority // 选项优先级
    {
        Primary,
        Secondary,
        Hidden,
    }

    // Tag控制卡片样式（颜色）
    public enum StoryCardStyle
    {
        Normal,      // 默认
        Combat,      // 战斗/挑战
        Important,   // 重要
        Repeatable,  // 可重复
    }

    public StoryCardStyle cardStyle = StoryCardStyle.Normal;
}

[CreateAssetMenu(fileName = "NewEvent", menuName = "Narrative/Event")]
public class NarrativeEvent : ScriptableObject
{
    public string eventId;              // 事件 ID
    public string title;                // 事件标题
    [TextArea] public string description;  // HeaderCard事件描述

    public bool singleUse;
    public bool isImportant;

    public List<EventTag> tags = new List<EventTag>();       // 事件标签
    public List<EventChoice> choices = new List<EventChoice>();

    // 标签操作函数
    public bool HasTag(EventTag tag)
    {
        return tags.Contains(tag);
    }

    public void AddTag(EventTag tag)
    {
        if (!tags.Contains(tag))
            tags.Add(tag);
    }

    public void RemoveTag(EventTag tag)
    {
        if (tags.Contains(tag))
            tags.Remove(tag);
    }
}