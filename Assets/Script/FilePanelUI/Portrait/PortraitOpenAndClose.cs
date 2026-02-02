using UnityEngine;
using UnityEngine.UI;

public class PortraitOpenAndClose : MonoBehaviour
{
    [Header("绑定组件")]
    public GameObject portraitPanel;
    public GameObject clickCatcherP;
    public Button closeButton;
    public Button openButton;

    public void OpenPortraitPanel()
    {
        AudioManager.Instance.PlaySFX("Setting_OpenClose"); // 播放点击音效
        clickCatcherP.SetActive(true);
        portraitPanel.SetActive(true);
    }    
    public void ClickCatcherClose()
    {
       AudioManager.Instance.PlaySFX("Setting_OpenClose"); // 播放点击音效
       clickCatcherP.SetActive(false);
       portraitPanel.SetActive(false);
    }
    public void CloseButtonCLose()
    {
        AudioManager.Instance.PlaySFX("Setting_OpenClose"); // 播放点击音效
        clickCatcherP.SetActive(false);
        portraitPanel.SetActive(false);
    }
}
