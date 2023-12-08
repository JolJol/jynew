using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZZY_test
{
    [CreateAssetMenu(fileName = "[技能名称]", menuName = "金庸重制版_ZZY_test/技能展现配置")]
    public class Jyx2SkillDisplayAsset : ScriptableObject
    {
        public static IList<Jyx2SkillDisplayAsset> All;

        public static Jyx2SkillDisplayAsset Get(string skillName)
        {
            var displayerAsset = All.FirstOrDefault(s => s.name == skillName);
            if (displayerAsset == null)
            {
                Debug.LogErrorFormat("目标技能展现配置不存在，技能名:[{0}]", skillName);
            }

            return displayerAsset; 
        }

        private const int MAX_SKILL_DURATION = 5;

#if UNITY_EDITOR
        [ButtonGroup("操作")]
        [Button("预览", ButtonSizes.Large, ButtonStyle.CompactBox)]
        [PropertyOrder(-1)]
        private void FullPreview()
        {
            if (!Application.isPlaying)
            {
                SceneHelper.StartScene("Assets/Jyx2Tools/Jyx2SkillEditor.unity");
                Debug.Log("需要再次点击预览按钮，才可以查看（在运行模式下方可预览技能效果）");
            }
            else
            {
                var scene = SceneManager.GetActiveScene();
                if (scene != null && scene.name != "Jyx2SkillEditor")
                {
                    EditorUtility.DisplayDialog("错误", "当前运行的场景不是技能编辑器的场景", "OK");
                    return;
                }

                // var skillEditor = FindObjectOfType<Jyx2SkillEditor>();
                // skillEditor.PreviewSkill(this);
            }
        }
#endif
        [BoxGroup("基础配置")] [LabelText("武器代码")] public ModelAsset.WeaponPartType weaponCode;

        [BoxGroup("动作")] [LabelText("受击")] [AssetSelector(Paths = "Assets/BuildsSource/Animations")]
        public AnimationClip beHitClip;

        [BoxGroup("动作")] [LabelText("移动")] [AssetSelector(Paths = "Assets/BuildsSource/Animations")]
        public AnimationClip moveClip;

        [BoxGroup("动作")] [LabelText("待机")] [AssetSelector(Paths = "Assets/BuildsSource/Animations")]
        public AnimationClip idleClip;

        [BoxGroup("动作")] [LabelText("攻击")] [AssetSelector(Paths = "Assets/BuildsSource/Animations")]
        public AnimationClip attackClip;

        [BoxGroup("动作")] [LabelText("眩晕")] [AssetSelector(Paths = "Assets/BuildsSource/Animations")]
        public AnimationClip stunClip;

        [SuffixLabel("秒", Overlay = true)] [BoxGroup("技能详细配置")] [LabelText("出招动作延迟")] [PropertyRange(0, 1)]
        public float animationDelay;

        [SuffixLabel("秒", Overlay = true)]
        [BoxGroup("技能详细配置")]
        [LabelText("动画时长")]
        [PropertyRange(0, MAX_SKILL_DURATION)]
        public float duration = 2;

        [SuffixLabel("秒", Overlay = true)]
        [BoxGroup("技能详细配置")]
        [LabelText("受击延迟")]
        [PropertyRange(0, MAX_SKILL_DURATION)]
        public float behitDelay = 0.5f;

        [BoxGroup("特效")] [LabelText("特效")] public GameObject particlePrefab;

        [BoxGroup("特效")] [LabelText("特效施展延迟")] [PropertyRange(0, MAX_SKILL_DURATION)]
        public float particleDelay;

        [BoxGroup("特效")] [LabelText("施展特效偏移量")]
        public Vector3 particleOffset;

        [BoxGroup("特效")] [LabelText("特效放大倍数")] public float particleScale = 1;

        [BoxGroup("特效（格子）")] [LabelText("特效")] public GameObject blockParticlePrefab;

        [SuffixLabel("秒", Overlay = true)]
        [BoxGroup("特效（格子）")]
        [LabelText("格子特效延迟")]
        [PropertyRange(0, MAX_SKILL_DURATION)]
        public float blockParticleDelay = 0.5f;

        [BoxGroup("特效（格子）")] [LabelText("格子特效偏移")]
        public Vector3 blockParticleOffset;

        [BoxGroup("特效（格子）")] [LabelText("格子特效放大倍数")]
        public float blockParticleScale = 1;

        [BoxGroup("特效（格子）")] [LabelText("特效2")]
        public GameObject blockParticlePrefabAdd;

        [SuffixLabel("秒", Overlay = true)]
        [BoxGroup("特效（格子）")]
        [LabelText("格子特效2延迟")]
        [PropertyRange(0, MAX_SKILL_DURATION)]
        public float blockParticleDelayAdd = 0.5f;

        [BoxGroup("特效（格子）")] [LabelText("格子特效2时长")]
        public float blockParticleAddDuration;

        [BoxGroup("特效（格子）")] [LabelText("格子特效2偏移")]
        public Vector3 blockParticleOffsetAdd;

        [BoxGroup("特效（格子）")] [LabelText("格子特效2放大倍数")]
        public float blockParticleScaleAdd = 1;

        [BoxGroup("音效")] [LabelText("音效")] [AssetSelector(Paths = "Assets/BuildSource/sound")]
        public AudioClip audio;


        [BoxGroup("音效")] [LabelText("音效延迟")] [PropertyRange(0, MAX_SKILL_DURATION)]
        public float audioDelay = 0.5f;

        [BoxGroup("音效2")] [LabelText("音效2")] [AssetSelector(Paths = "Assets/BuildSource/sound")]
        public AudioClip audio2;

        [BoxGroup("音效2")][LabelText("音效延迟2")][PropertyRange(0,MAX_SKILL_DURATION)]
        public float audioDelay2 = 0.5f;

        [BoxGroup("附加")][LabelText("动画控制器")]
        [AssetSelector(Paths = "Assets/BuildSource/AnimationControllers")]
        public RuntimeAnimatorController controller;
        
        //-------附加残影效果
        [BoxGroup("残影效果")]
        [LabelText("是否开启残影")]
        public bool isGhostShadowOn;

        [BoxGroup("残影效果")]
        [LabelText("残影颜色")]
        public Color ghostShadowColor = Color.blue;

        public RuntimeAnimatorController GetAnimationController()
        {
            if (controller != null)
            {
                return controller;
            }

            return GlobalAssetConfig.Instance.defaultAnimatorController;
        }

        private static RuntimeAnimatorController _defaultController = null;
        private static AnimationClip _defaultBehitClip = null;

        public AnimationClip LoadAnimation(Jyx2RoleAnimationType type)
        {
            switch (type)
            {
                case Jyx2RoleAnimationType.Idle:
                    return idleClip;
                case Jyx2RoleAnimationType.Behit:
                    return beHitClip == null? GlobalAssetConfig.Instance.defaultBeHitClip:beHitClip;
                case Jyx2RoleAnimationType.Move:
                    return moveClip == null ? GlobalAssetConfig.Instance.defaultMoveClip : moveClip;
                case Jyx2RoleAnimationType.Attack:
                    return attackClip;
                case Jyx2RoleAnimationType.Stun:
                    return stunClip == null ? global::GlobalAssetConfig.Instance.defaultStunClip : moveClip;
                default:
                    Debug.LogError("invalid Jyx2RoleAnimationType" +type);
                    return null;
            }
        }

    }

    public enum Jyx2RoleAnimationType
    {
        Idle,
        Move,
        Behit,
        Attack,
        Stun
    }
}