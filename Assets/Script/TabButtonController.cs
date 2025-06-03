using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class TabButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image background;
    public TMP_Text labelText;
    public LayoutElement layoutElement;

    private Color normalColor = new Color32(22, 24, 34, 255);
    private Color hoverColor = new Color32(40, 45, 70, 255); //#282D46
    private Color activeTextColor = new Color32(200, 200, 255, 255);//亮黄色(187, 202, 70, 255)
    private Color normalTextColor = new Color32(160, 160, 160, 255);

    private bool isActive = false;

    void Start()
    {
        SetInactiveStyle();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isActive)
        {
            background.color = hoverColor;
            layoutElement.preferredWidth = 120;
            labelText.alignment = TextAlignmentOptions.MidlineRight;
            labelText.color = activeTextColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isActive)
        {
            SetInactiveStyle();
        }
    }

    public void SetActiveStyle()
    {
        isActive = true;
        background.color = hoverColor;
        layoutElement.preferredWidth = 120;
        labelText.alignment = TextAlignmentOptions.MidlineRight;
        labelText.color = activeTextColor;
    }

    public void SetInactiveStyle()//未选中默认样式
    {
        isActive = false;
        background.color = normalColor;
        layoutElement.preferredWidth = 100;
        labelText.alignment = TextAlignmentOptions.Midline;
        labelText.color = normalTextColor;
    }
    public TabManager tabManager; // 在 Inspector 中手动绑定

    public enum TabType { Story, File, Authoriz, Burden }
    public TabType tabType;

    public void OnClick()
    {
        switch (tabType)
        {
            case TabType.Story:
                tabManager.ShowStoryPanel(this);
                break;
            case TabType.File:
                tabManager.ShowFilePanel(this);
                break;
            case TabType.Authoriz:
                tabManager.ShowAuthorizPanel(this);
                break;
            case TabType.Burden:
                tabManager.ShowBurdenPanel(this);
                break;
        }
    }
}
