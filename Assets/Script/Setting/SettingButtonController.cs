using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButtonController : MonoBehaviour
{
    public Button storySettingButton;
    public GameObject settingPanel;
    public Button settingCloseButton;

    public void OpensettingPanel() 
    {
        AudioManager.Instance.PlaySFX("Setting_OpenClose"); // 播放点击音效
        settingPanel.SetActive(true);
    }
    public void CloseSettingPanel()
    {
        AudioManager.Instance.PlaySFX("Setting_OpenClose"); // 播放点击音效
        settingPanel.SetActive(false);
    }
}
