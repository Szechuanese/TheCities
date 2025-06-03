using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    public string id;
    public string displayName;
    [Range(0f, 100f)]
    public float value;
}

public class CharacterSystem : MonoBehaviour
{
    public delegate void CharacterChangedHandler();
    public event CharacterChangedHandler OnCharacterChanged;

    public List<Character> characters = new List<Character>();
    private Dictionary<string, Character> characterDict = new Dictionary<string, Character>();

    void Awake()
    {
        foreach (var c in characters)
        {
            if (!characterDict.ContainsKey(c.id))
                characterDict[c.id] = c;
        }
    }

    public int GetCharacter(string id)
    {
        return characterDict.TryGetValue(id, out Character c) ? Mathf.RoundToInt(c.value) : 0;
    }

    public float GetCharacterRaw(string id)
    {
        return characterDict.TryGetValue(id, out Character c) ? c.value : 0f;
    }

    public void ModifyCharacter(string id, float amount)
    {
        if (characterDict.TryGetValue(id, out Character c))
        {
            c.value = Mathf.Clamp(c.value + amount, 0f, 100f);
        }
        else
        {
            c = new Character { id = id, displayName = id, value = Mathf.Clamp(amount, 0f, 100f) };
            characters.Add(c);
            characterDict[id] = c;
        }

        Debug.Log($"🎯 [{id}] 当前值：{Mathf.RoundToInt(c.value)}");
        OnCharacterChanged?.Invoke();
    }

    public Dictionary<string, int> GetAllCharacters()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();
        foreach (var kv in characterDict)
        {
            result[kv.Key] = Mathf.RoundToInt(kv.Value.value);
        }
        return result;
    }
}