using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 视觉效果管理器 - 负责管理游戏中的各种屏幕特效和视觉效果
/// 使用单例模式确保全局访问，提供淡入淡出、闪光、震动等特效功能
/// </summary>
public class VisualEffects : MonoBehaviour
{
    /// <summary>
    /// 视觉效果管理器单例实例
    /// </summary>
    public static VisualEffects Instance { get; private set; }
    
    [Header("屏幕效果")]
    [Tooltip("淡入淡出遮罩图像")]
    public Image fadeImage;
    [Tooltip("闪光效果图像")]
    public Image flashImage;
    [Tooltip("屏幕震动效果图像")]
    public Image shakeImage;
    
    [Header("特效设置")]
    [Tooltip("淡入淡出持续时间（秒）")]
    public float fadeDuration = 1.0f;
    [Tooltip("闪光效果持续时间（秒）")]
    public float flashDuration = 0.5f;
    [Tooltip("屏幕震动强度")]
    public float shakeIntensity = 10f;
    [Tooltip("屏幕震动持续时间（秒）")]
    public float shakeDuration = 0.5f;
    
    /// <summary>
    /// 初始化方法，设置单例实例
    /// </summary>
    private void Awake()
    {
        // 单例模式实现
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// 屏幕淡入效果（从黑屏到正常）
    /// </summary>
    public void FadeIn()
    {
        StartCoroutine(Fade(1, 0, fadeDuration));
    }
    
    /// <summary>
    /// 屏幕淡出效果（从正常到黑屏）
    /// </summary>
    public void FadeOut()
    {
        StartCoroutine(Fade(0, 1, fadeDuration));
    }
    
    /// <summary>
    /// 淡入淡出协程实现
    /// </summary>
    /// <param name="startAlpha">起始透明度</param>
    /// <param name="endAlpha">结束透明度</param>
    /// <param name="duration">持续时间</param>
    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        fadeImage.color = new Color(0, 0, 0, startAlpha);
        fadeImage.gameObject.SetActive(true);
        
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        
        fadeImage.color = new Color(0, 0, 0, endAlpha);
        
        if (endAlpha == 0)
        {
            fadeImage.gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// 屏幕闪光效果
    /// </summary>
    /// <param name="flashColor">闪光颜色</param>
    public void Flash(Color flashColor)
    {
        StartCoroutine(FlashEffect(flashColor));
    }
    
    /// <summary>
    /// 闪光效果协程实现
    /// </summary>
    /// <param name="flashColor">闪光颜色</param>
    private IEnumerator FlashEffect(Color flashColor)
    {
        flashImage.color = flashColor;
        flashImage.gameObject.SetActive(true);
        
        for (float t = 0; t < flashDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1, 0, t / flashDuration);
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }
        
        flashImage.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 屏幕震动效果
    /// </summary>
    public void ScreenShake()
    {
        StartCoroutine(ShakeEffect());
    }
    
    /// <summary>
    /// 屏幕震动协程实现
    /// </summary>
    private IEnumerator ShakeEffect()
    {
        Vector3 originalPosition = shakeImage.transform.localPosition;
        float elapsed = 0f;
        
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            
            shakeImage.transform.localPosition = new Vector3(x, y, originalPosition.z);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        shakeImage.transform.localPosition = originalPosition;
    }
    
    /// <summary>
    /// 切换背景图像
    /// </summary>
    /// <param name="newBackground">新背景精灵</param>
    /// <param name="transitionTime">过渡时间</param>
    public void ChangeBackground(Sprite newBackground, float transitionTime = 1.0f)
    {
        StartCoroutine(BackgroundTransition(newBackground, transitionTime));
    }
    
    /// <summary>
    /// 背景切换协程实现
    /// </summary>
    /// <param name="newBackground">新背景精灵</param>
    /// <param name="transitionTime">过渡时间</param>
    private IEnumerator BackgroundTransition(Sprite newBackground, float transitionTime)
    {
        // 这里需要与DialogueSystem配合实现背景切换
        yield return null;
    }
    
    /// <summary>
    /// 在指定位置播放粒子特效
    /// </summary>
    /// <param name="position">特效位置</param>
    public void PlayParticleEffect(Vector3 position)
    {
        // 实现粒子特效播放
    }
    
    /// <summary>
    /// 播放闪电特效
    /// </summary>
    public void PlayLightningEffect()
    {
        StartCoroutine(LightningEffect());
    }
    
    /// <summary>
    /// 闪电特效协程实现
    /// </summary>
    private IEnumerator LightningEffect()
    {
        // 实现闪电特效
        yield return null;
    }
}