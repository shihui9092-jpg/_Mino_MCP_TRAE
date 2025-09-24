using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI管理器 - 负责管理游戏中的所有用户界面
/// 使用单例模式确保全局访问，处理界面切换、设置管理和存档加载
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// UI管理器单例实例
    /// </summary>
    public static UIManager Instance { get; private set; }
    
    [Header("主界面")]
    [Tooltip("主菜单面板")]
    public GameObject mainMenuPanel;
    [Tooltip("加载游戏面板")]
    public GameObject loadGamePanel;
    [Tooltip("设置面板")]
    public GameObject settingsPanel;
    
    [Header("游戏界面")]
    [Tooltip("对话面板")]
    public GameObject dialoguePanel;
    [Tooltip("物品栏面板")]
    public GameObject inventoryPanel;
    [Tooltip("状态面板")]
    public GameObject statusPanel;
    
    [Header("设置界面")]
    [Tooltip("背景音乐音量滑块")]
    public Slider bgmVolumeSlider;
    [Tooltip("语音音量滑块")]
    public Slider voiceVolumeSlider;
    [Tooltip("音效音量滑块")]
    public Slider sfxVolumeSlider;
    [Tooltip("自动播放开关")]
    public Toggle autoPlayToggle;
    [Tooltip("跳过已读文本开关")]
    public Toggle skipReadToggle;
    
    [Header("文本速度")]
    [Tooltip("文本显示速度滑块")]
    public Slider textSpeedSlider;
    
    [Header("设置面板按钮")]
    [Tooltip("应用设置按钮")]
    public Button applySettingsButton;
    [Tooltip("取消设置按钮")]
    public Button cancelSettingsButton;
    [Tooltip("恢复默认设置按钮")]
    public Button defaultSettingsButton;

    [Header("加载游戏面板按钮")]
    [Tooltip("返回主菜单按钮")]
    public Button backToMenuButton;
    [Tooltip("存档槽位按钮列表")]
    public Button[] saveSlotButtons;
    [Tooltip("删除存档按钮")]
    public Button deleteSaveButton;

    [Header("加载游戏面板UI组件")]
    [Tooltip("存档缩略图占位符列表")]
    public Image[] thumbnailImages;
    [Tooltip("槽位编号文本列表")]
    public TextMeshProUGUI[] slotNumberTexts;
    [Tooltip("章节信息文本列表")]
    public TextMeshProUGUI[] chapterInfoTexts;
    [Tooltip("游戏时长文本列表")]
    public TextMeshProUGUI[] playTimeTexts;
    [Tooltip("存档日期文本列表")]
    public TextMeshProUGUI[] saveDateTexts;
    [Tooltip("空槽位提示文本列表")]
    public TextMeshProUGUI[] emptySlotTexts;
    
    [Header("对话面板按钮")]
    [Tooltip("继续按钮")]
    public Button continueButton;
    [Tooltip("跳过按钮")]
    public Button skipButton;
    [Tooltip("自动播放按钮")]
    public Button autoPlayButton;
    [Tooltip("日志按钮")]
    public Button logButton;
    [Tooltip("设置按钮")]
    public Button settingsButton;
    [Tooltip("存档按钮")]
    public Button saveButton;
    [Tooltip("读档按钮")]
    public Button loadButton;
    [Tooltip("物品栏按钮")]
    public Button inventoryButton;
    [Tooltip("状态按钮")]
    public Button statusButton;
    
    // 存储原始设置值，用于取消操作
    private float originalBGMVolume;
    private float originalVoiceVolume;
    private float originalSFXVolume;
    private bool originalAutoPlay;
    private bool originalSkipRead;
    private float originalTextSpeed;
    
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
    /// 游戏开始时加载设置并显示主菜单
    /// </summary>
    private void Start()
    {
        ShowMainMenu();
        LoadSettings();
        
        // 设置按钮事件监听
        SetupSettingsButtonListeners();
        SetupDialogueButtonListeners();
        SetupLoadGameButtonListeners();
    }
    
    /// <summary>
    /// 设置对话面板按钮监听器
    /// </summary>
    private void SetupDialogueButtonListeners()
    {
        // 继续按钮 - 推进对话
        if (continueButton != null)
        {
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(OnContinueClicked);
        }
        
        // 跳过按钮 - 跳过当前对话
        if (skipButton != null)
        {
            skipButton.onClick.RemoveAllListeners();
            skipButton.onClick.AddListener(OnSkipClicked);
        }
        
        // 自动播放按钮 - 切换自动播放模式
        if (autoPlayButton != null)
        {
            autoPlayButton.onClick.RemoveAllListeners();
            autoPlayButton.onClick.AddListener(OnAutoPlayClicked);
        }
        
        // 日志按钮 - 显示对话日志
        if (logButton != null)
        {
            logButton.onClick.RemoveAllListeners();
            logButton.onClick.AddListener(OnLogClicked);
        }
        
        // 设置按钮 - 打开设置面板
        if (settingsButton != null)
        {
            settingsButton.onClick.RemoveAllListeners();
            settingsButton.onClick.AddListener(OnSettingsClicked);
        }
        
        // 存档按钮 - 快速保存游戏
        if (saveButton != null)
        {
            saveButton.onClick.RemoveAllListeners();
            saveButton.onClick.AddListener(OnSaveClicked);
        }
        
        // 读档按钮 - 快速加载游戏
        if (loadButton != null)
        {
            loadButton.onClick.RemoveAllListeners();
            loadButton.onClick.AddListener(OnLoadClicked);
        }
        
        // 物品栏按钮 - 切换物品栏显示
        if (inventoryButton != null)
        {
            inventoryButton.onClick.RemoveAllListeners();
            inventoryButton.onClick.AddListener(OnInventoryClicked);
        }
        
        // 状态按钮 - 切换状态面板显示
        if (statusButton != null)
        {
            statusButton.onClick.RemoveAllListeners();
            statusButton.onClick.AddListener(OnStatusClicked);
        }
        
        Debug.Log("对话面板按钮事件监听器已设置完成");
    }

    /// <summary>
    /// 设置加载游戏面板按钮监听器
    /// </summary>
    private void SetupLoadGameButtonListeners()
    {
        // 返回主菜单按钮
        if (backToMenuButton != null)
        {
            backToMenuButton.onClick.RemoveAllListeners();
            backToMenuButton.onClick.AddListener(OnBackToMenuClicked);
        }

        // 存档槽位按钮
        if (saveSlotButtons != null && saveSlotButtons.Length > 0)
        {
            for (int i = 0; i < saveSlotButtons.Length; i++)
            {
                int slotIndex = i + 1; // 槽位从1开始
                if (saveSlotButtons[i] != null)
                {
                    saveSlotButtons[i].onClick.RemoveAllListeners();
                    saveSlotButtons[i].onClick.AddListener(() => OnSaveSlotClicked(slotIndex));
                }
            }
        }

        // 删除存档按钮
        if (deleteSaveButton != null)
        {
            deleteSaveButton.onClick.RemoveAllListeners();
            deleteSaveButton.onClick.AddListener(OnDeleteSaveClicked);
        }

        Debug.Log("加载游戏面板按钮事件监听器已设置完成");
    }

    /// <summary>
    /// 设置设置面板按钮监听器
    /// </summary>
    private void SetupSettingsButtonListeners()
    {
        // 绑定按钮点击事件
        if (applySettingsButton != null)
        {
            applySettingsButton.onClick.RemoveAllListeners();
            applySettingsButton.onClick.AddListener(OnApplySettings);
        }
        
        if (cancelSettingsButton != null)
        {
            cancelSettingsButton.onClick.RemoveAllListeners();
            cancelSettingsButton.onClick.AddListener(OnCancelSettings);
        }
        
        if (defaultSettingsButton != null)
        {
            defaultSettingsButton.onClick.RemoveAllListeners();
            defaultSettingsButton.onClick.AddListener(OnDefaultSettings);
        }
        
        // 绑定滑块值变化事件
        if (bgmVolumeSlider != null)
        {
            bgmVolumeSlider.onValueChanged.RemoveAllListeners();
            bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        }
        
        if (voiceVolumeSlider != null)
        {
            voiceVolumeSlider.onValueChanged.RemoveAllListeners();
            voiceVolumeSlider.onValueChanged.AddListener(OnVoiceVolumeChanged);
        }
        
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.RemoveAllListeners();
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }
        
        if (textSpeedSlider != null)
        {
            textSpeedSlider.onValueChanged.RemoveAllListeners();
            textSpeedSlider.onValueChanged.AddListener(OnTextSpeedChanged);
        }
        
        // 绑定开关状态变化事件
        if (autoPlayToggle != null)
        {
            autoPlayToggle.onValueChanged.RemoveAllListeners();
            autoPlayToggle.onValueChanged.AddListener(OnAutoPlayChanged);
        }
        
        if (skipReadToggle != null)
        {
            skipReadToggle.onValueChanged.RemoveAllListeners();
            skipReadToggle.onValueChanged.AddListener(OnSkipReadChanged);
        }
        
        Debug.Log("设置面板事件监听器已设置完成");
    }
    
    /// <summary>
    /// 显示主菜单界面
    /// </summary>
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        loadGamePanel.SetActive(false);
        settingsPanel.SetActive(false);
        dialoguePanel.SetActive(false);
    }
    
    /// <summary>
    /// 显示加载游戏界面
    /// </summary>
    public void ShowLoadGame()
    {
        mainMenuPanel.SetActive(false);
        loadGamePanel.SetActive(true);
        settingsPanel.SetActive(false);
        dialoguePanel.SetActive(false);
        
        // 加载存档列表
        UpdateSaveSlots();
    }
    
    /// <summary>
    /// 显示设置界面
    /// </summary>
    public void ShowSettings()
    {
        mainMenuPanel.SetActive(false);
        loadGamePanel.SetActive(false);
        settingsPanel.SetActive(true);
        dialoguePanel.SetActive(false);
        
        // 保存当前设置值，用于可能的取消操作
        SaveOriginalSettings();
    }
    
    /// <summary>
    /// 保存原始设置值
    /// </summary>
    private void SaveOriginalSettings()
    {
        originalBGMVolume = bgmVolumeSlider.value;
        originalVoiceVolume = voiceVolumeSlider.value;
        originalSFXVolume = sfxVolumeSlider.value;
        originalAutoPlay = autoPlayToggle.isOn;
        originalSkipRead = skipReadToggle.isOn;
        originalTextSpeed = textSpeedSlider.value;
    }
    
    /// <summary>
    /// 应用设置按钮点击事件
    /// </summary>
    private void OnApplySettings()
    {
        SaveSettings();
        Debug.Log("设置已应用并保存");
    }
    
    /// <summary>
    /// 取消设置按钮点击事件
    /// </summary>
    private void OnCancelSettings()
    {
        // 恢复原始设置值
        bgmVolumeSlider.value = originalBGMVolume;
        voiceVolumeSlider.value = originalVoiceVolume;
        sfxVolumeSlider.value = originalSFXVolume;
        autoPlayToggle.isOn = originalAutoPlay;
        skipReadToggle.isOn = originalSkipRead;
        textSpeedSlider.value = originalTextSpeed;
        
        // 应用恢复的设置
        ApplySettings();
        
        // 返回主菜单
        ShowMainMenu();
        Debug.Log("设置已取消");
    }
    
    /// <summary>
    /// 恢复默认设置按钮点击事件
    /// </summary>
    private void OnDefaultSettings()
    {
        // 设置默认值
        bgmVolumeSlider.value = 0.7f;
        voiceVolumeSlider.value = 0.8f;
        sfxVolumeSlider.value = 0.8f;
        autoPlayToggle.isOn = false;
        skipReadToggle.isOn = false;
        textSpeedSlider.value = 1.0f;
        
        // 应用默认设置
        ApplySettings();
        Debug.Log("已恢复默认设置");
    }
    
    /// <summary>
    /// 背景音乐音量变化事件
    /// </summary>
    private void OnBGMVolumeChanged(float volume)
    {
        // 实时应用音量变化
        AudioManager.Instance?.SetBGMVolume(volume);
    }
    
    /// <summary>
    /// 语音音量变化事件
    /// </summary>
    private void OnVoiceVolumeChanged(float volume)
    {
        // 实时应用音量变化
        AudioManager.Instance?.SetVoiceVolume(volume);
    }
    
    /// <summary>
    /// 音效音量变化事件
    /// </summary>
    private void OnSFXVolumeChanged(float volume)
    {
        // 实时应用音量变化
        AudioManager.Instance?.SetSFXVolume(volume);
    }
    
    /// <summary>
    /// 自动播放开关变化事件
    /// </summary>
    private void OnAutoPlayChanged(bool isOn)
    {
        // 实时更新自动播放设置
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.autoPlayEnabled = isOn;
        }
    }
    
    /// <summary>
    /// 跳过已读文本开关变化事件
    /// </summary>
    private void OnSkipReadChanged(bool isOn)
    {
        // 实时更新跳过已读设置
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.skipReadEnabled = isOn;
        }
    }
    
    /// <summary>
    /// 文本速度变化事件
    /// </summary>
    private void OnTextSpeedChanged(float speed)
    {
        // 实时更新文本速度
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.textSpeed = speed;
        }
    }
    
    /// <summary>
    /// 开始新游戏
    /// </summary>
    public void StartNewGame()
    {
        mainMenuPanel.SetActive(false);
        loadGamePanel.SetActive(false);
        settingsPanel.SetActive(false);
        dialoguePanel.SetActive(true);
        
        // 开始新游戏
        GameManager.Instance.InitializeGameData();
        DialogueSystem.Instance.StartScene("scene_1_1");
    }
    
    /// <summary>
    /// 加载指定槽位的存档
    /// </summary>
    /// <param name="slot">存档槽位编号（1-3）</param>
    public void LoadGame(int slot)
    {
        GameManager.Instance.LoadGame(slot);
        mainMenuPanel.SetActive(false);
        loadGamePanel.SetActive(false);
        settingsPanel.SetActive(false);
        dialoguePanel.SetActive(true);
    }
    
    /// <summary>
    /// 更新存档槽位显示
    /// </summary>
    private void UpdateSaveSlots()
    {
        // 实现存档槽位更新逻辑
        // 检查每个槽位是否有存档，显示相应信息
        if (saveSlotButtons != null && saveSlotButtons.Length > 0)
        {
            for (int i = 0; i < saveSlotButtons.Length; i++)
            {
                int slotIndex = i + 1;
                bool hasSave = GameManager.Instance.HasSaveFile(slotIndex);
                
                // 更新槽位编号显示
                if (slotNumberTexts != null && i < slotNumberTexts.Length && slotNumberTexts[i] != null)
                {
                    slotNumberTexts[i].text = $"槽位 {slotIndex}";
                }
                
                // 更新存档信息组件显示状态
                UpdateSaveSlotUI(i, hasSave, slotIndex);
                
                // 设置按钮交互状态
                if (saveSlotButtons[i] != null)
                {
                    saveSlotButtons[i].interactable = hasSave;
                }
            }
        }
        
        // 更新删除按钮状态
        if (deleteSaveButton != null)
        {
            deleteSaveButton.interactable = false; // 默认禁用，需要选择存档后才能启用
        }
    }
    
    /// <summary>
    /// 更新单个存档槽位的UI组件显示
    /// </summary>
    /// <param name="index">槽位索引</param>
    /// <param name="hasSave">是否有存档</param>
    /// <param name="slotIndex">槽位编号</param>
    private void UpdateSaveSlotUI(int index, bool hasSave, int slotIndex)
    {
        // 获取存档数据（如果有）
        SaveData saveData = hasSave ? GameManager.Instance.GetSaveData(slotIndex) : null;
        
        // 更新缩略图显示
        if (thumbnailImages != null && index < thumbnailImages.Length && thumbnailImages[index] != null)
        {
            thumbnailImages[index].gameObject.SetActive(hasSave);
            // 如果有存档，可以在这里设置缩略图内容
            if (hasSave && saveData != null && !string.IsNullOrEmpty(saveData.thumbnailPath))
            {
                // thumbnailImages[index].sprite = Resources.Load<Sprite>(saveData.thumbnailPath);
            }
        }
        
        // 更新章节信息显示
        if (chapterInfoTexts != null && index < chapterInfoTexts.Length && chapterInfoTexts[index] != null)
        {
            if (hasSave && saveData != null)
            {
                chapterInfoTexts[index].text = $"第{saveData.chapter}章 第{saveData.scene}幕";
                chapterInfoTexts[index].gameObject.SetActive(true);
            }
            else
            {
                chapterInfoTexts[index].gameObject.SetActive(false);
            }
        }
        
        // 更新游戏时长显示
        if (playTimeTexts != null && index < playTimeTexts.Length && playTimeTexts[index] != null)
        {
            if (hasSave && saveData != null)
            {
                string formattedTime = GameManager.Instance.FormatPlayTime(saveData.playTime);
                playTimeTexts[index].text = $"游戏时长: {formattedTime}";
                playTimeTexts[index].gameObject.SetActive(true);
            }
            else
            {
                playTimeTexts[index].gameObject.SetActive(false);
            }
        }
        
        // 更新存档日期显示
        if (saveDateTexts != null && index < saveDateTexts.Length && saveDateTexts[index] != null)
        {
            if (hasSave && saveData != null)
            {
                saveDateTexts[index].text = $"存档时间: {saveData.saveDate}";
                saveDateTexts[index].gameObject.SetActive(true);
            }
            else
            {
                saveDateTexts[index].gameObject.SetActive(false);
            }
        }
        
        // 更新空槽位提示文本
        if (emptySlotTexts != null && index < emptySlotTexts.Length && emptySlotTexts[index] != null)
        {
            emptySlotTexts[index].gameObject.SetActive(!hasSave);
            if (!hasSave)
            {
                emptySlotTexts[index].text = "空槽位 - 点击开始新游戏";
            }
            else
            {
                emptySlotTexts[index].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 返回主菜单按钮点击事件
    /// </summary>
    private void OnBackToMenuClicked()
    {
        ShowMainMenu();
        PlayButtonSound();
    }

    /// <summary>
    /// 存档槽位按钮点击事件
    /// </summary>
    /// <param name="slot">存档槽位编号</param>
    private void OnSaveSlotClicked(int slot)
    {
        // 加载选中的存档
        LoadGame(slot);
        PlayButtonSound();
    }

    /// <summary>
    /// 删除存档按钮点击事件
    /// </summary>
    private void OnDeleteSaveClicked()
    {
        // 这里可以实现删除存档的逻辑
        // 需要先选择存档槽位，然后确认删除
        Debug.Log("删除存档功能待实现");
        PlayButtonSound();
    }
    
    /// <summary>
    /// 保存当前设置到PlayerPrefs
    /// </summary>
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("BGMVolume", bgmVolumeSlider.value);
        PlayerPrefs.SetFloat("VoiceVolume", voiceVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetInt("AutoPlay", autoPlayToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("SkipRead", skipReadToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("TextSpeed", textSpeedSlider.value);
        PlayerPrefs.Save();
        
        ApplySettings();
        ShowMainMenu();
    }
    
    /// <summary>
    /// 从PlayerPrefs加载设置
    /// </summary>
    private void LoadSettings()
    {
        bgmVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.7f);
        voiceVolumeSlider.value = PlayerPrefs.GetFloat("VoiceVolume", 0.8f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        autoPlayToggle.isOn = PlayerPrefs.GetInt("AutoPlay", 0) == 1;
        skipReadToggle.isOn = PlayerPrefs.GetInt("SkipRead", 0) == 1;
        textSpeedSlider.value = PlayerPrefs.GetFloat("TextSpeed", 1.0f);
        
        ApplySettings();
    }
    
    /// <summary>
    /// 应用当前设置到游戏系统
    /// </summary>
    private void ApplySettings()
    {
        // 应用音频设置
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in audioSources)
        {
            if (source.CompareTag("BGM"))
            {
                source.volume = bgmVolumeSlider.value;
            }
            else if (source.CompareTag("Voice"))
            {
                source.volume = voiceVolumeSlider.value;
            }
            else if (source.CompareTag("SFX"))
            {
                source.volume = sfxVolumeSlider.value;
            }
        }
        
        // 应用文本速度
        if (DialogueSystem.Instance != null)
        {
            // 这里需要根据文本速度调整打字效果
        }
    }
    
    /// <summary>
    /// 切换物品栏显示状态
    /// </summary>
    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
    
    /// <summary>
    /// 切换状态面板显示状态
    /// </summary>
    public void ToggleStatus()
    {
        statusPanel.SetActive(!statusPanel.activeSelf);
    }
    
    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
    
    /// <summary>
    /// 继续按钮点击事件
    /// </summary>
    private void OnContinueClicked()
    {
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.DisplayNextLine();
        }
        PlayButtonSound();
    }

    /// <summary>
    /// 跳过按钮点击事件
    /// </summary>
    private void OnSkipClicked()
    {
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.SkipCurrentDialogue();
        }
        PlayButtonSound();
    }

    /// <summary>
    /// 自动播放按钮点击事件
    /// </summary>
    private void OnAutoPlayClicked()
    {
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.ToggleAutoPlay();
        }
        PlayButtonSound();
        UpdateAutoPlayIndicator();
    }

    /// <summary>
    /// 日志按钮点击事件
    /// </summary>
    private void OnLogClicked()
    {
        // 显示对话日志面板
        Debug.Log("打开对话日志");
        PlayButtonSound();
    }

    /// <summary>
    /// 设置按钮点击事件
    /// </summary>
    private void OnSettingsClicked()
    {
        ShowSettings();
        PlayButtonSound();
    }

    /// <summary>
    /// 存档按钮点击事件
    /// </summary>
    private void OnSaveClicked()
    {
        if (GameManager.Instance != null)
        {
            // 使用当前选中的存档槽位进行快速存档
            GameManager.Instance.SaveGame(GameManager.Instance.currentSaveSlot);
            Debug.Log("游戏已快速保存到槽位 " + GameManager.Instance.currentSaveSlot);
        }
        PlayButtonSound();
    }

    /// <summary>
    /// 读档按钮点击事件
    /// </summary>
    private void OnLoadClicked()
    {
        // 显示读档面板，让用户选择存档槽位
        ShowLoadGame();
        PlayButtonSound();
    }

    /// <summary>
    /// 物品栏按钮点击事件
    /// </summary>
    private void OnInventoryClicked()
    {
        ToggleInventory();
        PlayButtonSound();
    }

    /// <summary>
    /// 状态按钮点击事件
    /// </summary>
    private void OnStatusClicked()
    {
        ToggleStatus();
        PlayButtonSound();
    }

    /// <summary>
    /// 播放按钮点击音效
    /// </summary>
    private void PlayButtonSound()
    {
        if (DialogueSystem.Instance != null && DialogueSystem.Instance.buttonSound != null)
        {
            AudioManager.Instance?.PlaySFX(DialogueSystem.Instance.buttonSound);
        }
    }

    /// <summary>
    /// 更新自动播放指示器状态
    /// </summary>
    private void UpdateAutoPlayIndicator()
    {
        if (autoPlayButton != null && DialogueSystem.Instance != null)
        {
            // 这里可以添加自动播放状态的视觉反馈
            // 例如改变按钮颜色或显示状态文本
        }
    }
}