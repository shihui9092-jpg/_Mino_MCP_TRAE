using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 设置面板测试脚本
/// 用于验证设置面板UI组件的事件绑定是否正常工作
/// </summary>
public class TestSettingsPanel : MonoBehaviour
{
    [Header("测试组件")]
    public Slider testBGMVolumeSlider;
    public Slider testVoiceVolumeSlider;
    public Slider testSFXVolumeSlider;
    public Slider testTextSpeedSlider;
    public Toggle testAutoPlayToggle;
    public Toggle testSkipReadToggle;
    public Button testApplyButton;
    public Button testCancelButton;
    public Button testDefaultButton;
    
    [Header("测试输出")]
    public Text testOutputText;
    
    private void Start()
    {
        // 绑定测试事件
        if (testBGMVolumeSlider != null)
            testBGMVolumeSlider.onValueChanged.AddListener(OnTestBGMVolumeChanged);
            
        if (testVoiceVolumeSlider != null)
            testVoiceVolumeSlider.onValueChanged.AddListener(OnTestVoiceVolumeChanged);
            
        if (testSFXVolumeSlider != null)
            testSFXVolumeSlider.onValueChanged.AddListener(OnTestSFXVolumeChanged);
            
        if (testTextSpeedSlider != null)
            testTextSpeedSlider.onValueChanged.AddListener(OnTestTextSpeedChanged);
            
        if (testAutoPlayToggle != null)
            testAutoPlayToggle.onValueChanged.AddListener(OnTestAutoPlayChanged);
            
        if (testSkipReadToggle != null)
            testSkipReadToggle.onValueChanged.AddListener(OnTestSkipReadChanged);
            
        if (testApplyButton != null)
            testApplyButton.onClick.AddListener(OnTestApplyClicked);
            
        if (testCancelButton != null)
            testCancelButton.onClick.AddListener(OnTestCancelClicked);
            
        if (testDefaultButton != null)
            testDefaultButton.onClick.AddListener(OnTestDefaultClicked);
            
        UpdateTestOutput("设置面板测试脚本已启动");
    }
    
    private void OnTestBGMVolumeChanged(float volume)
    {
        UpdateTestOutput($"BGM音量变化: {volume:F2}");
    }
    
    private void OnTestVoiceVolumeChanged(float volume)
    {
        UpdateTestOutput($"语音音量变化: {volume:F2}");
    }
    
    private void OnTestSFXVolumeChanged(float volume)
    {
        UpdateTestOutput($"音效音量变化: {volume:F2}");
    }
    
    private void OnTestTextSpeedChanged(float speed)
    {
        UpdateTestOutput($"文本速度变化: {speed:F2}");
    }
    
    private void OnTestAutoPlayChanged(bool isOn)
    {
        UpdateTestOutput($"自动播放: {isOn}");
    }
    
    private void OnTestSkipReadChanged(bool isOn)
    {
        UpdateTestOutput($"跳过已读: {isOn}");
    }
    
    private void OnTestApplyClicked()
    {
        UpdateTestOutput("应用按钮被点击");
    }
    
    private void OnTestCancelClicked()
    {
        UpdateTestOutput("取消按钮被点击");
    }
    
    private void OnTestDefaultClicked()
    {
        UpdateTestOutput("默认设置按钮被点击");
    }
    
    private void UpdateTestOutput(string message)
    {
        if (testOutputText != null)
        {
            testOutputText.text = message;
        }
        Debug.Log($"[测试] {message}");
    }
}