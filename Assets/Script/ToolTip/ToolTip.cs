using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//ToolTIp系统,控制排版
public class ToolTip : MonoBehaviour
{
    //绑定组件
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI descriptionField;
    public LayoutElement layoutElement;
    //确认字体包裹限制;
    public int characterWrapLimit;


    private void Update()
    {
        int headerLength = headerField.text.Length;
        int descriptionLength = descriptionField.text.Length;

        //三目判断，十分简洁。
        layoutElement.enabled = (headerLength > characterWrapLimit || descriptionLength > characterWrapLimit)
            ? true : false;
    }
}

