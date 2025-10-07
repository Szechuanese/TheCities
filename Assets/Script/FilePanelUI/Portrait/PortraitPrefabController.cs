using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitPrefabController : MonoBehaviour
{
    public GameObject portraitPrefab;
    public string portraitID;
    public Image portraitImage;
    public void GetPortraitid(string id,Sprite portraitIcon)
    {
        portraitID = id;
        if (portraitImage != null && portraitIcon != null)
            portraitImage.sprite = portraitIcon;    
    }
}
