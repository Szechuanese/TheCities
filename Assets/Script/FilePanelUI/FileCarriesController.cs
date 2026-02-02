using UnityEngine;
using TMPro;
//这个脚本只用于File上右方的Carries面板。

public class FileCarriesController : MonoBehaviour
{
    public TMP_Text originTitle;
    public TMP_Text originDescription;
    public TMP_Text ambitionTitle;
    public TMP_Text ambitionDescription;
    public TMP_Text homeName;
    public TMP_Text homeDescription;

    public ValueSystem valueSystem;
    private void Start()
    {
        if (valueSystem != null)
            valueSystem.OnValueChanged += generateBearDescription;
            generateBearDescription();
    }
    private void OnEnable()
    {
        if (valueSystem != null)
            valueSystem.OnValueChanged += generateBearDescription;
    }

    private void OnDisable()
    {
        if (valueSystem != null)
            valueSystem.OnValueChanged -= generateBearDescription;
    }
    public void generateBearDescription() 
    {
        foreach (var val in valueSystem.GetValuesByType(ValueType.Bear)) 
        {
            if (val.value >= 1f)
            {
                if (val.id == "Origin_Orphan")
                {
                    originTitle.text = val.displayName;
                    originDescription.text = "你被教会养大，现在你要自己寻找真理。";
                    homeName.text = "孤儿院的地下室";
                    homeName.text = "老旧的孤儿院";
                    //下面这串描述应该用于人情 flavor
                    homeDescription.text = "";
                }
                else if (val.id == "Origin_Autie")
                {
                    originTitle.text = val.displayName;
                    originDescription.text = "你被姨妈养大，她教会了你去歌唱、写作，去爱。";
                    homeName.text = "未陶先生的单身公寓";
                    //下面这串描述应该用于人情 flavor
                    //homeDescription.text = "未陶先生是你姨妈的好朋友，你印象中姨妈是个悲观的人，但只要未陶先生一出现，" +
                    //    "她的脸上就有了笑容，他对你很好，但嘱咐你，不要碰房里的东西。";
                }
                else if (val.id == "Origin_Corporate")
                {
                    originTitle.text = val.displayName;
                    originDescription.text = "你经历战争、纪律、绩效，已把命债结清，现在你只为自己而活。";
                    homeName.text = "联合食品第五宿舍";
                    homeDescription.text = "";
                    
                }
                else if (val.id == "Origin_Streets")
                {
                    originTitle.text = val.displayName;
                    originDescription.text = "你白手起家，自力更生，你没有别人的捷径，也没有别人的负担。";
                    homeName.text = "一处废弃工厂";
                }
                else if (val.id == "Ambition_Home")
                {
                    ambitionTitle.text = val.displayName;
                    ambitionDescription.text = "你总是不自觉地揉搓那张老照片，期待那能产生不一样的触感。";
                }
                else if (val.id == "Ambition_Fortuna")
                {
                    ambitionTitle.text = val.displayName;
                    ambitionDescription.text = "你决心得到尊重、艳羡、服从。即使这路万般凶险。";
                }
                else if (val.id == "Ambition_Adventure")
                {
                    ambitionTitle.text = val.displayName;
                    ambitionDescription.text = "你常顺着楼宇的方向望失温岭的山雾，你知道，自己的命运在那后面。";
                }
                else if (val.id == "Ambition_Truth")
                {
                    ambitionTitle.text = val.displayName;
                    ambitionDescription.text = "你依然会在深夜惊醒，汗液浸透你的衣衫，看向窗外时，你发现街道的色彩发生了变化。";
                }
            }
        }
        }
    }
