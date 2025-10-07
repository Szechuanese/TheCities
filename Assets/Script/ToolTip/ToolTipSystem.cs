using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//提示框系统，未来要将所有ToolTip在这个脚本里进行统一管理关闭与开启。
//目前未实装
public class ToolTipSystem : MonoBehaviour
{
    //单例化
    private static ToolTipSystem toolTipSystem;
    //绑定提示框
    public ToolTip toolTip;
    //引用唤醒
    public void Awake()
    {
        toolTipSystem=this;
    }

    public static void Show()
    {
        toolTipSystem.toolTip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        toolTipSystem.toolTip.gameObject.SetActive(false);

    }
}
