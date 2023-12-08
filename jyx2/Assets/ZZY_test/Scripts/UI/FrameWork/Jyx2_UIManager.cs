using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ES3Types;
using Lean.Common.Inspector;
using Rewired.Integration.UnityUI;
using UnityEngine;

namespace ZZY_test
{
    public enum UILayer
    {
        MainUI = 0,//主界面层
        NormalUI = 1,//普通界面层
        PopupUI=2,//弹出层
        Top = 3,//top层
    }
    public class Jyx2_UIManager : MonoBehaviour
    {
        private static Jyx2_UIManager _instance;

        public static Jyx2_UIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var rewiredObj = FindObjectOfType<Rewired.InputManager>();
                    if (rewiredObj == null)
                    {
                        var inputMgrPrefab = Resources.Load<GameObject>("ZZY_text/RewiredInputManager");
                        var go = Instantiate(inputMgrPrefab);
                        rewiredObj = go.GetComponent<Rewired.InputManager>();
                    }

                    var canvasPrefab = Resources.Load<GameObject>("ZZY_text/MainCanvas");
                    var canvasGo = Instantiate(canvasPrefab);
                    canvasGo.name = "MainCanvas";
                    _instance = canvasGo.GetComponent<Jyx2_UIManager>();
                    _instance.Init();
                    
                    var rewiredInputModule = canvasGo.GetComponentInChildren<RewiredStandaloneInputModule>();
                    rewiredInputModule.RewiredInputManager = rewiredObj;
                    
                    DontDestroyOnLoad(_instance);
                }
                return _instance;
            }
        }

        public static void Clear()
        {
            if (_instance == null) return;
            Destroy(_instance.gameObject);
            _instance = null;
        }

        private Transform m_mainParent;
        private Transform m_normalParent;
        private Transform m_popParent;
        private Transform m_topParent;
        
        private Dictionary<string,Jyx2_UIBase> m_uiDic = new Dictionary<string, Jyx2_UIBase>();
        private Jyx2_UIBase m_currentMainUI;
        private List<Jyx2_UIBase> m_NormalUIs = new List<Jyx2_UIBase>();
        private List<Jyx2_UIBase> m_PopUIs = new List<Jyx2_UIBase>();
        void Init()
        {
            m_mainParent = transform.Find("MainUI");
            m_normalParent = transform.Find("NormalUI");
            m_popParent = transform.Find("PopupUI");
            m_topParent = transform.Find("Top");
        }

        public bool IsTopVisibleUI(Jyx2_UIBase ui)
        {
            if (!ui.gameObject.activeSelf)
            {
                return false;
            }

            if (ui.Layer == UILayer.MainUI)
            {
                
            }
            return false;
        }

        private bool noShowingNormalUi()
        {
            return !m_NormalUIs.Any(ui => ui.gameObject.activeSelf);
        }

        // private bool noInterferingPopupUI()
        // {
        //     return !m_NormalUIs.Any(ui=>ui.gameObject.activeSelf)||m_PopUIs.All(p=>p iscont)
        // }
    }
}

