using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色数据结构 - 存储角色的完整信息
/// </summary>
[System.Serializable]
public class Character
{
    [Tooltip("角色标识名称（英文）")]
    public string characterName;
    [Tooltip("角色完整名称（中文）")]
    public string fullName;
    [Tooltip("角色年龄")]
    public int age;
    [Tooltip("角色性格描述")]
    public string personality;
    [Tooltip("角色背景故事")]
    public string background;
    [Tooltip("角色表情精灵数组，按Expression枚举顺序排列")]
    public Sprite[] expressions;
    [Tooltip("角色名称显示颜色")]
    public Color nameColor;
}

/// <summary>
/// 角色管理器 - 负责管理游戏中的所有角色信息和表情系统
/// 使用单例模式确保全局访问，支持角色信息查询和表情获取
/// </summary>
public class CharacterManager : MonoBehaviour
{
    /// <summary>
    /// 角色管理器单例实例
    /// </summary>
    public static CharacterManager Instance { get; private set; }
    
    [Header("角色列表")]
    [Tooltip("所有游戏角色的列表")]
    public List<Character> characters = new List<Character>();
    
    // 角色字典，用于快速查找
    private Dictionary<string, Character> characterDictionary = new Dictionary<string, Character>();
    
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
            InitializeCharacters(); // 初始化角色字典
        }
        else
        {
            Destroy(gameObject); // 销毁重复实例
        }
    }
    
    /// <summary>
    /// 初始化角色字典，将角色列表转换为字典以便快速查找
    /// </summary>
    private void InitializeCharacters()
    {
        // 初始化角色字典
        foreach (Character character in characters)
        {
            characterDictionary[character.characterName] = character;
        }
    }
    
    /// <summary>
    /// 根据角色名称获取角色信息
    /// </summary>
    /// <param name="characterName">角色名称</param>
    /// <returns>角色信息，如果不存在则返回null</returns>
    public Character GetCharacter(string characterName)
    {
        if (characterDictionary.ContainsKey(characterName))
        {
            return characterDictionary[characterName];
        }
        return null;
    }
    
    /// <summary>
    /// 获取指定角色的表情精灵
    /// </summary>
    /// <param name="characterName">角色名称</param>
    /// <param name="expression">表情类型</param>
    /// <returns>表情精灵，如果不存在则返回null</returns>
    public Sprite GetExpressionSprite(string characterName, Expression expression)
    {
        Character character = GetCharacter(characterName);
        if (character != null && character.expressions.Length > (int)expression)
        {
            return character.expressions[(int)expression];
        }
        return null;
    }
    
    /// <summary>
    /// 获取指定角色的名称颜色
    /// </summary>
    /// <param name="characterName">角色名称</param>
    /// <returns>名称颜色，如果不存在则返回白色</returns>
    public Color GetNameColor(string characterName)
    {
        Character character = GetCharacter(characterName);
        return character?.nameColor ?? Color.white;
    }
    
    /// <summary>
    /// 获取角色的完整信息字符串
    /// </summary>
    /// <param name="characterName">角色名称</param>
    /// <returns>格式化后的角色信息字符串</returns>
    public string GetCharacterInfo(string characterName)
    {
        Character character = GetCharacter(characterName);
        if (character != null)
        {
            return $"姓名: {character.fullName}\n年龄: {character.age}\n性格: {character.personality}\n背景: {character.background}";
        }
        return "角色信息不存在";
    }
    
    /// <summary>
    /// 获取所有角色名称列表
    /// </summary>
    /// <returns>所有角色名称的列表</returns>
    public List<string> GetAllCharacterNames()
    {
        return new List<string>(characterDictionary.Keys);
    }
}