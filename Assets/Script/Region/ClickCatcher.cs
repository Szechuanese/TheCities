// ✅ ClickCatcher.cs
// 点击地图以外区域关闭 WorldMapPanel

using UnityEngine;

public class ClickCatcher : MonoBehaviour
{
    [Header("地图总面板（点击后会关闭它）")]
    public GameObject worldMapPanel;

    public void OnClickCloseMap()
    {
        if (worldMapPanel != null)
        {
            worldMapPanel.SetActive(false);
        }
    }
}
