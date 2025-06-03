using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ChallengeSystem : MonoBehaviour
{
    public GameObject challengeUIPanel;
    public Image diceImage;
    public TMP_Text resultText;
    public Sprite[] diceFaces;

    public float animationDuration = 1.5f;

    public void ShowChallengeResult(bool success)
    {
        challengeUIPanel.SetActive(true); //确保先显示面板
        StartCoroutine(ShowChallengeResultCoroutine(success));
    }

    public IEnumerator ShowChallengeResultCoroutine(bool success)
    {
        challengeUIPanel.SetActive(true);
        resultText.text = "";
        resultText.transform.localScale = Vector3.one;
        diceImage.transform.rotation = Quaternion.identity;

        float timer = 0f;
        int index = 0;

        while (timer < animationDuration)
        {
            timer += Time.deltaTime;

            if (diceFaces.Length > 0)
            {
                index = (index + 1) % diceFaces.Length;
                diceImage.sprite = diceFaces[index];
            }

            diceImage.transform.Rotate(0, 0, 30f);
            yield return new WaitForSeconds(0.05f);
        }

        diceImage.transform.rotation = Quaternion.identity;
        resultText.text = success ? "挑战成功！" : "挑战失败…";
        resultText.color = success ? Color.green : Color.red;

        float t = 0f;
        float scaleTime = 0.5f;

        while (t < scaleTime)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(1f, 1.3f, t / scaleTime);
            resultText.transform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }

        t = 0f;
        while (t < scaleTime)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(1.3f, 1f, t / scaleTime);
            resultText.transform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        Debug.Log("准备关闭 ChallengeUIPanel，当前 active 状态：" + challengeUIPanel.activeSelf);
        challengeUIPanel.SetActive(false);
        Debug.Log("动画结束，准备跳转事件");
    }
}