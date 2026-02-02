using System.Collections.Generic;
using UnityEngine;
using static BurdenCategory;

public class BurdenPanelManager : MonoBehaviour
{
    [Header("Systems")]
    public BurdenSystem burdenSystem;

    [Header("Prefab")]
    public BurdenItemIconView iconPrefab;

    [Header("Category Boxes (英文名+Box)")]
    public Transform GlimpseBox;
    public Transform BookBox;
    public Transform StiffBox;
    public Transform SpiceBox;
    public Transform InadmissibleBox;
    public Transform RemnantsBox;
    public Transform DeedBox;
    public Transform AppearanceBox;

    private Dictionary<E_BurdenCategory, Transform> boxMap;

    private void Awake()
    {
        if (burdenSystem == null)
            burdenSystem = FindFirstObjectByType<BurdenSystem>();

        boxMap = new Dictionary<E_BurdenCategory, Transform>
        {
            { E_BurdenCategory.Glimpse, GlimpseBox },
            { E_BurdenCategory.Book, BookBox },
            { E_BurdenCategory.Stiff, StiffBox },
            { E_BurdenCategory.Spice, SpiceBox },
            { E_BurdenCategory.Inadmissible, InadmissibleBox },
            { E_BurdenCategory.Remnants, RemnantsBox },
            { E_BurdenCategory.Deed, DeedBox },
            { E_BurdenCategory.Appearance, AppearanceBox },
        };
    }

    private void OnEnable()
    {
        if (burdenSystem != null)
            burdenSystem.OnBurdenChanged += Refresh;

        Refresh();
    }

    private void OnDisable()
    {
        if (burdenSystem != null)
            burdenSystem.OnBurdenChanged -= Refresh;
    }

    public void Refresh()
    {
        if (iconPrefab == null) return;

        // 1) 清空所有分类Box
        foreach (var kv in boxMap)
        {
            var box = kv.Value;
            if (box == null) continue;

            for (int i = box.childCount - 1; i >= 0; i--)
                Destroy(box.GetChild(i).gameObject);
        }

        if (burdenSystem == null) return;

        // 2) 遍历背包物品，按分类生成 icon
        foreach (var it in burdenSystem.items)
        {
            if (it == null || it.definition == null) continue;
            if (it.amount <= 0) continue;

            var cat = it.definition.category;

            if (!boxMap.TryGetValue(cat, out var parent) || parent == null)
                continue;

            var view = Instantiate(iconPrefab, parent);
            view.SetSprite(it.definition.icon);
        }
    }
}
