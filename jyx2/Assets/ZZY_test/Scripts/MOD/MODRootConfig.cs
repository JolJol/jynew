using System;
using System.Collections.Generic;
using Jyx2.MOD.ModV2;
using Sirenix.OdinInspector;
using UnityEngine;
using ZZY_test.Middleware;

namespace ZZY_test
{
    /// <summary>
    /// MOD的根配置
    ///
    /// 本运行框架下，所有的可游玩内容都对等为一个MOD。
    /// </summary>
    [CreateAssetMenu(menuName = "金庸重制版_ZZY_test/MOD/生成根配置文件", fileName = "ModRootConfig")]
    public class MODRootConfig : MonoBehaviour
    {
        [LabelText("MOD ID(全局唯一)")] public string ModId;

        [LabelText("是否原生MOD(随打包一起发布)")] public bool IsNativeMod;
        [Multiline] [LabelText("MOD简介")] public string Desc;
        [LabelText("MOD版本号")] public string Version;
        [LabelText("MOD名称")] public string ModName;
        [LabelText("游戏MOD的根目录")] public string ModRootDir;
        [LabelText("游戏作者名")] public string Author;

        [LabelText("LUA文件名配置")] public string LuaFielPatten = "ka{0}";

        [LabelText("MOD存档版本号")] public int ModArchiveVersion = 0;
        [LabelText("主角姓名")] public string PlayerName;
        [LabelText("预加载的lua文件（比如热更新）")] public List<string> PreloaderLua;

        [LabelText("只允许大地图存档")] public bool EnableSaveBigMapOnly = true;
        [LabelText("只允许大地图离队")] public bool EnableKickTeammateBigMapOnly = true;
        [LabelText("只允许自动战斗")] public bool AutoBattleOnly;
        [LabelText("默认战斗倍速")] public float BattleTimeScale = 1;
        [LabelText("战斗中显示招式名字")] public bool ShowSkillNameInBattle;
        [LabelText("是否打开控制台")] public bool IsConsoleEnable = true;
        [LabelText("在哪些难度中禁止使用控制台")] public List<Jyx2_GameDifficulty> ConsoleDisableDifficulty;
        [LabelText("战斗中是否播放使用道具动作")] public bool IsPlayUseItemAnimation;

        [InfoBox("某些角色名与人物ID不严格对正，再次修正。用于对话中正确显示名字")] [BoxGroup("对话人物ID修正")] [TableList] [HideLabel]
        public List<StoryIdNameFix> StoryIdNameFixes;
        
        [LabelText("俯视相机偏移（近，为0使用全局设置）")] public Vector3 CameraOffsetNeat = Vector3.zero;
        [LabelText("俯视角相机偏移（远，为0使用全局设置）")] public Vector3 CameraOffsetFar = Vector3.zero;
        
        #if UNITY_EDITOR
        [Button("生成配置表")]
        public void GenerateConfigs()
        {
            //生成Lua配置表
            ExcelToLua.ExportSingleLuaFile($"{ModRootDir}/Configs",$"{ModRootDir}/Configs/Lua");
            UnityEditor.AssetDatabase.Refresh();
        }
        
        #endif
        public GameModInfo CreateModInfo()
        {
            GameModInfo info = new GameModInfo();
            info.Id = ModId.ToLower();
            info.Name = ModName;
            info.Author = Author;
            info.Version = Version;
            info.ClientVersion = Application.version;
            info.CreateTime = DateTime.Now.ToString("yyyyMMdd H:m:s");
            info.Desc = Desc;
            return info;
        }
    }
}