// ✅ EventTagDrawer.cs
// 自定义 Inspector，允许 NarrativeEvent 中使用多选 Enum Tag（List<EventTag> 兼容版）

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(NarrativeEvent))]
public class EventTagDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NarrativeEvent evt = (NarrativeEvent)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("[标签设置 - Tag Editor]", EditorStyles.boldLabel);

        // 将 List<EventTag> 合并为单个 EnumFlags 值
        EventTag combined = 0;
        foreach (var tag in evt.tags)
        {
            combined |= tag;
        }

        // 显示为多选枚举字段
        EventTag newCombined = (EventTag)EditorGUILayout.EnumFlagsField("事件标签 (Event Tags)", combined);

        // ✅ 只有当变化时才写入 evt.tags，避免初始覆盖
        if (newCombined != combined)
        {
            evt.tags = new List<EventTag>();
            foreach (EventTag tag in System.Enum.GetValues(typeof(EventTag)))
            {
                if (newCombined.HasFlag(tag))
                {
                    evt.tags.Add(tag);
                }
            }

            // 标记资源为已修改，支持 Ctrl+S 保存
            EditorUtility.SetDirty(evt);
        }
    }
}

