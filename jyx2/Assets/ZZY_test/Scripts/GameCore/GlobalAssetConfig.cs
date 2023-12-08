using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ZZY_test
{
    [CreateAssetMenu(fileName = "GlobalAssetConfig", menuName = "金庸重制版_ZZY_test/全局资源配置文件")]
    public class GlobalAssetConfig : ScriptableObject
    {
        public static GlobalAssetConfig Instance { get; set; }

        [BoxGroup("游戏动作")] [LabelText("默认受击动作")]
        public AnimationClip defaultBeHitClip;

        [BoxGroup("游戏动作")] [LabelText("默认移动动作")]
        public AnimationClip defaultMoveClip;

        [BoxGroup("游戏动作")] [LabelText("默认待机动作")]
        public AnimationClip defaultIdleClip;
        
        [BoxGroup("游戏动作")] [LabelText("默认眩晕动作")]
        public AnimationClip defaultStunClip;
        [BoxGroup("游戏动作")] [LabelText("默认角色动作控制器")]
        public RuntimeAnimatorController defaultAnimatorController;
    }

    [Serializable]
    public class StoryIdNameFix
    {
        [LabelText("ID")] public int Id;
        [LabelText("姓名")] public string Name;
    }
}