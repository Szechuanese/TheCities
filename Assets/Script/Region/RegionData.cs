using UnityEngine;

[CreateAssetMenu(fileName = "NewRegionData", menuName = "TwoCities/RegionData")]
public class RegionData : ScriptableObject
{
    public string regionId;          //����ID
    public string regionDisplayName;  //��ʾ��
    public string regionDescription;//��������
    public NarrativeEvent[] regionEvents; //�����¼�
}
