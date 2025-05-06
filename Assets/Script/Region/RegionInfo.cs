using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RegionInfo
{
    public string id;                 // 用于跳转事件或判断解锁
    public string displayName;       // 显示名（可用于悬停提示）
    [TextArea] public string description; // 区域描述（Tooltip）
    public bool isUnlocked = true;   // 是否解锁
    public Button regionButton;      // 绑定的按钮对象
    public RegionData regionData;  //引用
}
