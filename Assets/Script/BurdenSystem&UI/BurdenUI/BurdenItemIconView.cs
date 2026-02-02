using UnityEngine;
using UnityEngine.UI;

public class BurdenItemIconView : MonoBehaviour
{
    public Image icon;

    public void SetSprite(Sprite sp)
    {
        if (icon == null) return;
        icon.sprite = sp;
        icon.enabled = (sp != null);
    }
}
