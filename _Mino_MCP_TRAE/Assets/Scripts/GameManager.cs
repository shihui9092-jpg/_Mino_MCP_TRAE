using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏管理器 - 负责管理游戏全局状态、存档系统和游戏进度
/// 使用单例模式确保全局访问，管理章节进度、角色关系和游戏变量
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 游戏管理器单例实例
    /// </summary>
    public static GameManager Instance { get; private set; }
    
    [Header("游戏状态")]
    [Tooltip("当前章节编号")]
    public int currentChapter = 1;
    [Tooltip("当前场景编号")]
    public int currentScene = 1;
    [Tooltip("游戏标志字典，用于存储布尔状态")]
    public Dictionary<string, bool> flags = new Dictionary<string, bool>();
    [Tooltip("游戏变量字典，用于存储数值状态")]
    public Dictionary<string, int> variables = new Dictionary<string, int>();
    
    [Header("角色关系")]
    [Tooltip("角色好感度字典，存储角色名称和好感度值")]
    public Dictionary<string, int> characterAffection = new Dictionary<string, int>();
    
    [Header("存档系统")]
    [Tooltip("当前选中的存档槽位")]
    public int currentSaveSlot = 0;
    [Tooltip("游戏总时长（秒）")]
    public float totalPlayTime = 0f;
    [Tooltip("存档数据字典")]
    public Dictionary<int, SaveData> saveData = new Dictionary<int, SaveData>();
    
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
            InitializeGameData(); // 初始化游戏数据
        }
        else
        {
            Destroy(gameObject); // 销毁重复实例
        }
    }
    
    /// <summary>
    /// 初始化游戏数据，设置默认角色关系和游戏变量
    /// </summary>
    public void InitializeGameData()
    {
        // 初始化角色好感度
        characterAffection["Alice"] = 0;
        characterAffection["Bob"] = 0;
        characterAffection["Carol"] = 0;
        
        // 初始化游戏变量
        variables["money"] = 100;
        variables["reputation"] = 0;
    }
    
    /// <summary>
    /// 设置游戏标志状态
    /// </summary>
    /// <param name="flagName">标志名称</param>
    /// <param name="value">标志值</param>
    public void SetFlag(string flagName, bool value)
    {
        flags[flagName] = value;
    }
    
    /// <summary>
    /// 获取游戏标志状态
    /// </summary>
    /// <param name="flagName">标志名称</param>
    /// <returns>标志状态，如果不存在则返回false</returns>
    public bool GetFlag(string flagName)
    {
        return flags.ContainsKey(flagName) && flags[flagName];
    }
    
    /// <summary>
    /// 改变角色好感度
    /// </summary>
    /// <param name="characterName">角色名称</param>
    /// <param name="amount">变化量（可为负值）</param>
    public void ChangeAffection(string characterName, int amount)
    {
        if (characterAffection.ContainsKey(characterName))
        {
            characterAffection[characterName] += amount;
        }
    }
    
    /// <summary>
    /// 获取角色好感度
    /// </summary>
    /// <param name="characterName">角色名称</param>
    /// <returns>好感度值，如果不存在则返回0</returns>
    public int GetAffection(string characterName)
    {
        return characterAffection.ContainsKey(characterName) ? characterAffection[characterName] : 0;
    }
    
    /// <summary>
    /// 保存游戏到指定槽位
    /// </summary>
    /// <param name="slot">存档槽位编号（0-2）</param>
    public void SaveGame(int slot)
    {
        // 实现存档逻辑
        Debug.Log($"游戏已保存到槽位 {slot}");
    }
    
    /// <summary>
    /// 从指定槽位加载游戏
    /// </summary>
    /// <param name="slot">存档槽位编号（0-2）</param>
    public void LoadGame(int slot)
    {
        // 实现读档逻辑
        Debug.Log($"从槽位 {slot} 加载游戏");
    }

    /// <summary>
    /// 检查指定槽位是否有存档文件
    /// </summary>
    /// <param name="slot">存档槽位编号（1-10）</param>
    /// <returns>如果槽位有存档返回true，否则返回false</returns>
    public bool HasSaveFile(int slot)
    {
        // 检查内存中的存档数据
        if (saveData.ContainsKey(slot) && saveData[slot] != null)
        {
            return true;
        }
        
        // 检查PlayerPrefs中是否有存档数据
        string saveKey = $"SaveData_Slot{slot}";
        return PlayerPrefs.HasKey(saveKey);
    }
    
    /// <summary>
    /// 获取指定槽位的存档数据
    /// </summary>
    /// <param name="slot">存档槽位编号</param>
    /// <returns>存档数据，如果不存在返回null</returns>
    public SaveData GetSaveData(int slot)
    {
        if (saveData.ContainsKey(slot))
        {
            return saveData[slot];
        }
        
        // 从PlayerPrefs加载存档数据
        string saveKey = $"SaveData_Slot{slot}";
        if (PlayerPrefs.HasKey(saveKey))
        {
            string jsonData = PlayerPrefs.GetString(saveKey);
            return JsonUtility.FromJson<SaveData>(jsonData);
        }
        
        return null;
    }
    
    /// <summary>
    /// 格式化游戏时长为可读字符串
    /// </summary>
    /// <param name="totalSeconds">总秒数</param>
    /// <returns>格式化后的时间字符串</returns>
    public string FormatPlayTime(float totalSeconds)
    {
        int hours = Mathf.FloorToInt(totalSeconds / 3600);
        int minutes = Mathf.FloorToInt((totalSeconds % 3600) / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);
        return $"{hours:00}:{minutes:00}:{seconds:00}";
    }
}

/// <summary>
/// 存档数据类 - 存储游戏存档的元数据信息
/// </summary>
[System.Serializable]
public class SaveData
{
    public int slotIndex;          // 存档槽位索引
    public int chapter;           // 当前章节
    public int scene;             // 当前场景
    public float playTime;        // 游戏时长（秒）
    public string saveDate;       // 存档日期
    public string thumbnailPath;  // 缩略图路径
    
    // 游戏状态数据
    public Dictionary<string, bool> flags;
    public Dictionary<string, int> variables;
    public Dictionary<string, int> characterAffection;
    
    public SaveData(int slot, int currentChapter, int currentScene, float totalPlayTime)
    {
        slotIndex = slot;
        chapter = currentChapter;
        scene = currentScene;
        playTime = totalPlayTime;
        saveDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        thumbnailPath = string.Empty;
    }
    
    /// <summary>
    /// 将字典数据序列化为JSON字符串
    /// </summary>
    public string SerializeDictionaries()
    {
        // 这里可以实现字典的序列化逻辑
        return string.Empty;
    }
    
    /// <summary>
    /// 从JSON字符串反序列化字典数据
    /// </summary>
    public void DeserializeDictionaries(string jsonData)
    {
        // 这里可以实现字典的反序列化逻辑
    }
}