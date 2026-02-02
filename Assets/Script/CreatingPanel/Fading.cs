using UnityEngine;
using UnityEngine.UI;

public class Fading : MonoBehaviour
{
    public float time=1.3f;
    public Image Image;
    // Start is called before the first frame update
    void Start()
    {
        Image img = GetComponent<Image>();
        // 1. 先瞬间设为完全透明
        img.canvasRenderer.SetAlpha(0f);
        // 2. 在 2 秒内渐变到不透明 (1.0f)
        // 参数：目标透明度，持续时间，是否忽略 TimeScale
        img.CrossFadeAlpha(1.0f, time, false);
    }
}
