using System;
using Animancer;
using Rewired.Data;
using UnityEngine;

namespace ZZY_test
{
    public abstract class Jyx2AnimationBattleRole : MonoBehaviour
    {
        public abstract Animator GetAnimator();
        private HybridAnimancerComponent _animancer;

        public HybridAnimancerComponent GetAnimancer()
        {
            var animator = GetAnimator();
            _animancer = GameUtil.GetOrAddComponent<HybridAnimancerComponent>(animator.transform);

            if (_animancer.Animator == null)
                _animancer.Animator = animator;
            if (_animancer.Controller == null)
                _animancer.Controller = animator.runtimeAnimatorController;
            return _animancer;
        }

        protected void InitAnimationSystem()
        {
            GetAnimator();
            GetAnimancer();
        }

        /// <summary>
        /// 当前的技能播放
        /// </summary>
        public Jyx2SkillDisplayAsset CurDisplay { get; set; }

        bool IsStandardModelAvatar()
        {
            var animtor = GetAnimator();
            var controller = animtor.runtimeAnimatorController;
            return controller.name == "jyx2humanoidController.controller";
        }

        public virtual void Idle()
        {
            if (this == null || CurDisplay == null)
                return;
        }

        public void PlayAnimation(AnimationClip clip, Action callback = null, float fadeDuration = 0.25f)
        {
            if (clip == null)
            {
                Debug.LogError("调用了空的动作");
                callback?.Invoke();
                return;
            }

            var animancer = GetAnimancer();

            if (clip.isLooping && callback != null)
            {
                Debug.LogError($"动作设置了loop但是会有回调！请检查{clip.name}");
            }
            else if (!clip.isLooping && callback == null)
            {
                Debug.LogError($"动作没设置loop但是没有回调！请检查{clip.name}");
            }

            var state = animancer.Play(clip, fadeDuration);

            if (callback != null)
            {
                if (fadeDuration > 0)
                {
                    GameUtil.CallWithDelay(state.Duration + fadeDuration, callback, this);
                }
                else
                {
                    state.Events.OnEnd = callback;
                }
            }
        }
    }
}