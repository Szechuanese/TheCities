using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TraitChange
{
    public string traitId;
    public float changeAmount;
}

[System.Serializable]
public class CharacterChange
{
    public string characterId;
    public float changeAmount;
}

[System.Serializable]
public class EventChoice
{
    public string text;           // 选项标题
    public string description;    //选项描述
    public string nextEventId;          // 下一事件 ID（可为空)
    public bool isChallenge;  // 是否为挑战选项
    public string challengeTraitId;   // 用哪个 Trait 判定
    public float successChancePerPoint; // 每点 Trait 对应的成功率（如 10%）
    public string nextEventIdSuccess;
    public string nextEventIdFailure;
    [System.Serializable]
    public class TraitRequirement//多个角色状态判定
    {
        public string traitId;
        public int requiredValue;
    }

    public List<TraitRequirement> traitRequirements = new List<TraitRequirement>();

    // 多个特质/角色状态修改
    public List<TraitChange> traitChanges = new List<TraitChange>();
    public List<CharacterChange> characterChanges = new List<CharacterChange>();

    public ChoicePriority priority = ChoicePriority.Secondary; // 选项优先级
    public enum ChoicePriority
    {
        Primary,
        Secondary,
        Hidden
    }
}

[CreateAssetMenu(fileName = "NewEvent", menuName = "Narrative/Event")]
public class NarrativeEvent : ScriptableObject
{
    public string eventId;              // 事件 ID
    public string title;                // 事件标题
    [TextArea] public string description;  // HeadrCard事件描述
    public List<string> tags;
    public bool singleUse;
    public bool isImportant;
    public List<EventChoice> choices;
}