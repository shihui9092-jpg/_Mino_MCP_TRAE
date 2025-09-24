using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景数据管理器 - 负责管理游戏中的所有对话场景数据
/// 使用单例模式确保全局访问，提供场景数据的加载、查询和管理功能
/// </summary>
public class SceneDataManager : MonoBehaviour
{
    /// <summary>
    /// 场景数据管理器单例实例
    /// </summary>
    public static SceneDataManager Instance { get; private set; }
    
    [Header("场景数据")]
    [Tooltip("所有游戏场景的列表")]
    public List<SceneData> allScenes = new List<SceneData>();
    
    // 场景字典，用于快速查找
    private Dictionary<string, SceneData> sceneDictionary = new Dictionary<string, SceneData>();
    
    /// <summary>
    /// 初始化方法，设置单例实例并初始化场景数据
    /// </summary>
    private void Awake()
    {
        // 单例模式实现
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 跨场景不销毁
            InitializeScenes(); // 初始化场景数据
        }
        else
        {
            Destroy(gameObject); // 销毁重复实例
        }
    }
    
    /// <summary>
    /// 初始化场景数据，将场景列表转换为字典以便快速查找
    /// 如果没有场景数据，会自动创建演示场景
    /// </summary>
    private void InitializeScenes()
    {
        // 初始化场景字典
        foreach (SceneData scene in allScenes)
        {
            sceneDictionary[scene.sceneId] = scene;
        }
        
        // 如果没有场景数据，创建示例场景
        if (allScenes.Count == 0)
        {
            CreateDemoScenes();
        }
    }
    
    /// <summary>
    /// 创建演示场景数据，用于开发和测试
    /// </summary>
    private void CreateDemoScenes()
    {
        // 创建示例场景1
        SceneData scene1 = new SceneData
        {
            sceneId = "scene_1_1",
            dialogueLines = new List<DialogueLine>
            {
                new DialogueLine
                {
                    characterName = "Alice",
                    dialogueText = "你好，旅行者！欢迎来到我们的世界。",
                    expression = Expression.Happy
                },
                new DialogueLine
                {
                    characterName = "Bob",
                    dialogueText = "我是Bob，很高兴认识你。",
                    expression = Expression.Normal
                }
            },
            choices = new List<Choice>
            {
                new Choice
                {
                    choiceText = "向Alice问好",
                    nextScene = "scene_1_2",
                    effects = new Dictionary<string, int>
                    {
                        { "affection_Alice", 5 }
                    }
                },
                new Choice
                {
                    choiceText = "向Bob问好",
                    nextScene = "scene_1_3",
                    effects = new Dictionary<string, int>
                    {
                        { "affection_Bob", 5 }
                    }
                }
            }
        };
        
        // 创建示例场景2
        SceneData scene2 = new SceneData
        {
            sceneId = "scene_1_2",
            dialogueLines = new List<DialogueLine>
            {
                new DialogueLine
                {
                    characterName = "Alice",
                    dialogueText = "谢谢你选择了我！让我们开始冒险吧！",
                    expression = Expression.Happy
                }
            },
            choices = new List<Choice>()
        };
        
        // 创建示例场景3
        SceneData scene3 = new SceneData
        {
            sceneId = "scene_1_3",
            dialogueLines = new List<DialogueLine>
            {
                new DialogueLine
                {
                    characterName = "Bob",
                    dialogueText = "很高兴你选择了我！准备好开始旅程了吗？",
                    expression = Expression.Happy
                }
            },
            choices = new List<Choice>()
        };
        
        allScenes.Add(scene1);
        allScenes.Add(scene2);
        allScenes.Add(scene3);
        
        sceneDictionary[scene1.sceneId] = scene1;
        sceneDictionary[scene2.sceneId] = scene2;
        sceneDictionary[scene3.sceneId] = scene3;
    }
    
    /// <summary>
    /// 根据场景ID获取场景数据
    /// </summary>
    /// <param name="sceneId">场景标识符</param>
    /// <returns>场景数据，如果不存在则返回null</returns>
    public SceneData GetScene(string sceneId)
    {
        if (sceneDictionary.ContainsKey(sceneId))
        {
            return sceneDictionary[sceneId];
        }
        return null;
    }
    
    /// <summary>
    /// 根据章节编号获取该章节的所有场景
    /// </summary>
    /// <param name="chapter">章节编号</param>
    /// <returns>该章节的场景列表</returns>
    public List<SceneData> GetScenesByChapter(int chapter)
    {
        return allScenes.FindAll(scene => scene.sceneId.StartsWith($"scene_{chapter}_"));
    }
    
    /// <summary>
    /// 添加新场景到管理器
    /// </summary>
    /// <param name="scene">要添加的场景数据</param>
    public void AddScene(SceneData scene)
    {
        if (!sceneDictionary.ContainsKey(scene.sceneId))
        {
            allScenes.Add(scene);
            sceneDictionary[scene.sceneId] = scene;
        }
    }
    
    /// <summary>
    /// 从管理器中移除指定场景
    /// </summary>
    /// <param name="sceneId">要移除的场景ID</param>
    public void RemoveScene(string sceneId)
        {
        if (sceneDictionary.ContainsKey(sceneId))
        {
            SceneData scene = sceneDictionary[sceneId];
            allScenes.Remove(scene);
            sceneDictionary.Remove(sceneId);
        }
    }
    
    /// <summary>
    /// 获取指定场景的下一场景ID
    /// </summary>
    /// <param name="currentSceneId">当前场景ID</param>
    /// <returns>下一场景ID，如果没有则返回null</returns>
    public string GetNextScene(string currentSceneId)
    {
        SceneData currentScene = GetScene(currentSceneId);
        if (currentScene != null && currentScene.choices.Count > 0)
        {
            return currentScene.choices[0].nextScene;
        }
        return null;
    }
    
    /// <summary>
    /// 检查指定场景是否有选择分支
    /// </summary>
    /// <param name="sceneId">场景ID</param>
    /// <returns>是否有选择分支</returns>
    public bool HasChoices(string sceneId)
    {
        SceneData scene = GetScene(sceneId);
        return scene != null && scene.choices.Count > 0;
    }
}