using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimationSelectLog : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public UnityEngine.UI.Image APageWord;
    private Coroutine fillRoutine;
    public float duration = 1f;
    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX("Log_Hover");
        if (fillRoutine != null) StopCoroutine(fillRoutine);
        fillRoutine = StartCoroutine(StartWriting(duration));
    }
    public void OnDeselect(BaseEventData eventData)
    {
        if (fillRoutine != null) StopCoroutine(fillRoutine);
        if (APageWord != null) APageWord.fillAmount = 0f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //设置选中对象为当前按钮
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //清除选中对象
        eventData.selectedObject = null;
    }
    private IEnumerator StartWriting(float duration)
    {
        if (APageWord == null) yield break;

        APageWord.fillAmount = 0f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            APageWord.fillAmount = Mathf.Clamp01(t / duration);
            yield return null;
        }

        APageWord.fillAmount = 1f;
    }
}
