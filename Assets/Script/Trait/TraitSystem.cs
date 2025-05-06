using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trait
{
    public string id;              // ���� ID������ "sanity"
    public string displayName;     // ��ʾ����
    [Range(0f, 100f)]
    public float value;            // ����ֵ��С������Χ 0 - 100��
}

public class TraitSystem : MonoBehaviour
{
    public List<Trait> traits = new List<Trait>();

    // ��ȡָ�����ʵ�ֵ
    public float GetTrait(string id)
    {
        var t = traits.Find(t => t.id == id);
        return t != null ? t.value : 0f;
    }

    // �޸�ָ�����ʵ�ֵ���Զ������� 0 - 100��
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

        Debug.Log($"���� [{id}] ��ǰֵ��{GetTrait(id):F1}");
    }

    public Dictionary<string, float> GetAllTraits()
    {
        Dictionary<string, float> result = new Dictionary<string, float>();
        foreach (var trait in traits)
        {
            result[trait.id] = trait.value;
        }
        return result;
        /*traitSystem.ModifyTrait("sanity", 5.2f); // ���� 5.2
        float currentSanity = traitSystem.GetTrait("sanity"); // ��ȡ float ֵ*/
    }
}
