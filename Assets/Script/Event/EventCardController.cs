using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static EventChoice;
//Tag控制卡片样式在单个卡片控制器
public class EventCardController : MonoBehaviour//选项控制器
{
    public TMP_Text titleText;             //选项标题
    public TMP_Text storyRequireValue;     // 需求条件
    public TMP_Text storyDescription;      // 选项描述
    public TMP_Text tagText;               //选项类型
    public Button goButton;
    public Image challengeIcon;          //挑战图标
    public TMP_Text challengeText;       //挑战文本
    public IconDatabase iconDatabase; // 绑定 Icon 数据库

    private NarrativeEvent linkedEvent;
    private EventManager eventManager;

    [HideInInspector] public EventUIManager eventUIManager;

    public void SetDataFromChoice(string title, string requireText, string descriptionText, string tag, bool interactable, EventChoice choice)
    {//oringin事件生成函数
        if (titleText != null) titleText.text = title;
        if (storyRequireValue != null) storyRequireValue.text = requireText;
        if (storyDescription != null) storyDescription.text = descriptionText;
        if (tagText != null) tagText.text = tag;
        if (goButton != null) goButton.interactable = interactable;
        if (interactable)
        {
            SetAvailableStyle(); //重置样式
        }
        else
        {
            SetUnavailableStyle();
        }

        if (choice.isChallenge && challengeIcon != null && challengeText != null)
        {
            // 设置挑战图标
            if (iconDatabase != null)
            {
                var icon = iconDatabase.GetIcon(choice.challengeTraitId);
                if (icon != null)
                    challengeIcon.sprite = icon;
            }

            // 计算成功率
            float traitValue = eventUIManager.eventManager.traitSystem.GetTrait(choice.challengeTraitId);
            float successChance = traitValue * choice.successChancePerPoint;
            successChance = Mathf.Clamp01(successChance);
            challengeText.text = $"你的成功率为 {Mathf.RoundToInt(successChance * 100)}%";//.1为10%

            challengeIcon.gameObject.SetActive(true);
            challengeText.gameObject.SetActive(true);
        }
        else
        {
            if (challengeIcon != null) challengeIcon.gameObject.SetActive(false);
            if (challengeText != null) challengeText.gameObject.SetActive(false);
        }

        // Tag控制卡片样式，样式设置
        var bg = GetComponent<Image>();
        if (bg != null)
        {
            switch (choice.cardStyle)
            {
                case StoryCardStyle.Combat:
                    bg.color = new Color(0.8f, 0.2f, 0.2f, 1f);   //战斗 红色
                    break;
                case StoryCardStyle.Important:
                    bg.color = new Color(0.8f, 0.8f, 0.2f, 1f);   //重要 黄色
                    break;
                case StoryCardStyle.Repeatable:
                    bg.color = new Color(0.2f, 0.2f, 0.8f, 1f);   //重复事件 蓝色
                    break;
                default:
                    bg.color = new Color32(62, 70, 103, 255);     //默认色
                    break;
            }
        }
    }

    public void LoadEvent(NarrativeEvent e, EventManager manager, bool isPreview = false)//regionPanel事件生成;
    {
        linkedEvent = e;
        eventManager = manager;

        if (titleText != null) titleText.text = e.title;
        if (storyDescription != null) storyDescription.text = e.description;
        if (tagText != null) tagText.text = string.Join(", ", e.tags);

        bool canAccess = manager.CanEnterEvent(e);
        goButton.interactable = canAccess;

        goButton.onClick.RemoveAllListeners();

        bool isEntryPoint = e.HasTag(EventTag.Entrypoint);

        if (canAccess)
        {
            if (!isPreview || isEntryPoint) //入口事件允许点击
            {
                goButton.onClick.AddListener(() =>
                {
                    manager.StartEventDetail(linkedEvent);
                    eventUIManager?.StartCoroutine(eventUIManager.RefreshLayoutDelayed());
                    eventUIManager.regionPanel.SetActive(false);
                });
            }
        }
        else
        {
            goButton.onClick.AddListener(() =>
            {
                Debug.Log("不满足进入条件！");
            });
        }
        if (challengeIcon != null) challengeIcon.gameObject.SetActive(false);
        if (challengeText != null) challengeText.gameObject.SetActive(false);
    }
    public void SetUnavailableStyle()//按钮不可用
    {
        // 背景灰色
        var bg = GetComponent<Image>();
        if (bg != null)
        {
            bg.color = new Color(0.6f, 0.6f, 0.6f, 0.7f); // 半透明灰
        }

        // 按钮透明度降低
        if (goButton != null)
        {
            var colors = goButton.colors;
            colors.normalColor = new Color(0.5f, 0.5f, 0.5f);
            colors.highlightedColor = new Color(0.5f, 0.5f, 0.5f);
            goButton.colors = colors;
        }

        // 字体颜色也稍作淡化
        Color faded = new Color(0.7f, 0.7f, 0.7f);
        if (titleText != null) titleText.color = faded;
        if (storyRequireValue != null) storyRequireValue.color = faded;
        if (storyDescription != null) storyDescription.color = faded;
        if (tagText != null) tagText.color = faded;
        if (challengeText != null) challengeText.color = faded;
    }
    public void SetAvailableStyle()//按钮可用
    {
        //背景
        var bg = GetComponent<Image>();
        if (bg != null)
        {
            bg.color = new Color32(62, 70, 103, 255);
        }
        //按钮
        if (goButton != null)
        {
            var colors = goButton.colors;
            colors.normalColor = new Color32(57, 54, 111, 255);
            colors.highlightedColor = new Color32(91, 88, 156, 255);
            colors.pressedColor = new Color32(45, 42, 84, 255);
            colors.selectedColor = new Color32(91, 88, 156, 255);
            goButton.colors = colors;
        }
        //字体
        Color normal = Color.black;
        if (titleText != null) titleText.color = normal;
        if (storyRequireValue != null) storyRequireValue.color = normal;
        if (storyDescription != null) storyDescription.color = normal;
        if (tagText != null) tagText.color = normal;
        if (challengeText != null) challengeText.color = normal;
    }

}