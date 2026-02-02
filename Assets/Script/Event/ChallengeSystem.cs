using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
//迟早会改，目前是控制骰子转动，但是实在是不堪其用
//我认为，页面切换的逻辑，点击挑战触发按钮，等待一下，然后进入挑战失败或成功的页面——
//——这个逻辑没有问题，问题只在于动画的方式，挑战页面应该有Value数值的高低变化，技能条变化等等。让玩家看到。
public class ChallengeSystem : MonoBehaviour
{
    public GameObject challengeUIPanel;
    public Image diceImage;
    public TMP_Text resultText;

    //数组化diceFaces（图片）
    public Sprite[] diceFaces;


    //动画持续时间。
    public float animationDuration = 1.5f;

    public void ShowChallengeResult(bool success)
    {
        //确保先显示面板
        challengeUIPanel.SetActive(true); 
        StartCoroutine(ShowChallengeResultCoroutine(success));
    }


    public IEnumerator ShowChallengeResultCoroutine(bool success)
    {
        challengeUIPanel.SetActive(true);
        resultText.text = "";
        resultText.transform.localScale = Vector3.one;
        diceImage.transform.rotation = Quaternion.identity;

        float timer = 0f;
        //内部计算数，用于确定骰子图片，点数。
        int index = 0;

        while (timer < animationDuration)
        {
            timer += Time.deltaTime;

            if (diceFaces.Length > 0)
            {
                //计算数=计算数+1余骰子图片的长度
                index = (index + 1) % diceFaces.Length;
                //骰子图片=第计算数张骰子图片（计算数）
                diceImage.sprite = diceFaces[index];
            }
            //旋转图片
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