using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BurdenItemDatabase", menuName = "TwoCities/BurdenItemDatabase")]
public class BurdenItemDatabase : ScriptableObject
{
    public List<BurdenItemDefinition> items = new List<BurdenItemDefinition>();

    private Dictionary<string, BurdenItemDefinition> _map;

    private void OnEnable()
    {
        BuildIndex();
    }

    public void BuildIndex()
    {
        _map = new Dictionary<string, BurdenItemDefinition>();

        foreach (var def in items)
        {
            if (def == null) continue;

            //假设 BurdenItemDefinition 里字段名叫 id（你说是 id + category + icon + valueModifiers）
            if (string.IsNullOrWhiteSpace(def.id))
            {
                Debug.LogWarning($"[BurdenItemDatabase] 发现空 id：{def.name}", this);
                continue;
            }

            if (_map.ContainsKey(def.id))
            {
                Debug.LogError($"[BurdenItemDatabase] 重复 id：{def.id}（后者会覆盖前者）", this);
            }

            _map[def.id] = def;
        }
    }

    public bool TryGet(string id, out BurdenItemDefinition def)
    {
        if (_map == null) BuildIndex();
        return _map.TryGetValue(id, out def);
    }

    public BurdenItemDefinition Get(string id)
    {
        if (TryGet(id, out var def)) return def;
        return null;
    }
}
