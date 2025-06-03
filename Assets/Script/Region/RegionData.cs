using UnityEngine;

[CreateAssetMenu(fileName = "NewRegionData", menuName = "TwoCities/RegionData")]
public class RegionData : ScriptableObject
{
    public string regionId;          //地区ID
    public string regionDisplayName;  //显示名
    public string regionDescription;//地区描述
    public NarrativeEvent[] regionEvents; //地区事件
}
