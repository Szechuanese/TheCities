
// 点击地图以外区域关闭 WorldMapPanel

using UnityEngine;

public class ClickCatcher : MonoBehaviour
{//地图页面关闭
    [Header("地图总面板（点击后会关闭它）")]
    public GameObject worldMapPanel;
    public GameObject storyContent;


    public void OnClickCloseMap()
    {
        if (worldMapPanel != null)
        {
            worldMapPanel.SetActive(false);
        }
    }
}
