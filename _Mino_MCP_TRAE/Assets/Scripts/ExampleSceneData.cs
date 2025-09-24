using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 示例场景数据类 - 提供演示用的角色和场景数据
/// 用于开发和测试阶段展示游戏功能，包含完整的示例内容
/// </summary>
public class ExampleSceneData : MonoBehaviour
{
    [Header("示例角色数据")]
    [Tooltip("爱丽丝角色示例数据")]
    public Character aliceCharacter;
    [Tooltip("鲍勃角色示例数据")]
    public Character bobCharacter;
    [Tooltip("卡罗尔角色示例数据")]
    public Character carolCharacter;
    
    [Header("示例场景数据")]
    [Tooltip("介绍场景示例数据")]
    public SceneData introScene;
    [Tooltip("选择场景示例数据")]
    public SceneData choiceScene;
    [Tooltip("爱丽丝路线场景示例数据")]
    public SceneData aliceRouteScene;
    [Tooltip("鲍勃路线场景示例数据")]
    public SceneData bobRouteScene;
    
    /// <summary>
    /// 初始化方法，创建示例角色和场景数据
    /// </summary>
    private void Awake()
    {
        CreateExampleCharacters();
        CreateExampleScenes();
    }
    
    /// <summary>
    /// 创建示例角色数据
    /// </summary>
    private void CreateExampleCharacters()
    {
        // 创建Alice角色
        aliceCharacter = new Character
        {
            characterName = "Alice",
            fullName = "爱丽丝·约翰逊",
            age = 18,
            personality = "活泼开朗，喜欢冒险",
            background = "来自魔法学院的优等生",
            nameColor = new Color(1f, 0.6f, 0.8f) // 粉色
        };
        
        // 创建Bob角色
        bobCharacter = new Character
        {
            characterName = "Bob",
            fullName = "鲍勃·史密斯",
            age = 20,
            personality = "冷静沉着，擅长剑术",
            background = "皇家骑士团的成员",
            nameColor = new Color(0.4f, 0.6f, 1f) // 蓝色
        };
        
        // 创建Carol角色
        carolCharacter = new Character
        {
            characterName = "Carol",
            fullName = "卡罗尔·戴维斯",
            age = 19,
            personality = "神秘莫测，精通魔法",
            background = "来自古老魔法家族",
            nameColor = new Color(0.8f, 0.4f, 1f) // 紫色
        };
        
        // 添加到角色管理器
        if (CharacterManager.Instance != null)
        {
            CharacterManager.Instance.characters.Add(aliceCharacter);
            CharacterManager.Instance.characters.Add(bobCharacter);
            CharacterManager.Instance.characters.Add(carolCharacter);
        }
    }
    
    /// <summary>
    /// 创建示例场景数据
    /// </summary>
    private void CreateExampleScenes()
    {
        // 创建介绍场景
        introScene = new SceneData
        {
            sceneId = "intro_scene",
            dialogueLines = new List<DialogueLine>
            {
                new DialogueLine
                {
                    characterName = "旁白",
                    dialogueText = "在一个遥远的魔法世界里，三个命运交织的年轻人即将开始他们的冒险...",
                    expression = Expression.Normal
                },
                new DialogueLine
                {
                    characterName = "Alice",
                    dialogueText = "欢迎来到我们的世界！我是爱丽丝，很高兴认识你！",
                    expression = Expression.Happy
                },
                new DialogueLine
                {
                    characterName = "Bob",
                    dialogueText = "我是鲍勃，准备好开始这段旅程了吗？",
                    expression = Expression.Normal
                },
                new DialogueLine
                {
                    characterName = "Carol",
                    dialogueText = "命运的选择就在你的手中...",
                    expression = Expression.Surprised
                }
            },
            choices = new List<Choice>
            {
                new Choice
                {
                    choiceText = "跟随爱丽丝探索魔法学院",
                    nextScene = "alice_route_1",
                    effects = new Dictionary<string, int>
                    {
                        { "affection_Alice", 10 },
                        { "flag_chose_alice", 1 }
                    }
                },
                new Choice
                {
                    choiceText = "与鲍勃一起加入骑士团",
                    nextScene = "bob_route_1",
                    effects = new Dictionary<string, int>
                    {
                        { "affection_Bob", 10 },
                        { "flag_chose_bob", 1 }
                    }
                },
                new Choice
                {
                    choiceText = "探寻卡罗尔的秘密",
                    nextScene = "carol_route_1",
                    effects = new Dictionary<string, int>
                    {
                        { "affection_Carol", 10 },
                        { "flag_chose_carol", 1 }
                    }
                }
            }
        };
        
        // 创建爱丽丝路线场景
        aliceRouteScene = new SceneData
        {
            sceneId = "alice_route_1",
            dialogueLines = new List<DialogueLine>
            {
                new DialogueLine
                {
                    characterName = "Alice",
                    dialogueText = "太好了！你选择了和我一起冒险！",
                    expression = Expression.Happy
                },
                new DialogueLine
                {
                    characterName = "Alice",
                    dialogueText = "魔法学院有很多神奇的地方，让我带你参观吧！",
                    expression = Expression.Surprised
                }
            },
            choices = new List<Choice>()
        };
        
        // 创建鲍勃路线场景
        bobRouteScene = new SceneData
        {
            sceneId = "bob_route_1",
            dialogueLines = new List<DialogueLine>
            {
                new DialogueLine
                {
                    characterName = "Bob",
                    dialogueText = "明智的选择。骑士团需要像你这样的伙伴。",
                    expression = Expression.Normal
                },
                new DialogueLine
                {
                    characterName = "Bob",
                    dialogueText = "拿起你的剑，我们一起守护这个王国！",
                    expression = Expression.Happy
                }
            },
            choices = new List<Choice>()
        };
        
        // 添加到场景数据管理器
        if (SceneDataManager.Instance != null)
        {
            SceneDataManager.Instance.allScenes.Add(introScene);
            SceneDataManager.Instance.allScenes.Add(aliceRouteScene);
            SceneDataManager.Instance.allScenes.Add(bobRouteScene);
        }
    }
    
    /// <summary>
    /// 获取示例对话文本列表
    /// </summary>
    /// <returns>示例对话文本列表</returns>
    public static List<string> GetExampleDialogueLines()
    {
        return new List<string>
        {
            "你好，旅行者！欢迎来到我们的世界。",
            "我是爱丽丝，很高兴认识你！",
            "命运的选择就在你的手中...",
            "每一个决定都会改变故事的走向。",
            "你准备好开始这段奇妙的冒险了吗？"
        };
    }
    
    /// <summary>
    /// 获取示例选择项列表
    /// </summary>
    /// <returns>示例选择项列表</returns>
    public static List<string> GetExampleChoices()
    {
        return new List<string>
        {
            "接受爱丽丝的邀请",
            "跟随鲍勃去训练",
            "探寻卡罗尔的秘密",
            "独自探索这个世界",
            "回到起点重新选择"
        };
    }
}