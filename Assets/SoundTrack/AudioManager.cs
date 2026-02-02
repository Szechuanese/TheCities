using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //单例模式
    public static AudioManager Instance { get; private set; }

    // 音效资源表（手动在 Inspector 里拖拽设置）
    public List<AudioClip> audioClips; //在Inspector里手动添加
    private Dictionary<string, AudioClip> clipDict = new Dictionary<string, AudioClip>();

    private AudioSource audioSource;

    void Awake()
    {
        // 保证只有一个 AudioManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        //生成字典，方便用名字调用
        foreach (var clip in audioClips)
        {
            if (clip != null && !clipDict.ContainsKey(clip.name))
            {
                clipDict.Add(clip.name, clip);
            }
        }
    }

    // 通用方法：播放指定名字的音效
    public void PlaySFX(string clipName)
    {
        if (clipDict.ContainsKey(clipName))
        {
            audioSource.PlayOneShot(clipDict[clipName]);
        }
        else
        {
            Debug.LogWarning("未找到音效: " + clipName);
        }
    }
}
