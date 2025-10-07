using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PortraitIconDatabase", menuName = "UI/Portrait Icon")]
public class PortraitsIcon : ScriptableObject
{
    [System.Serializable]
    public class PortraitIconEntry
    {
        public string portraitId;       
        public Sprite portraitIcon;
    }
    public List<PortraitIconEntry> portraitIcons;
    private Dictionary<string, Sprite> portraitIconDict;
    public void Initialize()
    {
        portraitIconDict = new Dictionary<string, Sprite>();
        foreach (var entry in portraitIcons)
        {
            if (!portraitIconDict.ContainsKey(entry.portraitId))
                portraitIconDict.Add(entry.portraitId, entry.portraitIcon);
        }
    }

    public Sprite GetPortraitIcon(string id)
    {
        if (portraitIconDict == null) Initialize();
        if (portraitIconDict.TryGetValue(id, out Sprite sprite))
            return sprite;
        return null;
    }
}
