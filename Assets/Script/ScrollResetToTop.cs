using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollResetToTop : MonoBehaviour
{
    public ScrollRect storyScrollRect;
    public ScrollRect fileScrollRect;
    public float delayFrames = 3f;

    public void ResetScrollToTop()
    {
        StartCoroutine(ForceScrollToTopAfterFrames());
    }

    private IEnumerator ForceScrollToTopAfterFrames()
    {
        // 等待若干帧以确保内容加载与布局完成
        for (int i = 0; i < delayFrames; i++)
        {
            yield return new WaitForEndOfFrame();
        }

        Canvas.ForceUpdateCanvases();

        if (storyScrollRect != null)
        {
            storyScrollRect.verticalNormalizedPosition = 1f;
            Debug.Log("故事页面滚动条归位顶部");
        }
        else
        {
            Debug.LogWarning("未绑定 storyScrollRect！");
        }

        if (fileScrollRect != null)
        {
            fileScrollRect.verticalNormalizedPosition = 1f;
            Debug.Log("File 页面滚动条归位顶部");
        }
        else
        {
            Debug.LogWarning("未绑定 fileScrollRect！");
        }
    }
}