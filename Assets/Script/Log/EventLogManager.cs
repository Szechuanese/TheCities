using System.Collections.Generic;
using UnityEngine;

public class EventLogManager : MonoBehaviour
{
    public static EventLogManager instance;

    private List<string> logs = new List<string>();
    public EventLogUI eventLogUI;

    void Awake()
    {
        instance = this;
    }

    public void AddLog(string text)
    {
        logs.Add(text);
        Debug.Log("📝 记录日志：" + text);

        if (eventLogUI != null)
        {
            eventLogUI.AddNewLogCard(text);
        }
    }

    public List<string> GetLogs()
    {
        return logs;
    }
}
