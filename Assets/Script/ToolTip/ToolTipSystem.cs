using UnityEngine;


//目前的tooltip系统用了太多脚本文件了，我觉得这很扯淡。
//提示框系统，未来要将所有ToolTip在这个脚本里进行统一管理关闭与开启。
//暂时未实装
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
