using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

//控制Icon
public class UpperValueIconController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("每张卡片的倾斜角度步长（°）")]
    public float tiltStep = 5f;

    [Header("各项绑定")]
    private string traitId;
    //绑定图像，不然没法显示ToolTip
    public Image iconImage;
    [Tooltip("高亮时 Canvas sortingOrder")]
    public int hoverSortingOrder = 100;

    [Header("Hover 效果设置")]
    public float hoverScale = 1.2f;   //放大倍数
    public float animDuration = 0.2f;   //动画时长（秒）
    //public Color glowColor = Color.yellow;   //发光颜色
    //public float glowDistance = 5f;     //发光距离（像素）

    //private Outline outlineComp;          //发光

    private Canvas iconCanvas;//绑定Canvas用于让选中Icon浮于其他Icon之上
    void Awake()
    {
        // 确保有 Canvas，并关闭 overrideSorting
        iconCanvas = GetComponent<Canvas>();
        iconCanvas.overrideSorting = false;
        // 如果需要，还可以设置 iconCanvas.sortingLayerID
    }
    void Start()
    {
        int count = transform.childCount;

        // 中心索引：让中间的卡片角度为 0°
        float mid = (count - 1) / 2f;

        for (int i = 0; i < count; i++)
        {
            Transform card = transform.GetChild(i);

            // 计算每张卡片的倾斜角度
            //实现了吗？我感觉这个方法有点问题
            float angle = (i - mid) * tiltStep;
            card.localRotation = Quaternion.Euler(0, 0, angle);

            // 根据 i 的大小决定渲染顺序：
            // 索引大的卡片（更左边）放到最上面
            card.SetSiblingIndex(count - 1 - i);
        }
    }

    //初始化调用。
    public void GetId(string id, Sprite icon)
    {
        traitId = id;
        if (iconImage != null && icon != null)
            iconImage.sprite = icon;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(traitId))
        //弹出 Tooltip
        TraitToolTipManager.instance?.ShowById(traitId);


        //提到最前面
        iconCanvas.overrideSorting = true;
        iconCanvas.sortingOrder = hoverSortingOrder;



        //添加／启用 Outline 发光，目前效果太过丑陋，暂时隐藏。
        //if (outlineComp == null)
        //{
        //    outlineComp = iconImage.gameObject.AddComponent<Outline>();
        //    outlineComp.effectColor = glowColor;
        //    outlineComp.effectDistance = new Vector2(glowDistance, glowDistance);
        //}
        //else
        //{
        //    outlineComp.enabled = true;
        //}

        // —— 4. 缩放动画 ——
        transform
            .DOScale(hoverScale, animDuration)
            .SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 隐藏 Tooltip
        TraitToolTipManager.instance?.HideTooltip();
        // 还原 overrideSorting
        iconCanvas.overrideSorting = false;
        // 取消发光
        //if (outlineComp != null)
        //    outlineComp.enabled = false;

        // 恢复缩放
        transform
            .DOScale(1f, animDuration)
            .SetEase(Ease.InCubic);
    }
}
