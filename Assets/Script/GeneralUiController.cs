using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GeneralUiController : MonoBehaviour
{
    //CanvasÍ³Ò»¿ØÖÆ
    public Canvas stockMarketCanvas;
    public Canvas mapCanvas;
    public Canvas storyCanvas;
    void Start()
    {
        stockMarketCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }
}
