using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCatcherMap : MonoBehaviour
{   //地图页面关闭
    [Header("地图总面板（点击后会关闭它）")]
    public GameObject worldMapPanel;
    public GameObject storyContent;


    public void OnClickCloseMap()
    {
        if (worldMapPanel != null)
        {
            UIManager.Instance.SwitchState(UIManager.UIState.Region);//只能在region状态下打开地图。
        }
    }
}
