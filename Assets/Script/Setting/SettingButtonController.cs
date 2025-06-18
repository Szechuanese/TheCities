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
        settingPanel.SetActive(true);
    }
    public void CloseSettingPanel()
    {
        settingPanel.SetActive(false);
    }
}
