using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BurdenEquipOptionView : MonoBehaviour
{
    public Button button;
    public Image icon;
    public TMP_Text title;

    private BurdenItemRuntime runtime;
    private Action<BurdenItemRuntime> onClick;

    public void Bind(BurdenItemRuntime item, Action<BurdenItemRuntime> click)
    {
        runtime = item;
        onClick = click;

        if (title != null) title.text = item.definition != null ? item.definition.displayName : item.id;
        if (icon != null) icon.sprite = item.definition != null ? item.definition.icon : null;

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick?.Invoke(runtime));
        }
    }
}

