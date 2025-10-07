using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Value
{
    public string id;
    public string displayName;
    [Range(0f, 100f)]
    public float value;
    public ValueType type;
}

/// <summary>
/// 总类型
/// </summary>
public enum ValueType
{
    /// <summary>
    /// Trait类型，角色的主要判断属性
    /// </summary>
    Trait,
    Character,
    /// <summary>
    /// 开局类型，通常用于角色的File页面显示描述
    /// </summary>
    Bear,
    /// <summary>
    /// 我将MoneyValue进行特殊化，这样它就不会显示在它不该现实的地方。
    /// </summary>
    Money
}

public class ValueSystem : MonoBehaviour
{
    public delegate void ValueChangedHandler();
    public event ValueChangedHandler OnValueChanged;

    public List<Value> values = new List<Value>();
    private Dictionary<string, Value> valueDict = new Dictionary<string, Value>();

    void Awake()
    {
        foreach (var v in values)
        {
            if (!valueDict.ContainsKey(v.id))
                valueDict[v.id] = v;
        }

        // 初始化常驻字段，如资金 money
        if (!valueDict.ContainsKey("money"))
        {
            var money = new Value { id = "money", displayName = "资金", value = 10000f, type = ValueType.Money };
            values.Add(money);
            valueDict["money"] = money;
        }
    }

    //得到Value
    public float GetValue(string id)
    {
        return valueDict.TryGetValue(id, out var val) ? val.value : 0f;
    }
    //模块化Value
    public void ModifyValue(string id, float amount)
    {
        if (!valueDict.TryGetValue(id, out var val))
        {
            val = new Value { id = id, displayName = id, value = Mathf.Clamp(amount, 0f, 100f), type = ValueType.Trait };
            values.Add(val);
            valueDict[id] = val;
        }
        else
        {
            val.value = Mathf.Clamp(val.value + amount, 0f, 100f);
        }

        Debug.Log($"📊 Value [{id}] 当前值：{val.value}");
        OnValueChanged?.Invoke();
    }

    public List<Value> GetValuesByType(ValueType type)
    {
        return values.FindAll(v => v.type == type);
    }
}
