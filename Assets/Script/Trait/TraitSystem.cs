using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trait
{
    public string id;              // 特质 ID，例如 "sanity"
    public string displayName;     // 显示名称
    [Range(0f, 100f)]
    public float value;            // 特质值（小数，范围 0 - 100）
}

public class TraitSystem : MonoBehaviour
{
    public List<Trait> traits = new List<Trait>();

    // 获取指定特质的值
    public float GetTrait(string id)
    {
        var t = traits.Find(t => t.id == id);
        return t != null ? t.value : 0f;
    }

    // 修改指定特质的值（自动限制在 0 - 100）
    public void ModifyTrait(string id, float amount)
    {
        var t = traits.Find(t => t.id == id);
        if (t != null)
        {
            t.value = Mathf.Clamp(t.value + amount, 0f, 100f);
        }
        else
        {
            traits.Add(new Trait { id = id, displayName = id, value = Mathf.Clamp(amount, 0f, 100f) });
        }

        Debug.Log($"特质 [{id}] 当前值：{GetTrait(id):F1}");
    }

    public Dictionary<string, float> GetAllTraits()
    {
        Dictionary<string, float> result = new Dictionary<string, float>();
        foreach (var trait in traits)
        {
            result[trait.id] = trait.value;
        }
        return result;
        /*traitSystem.ModifyTrait("sanity", 5.2f); // 增加 5.2
        float currentSanity = traitSystem.GetTrait("sanity"); // 获取 float 值*/
    }
}
