using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RegionInfo
{
    public string id;                 // ������ת�¼����жϽ���
    public string displayName;       // ��ʾ������������ͣ��ʾ��
    [TextArea] public string description; // ����������Tooltip��
    public bool isUnlocked = true;   // �Ƿ����
    public Button regionButton;      // �󶨵İ�ť����
    public RegionData regionData;  //����
}
