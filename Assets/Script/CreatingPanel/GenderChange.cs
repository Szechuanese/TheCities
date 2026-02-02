using System;
using UnityEngine;
using UnityEngine.UI;

public class GenderChange : MonoBehaviour
{
    public Button b;
    public Image target;
    public Sprite woman;
    public Sprite man;
    public void ChangeImage()
    {
        if (target != null)
        {
            if (target.sprite == man)
            {
                target.sprite = woman;
            }
            else if(target.sprite==woman)
            {
                target.sprite = man;
            }
            else 
            {
                target.sprite = man;
            }
        }
    }

}
