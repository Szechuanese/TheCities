using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IconDatabase", menuName = "UI/Icon Database")]
public class IconDatabase : ScriptableObject
{
    [System.Serializable]
    public class IconEntry
    {
        public string id;       // Ö§³Ö traitId »ò characterId
        public Sprite icon;
    }

    public List<IconEntry> icons;

    private Dictionary<string, Sprite> iconDict;

    public void Initialize()
    {
        iconDict = new Dictionary<string, Sprite>();
        foreach (var entry in icons)
        {
            if (!iconDict.ContainsKey(entry.id))
                iconDict.Add(entry.id, entry.icon);
        }
    }

    public Sprite GetIcon(string id)
    {
        if (iconDict == null) Initialize();
        if (iconDict.TryGetValue(id, out Sprite sprite))
            return sprite;
        return null;
    }
}
