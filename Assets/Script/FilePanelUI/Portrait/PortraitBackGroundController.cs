using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//根据当前头像切换背景
public class PortraitBackGroundController : MonoBehaviour
{
    public List<Image> HeadShotBackGrounds= new List<Image>();
    public enum E_BackGround
    {
        striveHood,
        suitMan,
        soldier,
        shepherdGirl,
        maiden,
        gentleMan,
        detective,
        CR
    }
    [Header("背景图片")]
    public Sprite striveHoodSprite;
    public Sprite suitManSprite;
    public Sprite soldierSprite;
    public Sprite shepherdGirlSprite;
    public Sprite maidenSprite;
    public Sprite gentleManSprite;
    public Sprite detectiveSprite;
    public Sprite CRSprite;
    public void ChangeBackGround(E_BackGround backGround)
    {
        if (HeadShotBackGrounds == null || HeadShotBackGrounds.Count == 0)
        {
            Debug.LogWarning("[PortraitBackGroundController] HeadShotBackGrounds 为空，请在 Inspector 里拖入至少一个背景 Image。");
            return;
        }

        // 1. 先根据枚举，选出这次要用的 Sprite
        Sprite target = null;

        switch (backGround)
        {
            case E_BackGround.striveHood:
                target = striveHoodSprite;
                break;
            case E_BackGround.gentleMan:
                target = gentleManSprite;
                break;
            case E_BackGround.shepherdGirl:
                target = shepherdGirlSprite;
                break;
            case E_BackGround.maiden:
                target = maidenSprite;
                break;
            case E_BackGround.suitMan:
                target = suitManSprite;
                break;
            case E_BackGround.soldier:
                target = soldierSprite;
                break;
            case E_BackGround.detective:
                target = detectiveSprite;
                break;
            case E_BackGround.CR:
                target = CRSprite;
                break;
        }

        if (target == null)
        {
            Debug.LogWarning($"[PortraitBackGroundController] {backGround} 对应的 Sprite 为空，请在 Inspector 里赋值。");
            return;
        }

        //把这个Sprite 应用到列表里的每一个背景 Image 上
        foreach (var img in HeadShotBackGrounds)
        {
            if (img == null) continue;

            img.sprite = target;
            img.type = Image.Type.Simple;
            img.preserveAspect = true;
            img.enabled = true;
        }

        Debug.Log($"[PortraitBackGroundController] 已切换背景到：{backGround}，共更新 {HeadShotBackGrounds.Count} 个 Image。");
    }
    public void ChangeBackGroundByPortraitId(string portraitId)
    {
        switch (portraitId)
        {
            case "StriveHoodMan":
                ChangeBackGround(E_BackGround.striveHood);
                break;
            case "GentleMan":
                ChangeBackGround(E_BackGround.gentleMan);
                break;
            case "ShepherdGirl":
                ChangeBackGround(E_BackGround.shepherdGirl);
                break;
            case "Maiden":
                ChangeBackGround(E_BackGround.maiden);
                break;
            case "SuitMan":
                ChangeBackGround(E_BackGround.suitMan);
                break;
            case "Soldier":
                ChangeBackGround(E_BackGround.soldier);
                break;
            case "Detective":
                ChangeBackGround(E_BackGround.detective);
                break;
            case "CR":
                ChangeBackGround(E_BackGround.CR);
                break; 
        }
    }
}
