using UnityEngine;

[CreateAssetMenu(fileName = "NewRegionData", menuName = "TwoCities/RegionData")]
public class RegionData : ScriptableObject
{
    public string regionId;
    public string regionDisplayName;
    public string regionDescription;
    public NarrativeEvent[] regionEvents;
}
