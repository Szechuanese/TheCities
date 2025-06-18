using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventChoice;

public class EventManager : MonoBehaviour
{
    public EventUIManager eventUIManager;
    public ChallengeSystem challengeManager;
    public List<NarrativeEvent> allEvents;
    public ValueSystem valueSystem;

    public RegionInfo lastRegion;
    public Stack<RegionInfo> regionHistory = new Stack<RegionInfo>();
    public HashSet<string> exploredRegionIds = new HashSet<string>();

    public RegionPanelManager regionPanelManager;

    private NarrativeEvent currentEvent;
    public NarrativeEvent CurrentEvent { get { return currentEvent; } }

    private HashSet<string> triggeredEventIds = new HashSet<string>();
    private bool challengeInProgress = false;
    private bool pendingEnterStockMarket = false;
    public EventTagHandler tagHandler;

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
            StartEventDetail(currentEvent);
        else
            Debug.LogError($"事件 {eventId} 没有找到！");
    }

    public void StartEventDetail(NarrativeEvent e)
    {
        if (challengeInProgress) return;

        currentEvent = e;

        if (pendingEnterStockMarket)
        {
            pendingEnterStockMarket = false;
            Debug.Log("📈 由选项触发 → 准备进入股市");
            tagHandler?.ExecuteStockMarketTransition(lastRegion);
            return;
        }

        if (e.singleUse) MarkTriggered(e.eventId);
        eventUIManager.ShowEvent(e);
    }

    public void SelectChoiceFrom(NarrativeEvent evt, int index)
    {
        currentEvent.RemoveTag(EventTag.Returnable);
        eventUIManager.ShowEvent(currentEvent);

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
            if (valueSystem.GetValue(req.traitId) < req.requiredValue)
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

        foreach (var vc in choice.valueChanges)
        {
            valueSystem.ModifyValue(vc.id, vc.changeAmount);
            EventLogManager.instance?.AddLog($"🧬 状态【{vc.id}】变化 {(vc.changeAmount >= 0 ? "+" : "")}{vc.changeAmount}");
        }
        Debug.Log($"开始处理tagChanges，数量: {choice.tagChanges?.Count ?? 0}");
        Debug.Log($"TagHandler是否为空: {tagHandler == null}");

        if (choice.tagChanges != null)
        {
            foreach (var tagChange in choice.tagChanges)
            {
                if (System.Enum.TryParse(tagChange.tagName, out EventTag parsedTag))
                {
                    if (tagChange.add)
                    {
                        currentEvent.AddTag(parsedTag);
                        tagHandler?.Handle(parsedTag, this);
                    }

                    if (parsedTag == EventTag.StockMarketEntry)//警告！！
                                                               //股市跳转需要Tag-EntryPoint!，因为所有RegionPanel内的都需要
                                                               //不只是StockMarketEntry和tagChange-StockMarketEntry
                    {
                        Debug.Log("📈 立即执行股市跳转");
                        tagHandler?.ExecuteStockMarketTransition(lastRegion);
                        return;
                    }
                }

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
            {
                currentEvent.RemoveTag(EventTag.Returnable);
                StartEvent(choice.nextEventId);
            }
        }
    }

    private IEnumerator SelectChoiceCoroutine(NarrativeEvent evt, int index)
    {
        yield return null;
        currentEvent = evt;
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
            if (valueSystem.GetValue(req.traitId) < req.requiredValue)
            {
                meetsAllRequirements = false;
                break;
            }
        }

        if (meetsAllRequirements)
        {
            EventLogManager.instance?.AddLog($"选择了【{choice.text}】");

            foreach (var vc in choice.valueChanges)
            {
                valueSystem.ModifyValue(vc.id, vc.changeAmount);
                EventLogManager.instance?.AddLog($"🧬 状态【{vc.id}】变化 {(vc.changeAmount >= 0 ? "+" : "")}{vc.changeAmount}");
            }

            if (choice.tagChanges != null)
            {
                foreach (var tagChange in choice.tagChanges)
                {
                    if (System.Enum.TryParse(tagChange.tagName, out EventTag parsedTag))
                    {
                        if (tagChange.add)
                            currentEvent.AddTag(parsedTag);
                        else
                            currentEvent.RemoveTag(parsedTag);
                    }
                    else
                    {
                        Debug.LogWarning($"Tag '{tagChange.tagName}' 无法转换为 EventTag 枚举");
                    }
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
            Debug.Log("不满足条件，无法选择该选项。");
        }
    }

    private IEnumerator HandleChallenge(EventChoice choice)
    {
        challengeInProgress = true;

        float traitValue = valueSystem.GetValue(choice.challengeTraitId);
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
        if (e == null)
            return false;

        //若标记为“入口事件”或“股市入口”，则默认可进入
        if (e.HasTag(EventTag.Entrypoint) || e.HasTag(EventTag.StockMarketEntry))
            return true;

        //若没有 choices，则不可进入（避免空引用）
        if (e.choices == null || e.choices.Count == 0)
            return false;

        foreach (var choice in e.choices)
        {
            if (choice == null || choice.traitRequirements == null)
                return true;

            bool meetsAll = true;
            foreach (var req in choice.traitRequirements)
            {
                if (valueSystem.GetValue(req.traitId) < req.requiredValue)
                {
                    meetsAll = false;
                    break;
                }
            }

            if (meetsAll)
                return true;
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

    public void SetPendingStockMarket(bool value)
    {
        pendingEnterStockMarket = value;
    }
}
