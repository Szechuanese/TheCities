using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Value
{
    public string id;
    public string displayName;
    //怕我自己忘，Range特性滚动条指定范围
    [Range(0f, 100f)]
    public float value;
    public ValueType type;
}

/// <summary>
/// Value类型
/// </summary>
public enum ValueType
{
    /// <summary>
    /// Trait类型，角色的主要判断属性，九大属性
    /// </summary>
    Trait,
    /// <summary>
    /// Character类型，角色的次要属性，通常用于File页面显示描述，例如性格特质
    /// </summary>
    Character,
    /// <summary>
    /// 开局类型，通常用于角色的File页面显示描述，例如Origin和Ambition
    /// </summary>
    Bear,
    /// <summary>
    /// 成就，用于记录角色达成的特殊事件或里程碑，在File界面显示
    /// </summary>
    Effort,
    /// <summary>
    /// 人情
    /// </summary>
    Flavor,
    /// <summary>
    /// 关系，用于记录角色与其他角色之间的关系强度，在File界面显示,weitao先生等等
    /// </summary>
    Relation,
    /// <summary>
    /// 鲜花(Romance):浪漫，爱情，另一种关系。或者是诅咒。
    /// </summary>
    Romance,
    /// <summary>
    /// 癖好。告诉别人你是复杂的。
    /// </summary>
    Parts,
    /// <summary>
    /// 触发点。创伤，抑郁，也许这是你到达终点的一条捷径。
    /// </summary>
    Scourge,
    /// <summary>
    /// 触发点。
    /// </summary>
    Spark,
    /// <summary>
    /// 在下面我将MoneyValue进行特殊化，这样它就不会显示在它不该现实的地方。
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

        //初始化常驻资金字段
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
            val = new Value { id = id, displayName = id, value = amount, type = ValueType.Trait };
            values.Add(val);
            valueDict[id] = val;
        }
        else
        {
            if (val.type == ValueType.Money)
            {
                //money只限制不能为负数
                val.value = Mathf.Max(0f, val.value + amount);
            }
            else
            {
                //其他依旧限制在 0~100
                val.value = Mathf.Clamp(val.value + amount, 0f, 100f);
            }
        }

        Debug.Log($"Value [{id}] 当前值：{val.value}");
        OnValueChanged?.Invoke();
    }
    //通过类型寻找Value
    public List<Value> GetValuesByType(ValueType type)
    {
        return values.FindAll(v => v.type == type);
    }
}
