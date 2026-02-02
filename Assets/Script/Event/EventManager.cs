using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public EventUIManager eventUIManager;   //绑定事件UI界面
    public ChallengeSystem challengeManager;//绑定挑战系统
    public List<NarrativeEvent> allEvents;  //所有事件列表
    public ValueSystem valueSystem;         //绑定特质系统

    public RegionInfo lastRegion;           //上次所在区域
    public Stack<RegionInfo> regionHistory = new Stack<RegionInfo>(); //区域历史记录
    public HashSet<string> exploredRegionIds = new HashSet<string>(); //已探索区域ID集合

    public RegionPanelManager regionPanelManager;       //绑定区域面板管理器

    private NarrativeEvent currentEvent;                //当前事件
    public NarrativeEvent CurrentEvent { get { return currentEvent; } }     //

    private HashSet<string> triggeredEventIds = new HashSet<string>();  //已触发事件ID集合
    private bool challengeInProgress = false;               //是否有挑战进行中
    private bool pendingEnterStockMarket = false;           //进入股市面板判断
    public EventTagHandler tagHandler;                      //协助进入股市绑定脚本


    [Header("Burden")]
    public BurdenSystem burdenSystem;
    public BurdenItemDatabase burdenItemDatabase;


    /// <summary>
    /// 说明：启动时自动寻找 start_event 并进入；如果找不到报错。
    /// </summary>
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


    /// <summary>
    /// 说明：通过 eventId 从 allEvents 查找事件并进入详情（内部调用 StartEventDetail）。
    /// </summary>
    /// <param name="eventId"></param>
    public void StartEvent(string eventId)
    {
        currentEvent = allEvents.Find(e => e.eventId == eventId);
        if (currentEvent != null)
            StartEventDetail(currentEvent);
        else
            Debug.LogError($"事件 {eventId} 没有找到！");
    }


    /// <summary>
    /// 说明:切换当前事件并刷新 UI；
    /// </summary>
    /// <param name="e"></param>
    public void StartEventDetail(NarrativeEvent e)
    {
        if (challengeInProgress) return;

        currentEvent = e;
        //如果pendingEnterStockMarket则改为跳转股市；如果singleUse 则标记触发；挑战进行中禁止进入。
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
    /// <summary>
    /// 说明：从“外部指定事件+索引”触发选择（协程入口）；目前包含 Returnable 标签的特殊处理。
    /// </summary>
    /// <param name="evt"></param>
    /// <param name="index"></param>
    public void SelectChoiceFrom(NarrativeEvent evt, int index)
    {


        //这里有问题，会导致returnbale标签丢失，因为这个方法即是
        //表示 当前事件是一个‘区域入口 / 缓冲事件’，
        //在这个事件里你可以随时点 返回 回到区域事件列表。
        //但是一旦你做出选择，进入下一层事件链，就不应该还能从后面的事件直接‘传送回区域’了。
        //我草，我他妈给这句注释了，结果发现从其他事件进入添加了这个标签的事件，那个事件也没有Returnable标签。
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
    /// <summary>
    /// 卡片点击选项的主入口
    /// </summary>
    /// <param name="choice"></param>
    public void SelectChoiceDirect(EventChoice choice)
    {
        //先校验 TraitRequirement；应用 valueChanges；非挑战应用通用物品变化；
        //处理 tagChanges（StockMarketEntry 会直接跳转并 return）；
        //挑战则进入 HandleChallenge，否则进入nextEvent。
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
            EventLogManager.instance?.AddLog($"状态【{vc.id}】变化 {(vc.changeAmount >= 0 ? "+" : "")}{vc.changeAmount}");
        }
        //只有非挑战选项才结算“通用物品变化”
        //挑战奖励/惩罚统一在 HandleChallenge 里结算
        if (!choice.isChallenge)
        {
            ApplyBurdenChanges(choice.burdenItemChanges);
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
                //这里又有一段returnable标签的代码,这一段我原本的用意是什么呢？
                //可能我需要完成UI层再来收拾这段代码吧。
                currentEvent.RemoveTag(EventTag.Returnable);
                StartEvent(choice.nextEventId);
            }
        }
    }
    /// <summary>
    /// 通过点击卡片应用物品变化，整合挑战系统
    /// </summary>
    /// <param name="choice"></param>
    private void ApplyBurdenChanges(List<EventChoice.BurdenItemChange> changes)
    {
        if (changes == null || changes.Count == 0) return;

        if (burdenSystem == null)
        {
            Debug.LogWarning("[Burden] EventManager 没有绑定 BurdenSystem，无法执行物品变化");
            return;
        }
        if (burdenItemDatabase == null)
        {
            Debug.LogWarning("[Burden] EventManager 没有绑定 BurdenItemDatabase，无法通过 itemId 查定义");
            return;
        }

        foreach (var c in changes)
        {
            if (c == null) continue;
            if (string.IsNullOrWhiteSpace(c.itemId) || c.amount == 0) continue;

            var def = burdenItemDatabase.Get(c.itemId);
            if (def == null)
            {
                Debug.LogWarning($"[Burden] 未找到 itemId={c.itemId} 的 BurdenItemDefinition（检查数据库）");
                continue;
            }

            if (c.amount > 0)
            {
                //BurdenSystem已支持amount 版本
                burdenSystem.AddItem(def, c.amount);
                EventLogManager.instance?.AddLog($"📦 获得【{c.itemId}】x{c.amount}");
            }
            else
            {
                int removeCount = -c.amount;
                burdenSystem.RemoveItem(c.itemId, removeCount);
                EventLogManager.instance?.AddLog($"📦 失去【{c.itemId}】x{removeCount}");
            }
        }
    }

    /// <summary>
    /// 协程转发到 SelectChoice（保留一帧以等待 UI/状态稳定）。
    /// </summary>
    /// <param name="evt"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private IEnumerator SelectChoiceCoroutine(NarrativeEvent evt, int index)
    {
        yield return null;
        currentEvent = evt;
        SelectChoice(index);
    }
    /// <summary>
    /// 老入口/索引入口；逻辑与 SelectChoiceDirect 类似，用于协程调用与兼容。
    /// </summary>
    /// <param name="index"></param>
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
            //只有非挑战选项才结算“通用物品变化”
            //挑战奖励/惩罚统一在 HandleChallenge里结算
            if (!choice.isChallenge)
            {
                ApplyBurdenChanges(choice.burdenItemChanges);
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
    /// <summary>
    /// 执行挑战掷骰、展示结果动画、结算成功/失败物品变化、跳转对应事件；期间锁定 challengeInProgress。
    /// </summary>
    /// <param name="choice"></param>
    /// <returns></returns>
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
        //在这里结算挑战的物品奖励/惩罚
        if (success)
            ApplyBurdenChanges(choice.burdenItemChangesSuccess);
        else
            ApplyBurdenChanges(choice.burdenItemChangesFailure);


        //结束挑战
        challengeInProgress = false;
        string nextId = success ? choice.nextEventIdSuccess : choice.nextEventIdFailure;
        if (!string.IsNullOrEmpty(nextId))
        {
            StartEvent(nextId);
        }
    }
    /// <summary>
    /// 用于区域列表判断“是否可进入”；入口事件直接可进；否则只要存在至少一个满足 TraitRequirement 的 choice 即可进入。
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public bool CanEnterEvent(NarrativeEvent e)
    {
        if (e == null)
            return false;

        //若标记为“入口事件”或“股市入口”，则默认可进入
        if (e.HasTag(EventTag.Entrypoint) || e.HasTag(EventTag.StockMarketEntry))
            return true;

        //若没有choices，则不可进入（避免空引用）
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
    /// <summary>
    /// singleUse事件（只能触发一次的事件）触发记录与查询。
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    public bool HasTriggered(string eventId)
    {
        return triggeredEventIds.Contains(eventId);
    }

    public void MarkTriggered(string eventId)
    {
        triggeredEventIds.Add(eventId);
    }
    /// <summary>
    /// 设置股市跳转延迟标志，由 tagHandler/其他系统触发。
    /// </summary>
    /// <param name="value"></param>
    public void SetPendingStockMarket(bool value)
    {
        pendingEnterStockMarket = value;
    }
}
