# Unity编辑器操作指南 - Inspector配置

## 🎯 当前状态
Unity编辑器已在终端3中启动，项目路径正确。

## 📋 操作步骤

### 步骤1: 打开MainScene场景
1. 在Unity编辑器中，转到菜单栏: `File` → `Open Scene`
2. 导航到: `Assets/Scenes/MainScene.unity`
3. 点击打开

### 步骤2: 创建管理器GameObject
1. 在Hierarchy面板中右键点击空白处
2. 选择: `Create Empty`
3. 重命名为: `Managers`
4. 在Inspector面板中点击 `Add Component`
5. 依次添加以下脚本组件:
   - `CharacterManager`
   - `UIManager`
   - `AudioManager`
   - `GameManager`
   - `DialogueSystem`
   - `SceneDataManager`

### 步骤3: 配置CharacterManager
1. 选中 `Managers` GameObject
2. 在CharacterManager组件中:
   - 点击 `characters` 列表的 `+` 按钮添加新角色
   - 为每个角色配置:
     - `characterName`: 角色英文标识（如"Alice"）
     - `fullName`: 角色完整中文名
     - `age`: 年龄
     - `personality`: 性格描述
     - `background`: 背景故事
     - `expressions`: 拖拽表情精灵（Normal, Happy, Sad, Angry, Surprised）
     - `nameColor`: 名称显示颜色

### 步骤4: 配置UIManager（最重要且复杂）
**需要绑定的UI组件:**

#### 主界面面板:
- `mainMenuPanel` - 主菜单面板
- `loadGamePanel` - 加载游戏面板  
- `settingsPanel` - 设置面板

#### 游戏界面面板:
- `dialoguePanel` - 对话面板
- `inventoryPanel` - 物品栏面板
- `statusPanel` - 状态面板

#### 设置界面滑块和开关:
- `bgmVolumeSlider` - BGM音量滑块
- `voiceVolumeSlider` - 语音音量滑块
- `sfxVolumeSlider` - 音效音量滑块
- `autoPlayToggle` - 自动播放开关
- `skipReadToggle` - 跳过已读文本开关
- `textSpeedSlider` - 文本速度滑块

#### 按钮组件:
- `applySettingsButton` - 应用设置按钮
- `cancelSettingsButton` - 取消设置按钮
- `defaultSettingsButton` - 恢复默认设置按钮
- `backToMenuButton` - 返回主菜单按钮
- `saveSlotButtons` - 存档槽位按钮数组
- 各种对话面板按钮

### 步骤5: 配置AudioManager
1. 首先确保有AudioSource组件:
   - 为BGM、语音、音效、环境音分别创建AudioSource
2. 绑定音频源:
   - `bgmSource` - 背景音乐音频源
   - `voiceSource` - 语音音频源
   - `sfxSource` - 音效音频源
   - `ambientSource` - 环境音音频源
3. 填充音频数组:
   - `bgmTracks` - 添加BGM音频剪辑
   - `ambientSounds` - 添加环境音效
   - `uiSfx` - 添加UI音效

### 步骤6: 配置DialogueSystem
**需要绑定的组件:**
- `characterNameText` - 角色名称TextMeshPro
- `dialogueText` - 对话文本TextMeshPro
- `characterImage` - 角色头像Image
- `backgroundImage` - 背景Image
- `choicePanel` - 选择面板
- `choiceButtonPrefab` - 选择按钮预制体
- 音频源绑定
- 填充 `scenes` 场景数据列表

### 步骤7: 配置SceneDataManager
- 填充 `allScenes` 列表（或使用自动生成的演示场景）

### 步骤8: 配置GameManager
- 设置 `currentChapter` 和 `currentScene` 初始值

## 🎮 运行测试
1. 点击Unity编辑器顶部的 `Play` 按钮
2. 观察Console窗口（Window → General → Console）
3. 检查是否有任何错误或警告
4. 测试基本功能:
   - 对话显示
   - 按钮点击
   - 音频播放

## 🔧 故障排除

### 常见问题:
1. **MissingReferenceException** - Inspector字段未绑定
2. **NullReferenceException** - 脚本引用为空
3. **音频不播放** - AudioSource未配置或音频剪辑未分配

### 解决方案:
- 逐个检查所有Inspector字段是否绑定正确
- 确保所有需要的GameObject存在于场景中
- 验证资源路径和命名

## 💡 提示

1. **批量操作**: 可以一次性为所有管理器脚本绑定字段
2. **使用搜索**: 在Project窗口搜索需要的资源
3. **分层处理**: 先配置基础功能，再完善高级功能
4. **定期保存**: 配置过程中经常保存场景和项目

## 📞 需要进一步帮助
如果遇到具体错误，请:
1. 截图Console窗口的错误信息
2. 描述具体哪个功能不正常
3. 提供相关的脚本和场景信息

---
*指南创建时间: 2025年9月19日* 
*基于项目分析结果*