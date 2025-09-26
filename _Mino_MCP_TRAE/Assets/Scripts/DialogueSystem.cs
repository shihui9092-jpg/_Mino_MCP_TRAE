using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 对话行数据结构 - 存储单行对话的所有信息
/// </summary>
[System.Serializable]
public class DialogueLine
{
    [Tooltip("角色名称")]
    public string characterName;
    [Tooltip("对话文本内容")]
    public string dialogueText;
    [Tooltip("角色头像精灵")]
    public Sprite characterSprite;
    [Tooltip("语音音频剪辑")]
    public AudioClip voiceClip;
    [Tooltip("角色表情")]
    public Expression expression;
}

/// <summary>
/// 选择项数据结构 - 存储对话选择项的信息
/// </summary>
[System.Serializable]
public class Choice
{
    [Tooltip("选择项文本")]
    public string choiceText;
    [Tooltip("选择后跳转的场景ID")]
    public string nextScene;
    [Tooltip("选择效果字典（角色好感度变化、标志设置等）")]
    public Dictionary<string, int> effects;
}

/// <summary>
/// 场景数据结构 - 存储完整对话场景的所有信息
/// </summary>
[System.Serializable]
public class SceneData
{
    [Tooltip("场景唯一标识符")]
    public string sceneId;
    [Tooltip("对话行列表")]
    public List<DialogueLine> dialogueLines;
    [Tooltip("选择项列表")]
    public List<Choice> choices;
    [Tooltip("背景音乐音频剪辑")]
    public AudioClip backgroundMusic;
    [Tooltip("背景图片精灵")]
    public Sprite backgroundImage;
}

/// <summary>
/// 角色表情枚举 - 定义角色可用的表情状态
/// </summary>
public enum Expression
{
    [Tooltip("普通表情")]
    Normal,
    [Tooltip("开心表情")]
    Happy,
    [Tooltip("悲伤表情")]
    Sad,
    [Tooltip("生气表情")]
    Angry,
    [Tooltip("惊讶表情")]
    Surprised,
    [Tooltip("神秘表情")]
    Mysterious,
    [Tooltip("严肃表情")]
    Serious,
    [Tooltip("兴奋表情")]
    Excited,
    [Tooltip("自信表情")]
    Confident
}

/// <summary>
/// 对话系统 - 负责管理游戏中的对话流程、选择系统和UI显示
/// 使用单例模式确保全局访问，支持多角色对话、选择分支和表情系统
/// </summary>
public class DialogueSystem : MonoBehaviour
{
    /// <summary>
    /// 对话系统单例实例
    /// </summary>
    public static DialogueSystem Instance { get; private set; }
    
    [Header("UI组件")]
    [Tooltip("对话面板游戏对象")]
    public GameObject dialoguePanel;
    [Tooltip("角色名称文本组件")]
    public TextMeshProUGUI characterNameText;
    [Tooltip("对话文本组件")]
    public TextMeshProUGUI dialogueText;
    [Tooltip("角色头像图像组件")]
    public Image characterImage;
    [Tooltip("背景图像组件")]
    public Image backgroundImage;
    [Tooltip("选择面板游戏对象")]
    public GameObject choicePanel;
    [Tooltip("选择按钮预制体")]
    public GameObject choiceButtonPrefab;
    
    [Header("音频组件")]
    [Tooltip("背景音乐音频源")]
    public AudioSource bgmSource;
    [Tooltip("语音音频源")]
    public AudioSource voiceSource;
    [Tooltip("音效音频源")]
    public AudioSource sfxSource;
    
    [Header("对话设置")]
    [Tooltip("打字效果速度（秒/字符）")]
    public float textSpeed = 0.05f;
    
    [Header("场景数据")]
    [Tooltip("所有场景数据列表")]
    public List<SceneData> scenes;
    
    // 当前对话状态
    private int currentLineIndex = 0;
    private SceneData currentScene;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    
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
    /// 开始指定场景的对话
    /// </summary>
    /// <param name="sceneId">场景ID</param>
    public void StartScene(string sceneId)
    {
        // 检查SceneDataManager是否已初始化
        if (SceneDataManager.Instance == null)
        {
            Debug.LogError("SceneDataManager实例为空！请确保SceneDataManager已正确初始化。");
            return;
        }
        
        SceneData scene = SceneDataManager.Instance.GetScene(sceneId);
        
        // 添加空值检查
        if (scene == null)
        {
            Debug.LogError($"场景 {sceneId} 不存在！请检查场景数据配置。");
            return;
        }
        
        if (scene.dialogueLines == null)
        {
            Debug.LogWarning($"场景 {sceneId} 的dialogueLines为空，将创建空列表。");
            scene.dialogueLines = new List<DialogueLine>();
        }
        
        currentScene = scene;
        currentLineIndex = 0;
        
        // 重置UI状态
        if (choicePanel != null)
        {
            choicePanel.SetActive(false);
        }
        
        // 应用场景数据 - 背景音乐和背景图片
        ApplySceneData(scene);
        
        // 重置打字状态，确保新场景开始时没有正在进行的打字效果
        isTyping = false;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        
        // 检查对话面板是否已绑定
        if (dialoguePanel == null)
        {
            Debug.LogError("对话面板未绑定！请在Inspector中设置dialoguePanel。");
            return;
        }
        
        dialoguePanel.SetActive(true);
        
        // 记录场景开始信息
        Debug.Log($"开始场景对话: {sceneId}");
        
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        // 检查currentScene是否为空
        if (currentScene == null)
        {
            Debug.LogError("currentScene is null!");
            return;
        }
        
        // 检查dialogueLines是否为空
        if (currentScene.dialogueLines == null)
        {
            Debug.LogError("currentScene.dialogueLines is null!");
            currentScene.dialogueLines = new List<DialogueLine>();
            return;
        }
        
        if (isTyping)
        {
            CompleteTyping();
            return;
        }
        
        if (currentLineIndex < currentScene.dialogueLines.Count)
        {
            DialogueLine line = currentScene.dialogueLines[currentLineIndex];
            DisplayDialogueLine(line);
            currentLineIndex++;
        }
        else
        {
            ShowChoices();
        }
    }

    private void DisplayDialogueLine(DialogueLine line)
    {
        // 检查UI组件是否绑定
        if (characterNameText == null)
        {
            Debug.LogError("characterNameText is not assigned in Inspector!");
            return;
        }
        
        if (dialogueText == null)
        {
            Debug.LogError("dialogueText is not assigned in Inspector!");
            return;
        }
        
        // 设置角色名称 - 优先使用CharacterManager中的角色数据
        if (CharacterManager.Instance != null)
        {
            Character character = CharacterManager.Instance.GetCharacter(line.characterName);
            if (character != null)
            {
                // 使用CharacterManager中的角色完整名称和颜色
                characterNameText.text = character.fullName;
                characterNameText.color = character.nameColor;
            }
            else
            {
                characterNameText.text = line.characterName;
                characterNameText.color = Color.white;
            }
        }
        else
        {
            characterNameText.text = line.characterName;
            characterNameText.color = Color.white;
        }
        
        // 设置角色头像 - 优先使用CharacterManager中的表情系统
        Sprite characterSprite = null;
        if (CharacterManager.Instance != null && !string.IsNullOrEmpty(line.characterName))
        {
            characterSprite = CharacterManager.Instance.GetExpressionSprite(line.characterName, line.expression);
        }
        
        if (characterSprite != null)
        {
            characterImage.sprite = characterSprite;
            characterImage.gameObject.SetActive(true);
        }
        else if (line.characterSprite != null)
        {
            characterImage.sprite = line.characterSprite;
            characterImage.gameObject.SetActive(true);
        }
        else
        {
            characterImage.gameObject.SetActive(false);
        }
        
        // 播放语音
        if (line.voiceClip != null)
        {
            voiceSource.clip = line.voiceClip;
            voiceSource.Play();
        }
        
        // 开始打字效果
        typingCoroutine = StartCoroutine(TypeText(line.dialogueText));
    }
    
    /// <summary>
    /// 打字效果协程
    /// </summary>
    /// <param name="text">要显示的文本</param>
    /// <returns>协程迭代器</returns>
    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";
        
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed); // 使用可配置的打字速度
        }
        
        isTyping = false;
    }
    
    /// <summary>
    /// 完成当前打字效果，立即显示完整文本
    /// </summary>
    private void CompleteTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        
        // 检查索引边界，防止越界
        if (currentScene == null || currentScene.dialogueLines == null || currentLineIndex <= 0 || currentLineIndex > currentScene.dialogueLines.Count)
        {
            Debug.LogWarning("无法完成打字效果：索引越界或场景数据异常");
            isTyping = false;
            return;
        }
        
        DialogueLine line = currentScene.dialogueLines[currentLineIndex - 1];
        dialogueText.text = line.dialogueText;
        isTyping = false;
    }
    
    /// <summary>
    /// 显示选择项面板
    /// </summary>
    private void ShowChoices()
    {
        if (currentScene.choices.Count > 0)
        {
            choicePanel.SetActive(true);
            
            // 清除旧的选项按钮
            foreach (Transform child in choicePanel.transform)
            {
                Destroy(child.gameObject);
            }
            
            // 创建新的选项按钮
            foreach (Choice choice in currentScene.choices)
            {
                GameObject buttonObj = Instantiate(choiceButtonPrefab, choicePanel.transform);
                TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = choice.choiceText;
                
                Button button = buttonObj.GetComponent<Button>();
                button.onClick.AddListener(() => OnChoiceSelected(choice));
            }
        }
        else
        {
            // 没有选择，自动进入下一场景
            Debug.Log("场景结束");
        }
    }
    
    /// <summary>
    /// 选择项被选中时的处理
    /// </summary>
    /// <param name="choice">被选中的选择项</param>
    private void OnChoiceSelected(Choice choice)
    {
        // 播放选择音效
        PlaySFX(choiceSelectSound);
        
        // 应用选择效果
        if (choice.effects != null)
        {
            foreach (var effect in choice.effects)
            {
                if (effect.Key.StartsWith("affection_"))
                {
                    string characterName = effect.Key.Replace("affection_", "");
                    GameManager.Instance.ChangeAffection(characterName, effect.Value);
                }
                else if (effect.Key.StartsWith("flag_"))
                {
                    string flagName = effect.Key.Replace("flag_", "");
                    GameManager.Instance.SetFlag(flagName, effect.Value > 0);
                }
            }
        }
        
        // 关闭选择面板
        if (choicePanel != null)
        {
            choicePanel.SetActive(false);
        }
        
        // 记录选择信息
        Debug.Log($"选择了选项: {choice.choiceText}, 跳转到场景: {choice.nextScene}");
        
        // 检查目标场景是否存在
        SceneData nextSceneData = SceneDataManager.Instance.GetScene(choice.nextScene);
        if (nextSceneData == null)
        {
            Debug.LogError($"目标场景不存在: {choice.nextScene}");
            // 显示错误信息或返回主菜单
            return;
        }
        
        // 确保UI状态正确重置
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        
        // 清除当前对话状态
        currentScene = null;
        currentLineIndex = 0;
        
        // 短暂延迟后进入下一场景，确保UI状态完全重置
        StartCoroutine(DelayedSceneTransition(choice.nextScene));
    }
    
    /// <summary>
    /// 每帧更新检测输入
    /// </summary>
    private void Update()
    {
        // 只有在对话面板激活且当前场景不为空时才检测输入
        if (dialoguePanel != null && dialoguePanel.activeInHierarchy && currentScene != null)
        {
            // 鼠标左键或空格键触发下一对话
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                DisplayNextLine();
            }
        }
    }
    
    /// <summary>
    /// 应用场景数据 - 背景音乐和背景图片
    /// </summary>
    /// <param name="scene">场景数据</param>
    private void ApplySceneData(SceneData scene)
    {
        // 应用背景音乐
        if (scene.backgroundMusic != null && bgmSource != null)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayBGM(scene.backgroundMusic.name);
            }
            else
            {
                bgmSource.clip = scene.backgroundMusic;
                bgmSource.Play();
            }
        }
        
        // 应用背景图片
        if (scene.backgroundImage != null && backgroundImage != null)
        {
            backgroundImage.sprite = scene.backgroundImage;
            backgroundImage.gameObject.SetActive(true);
        }
        else if (backgroundImage != null)
        {
            backgroundImage.gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// 跳过当前对话
    /// </summary>
    public void SkipCurrentDialogue()
    {
        if (isTyping)
        {
            CompleteTyping();
        }
        else
        {
            DisplayNextLine();
        }
        
        // 播放跳过音效
        PlaySFX(skipSound);
    }
    
    /// <summary>
    /// 切换自动播放模式
    /// </summary>
    public void ToggleAutoPlay()
    {
        autoPlayEnabled = !autoPlayEnabled;
        
        // 播放切换音效
        PlaySFX(toggleSound);
        
        // 如果开启自动播放且当前没有在打字，自动推进
        if (autoPlayEnabled && !isTyping)
        {
            StartCoroutine(AutoPlayNextLine());
        }
    }
    
    /// <summary>
    /// 自动播放下一行
    /// </summary>
    private IEnumerator AutoPlayNextLine()
    {
        yield return new WaitForSeconds(autoPlayDelay);
        if (autoPlayEnabled && !isTyping)
        {
            DisplayNextLine();
        }
    }
    
    /// <summary>
    /// 延迟场景切换，确保UI状态完全重置
    /// </summary>
    /// <param name="sceneId">目标场景ID</param>
    private IEnumerator DelayedSceneTransition(string sceneId)
    {
        // 等待一帧，确保UI状态完全重置
        yield return null;
        
        // 进入下一场景
        StartScene(sceneId);
    }
    
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="clip">音效剪辑</param>
    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    [Header("对话设置")]
    [Tooltip("自动播放模式")]
    public bool autoPlayEnabled = false;
    [Tooltip("跳过已读文本模式")]
    public bool skipReadEnabled = false;
    [Tooltip("自动播放延迟时间（秒）")]
    public float autoPlayDelay = 2.0f;
    
    [Header("音效资源")]
    [Tooltip("按钮点击音效")]
    public AudioClip buttonSound;
    [Tooltip("跳过音效")]
    public AudioClip skipSound;
    [Tooltip("切换音效")]
    public AudioClip toggleSound;
    [Tooltip("选择音效")]
    public AudioClip choiceSelectSound;
    [Tooltip("打字机音效")]
    public AudioClip typewriterSound;
}