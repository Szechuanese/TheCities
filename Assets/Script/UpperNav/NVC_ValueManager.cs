using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//控制时间Naval栏的Icon布局。
public class NVC_ValueManager : MonoBehaviour
{

    //特质间距调整
    [Header("Adaptive Spacing Settings")]
    [Tooltip("拥有此及以下特质时，使用 MaxSpacing")]
    public int minTraitCount = 4;
    [Tooltip("拥有此及以上特质时，使用 MinSpacing")]
    public int maxTraitCount = 9;
    [Tooltip("最宽时的 spacing（特质数 <= minTraitCount）")]
    public float maxSpacing = 14f;
    [Tooltip("最窄时的 spacing（特质数 >= maxTraitCount）")]
    public float minSpacing = -86f;


    //绑定组件
    public ValueSystem valueSystem;
    public GameObject upperNav_ValuePrefab;
    public IconDatabase traitIconDatabase;
    public Transform navValueContainer;

    private Dictionary<string, UpperValueIconController> traitIcons = new Dictionary<string, UpperValueIconController>();


    void Start()
    {
        GenerateTraitCards();
        if (valueSystem != null)
            valueSystem.OnValueChanged += Refresh;
    }

    void OnDestroy()
    {
        if (valueSystem != null)
            valueSystem.OnValueChanged -= Refresh;
    }

    void Refresh() => GenerateTraitCards();

    public void GenerateTraitCards()
    {
        //清除旧的卡片
        foreach (Transform child in navValueContainer)
        {
            Destroy(child.gameObject);
        }
        traitIcons.Clear();
        //得到新卡片
        foreach (var value in valueSystem.GetValuesByType(ValueType.Trait))
        {
            if (value.value >= 1f)
            {
                GameObject card = Instantiate(upperNav_ValuePrefab, navValueContainer);
                UpperValueIconController controller = card.GetComponent<UpperValueIconController>();
                //从数据库取出对应 id 的 Sprite,又是一个三目运算符，我太喜欢了。
                Sprite icon = traitIconDatabase != null
                    ? traitIconDatabase.GetIcon(value.id)
                    : null;
                //调用重载，传入 id + Sprite
                controller.GetId(value.id, icon);
                traitIcons[value.id] = controller;

            }
            AdjustSpacing(traitIcons.Count);
        }

    }
    //每加一个特质间距减10
    private void AdjustSpacing(int count)
    {
        var hlg = navValueContainer.GetComponent<HorizontalLayoutGroup>();
        if (hlg == null) return;

        float spacing;
        if (count <= minTraitCount)
        {
            spacing = maxSpacing;
        }
        else if (count >= maxTraitCount)
        {
            spacing = minSpacing;
        }
        else
        {
            // 线性插值：随着 count 从 minTraitCount→maxTraitCount,
            // spacing 从 maxSpacing→minSpacing 变化
            float t = (count - minTraitCount) / (float)(maxTraitCount - minTraitCount);
            spacing = Mathf.Lerp(maxSpacing, minSpacing, t);
        }

        hlg.spacing = spacing;
    }

}
