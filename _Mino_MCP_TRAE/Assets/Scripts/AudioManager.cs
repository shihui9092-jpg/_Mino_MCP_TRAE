using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音频管理器 - 负责游戏中的所有音频播放和控制
/// 使用单例模式确保全局访问，支持BGM、语音、音效和环境音
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// 音频管理器单例实例
    /// </summary>
    public static AudioManager Instance { get; private set; }
    
    [Header("音频源配置")]
    [Tooltip("背景音乐音频源")]
    public AudioSource bgmSource;
    [Tooltip("角色语音音频源")]
    public AudioSource voiceSource;
    [Tooltip("音效音频源")]
    public AudioSource sfxSource;
    [Tooltip("环境音音频源")]
    public AudioSource ambientSource;
    
    [Header("音频剪辑资源")]
    [Tooltip("背景音乐剪辑数组")]
    public AudioClip[] bgmTracks;
    [Tooltip("环境音效剪辑数组")]
    public AudioClip[] ambientSounds;
    [Tooltip("UI界面音效剪辑数组")]
    public AudioClip[] uiSfx;
    
    // 音频剪辑字典，用于快速查找
    private Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> ambientDictionary = new Dictionary<string, AudioClip>();
    
    /// <summary>
    /// 初始化方法，设置单例实例
    /// </summary>
    private void Awake()
    {
        // 单例模式实现
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 跨场景不销毁
            InitializeAudioDictionaries(); // 初始化音频字典
        }
        else
        {
            Destroy(gameObject); // 销毁重复实例
        }
    }
    
    /// <summary>
    /// 初始化音频字典，将音频剪辑数组转换为字典以便快速查找
    /// </summary>
    private void InitializeAudioDictionaries()
    {
        // 初始化BGM字典
        foreach (AudioClip clip in bgmTracks)
        {
            bgmDictionary[clip.name] = clip;
        }
        
        // 初始化环境音字典
        foreach (AudioClip clip in ambientSounds)
        {
            ambientDictionary[clip.name] = clip;
        }
    }
    
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="bgmName">BGM名称</param>
    /// <param name="loop">是否循环播放</param>
    /// <param name="fadeDuration">淡入淡出持续时间（秒）</param>
    public void PlayBGM(string bgmName, bool loop = true, float fadeDuration = 1.0f)
    {
        if (bgmDictionary.ContainsKey(bgmName))
        {
            StartCoroutine(FadeBGM(bgmDictionary[bgmName], loop, fadeDuration));
        }
        else
        {
            Debug.LogWarning($"未找到BGM: {bgmName}");
        }
    }
    
    /// <summary>
    /// 立即播放背景音乐（无淡入效果）
    /// </summary>
    /// <param name="bgmName">BGM名称</param>
    /// <param name="loop">是否循环播放</param>
    public void PlayBGMImmediately(string bgmName, bool loop = true)
    {
        if (bgmDictionary.ContainsKey(bgmName))
        {
            bgmSource.Stop();
            bgmSource.clip = bgmDictionary[bgmName];
            bgmSource.loop = loop;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"未找到BGM: {bgmName}");
        }
    }
    
    /// <summary>
    /// BGM淡入淡出协程
    /// </summary>
    private IEnumerator FadeBGM(AudioClip newBGM, bool loop, float fadeDuration)
    {
        // 淡出当前BGM
        float startVolume = bgmSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }
        
        // 切换BGM并淡入
        bgmSource.Stop();
        bgmSource.clip = newBGM;
        bgmSource.loop = loop;
        bgmSource.volume = startVolume;
        bgmSource.Play();
    }
    
    /// <summary>
    /// 播放角色语音
    /// </summary>
    /// <param name="voiceClip">语音剪辑</param>
    public void PlayVoice(AudioClip voiceClip)
    {
        voiceSource.Stop();
        voiceSource.clip = voiceClip;
        voiceSource.Play();
    }
    
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="sfxClip">音效剪辑</param>
    /// <param name="volume">音量大小（0-1）</param>
    public void PlaySFX(AudioClip sfxClip, float volume = 1.0f)
    {
        sfxSource.PlayOneShot(sfxClip, volume);
    }
    
    /// <summary>
    /// 播放UI按钮音效（随机选择）
    /// </summary>
    public void PlayUIButtonSound()
    {
        if (uiSfx.Length > 0)
        {
            PlaySFX(uiSfx[Random.Range(0, uiSfx.Length)]);
        }
    }
    
    /// <summary>
    /// 播放环境音
    /// </summary>
    /// <param name="ambientName">环境音名称</param>
    /// <param name="loop">是否循环播放</param>
    public void PlayAmbient(string ambientName, bool loop = true)
    {
        if (ambientDictionary.ContainsKey(ambientName))
        {
            ambientSource.Stop();
            ambientSource.clip = ambientDictionary[ambientName];
            ambientSource.loop = loop;
            ambientSource.Play();
        }
        else
        {
            Debug.LogWarning($"未找到环境音: {ambientName}");
        }
    }
    
    /// <summary>
    /// 停止播放背景音乐
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }
    
    /// <summary>
    /// 停止播放环境音
    /// </summary>
    public void StopAmbient()
    {
        ambientSource.Stop();
    }
    
    /// <summary>
    /// 设置背景音乐音量
    /// </summary>
    /// <param name="volume">音量大小（0-1）</param>
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
    }
    
    /// <summary>
    /// 设置语音音量
    /// </summary>
    /// <param name="volume">音量大小（0-1）</param>
    public void SetVoiceVolume(float volume)
    {
        voiceSource.volume = Mathf.Clamp01(volume);
    }
    
    /// <summary>
    /// 设置音效音量
    /// </summary>
    /// <param name="volume">音量大小（0-1）</param>
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }
    
    /// <summary>
    /// 设置环境音音量
    /// </summary>
    /// <param name="volume">音量大小（0-1）</param>
    public void SetAmbientVolume(float volume)
    {
        ambientSource.volume = Mathf.Clamp01(volume);
    }
    
    /// <summary>
    /// 暂停所有音频播放
    /// </summary>
    public void PauseAllAudio()
    {
        bgmSource.Pause();
        voiceSource.Pause();
        sfxSource.Pause();
        ambientSource.Pause();
    }
    
    /// <summary>
    /// 恢复所有音频播放
    /// </summary>
    public void ResumeAllAudio()
    {
        bgmSource.UnPause();
        voiceSource.UnPause();
        sfxSource.UnPause();
        ambientSource.UnPause();
    }
}