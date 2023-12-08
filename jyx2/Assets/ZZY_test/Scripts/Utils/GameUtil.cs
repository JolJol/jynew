using System;
using UniRx;
using UnityEngine;

namespace ZZY_test
{
    public static class GameUtil
    {
        public static T GetOrAddComponent<T>(Transform trans) where T : Component
        {
            T com = trans.GetComponent<T>();
            if (com == null)
            {
                com = trans.gameObject.AddComponent<T>();
            }
            return com;
        }

        public static void CallWithDelay(double time,Action action,Component attachedComponent = null)
        {
            if (time <= 0)
            {
                action();
                return;
            }

            var observable = Observable.Timer(TimeSpan.FromSeconds(time)).Subscribe(ms =>
            {
                action();
            });
            
            //避免关联对象销毁后延时逻辑仍然访问该对象的问题
            if (attachedComponent != null)
                observable.AddTo(attachedComponent);
        }
    }
}