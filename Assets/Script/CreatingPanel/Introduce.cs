using System.Collections;
using TMPro;
using UnityEngine;

public class Introduce : MonoBehaviour
{
    public GameObject IntroduceBox;
    private CanvasGroup IntroduceCG;
    public TMP_Text IntroduceText; // 必须是 TextMeshPro 组件
    public float typeSpeed = 0.05f; // 打字速度，越小越快
    public GameObject OringinButtonBox;
    private CanvasGroup OringinCG;

    private void Start()
    {
        if (IntroduceBox != null || OringinButtonBox != null)
        {
            IntroduceBox.SetActive(true);
            IntroduceCG = IntroduceBox.GetComponent<CanvasGroup>();
            OringinCG = OringinButtonBox.GetComponent<CanvasGroup>();

            if (IntroduceCG != null) IntroduceCG.alpha = 0;
            if (OringinCG != null) OringinCG.alpha = 0;

            // 初始保持隐藏
            OringinButtonBox.SetActive(false);

            if (IntroduceText != null)
            {
                IntroduceText.maxVisibleCharacters = 0;
            }

            StopAllCoroutines();
            // 只启动这一个总控协程
            StartCoroutine(IntroduceSequence());
        }
    }

    // 使用一个序列协程来控制逻辑先后
    IEnumerator IntroduceSequence()
    {
        // 1. 执行背景渐变和打字
        yield return StartCoroutine(IntroduceBoxFadingAndWrite());

        // 2. 打字机完成后，等待一小会儿（可选，增加节奏感）
        yield return new WaitForSeconds(0.5f);

        // 3. 执行按钮渐变
        yield return StartCoroutine(OringinCGFading());
    }

    IEnumerator IntroduceBoxFadingAndWrite()
    {
        IntroduceText.ForceMeshUpdate();
        int totalCharacters = IntroduceText.textInfo.characterCount;
        IntroduceText.maxVisibleCharacters = 0;

        // 背景渐变
        while (IntroduceCG.alpha < 1f)
        {
            IntroduceCG.alpha += Time.deltaTime * 3f;
            yield return null;
        }
        IntroduceCG.alpha = 1f;

        yield return new WaitForSeconds(0.2f);

        // 打字机
        for (int i = 0; i <= totalCharacters; i++)
        {
            IntroduceText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    IEnumerator OringinCGFading()
    {
        OringinButtonBox.SetActive(true);
        while (OringinCG.alpha < 1f)
        {
            OringinCG.alpha += Time.deltaTime * 3f;
            yield return null;
        }
        OringinCG.alpha = 1f;
    }
}
