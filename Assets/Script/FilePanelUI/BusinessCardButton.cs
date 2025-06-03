using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//名片关闭与开启
public class BusinessCardButton : MonoBehaviour
{
    public GameObject businessCardBox_Float;
    public GameObject businessCard;
    public GameObject FloatBusinessCardCloseButton;
    public GameObject BusinessCardCloseButton;
    public void Start()
    {
        businessCardBox_Float.SetActive(false);
        businessCard.SetActive(true);
    }
    public void hideBusinessCardBox_float()
    {
        businessCardBox_Float.SetActive(false);
        businessCard.SetActive(true);
    }
    public void showBusinessCardBox_float()
    {
        businessCardBox_Float.SetActive(true);
        businessCard.SetActive(false);
        
    }

}
