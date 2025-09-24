using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 快速开始指南 - 演示如何使用框架创建第一个场景
/// </summary>
public class QuickStartGuide : MonoBehaviour
{
    [Header("角色管理器引用")]
    public CharacterManager characterManager;
    
    [Header("场景数据管理器引用")]
    public SceneDataManager sceneDataManager;
    
    [Header("对话系统引用")]
    public DialogueSystem dialogueSystem;
    
    void Start()
    {
        // 确保单例实例已初始化
        if (characterManager == null)
            characterManager = CharacterManager.Instance;
        
        if (sceneDataManager == null) 
            sceneDataManager = SceneDataManager.Instance;
            
        if (dialogueSystem == null)
            dialogueSystem = DialogueSystem.Instance;
        
        // 创建示例角色
        CreateExampleCharacters();
        
        // 创建示例场景
        CreateExampleScene();
        
        Debug.Log("快速开始指南已初始化！");
        Debug.Log("在Unity编辑器中配置好UI引用后，可以调用StartExampleGame()开始游戏");
    }
    
    /// <summary>
    /// 创建示例角色
    /// </summary>
    private void CreateExampleCharacters()
    {
        // 这里只是演示，实际应该在Inspector面板中配置
        Debug.Log("请在CharacterManager的Inspector面板中配置角色：");
        Debug.Log("1. 添加新角色到characters列表");
        Debug.Log("2. 设置characterName为'Alice'");
        Debug.Log("3. 设置fullName为'爱丽丝'");
        Debug.Log("4. 拖拽表情精灵到expressions数组");
    }
    
    /// <summary>
    /// 创建示例场景
    /// </summary>
    private void CreateExampleScene()
    {
        // 示例场景数据
        SceneData exampleScene = new SceneData
        {
            sceneId = "demo_scene_1",
            dialogueLines = new List<DialogueLine>
            {
                new DialogueLine
                {
                    characterName = "Alice",
                    dialogueText = "这是一个示例对话！恭喜你成功配置了框架。",
                    expression = Expression.Happy
                },
                new DialogueLine
                {
                    characterName = "Alice", 
                    dialogueText = "接下来你可以在SceneDataManager中创建更多的场景和选择分支。",
                    expression = Expression.Normal
                }
            },
            choices = new List<Choice>()
        };
        
        // 添加到场景管理器
        sceneDataManager.AddScene(exampleScene);
        Debug.Log("已创建示例场景: demo_scene_1");
    }
    
    /// <summary>
    /// 开始示例游戏
    /// </summary>
    public void StartExampleGame()
    {
        if (dialogueSystem != null)
        {
            dialogueSystem.StartScene("demo_scene_1");
            Debug.Log("示例游戏已开始！");
        }
        else
        {
            Debug.LogError("对话系统未找到！请确保DialogueSystem已添加到场景中");
        }
    }
    
    /// <summary>
    /// 在编辑器中测试场景
    /// </summary>
    [ContextMenu("测试示例场景")]
    public void TestExampleScene()
    {
        StartExampleGame();
    }
}