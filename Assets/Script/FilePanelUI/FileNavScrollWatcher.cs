using UnityEngine;
using UnityEngine.UI;

public class FileNavScrollWatcher : MonoBehaviour
{//控制filePanel触发浮动界面
    public ScrollRect scrollRect;
    public GameObject fileNavBox;
    public GameObject fileNavBoxF;

    private bool isAtTop = false;

    void Update()
    {
        if (scrollRect == null) return;

        // verticalNormalizedPosition == 1 表示滑动到了顶部
        bool nowAtTop = scrollRect.verticalNormalizedPosition >= .45f;//加一个ValueBox就加0.5

        if (nowAtTop && !isAtTop)
        {
            isAtTop = true;
            fileNavBox.SetActive(false);
            fileNavBoxF.SetActive(true);
            Debug.Log("滑动到顶：fileNavBox 隐藏，fileNavBoxF 显示");
        }
        else if (!nowAtTop && isAtTop)
        {
            isAtTop = false;
            fileNavBox.SetActive(true);
            fileNavBoxF.SetActive(false);
            Debug.Log("滑动离开顶端：fileNavBox 显示，fileNavBoxF 隐藏");
        }
    }
}
