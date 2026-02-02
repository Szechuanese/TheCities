using UnityEngine;
using UnityEngine.UI;

public class CloseBurdenPop :MonoBehaviour
{
    public Button CloseButton;
    public GameObject BurdenPop;

    public void Close()
    {
        BurdenPop.SetActive(false);
    }
}
