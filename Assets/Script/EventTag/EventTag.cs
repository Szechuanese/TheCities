//定义所有 NarrativeEvent每一个Event的枚举型标签

/// <summary>
/// 事件卡片枚举
/// </summary>
public enum EventTag
{
    None = 0,            
    /// <summary>
    /// 可以返回到区域面板，实现
    /// </summary>
    Returnable = 1 << 0,       // 1
    /// <summary>
    /// 入口点Tag，通常是故事的起始点或关键节点，实现
    /// </summary>
    Entrypoint = 1 << 1,       // 2
    /// <summary>
    /// 战斗Tag，未实现
    /// </summary>
    Combat = 1 << 2,       // 4
    /// <summary>
    /// 上锁Tag，未实现
    /// </summary>
    Locked = 1 << 3,       // 8
    /// <summary>
    /// 可重复Tag，未知是否实现
    /// </summary>
    Repeatable = 1 << 4,     // 16
    /// <summary>
    /// 重要Tag，未知是否实现
    /// </summary>
    Important = 1 << 5,
    /// <summary>
    /// 特殊Tag，股市入点，实现
    /// </summary>
    StockMarketEntry = 1 << 6, // 64
}
