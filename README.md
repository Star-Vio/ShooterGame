# ShooterGame

基于 **Unity 6** 开发的第三人称生存射击游戏。玩家在封闭场景中迎战不断生成的三类敌人，通过走位和射击尽可能存活更久。

> 🎬 [演示视频](https://github.com/user-attachments/assets/97b3fc22-3897-4467-895c-8fb9d501867a)

---

## 技术栈

| 技术 | 用途 |
|------|------|
| Unity 6000.5.0f1 | 游戏引擎 |
| C# (.NET) | 所有游戏逻辑 |
| NavMesh (AI Navigation) | 敌人寻路 |
| Unity Input System | 键鼠输入 |
| Animator Controller | 角色动画状态机 |
| Audio Mixer | Master/SFX 分层音频控制 |
| LineRenderer | 弹道拖尾特效 |
| Particle System | 枪口焰火 / 命中特效 |
| Unity Editor Tooling | 自定义编辑器工具 |

## 操作方式

| 按键 | 功能 |
|------|------|
| W / A / S / D | 移动 |
| 鼠标移动 | 瞄准（射线检测地面，角色朝向鼠标位置） |
| 鼠标左键 | 射击 |

## 已实现功能

### 玩家系统
- **鼠标瞄准**：从相机向鼠标位置发射射线，命中地面层后计算朝向，`Rigidbody.MoveRotation` 平滑旋转
- **射击**：Raycast 射线检测 Enemy 层，命中后造成伤害，附带枪口粒子 + 弹道线 + 枪声
- **血量**：100 HP，受伤红屏闪烁 Lerp 渐变，死亡触发动画后重载场景
- **计分**：静态计分系统，击杀敌人 +10 分，实时 UI 更新

### 敌人系统（3 种类型）
| 敌人 | 生成间隔 | 特点 |
|------|---------|------|
| Zombunny | 每 3-5 秒 | 小巧快速 |
| ZomBear | 每 3-5 秒 | 中等体型 |
| Hellephant | 每 8 秒 | 大型敌人 |

- **AI 寻路**：`NavMeshAgent.SetDestination` 持续追踪玩家，玩家死亡或自身死亡后停止
- **攻击**：Trigger Collider 检测玩家进入范围，每 0.8 秒造成 10 点伤害
- **死亡**：禁用碰撞体 → 播放死亡动画 → 下沉消失 → 2 秒后销毁

### 音效
- 背景音乐 + 枪声 / 玩家受伤 / 玩家死亡 / 三种敌人各自的受伤和死亡音效
- Audio Mixer 分离 Master 和 SoundEffects 通道

### 编辑工具
- `Tools > Fix All Materials`：批量检测 Materials 目录，自动将无效/隐藏 Shader 修复为 Built-in/URP/HDRP Lit Shader

## 项目结构

```
Assets/
├── Scripts/
│   ├── Player/
│   │   ├── MyPlayerMovement.cs    # WASD 移动 + 鼠标朝向
│   │   ├── MyPlayerShooting.cs    # Raycast 射击 + 特效
│   │   ├── MyPlayerHealth.cs      # 血量 + 受伤/死亡逻辑
│   │   └── MyPlayerScores.cs      # 计分 + UI 更新
│   ├── Enemy/
│   │   ├── MyEnemyMovement.cs     # NavMesh 寻路追踪玩家
│   │   ├── MyEnemyAttack.cs       # 触发检测 + 定时攻击
│   │   └── MyEnemyHealth.cs       # 血量 + 死亡下沉销毁
│   ├── Managers/
│   │   └── MyEnemyManager.cs      # 敌人生成器（InvokeRepeating）
│   ├── Camera/
│   │   └── CameraFllow.cs         # 正交相机平滑跟随
│   └── Editor/
│       └── FixMaterialsEditor.cs  # 批量材质修复工具
├── Models/                        # 角色 + 场景模型
├── Materials/                     # 材质（45 个）
├── Prefabs/                       # 特效 + 灯光预制体
├── Animation/                     # 动画控制器
├── Audio/                         # 音效 + 混音器
└── Game.unity                     # 主场景
```

## 如何运行

1. 使用 **Unity 6000.5.0f1** 或更高版本打开项目
2. 打开 `Assets/Game.unity`
3. 点击 Play

## 计划

- [ ] 主菜单 / 暂停菜单 / 游戏结束面板
- [ ] 武器切换（手枪 / 霰弹枪 / 步枪）
- [ ] 难度递增（波次系统，敌人属性随波次增长）
- [ ] 最高分本地持久化（PlayerPrefs）
- [ ] WebGL 部署可在线试玩

---

*本项目基于 Unity 官方 Survival Shooter 教程资产重新开发，所有游戏逻辑独立重写。*
