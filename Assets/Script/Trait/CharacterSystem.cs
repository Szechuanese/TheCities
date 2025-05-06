using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    public string id;
    public string displayName;
    [Range(0f, 100f)]
    public float value; // 支持浮点储存
}

public class CharacterSystem : MonoBehaviour
{
    public List<Character> characters = new List<Character>();

    // 获取角色特征值（四舍五入后返回 int）
    public int GetCharacter(string id)
    {
        var c = characters.Find(c => c.id == id);
        return c != null ? Mathf.RoundToInt(c.value) : 0;
    }

    // 获取原始 float 值（如需要）
    public float GetCharacterRaw(string id)
    {
        var c = characters.Find(c => c.id == id);
        return c != null ? c.value : 0f;
    }

    // 修改角色特征值（自动限制 0 ~ 100）
    public void ModifyCharacter(string id, float amount)
    {
        var c = characters.Find(c => c.id == id);
        if (c != null)
        {
            c.value = Mathf.Clamp(c.value + amount, 0f, 10f);
        }
        else
        {
            characters.Add(new Character { id = id, displayName = id, value = Mathf.Clamp(amount, 0f, 100f) });
        }

        Debug.Log($"🎯 [{id}] 当前值：{GetCharacter(id)}");
    }

    public Dictionary<string, int> GetAllCharacters()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();
        foreach (var c in characters)
        {
            result[c.id] = Mathf.RoundToInt(c.value);
        }
        return result;
    }
}
