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
                    nextScene = "alice_route_1",
                    effects = new Dictionary<string, int>
                    {
                        { "affection_Alice", 5 }
                    }
                },
                new Choice
                {
                    choiceText = "向Bob问好",
                    nextScene = "bob_route_1",
                    effects = new Dictionary<string, int>
                    {
                        { "affection_Bob", 5 }
                    }
                },
                new Choice
                {
                    choiceText = "探索Carol的秘密",
                    nextScene = "carol_route_1",
                    effects = new Dictionary<string, int>
                    {
                        { "affection_Carol", 5 }
                    }
                }
            }
        };
        
        // 创建Alice路线场景1
        SceneData aliceScene1 = new SceneData
        {
            sceneId = "alice_route_1",
            dialogueLines = new List<DialogueLine>
            {
                new DialogueLine
                {
                    characterName = "Alice",
                    dialogueText = "谢谢你选择了我！让我们开始魔法学院的冒险吧！",
                    expression = Expression.Happy
                },
                new DialogueLine
                {
                    characterName = "Alice",
                    dialogueText = "在魔法学院，你将学习各种强大的法术。准备好了吗？",
                    expression = Expression.Excited
                }
            },
            choices = new List<Choice>
            {
                new Choice
                {
                    choiceText = "学习火焰魔法",
                    nextScene = "alice_route_2_fire",
                    effects = new Dictionary<string, int>
                    {
                        { "affection_Alice", 10 },
                        { "flag_fire_magic", 1 }
                    }
                },
                new Choice
                {
                    choiceText = "学习冰霜魔法",
                    nextScene = "alice_route_2_ice",
                    effects = new Dictionary<string, int>
                    {
                        { "affection_Alice", 10 },
                        { "flag_ice_magic", 1 }
                    }
                }
            }
        };
        
        // 创建Bob路线场景1
        SceneData bobScene1 = new SceneData
        {
            sceneId = "bob_route_1",
            dialogueLines = new List<DialogueLine>
            {
                new DialogueLine
                {
                    characterName = "Bob",
                    dialogueText = "很高兴你选择了我！准备好开始骑士团的旅程了吗？",
                    expression = Expression.Happy
                },
                new DialogueLine
                {
                    characterName = "Bob",
                    dialogueText = "作为骑士，你需要选择你的专精方向。",
                    expression = Expression.Normal
                }
            },
            choices = new List<Choice>
            {
                new Choice
                {
                    choiceText = "成为剑术大师",
                    nextScene = "bob_route_2_sword",
                    effects = new Dictionary<string, int>
                    {
                        { "affection_Bob", 10 },
                        { "flag_sword_master", 1 }
                    }
                },
                new Choice
                {
                    choiceText = "成为防御专家",
                    nextScene = "bob_route_2_shield",
                    effects = new Dictionary<string, int>
                    {
                        { "affection_Bob", 10 },
                        { "flag_shield_expert", 1 }
                    }
                }
            }
        };
        
        // 创建Carol路线场景1
        SceneData carolScene1 = new SceneData
        {
            sceneId = "carol_route_1",
            dialogueLines = new List<DialogueLine>
            {
                new DialogueLine
                {
                    characterName = "Carol",
                    dialogueText = "命运的选择就在你的手中...让我们探索未知的秘密吧！",
                    expression = Expression.Mysterious
                },
                new DialogueLine
                {
                    characterName = "Carol",
                    dialogueText = "在命运的十字路口，你需要做出重要的决定。",
                    expression = Expression.Serious
                }
            },
            choices = new List<Choice>
            {
                new Choice
                {
                    choiceText = "探索古代遗迹",
                    nextScene = "carol_route_2_ruins",
                    effects = new Dictionary<string, int>
                    {
                        { "affection_Carol", 10 },
                        { "flag_ancient_ruins", 1 }
                    }
                },
                new Choice
                {
                    choiceText = "研究神秘符文",
                    nextScene = "carol_route_2_runes",
                    effects = new Dictionary<string, int>
                    {
                        { "affection_Carol", 10 },
                        { "flag_mystic_runes", 1 }
                    }
                }
            }
        };
        
        // 创建Alice路线的二级场景
        SceneData aliceScene2Fire = new SceneData
        {
            sceneId = "alice_route_2_fire",
            dialogueLines = new List<DialogueLine>
            {
                new DialogueLine
                {
                    characterName = "Alice",
                    dialogueText = "火焰魔法是最具破坏力的元素魔法！让我们开始学习吧！",
                    expression = Expression.Excited
                }
            },
            choices = new List<Choice>()
        };
        
        SceneData aliceScene2Ice = new SceneData
        {
            sceneId = "alice_route_2_ice",
            dialogueLines = new List<DialogueLine>
            {
                new DialogueLine
                {
                    characterName = "Alice",
                    dialogueText = "冰霜魔法能够冻结一切！这是非常优雅的魔法！",
                    expression = Expression.Happy
                }
            },
            choices = new List<Choice>()
        };
        
        // 创建Bob路线的二级场景
        SceneData bobScene2Sword = new SceneData
        {
            sceneId = "bob_route_2_sword",
            dialogueLines = new List<DialogueLine>
            {
                new DialogueLine
                {
                    characterName = "Bob",
                    dialogueText = "剑术大师之路充满挑战，但你会成为最强的战士！",
                    expression = Expression.Confident
                }
            },
            choices = new List<Choice>()
        };
        
        SceneData bobScene2Shield = new SceneData
        {
            sceneId = "bob_route_2_shield",
            dialogueLines = new List<DialogueLine>
            {
                new DialogueLine
                {
                    characterName = "Bob",
                    dialogueText = "防御专家能够保护所有人！这是最光荣的职责！",
                    expression = Expression.Serious
                }
            },
            choices = new List<Choice>()
        };
        
        // 创建Carol路线的二级场景
        SceneData carolScene2Ruins = new SceneData
        {
            sceneId = "carol_route_2_ruins",
            dialogueLines = new List<DialogueLine>
            {
                new DialogueLine
                {
                    characterName = "Carol",
                    dialogueText = "古代遗迹中隐藏着失落的文明秘密...让我们开始探索！",
                    expression = Expression.Mysterious
                }
            },
            choices = new List<Choice>()
        };
        
        SceneData carolScene2Runes = new SceneData
        {
            sceneId = "carol_route_2_runes",
            dialogueLines = new List<DialogueLine>
            {
                new DialogueLine
                {
                    characterName = "Carol",
                    dialogueText = "神秘符文蕴含着强大的力量，需要小心研究...",
                    expression = Expression.Serious
                }
            },
            choices = new List<Choice>()
        };
        
        allScenes.Add(scene1);
        allScenes.Add(aliceScene1);
        allScenes.Add(bobScene1);
        allScenes.Add(carolScene1);
        allScenes.Add(aliceScene2Fire);
        allScenes.Add(aliceScene2Ice);
        allScenes.Add(bobScene2Sword);
        allScenes.Add(bobScene2Shield);
        allScenes.Add(carolScene2Ruins);
        allScenes.Add(carolScene2Runes);
        
        sceneDictionary[scene1.sceneId] = scene1;
        sceneDictionary[aliceScene1.sceneId] = aliceScene1;
        sceneDictionary[bobScene1.sceneId] = bobScene1;
        sceneDictionary[carolScene1.sceneId] = carolScene1;
        sceneDictionary[aliceScene2Fire.sceneId] = aliceScene2Fire;
        sceneDictionary[aliceScene2Ice.sceneId] = aliceScene2Ice;
        sceneDictionary[bobScene2Sword.sceneId] = bobScene2Sword;
        sceneDictionary[bobScene2Shield.sceneId] = bobScene2Shield;
        sceneDictionary[carolScene2Ruins.sceneId] = carolScene2Ruins;
        sceneDictionary[carolScene2Runes.sceneId] = carolScene2Runes;
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