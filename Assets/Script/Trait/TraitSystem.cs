using System.Collections.Generic;
using UnityEngine;
//特质系统
[System.Serializable]
public class Trait
{
    public string id;
    public string displayName;
    [Range(0f, 100f)]
    public float value;
}

public class TraitSystem : MonoBehaviour
{
    public delegate void TraitChangedHandler();
    public event TraitChangedHandler OnTraitChanged;

    public List<Trait> traits = new List<Trait>();
    private Dictionary<string, Trait> traitDict = new Dictionary<string, Trait>();

    void Awake()
    {
        foreach (var t in traits)
        {
            if (!traitDict.ContainsKey(t.id))
                traitDict[t.id] = t;
        }

        if (!traitDict.ContainsKey("money")) //money特殊化
        {
            var moneyTrait = new Trait { id = "money", displayName = "金币", value = 10000f };
            traits.Add(moneyTrait);
            traitDict["money"] = moneyTrait;
        }
    }

    public float GetTrait(string id)
    {
        if (id == "money" && !traitDict.ContainsKey("money")) //money特殊化
        {
            var moneyTrait = new Trait { id = "money", displayName = "金币", value = 10000f };
            traits.Add(moneyTrait);
            traitDict["money"] = moneyTrait;
        }

        return traitDict.TryGetValue(id, out Trait trait) ? trait.value : 0f;
    }

    public void ModifyTrait(string id, float amount)
    {
        if (traitDict.TryGetValue(id, out Trait trait))
        {
            trait.value = Mathf.Clamp(trait.value + amount, 0f, 100f);
        }
        else
        {
            trait = new Trait { id = id, displayName = id, value = Mathf.Clamp(amount, 0f, 100f) };
            traits.Add(trait);
            traitDict[id] = trait;
        }

        Debug.Log($"特质 [{id}] 当前值：{trait.value:F1}");
        OnTraitChanged?.Invoke();
    }

    public Dictionary<string, float> GetAllTraits()
    {
        Dictionary<string, float> result = new Dictionary<string, float>();
        foreach (var kv in traitDict)
        {
            result[kv.Key] = kv.Value.value;
        }
        return result;
    }
}
