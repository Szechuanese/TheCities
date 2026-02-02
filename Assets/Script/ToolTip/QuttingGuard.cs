using UnityEngine;
//¾²Ì¬UnityÍË³öÊØÎÀ
public static class QuttingGuard 
{
    public static bool IsQuitting { get; private set; }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Reset() => IsQuitting = false;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Hook() => Application.quitting += () => IsQuitting = true;
}
