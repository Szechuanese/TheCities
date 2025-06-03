using System.Collections.Generic;
using UnityEngine;
using TMPro;  // 确保引入 TMPro


[CreateAssetMenu(menuName = "Stock/CompanyStock")]
public class CompanyStock : ScriptableObject
{
    [Header("公司股票信息")]
    public long totalShares = 1000000;     //总股本（单位：股）
    public string stockId;  //公司ID
    public string displayName; //显示名称
    public Sprite logo;  //图片
    public float volatility;   //波动率
    public float dropBias;   //跌幅
    public float currentPrice = 100f; //目前价格？

    public float marketCap => currentPrice * totalShares;  // 市值（计算属性）
    [System.Serializable]//预设股东
    public class ShareholderEntry
    {
        public string name;
        [Range(0f, 1f)]
        public float percentage;  // 占比（比例形式）
    }

    [Header("初始股东结构（不包含玩家）")]
    public List<ShareholderEntry> defaultShareholders = new List<ShareholderEntry>();
}