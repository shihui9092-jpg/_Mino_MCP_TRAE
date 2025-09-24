using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConfig
{
    // 游戏版本信息
    public const string GAME_VERSION = "1.0.0";
    public const string GAME_TITLE = "命运交织：魔法世界物语";
    
    // 文件路径配置
    public const string SAVE_FILE_PATH = "/Saves/";
    public const string CONFIG_FILE_PATH = "/Config/";
    
    // 游戏常量
    public const int MAX_SAVE_SLOTS = 10;
    public const int MAX_AFFECTION = 100;
    public const int MIN_AFFECTION = 0;
    
    // 默认设置
    public const float DEFAULT_BGM_VOLUME = 0.7f;
    public const float DEFAULT_VOICE_VOLUME = 0.8f;
    public const float DEFAULT_SFX_VOLUME = 0.8f;
    public const float DEFAULT_TEXT_SPEED = 1.0f;
    public const bool DEFAULT_AUTO_PLAY = false;
    public const bool DEFAULT_SKIP_READ = false;
    
    // 角色相关配置
    public static readonly Dictionary<string, Color> CHARACTER_NAME_COLORS = new Dictionary<string, Color>
    {
        { "Alice", new Color(1f, 0.6f, 0.8f) },     // 粉色
        { "Bob", new Color(0.4f, 0.6f, 1f) },       // 蓝色
        { "Carol", new Color(0.8f, 0.4f, 1f) },    // 紫色
        { "旁白", new Color(0.9f, 0.9f, 0.9f) },   // 白色
        { "系统", new Color(1f, 0.8f, 0.2f) }      // 黄色
    };
    
    // 场景过渡时间
    public const float SCENE_FADE_DURATION = 1.0f;
    public const float TEXT_TYPING_DELAY = 0.02f;
    
    // 音频配置
    public static readonly string[] BGM_TRACK_NAMES = 
    {
        "MainMenu",
        "Intro",
        "AliceTheme",
        "BobTheme", 
        "CarolTheme",
        "Battle",
        "Sad",
        "Happy"
    };
    
    public static readonly string[] AMBIENT_SOUND_NAMES =
    {
        "Forest",
        "City",
        "Rain",
        "Wind",
        "Fire",
        "Magic"
    };
    
    // 游戏成就ID
    public static readonly Dictionary<string, string> ACHIEVEMENT_IDS = new Dictionary<string, string>
    {
        { "FIRST_STEP", "完成第一章" },
        { "ALICE_ROUTE", "完成爱丽丝路线" },
        { "BOB_ROUTE", "完成鲍勃路线" },
        { "CAROL_ROUTE", "完成卡罗尔路线" },
        { "TRUE_ENDING", "达成真结局" },
        { "ALL_ENDINGS", "解锁所有结局" },
        { "MAX_AFFECTION", "与一个角色达到满好感度" },
        { "QUICK_READER", "快速阅读所有文本" }
    };
    
    // 游戏标签
    public static class Tags
    {
        public const string BGM = "BGM";
        public const string VOICE = "Voice";
        public const string SFX = "SFX";
        public const string AMBIENT = "Ambient";
        public const string UI = "UI";
        public const string PLAYER = "Player";
        public const string NPC = "NPC";
    }
    
    // 图层
    public static class Layers
    {
        public const string DEFAULT = "Default";
        public const string UI = "UI";
        public const string BACKGROUND = "Background";
        public const string CHARACTER = "Character";
        public const string FOREGROUND = "Foreground";
    }
    
    // 动画参数
    public static class AnimatorParameters
    {
        public const string FADE_IN = "FadeIn";
        public const string FADE_OUT = "FadeOut";
        public const string SHAKE = "Shake";
        public const string FLASH = "Flash";
    }
    
    // 玩家偏好键
    public static class PlayerPrefsKeys
    {
        public const string LANGUAGE = "Language";
        public const string TEXT_SPEED = "TextSpeed";
        public const string AUTO_PLAY = "AutoPlay";
        public const string SKIP_READ = "SkipRead";
        public const string BGM_VOLUME = "BGMVolume";
        public const string VOICE_VOLUME = "VoiceVolume";
        public const string SFX_VOLUME = "SFXVolume";
        public const string LAST_SAVE_SLOT = "LastSaveSlot";
        public const string TOTAL_PLAY_TIME = "TotalPlayTime";
    }
    
    // 错误消息
    public static class ErrorMessages
    {
        public const string SAVE_FAILED = "存档失败";
        public const string LOAD_FAILED = "读档失败";
        public const string CHARACTER_NOT_FOUND = "角色不存在";
        public const string SCENE_NOT_FOUND = "场景不存在";
        public const string AUDIO_NOT_FOUND = "音频文件不存在";
    }
    
    // 成功消息
    public static class SuccessMessages
    {
        public const string SAVE_SUCCESS = "游戏已保存";
        public const string LOAD_SUCCESS = "游戏已加载";
        public const string SETTINGS_SAVED = "设置已保存";
    }
}