using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    // 序列化字段：在Inspector中显示并可拖拽赋值
    [Header("UI References")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject loadGamePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject dialoguePanel;
    
    [Header("Button References")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    // 单例模式实现
    public static MainSceneController Instance { get; private set; }

    private void Awake()
    {
        // 单例模式设置
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        InitializeUI();
    }

    private void Start()
    {
        // 确保所有管理器都已初始化
        InitializeManagers();
        
        // 开始游戏流程
        StartCoroutine(GameStartSequence());
    }
    
    // UI初始化
    private void InitializeUI()
    {
        // 确保开始时只显示主菜单
        SetPanelActive(mainMenuPanel, true);
        SetPanelActive(loadGamePanel, false);
        SetPanelActive(settingsPanel, false);
        SetPanelActive(dialoguePanel, false);
        
        // 绑定按钮事件
        SetupButtonListeners();
    }

    // 设置按钮监听器
    private void SetupButtonListeners()
    {
        if (newGameButton != null)
            newGameButton.onClick.AddListener(OnNewGameSelected);
        if (loadGameButton != null)
            loadGameButton.onClick.AddListener(OnLoadGameSelected);
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsSelected);
        if (exitButton != null)
            exitButton.onClick.AddListener(OnExitSelected);
    }

    // 按钮事件处理方法
    public void OnNewGameSelected()
    {
        Debug.Log("新游戏按钮被点击");
        SetPanelActive(mainMenuPanel, false);
        SetPanelActive(dialoguePanel, true);
        
        // 开始新游戏序列
        StartCoroutine(StartNewGameSequence());
    }

    public void OnLoadGameSelected()
    {
        Debug.Log("加载游戏按钮被点击");
        SetPanelActive(mainMenuPanel, false);
        SetPanelActive(loadGamePanel, true);
    }

    public void OnSettingsSelected()
    {
        Debug.Log("设置按钮被点击");
        SetPanelActive(mainMenuPanel, false);
        SetPanelActive(settingsPanel, true);
    }

    public void OnExitSelected()
    {
        Debug.Log("退出按钮被点击");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // 通用面板显示控制方法
    private void SetPanelActive(GameObject panel, bool isActive)
    {
        if (panel != null)
        {
            panel.SetActive(isActive);
        }
    }

    // 返回主菜单
    public void ReturnToMainMenu()
    {
        SetPanelActive(mainMenuPanel, true);
        SetPanelActive(loadGamePanel, false);
        SetPanelActive(settingsPanel, false);
        SetPanelActive(dialoguePanel, false);
    }
    
    private void InitializeManagers()
    {
        // 这里可以确保所有必要的管理器都存在
        if (GameManager.Instance == null)
        {
            GameObject gameManager = new GameObject("GameManager");
            gameManager.AddComponent<GameManager>();
        }
        
        if (DialogueSystem.Instance == null)
        {
            GameObject dialogueSystem = new GameObject("DialogueSystem");
            dialogueSystem.AddComponent<DialogueSystem>();
        }
        
        if (UIManager.Instance == null)
        {
            GameObject uiManager = new GameObject("UIManager");
            uiManager.AddComponent<UIManager>();
        }
        
        if (CharacterManager.Instance == null)
        {
            GameObject characterManager = new GameObject("CharacterManager");
            characterManager.AddComponent<CharacterManager>();
        }
        
        if (SceneDataManager.Instance == null)
        {
            GameObject sceneDataManager = new GameObject("SceneDataManager");
            sceneDataManager.AddComponent<SceneDataManager>();
        }
        
        if (AudioManager.Instance == null)
        {
            GameObject audioManager = new GameObject("AudioManager");
            audioManager.AddComponent<AudioManager>();
        }
        
        if (VisualEffects.Instance == null)
        {
            GameObject visualEffects = new GameObject("VisualEffects");
            visualEffects.AddComponent<VisualEffects>();
        }
    }
    
    private IEnumerator GameStartSequence()
    {
        // 立即播放主菜单音乐（无延迟）
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayBGMImmediately("MainMenu", true);
        
        // 播放开场动画
        if (VisualEffects.Instance != null)
            VisualEffects.Instance.FadeIn();
        yield return new WaitForSeconds(2f);
        
        // 显示主菜单
        if (UIManager.Instance != null)
            UIManager.Instance.ShowMainMenu();
    }
    
    private IEnumerator StartNewGameSequence()
    {
        // 淡出效果
        if (VisualEffects.Instance != null)
            VisualEffects.Instance.FadeOut();
        yield return new WaitForSeconds(1f);
        
        // 停止菜单音乐
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopBGM();
        
        // 开始游戏
        if (UIManager.Instance != null)
            UIManager.Instance.StartNewGame();
        
        // 淡入效果
        if (VisualEffects.Instance != null)
            VisualEffects.Instance.FadeIn();
    }
    
    // 场景管理
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneSequence(sceneName));
    }
    
    private IEnumerator LoadSceneSequence(string sceneName)
    {
        if (VisualEffects.Instance != null)
            VisualEffects.Instance.FadeOut();
        yield return new WaitForSeconds(1f);
        
        SceneManager.LoadScene(sceneName);
        
        if (VisualEffects.Instance != null)
            VisualEffects.Instance.FadeIn();
    }
}