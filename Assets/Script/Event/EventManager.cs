using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public TraitSystem traitSystem;
    public CharacterSystem characterSystem;
    public EventUIManager eventUIManager;
    public ChallengeSystem challengeManager;
    public List<NarrativeEvent> allEvents;

    private NarrativeEvent currentEvent;
    public NarrativeEvent CurrentEvent { get { return currentEvent; } }

    private HashSet<string> triggeredEventIds = new HashSet<string>();
    private bool challengeInProgress = false;

    void Start()
    {
        if (allEvents == null || allEvents.Count == 0)
            return;

        var firstEvent = allEvents.Find(e => e != null && e.eventId == "start_event");
        if (firstEvent != null)
            StartEventDetail(firstEvent);
        else
            Debug.LogError("🚫 未找到 eventId 为 'start_event' 的事件！");
    }

    public void StartEvent(string eventId)
    {
        currentEvent = allEvents.Find(e => e.eventId == eventId);
        if (currentEvent != null)
        {
            StartEventDetail(currentEvent);
        }
        else
        {
            Debug.LogError($"事件 {eventId} 没有找到！");
        }
    }

    public void StartEventDetail(NarrativeEvent e)
    {
        if (challengeInProgress)
        {
            Debug.Log("⚠️ 当前挑战进行中，忽略事件跳转！");
            return;
        }
        Debug.Log($"🎯 StartEventDetail {e.eventId}，choices数量 = {e.choices.Count}");
        currentEvent = e;

        if (e.singleUse)
            MarkTriggered(e.eventId);

        eventUIManager.ShowEvent(e);
    }

    public void SelectChoiceFrom(NarrativeEvent evt, int index)
    {
        if (challengeInProgress)
        {
            Debug.Log("⚠️ 当前挑战进行中，禁止重复选择！");
            return;
        }

        if (evt == null || index < 0 || index >= evt.choices.Count)
        {
            Debug.LogError("无效的事件或选项索引");
            return;
        }

        StartCoroutine(SelectChoiceCoroutine(evt, index));
    }
    public void SelectChoiceDirect(EventChoice choice)
    {
        if (choice == null) return;

        bool meetsAllRequirements = true;
        foreach (var req in choice.traitRequirements)
        {
            if (traitSystem.GetTrait(req.traitId) < req.requiredValue)
            {
                meetsAllRequirements = false;
                break;
            }
        }

        if (!meetsAllRequirements)
        {
            Debug.Log("⚠️ 不满足条件，无法选择该选项！");
            return;
        }

        EventLogManager.instance?.AddLog($"选择了【{choice.text}】");

        if (choice.traitChanges != null)
        {
            foreach (var tc in choice.traitChanges)
            {
                traitSystem.ModifyTrait(tc.traitId, tc.changeAmount);
                EventLogManager.instance?.AddLog($"🧬 特质【{tc.traitId}】变化 {(tc.changeAmount >= 0 ? "+" : "")}{tc.changeAmount}");
            }
        }

        if (choice.characterChanges != null)
        {
            foreach (var cc in choice.characterChanges)
            {
                characterSystem.ModifyCharacter(cc.characterId, cc.changeAmount);
                EventLogManager.instance?.AddLog($"👤 角色【{cc.characterId}】变化 {(cc.changeAmount >= 0 ? "+" : "")}{cc.changeAmount}");
            }
        }

        if (choice.isChallenge)
        {
            if (!challengeInProgress)
            {
                StartCoroutine(HandleChallenge(choice)); // ✅ 执行挑战逻辑
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(choice.nextEventId))
            {
                StartEvent(choice.nextEventId); // ✅ 普通跳转
            }
        }
    }

    private IEnumerator SelectChoiceCoroutine(NarrativeEvent evt, int index)
    {
        yield return null; // 延迟一帧，确保事件数据稳定

        currentEvent = evt; // 再赋值，避免中途变化
        SelectChoice(index);
    }

    public void SelectChoice(int index)
    {
        if (challengeInProgress)
        {
            Debug.Log("⚠️ 当前挑战进行中，禁止重复选择！");
            return;
        }

        if (currentEvent == null || index < 0 || index >= currentEvent.choices.Count)
        {
            Debug.LogError("选择无效的选项！");
            return;
        }

        var choice = currentEvent.choices[index];

        bool meetsAllRequirements = true;
        foreach (var req in choice.traitRequirements)
        {
            if (traitSystem.GetTrait(req.traitId) < req.requiredValue)
            {
                meetsAllRequirements = false;
                break;
            }
        }

        if (meetsAllRequirements)
        {
            EventLogManager.instance?.AddLog($"选择了【{choice.text}】");

            if (choice.traitChanges != null)
            {
                foreach (var tc in choice.traitChanges)
                {
                    traitSystem.ModifyTrait(tc.traitId, tc.changeAmount);
                    EventLogManager.instance?.AddLog($"🧬 特质【{tc.traitId}】变化 {(tc.changeAmount >= 0 ? "+" : "")}{tc.changeAmount}");
                }
            }

            if (choice.characterChanges != null)
            {
                foreach (var cc in choice.characterChanges)
                {
                    characterSystem.ModifyCharacter(cc.characterId, cc.changeAmount);
                    EventLogManager.instance?.AddLog($"👤 角色【{cc.characterId}】变化 {(cc.changeAmount >= 0 ? "+" : "")}{cc.changeAmount}");
                }
            }

            if (choice.isChallenge)
            {
                if (!challengeInProgress)
                {
                    StartCoroutine(HandleChallenge(choice));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(choice.nextEventId))
                    StartEvent(choice.nextEventId);
            }
        }
        else
        {
            Debug.Log("⚠️ 不满足条件，无法选择该选项。");
        }
    }

    private IEnumerator HandleChallenge(EventChoice choice)
    {
        challengeInProgress = true;

        float traitValue = traitSystem.GetTrait(choice.challengeTraitId);
        float successChance = traitValue * choice.successChancePerPoint;
        float roll = Random.Range(0f, 1f);
        bool success = roll <= successChance;

        Debug.Log($"🎲 挑战掷骰：Trait={traitValue}, 成功率={successChance:P0}, 掷出={roll:F2} → {(success ? "成功" : "失败")}");

        if (EventLogManager.instance != null)
        {
            if (success)
                EventLogManager.instance.AddLog($"🎯 挑战成功！（{choice.challengeTraitId}）");
            else
                EventLogManager.instance.AddLog($"💥 挑战失败！（{choice.challengeTraitId}）");
        }

        if (challengeManager != null)
        {
            yield return challengeManager.ShowChallengeResultCoroutine(success);
        }

        challengeInProgress = false;

        string nextId = success ? choice.nextEventIdSuccess : choice.nextEventIdFailure;

        if (!string.IsNullOrEmpty(nextId))
        {
            StartEvent(nextId);
        }
    }

    public bool CanEnterEvent(NarrativeEvent e)
    {
        foreach (var choice in e.choices)
        {
            bool meetsAll = true;
            foreach (var req in choice.traitRequirements)
            {
                if (traitSystem.GetTrait(req.traitId) < req.requiredValue)
                {
                    meetsAll = false;
                    break;
                }
            }

            if (meetsAll) return true;
        }
        return false;
    }

    public bool HasTriggered(string eventId)
    {
        return triggeredEventIds.Contains(eventId);
    }

    public void MarkTriggered(string eventId)
    {
        triggeredEventIds.Add(eventId);
    }
}