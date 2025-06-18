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
                    originDescription.text = "�㱻�̻�����������Ҫ�Լ�Ѱ������";
                }
                else if (val.id == "Origin_Autie")
                {
                    originTitle.text = val.displayName;
                    originDescription.text = "�㱻�����������̻�����ȥ�質��д����ȥ����";
                }
                else if (val.id == "Origin_Corporate")
                {
                    originTitle.text = val.displayName;
                    originDescription.text = "�㾭����ս�������ɡ���Ч���Ѱ���ծ���壬������ֻΪ�Լ����";
                }
                else if (val.id == "Origin_Streets")
                {
                    originTitle.text = val.displayName;
                    originDescription.text = "�������ң�������������û�б��˵Ľݾ���Ҳû�б��˵ĸ�����";
                }
                else if (val.id == "Ambition_Home")
                {
                    ambitionTitle.text = val.displayName;
                    ambitionDescription.text = "�����ǲ��Ծ��������������Ƭ���ڴ����ܲ�����һ���Ĵ��С�";
                }
                else if (val.id == "Ambition_Fortuna")
                {
                    ambitionTitle.text = val.displayName;
                    ambitionDescription.text = "����ĵõ����ء����ۡ����ӡ���ʹ��·������ա�";
                }
                else if (val.id == "Ambition_Adventure")
                {
                    ambitionTitle.text = val.displayName;
                    ambitionDescription.text = "�㳣˳��¥��ķ�����ʧ�����ɽ����֪�����Լ����������Ǻ��档";
                }
                else if (val.id == "Ambition_Truth")
                {
                    ambitionTitle.text = val.displayName;
                    ambitionDescription.text = "����Ȼ������ҹ���ѣ���Һ��͸���������������ʱ���㷢�ֵֽ���ɫ�ʷ����˱仯��";
                }
            }
        }
        }
    }
