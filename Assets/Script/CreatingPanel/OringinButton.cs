using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class OringinButton : MonoBehaviour
{
    // 定义后果类型
    public enum OringinType { Orphan, Streets, Coporate, Autie }
    public TMP_Text OringinText;

    [Header("设置这个按钮属于哪种类型")]
    public OringinType type;

    public GameObject AmbitionButtonBox;
    public GameObject OringinTextBox;
    private CanvasGroup AmbitionButtonBoxCG;
    private CanvasGroup OringinCG;

    public Image targetImg;
    public Sprite newSprite;

    // 使用实例变量代替静态变量，配合逻辑实现全局一次性锁定
    private bool isImageChangeLocked = false;

    private void Awake()//这里我修改使用Awake来初始化，
                        //确保在任何 Start 之前就设置好初始状态,因为使用Start似乎不能成功，导致我没法修改UI。
    {
        if (AmbitionButtonBox != null)
        {
            AmbitionButtonBoxCG = AmbitionButtonBox.GetComponent<CanvasGroup>();
            AmbitionButtonBoxCG.alpha = 0f;
            AmbitionButtonBox.SetActive(false);
        }

        if (OringinTextBox != null)
        {
            OringinCG = OringinTextBox.GetComponent<CanvasGroup>();
            OringinCG.alpha = 0f;
            OringinTextBox.SetActive(false);
        }
    }
    //void Start()
    //{
    //    if (AmbitionButtonBox != null)
    //    {
    //        AmbitionButtonBoxCG = AmbitionButtonBox.GetComponent<CanvasGroup>();
    //        AmbitionButtonBoxCG.alpha = 0f;
    //        AmbitionButtonBox.SetActive(false);
    //    }

    //    if (OringinTextBox != null)
    //    {
    //        OringinCG = OringinTextBox.GetComponent<CanvasGroup>();
    //        OringinCG.alpha = 0f;
    //        OringinTextBox.SetActive(false);
    //    }
    //}

    public void ContinueTelling()
    {
        // 1. 更新文本（此功能不被锁定，玩家可以自由点击切换查看不同文本）
        ChangeOringinText();

        // 2. 尝试执行换图逻辑（只有在未锁定时才会执行第一次）
        if (!isImageChangeLocked)
        {
            ExecuteGlobalImageChange();
        }

        // 3. 处理显示逻辑（渐变效果始终执行）
        if (AmbitionButtonBox != null || OringinTextBox != null)
        {
            AmbitionButtonBox.SetActive(true);
            OringinTextBox.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(FadeIn());
            StartCoroutine(OringinCGFadeIn());
        }
    }

    private void ExecuteGlobalImageChange()
    {
        // 执行当前按钮对应的换图动作
        if (targetImg != null && newSprite != null)
        {
            targetImg.sprite = newSprite;
        }

        // 核心：找到场景中所有挂载 OringinButton 脚本的对象
        OringinButton[] allButtons = Object.FindObjectsOfType<OringinButton>();

        // 将它们全部锁定，确保之后点击任何按钮都不会再触发 ExecuteGlobalImageChange
        foreach (var btn in allButtons)
        {
            btn.isImageChangeLocked = true;
        }
    }

    private void ChangeOringinText()
    {
        switch (type)
        {
            case OringinType.Orphan:
                OringinText.text = "我是被教会抚养长大的孤儿。" +
                    "那位老师把我带进恒辩庭，在知识与争辩中度过童年。" +
                    "待我长到与他齐肩，根据社会慈善法，我被例行驱逐出校。";
                break;
            case OringinType.Streets:
                OringinText.text = "我无依无靠，自己在街头活了下来。靠着吹口哨学鸟叫、往收音机里塞纸条、替醉汉写道歉信之类的手艺，我混进了不少地方，" +
                    "也混过不少人。\r\n有一天，我站在联合果品大楼的影子下，" +
                    "对着一台自动门打了个喷嚏。" +
                    "门开了。我走了进去，没人拦。几分钟后，" +
                    "我坐在一间办公室里，手里拿着写有我名字的工作卡。" +
                    "没有人知道我怎么进来的，包括我自己。于是我成了联合食品第七部门的一名正式员工。";
                break;
            case OringinType.Coporate:
                OringinText.text = "我曾是联合食品孤儿基金会的“试点案例”之一。长成了个高大的小伙子，结实、安静、听话。他们送你上了9月9日的战场——不在前线，在后勤。战后，我便加入了公司。";
                break;
            case OringinType.Autie:
                OringinText.text = "我姨妈养育我直到接受了完整的教育，能熟练掌握辩论与写作。\r\n前不久，她因病去世。银行收走了她的房子。给我下了最后通牒,即刻搬出阁楼。\r\n好在，她临走前托人给我找了份不错的工作，就在联合食品。";
                break;
        }
    }

    IEnumerator FadeIn()
    {
        if (AmbitionButtonBoxCG == null) yield break;
        while (AmbitionButtonBoxCG.alpha < 1f)
        {
            AmbitionButtonBoxCG.alpha += Time.deltaTime * 2f;
            yield return null;
        }
        AmbitionButtonBoxCG.alpha = 1f;
    }

    IEnumerator OringinCGFadeIn()
    {
        if (OringinCG == null) yield break;
        while (OringinCG.alpha < 1f)
        {
            OringinCG.alpha += Time.deltaTime * 2f;
            yield return null;
        }
        OringinCG.alpha = 1f;
    }
}
