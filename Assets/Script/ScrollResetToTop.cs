using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollResetToTop : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform contentTransform;
    public float delayFrames = 3f;

    public void ResetScrollToTop()
    {
        StartCoroutine(ForceScrollToTopAfterFrames());
    }

    private IEnumerator ForceScrollToTopAfterFrames()
    {
        // 等待固定帧数（比判断高度更稳妥）
        for (int i = 0; i < delayFrames; i++)
        {
            yield return new WaitForEndOfFrame();
        }

        Canvas.ForceUpdateCanvases(); // 强制更新布局
        scrollRect.verticalNormalizedPosition = 1f;

        Debug.Log("⏫ ScrollRect 强制滚动到顶部");
    }
}