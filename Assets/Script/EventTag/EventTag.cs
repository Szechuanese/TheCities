//定义所有 NarrativeEvent 的枚举型标签

public enum EventTag
{
    None = 0,            // 显式保留 None
    Returnable = 1 << 0,       // 1
    Entrypoint = 1 << 1,       // 2
    Combat = 1 << 2,       // 4
    Locked = 1 << 3,       // 8
    Repeatable = 1 << 4,     // 16
    Important = 1 << 5,
    StockMarketEntry = 1 << 6, // 64
}
