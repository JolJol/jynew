using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ZZY_test
{
    public interface IUIAnimator
    {
        void DoShowAnimator();
        void DoHideAnimator();
    }
    public class Jyx2_UIBase : MonoBehaviour
    {

        public virtual UILayer Layer { get; } = UILayer.NormalUI;
        public virtual bool IsOnly { get; } = false;//同一层只能单独存在
        public virtual bool IsBlockControl { get; set; } = false;
        public virtual bool AlwaysDisplay { get; } = false;

        protected virtual void OnCreate()
        {
            
        }

        protected virtual void OnShowPanel(params object[] allParams)
        {
            
        }
        
        protected virtual void OnHidePanel(){}

        public void Init()
        {
            var rt = GetComponent<RectTransform>();
            rt.localPosition = Vector3.zero;
            rt.localScale = Vector3.one;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            
            OnCreate();
        }

        public void Show(params object[] allParams)
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            OnShowPanel(allParams);
            if (this is IUIAnimator)
            {
                (this as IUIAnimator).DoShowAnimator();
            }
        }

        public void Hide()
        {
            if (AlwaysDisplay) return;
            gameObject.SetActive(false);
            OnHidePanel();
        }

        public virtual void BindListener(Button button,UnityAction callback,bool supportGamepadButtonNav = true)
        {
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(callback);
            }
        }

        public virtual void Update()
        {
            
        }

        protected bool isOnTop()
        {
            return Jyx2_UIManager.Instance.IsTopVisibleUI(this);
        }
    }
}

