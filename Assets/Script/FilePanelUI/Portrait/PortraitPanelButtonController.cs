using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PortraitPanelButtonController : MonoBehaviour
{
    public Button openButton;
    public Button closeButton;
    public GameObject portraitPanel;
    public void OpenPortraitPanel()
    {
        AudioManager.Instance.PlaySFX("Setting_OpenClose"); // 播放点击音效
        portraitPanel.SetActive(true);
    }
    public void ClosePortraitPanel()
    {
        AudioManager.Instance.PlaySFX("Setting_OpenClose"); // 播放点击音效
        portraitPanel.SetActive(false);
    }
}
