using TMPro;
using UnityEngine;
using System.Collections;

public class AmbitionButton : MonoBehaviour
{
    public enum AmbitionType { photo, memory, brooch, adventure }
    public TMP_Text AmbitionText;

    [Header("设置这个按钮属于哪种类型")]
    public AmbitionType type;

    public GameObject AmbitionTextBox;

    private CanvasGroup AmbitionCG;
    private void Start()
    {
        
        if (AmbitionTextBox != null)
        {
            AmbitionCG=AmbitionTextBox.GetComponent<CanvasGroup>();
            AmbitionTextBox.SetActive(false);
        }
    }
    public void ContinueTelling()
    {
        // 1. 更新文本（不管点没点过，只要点了就更新文本）
        ChangeAmbitionText();
        if (AmbitionTextBox != null)
        {
            AmbitionTextBox.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(AmbitionCGFading());
        }
    }

    private void ChangeAmbitionText()
    {
        switch (type)
        {
            case AmbitionType.photo:
                AmbitionText.text = "我摸了摸口袋，锋利的侧锋划过我的手指\r\n我从口袋里挑出一张满是斑痕的照片，上面有一位衣冠楚楚的绅士和一位年轻的女士，他们站在一栋大房子前，房子遍是一片大湖，阳光明媚。\r\n我被告知那是你的父母。我从未见过他们，\r\n但那个房子是我儿时梦想和暗恋的地方。\r\n找到那里，找到他们。";
                break;
            case AmbitionType.brooch:
                AmbitionText.text = " 我的注意力被桌上的报纸吸引，上面写着又一位著名的探险者在紫林失踪\r\n一个荒唐的梦：我总是幻想双子城外的地方。那片遍布整片大陆的紫色树林，它被人们视作禁忌，也不被允许进入。但我去过。梦里，我和另一个人一起进入那片林地，翻越废旧的轨道站，看见了沉没在水库底部的车站大厅。\r\n我什么也没带回来。但我发誓，那是最真实的一个下午。";
                break;

            case AmbitionType.adventure:
                AmbitionText.text = "办公桌上放着一枚廉价的胸针，有粗糙的纹路和张扬的流苏\r\n昨夜的晚宴，一位佩戴胸针的男士向我举杯示意。出于礼节，我端着两杯酒走过去。我不属于那个地方，但我学会了如何让人以为我属于。\r\n当我走至他的身旁，他低头笑了一下，在我耳边轻声说： “我不知道你是怎么混进来的，我可没在总部见过你。你最好两分钟内离开，不然我就叫保安。”\r\n看了一眼他的胸针，我转身离开。我永远记得那枚胸针的形状。";
                break;
            case AmbitionType.memory:
                AmbitionText.text = "我曾在一间破旧失修的诊所醒来，手腕上绑着一根带金属片的绷带，上面印着编号：柱–313–柱。医生说这只是旧版病历标签，我很清楚不是。\r\n我开始在公司的档案系统里查找、拼接、对比。每个数字，每个“柱”后的空格都像一道门。\r\n我不知道编号背后是什么，但我确信那绝不是病——那是我要寻找的答案。";
                break;
        }

    }

    IEnumerator AmbitionCGFading()
    {
        if (AmbitionCG == null) yield break;
        while (AmbitionCG.alpha < 1f)
        {
            AmbitionCG.alpha += Time.deltaTime * 2f;
            yield return null;
        }
        AmbitionCG.alpha = 1f;
    }
}
