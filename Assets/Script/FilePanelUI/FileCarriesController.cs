using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FileCarriesController : MonoBehaviour
{
    public TMP_Text originTitle;
    public TMP_Text originDescription;
    public TMP_Text ambitionTitle;
    public TMP_Text ambitionDescription;

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
                }
                else if (val.id == "Origin_Autie")
                {
                    originTitle.text = val.displayName;
                    originDescription.text = "你被姨妈养大，她教会了你去歌唱、写作，去爱。";
                }
                else if (val.id == "Origin_Corporate")
                {
                    originTitle.text = val.displayName;
                    originDescription.text = "你经历过战争、纪律、绩效，已把命债结清，现在你只为自己而活。";
                }
                else if (val.id == "Origin_Streets")
                {
                    originTitle.text = val.displayName;
                    originDescription.text = "你白手起家，自力更生，你没有别人的捷径，也没有别人的负担。";
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
