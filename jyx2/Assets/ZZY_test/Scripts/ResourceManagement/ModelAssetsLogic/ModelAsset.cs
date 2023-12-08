using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Tools = ZZY_test.Middleware.Tools;

namespace ZZY_test
{
    [CreateAssetMenu(fileName = "NewModelAsset_ZZY_test", menuName = "金庸重制版_ZZY_test/角色模型配置文件Model Asset")]
    public class ModelAsset : ScriptableObject
    {
        public static IList<ModelAsset> All;

        public static ModelAsset Get(string roleName)
        {
            try
            {
                return All.Single(r => r.name == roleName);
            }
            catch (Exception e)
            {
                Debug.LogError($"{roleName}的模型资源找不到，随便挑一个。。。");
                Debug.LogException(e);
                return Tools.GetRandomElement(All); // 随便返回一个
            }
        }

        [BoxGroup("数据")]
        [Header("模型")]
        [InlineEditor(InlineEditorModes.LargePreview, Expanded = true)]
        [OnValueChanged("AutoBindModelData")]
        public GameObject m_View;

        public async UniTask<GameObject> GetView()
        {
            return m_View;
        }

        [BoxGroup("数据")] [Header("剑")] [SerializeReference]
        public SwordPart m_SwordWeapon;

        [BoxGroup("数据")] [Header("刀")] [SerializeReference]
        public KnifePart m_KnifeWeapon;

        [BoxGroup("数据")] [Header("长柄")] [SerializeReference]
        public SpearPart m_SpearWeapon;

        [BoxGroup("数据")] [Header("其他类型")] [SerializeReference]
        public List<WeaponPart> m_OtherWeapons;

        [EnumToggleButtons] [ShowInInspector] [LabelText("预览武器类型")]
        private WeaponPartType _weaponPartType = WeaponPartType.Sword;

        public enum WeaponPartType
        {
            [LabelText("空手")] Fist = 0,
            [LabelText("剑")] Sword = 1,
            [LabelText("刀")] Knife = 2,
            [LabelText("长柄")] Spear = 3,
            [LabelText("其他")] Other = 4
        }
#if UNITY_EDITOR
        [ButtonGroup("操作")]
        [Button("完整预览",ButtonSizes.Large,ButtonStyle.CompactBox)]
        private void FullPreview()
        {
            if (m_View == null) return;

            var scene = EditorSceneManager.OpenScene(
                "Assets/Scripts/ResourceManagement/ModelAssetsLogic/ModelPreviewScene.unity", OpenSceneMode.Additive);
            var gameObjects = scene.GetRootGameObjects();
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.name == m_View.name)
                {
                    DestroyImmediate(gameObject);
                }
            }

            viewWithWeapon = (GameObject) PrefabUtility.InstantiatePrefab(m_View, scene);
            viewWithWeapon.transform.SetAsFirstSibling();
            PrefabUtility.UnpackPrefabInstance(viewWithWeapon,PrefabUnpackMode.Completely,InteractionMode.AutomatedAction);
            EditorGUIUtility.PingObject(viewWithWeapon);
            Selection.activeGameObject = viewWithWeapon;
            SceneView.lastActiveSceneView.LookAt(viewWithWeapon.transform.position);
            
            DestroyImmediate(currentWeapon);
            var weaponPart = GetWeaponPart(_weaponPartType);
            if (weaponPart != null && weaponPart.m_PartView != null)
            {
                currentWeapon = (GameObject) PrefabUtility.InstantiatePrefab(weaponPart.m_PartView, scene);
                var parent = UnityTools.DeepFindChild(viewWithWeapon.transform, weaponPart.m_BindBone);
                currentWeapon.transform.SetParent(parent);
                currentWeapon.transform.localScale = weaponPart.m_OffsetScale;
                currentWeapon.transform.localPosition = weaponPart.m_OffsetPosition;
                currentWeapon.transform.localRotation = Quaternion.Euler(weaponPart.m_OffsetPosition);
            }
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
        [ButtonGroup("操作")]
        [Button("从预览场景导入武器偏移数据",ButtonSizes.Large,ButtonStyle.CompactBox)]
        private void AutoInputWeaponData()
        {
            var weaponPart = GetWeaponPart(_weaponPartType);
            weaponPart.m_OffsetScale = currentWeapon.transform.localScale;
            weaponPart.m_OffsetPosition = currentWeapon.transform.localPosition;
            weaponPart.m_OffsetRotation = currentWeapon.transform.localEulerAngles;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
        
        private GameObject currentWeapon=null;
        
        [InlineEditor(InlineEditorModes.LargePreview,Expanded = true,PreviewHeight = 600f)]
        [ShowInInspector]
        [ReadOnly]
        [HideLabel]
        [BoxGroup("完整预览",Order = 99)]
        private GameObject viewWithWeapon;
        
#endif
        /// <summary>
        /// 获取武器模型配置
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public WeaponPart GetWeaponPart(WeaponPartType type)
        {
            int index = (int) type;
            return GetWeaponPart(index.ToString());
        }

        public WeaponPart GetWeaponPart(string type)
        {
            switch (type)
            {
                case "1":
                {
                    return m_SwordWeapon;
                }
                case "2":
                {
                    return m_KnifeWeapon;
                }
                case "3":
                {
                    return m_SpearWeapon;
                }
                default:
                {
                    return m_OtherWeapons?.Find(delegate(WeaponPart part)
                    {
                        return part.m_Id.ToString() == type;
                    });
                }
            }
        }
        
        /// <summary>
        /// 自动绑定模型配置
        /// </summary>
        public void AutoBindModelData()
        {
            if(m_View==null)return;
            var animator = m_View.GetComponent<Animator>();
            
            //非人型清空武器配置
            if (animator == null | animator.avatar == null || !animator.avatar.isHuman)
            {
                m_SwordWeapon = null;
                m_KnifeWeapon = null;
                m_SpearWeapon = null;
                return;
            }
            
            //自定绑定武器配置
            if(m_SpearWeapon == null) m_SpearWeapon = new SpearPart();
            if(m_KnifeWeapon == null) m_KnifeWeapon = new KnifePart();
            if(m_SwordWeapon == null) m_SwordWeapon = new SwordPart();
            
            //自动绑定右手骨骼信息
            foreach (var bone in animator.avatar.humanDescription.human)
            {
                if (bone.humanName == "RightHand")
                {
                    if (m_SwordWeapon.m_BindBone == null)
                        m_SpearWeapon.m_BindBone = bone.boneName;
                    if (m_KnifeWeapon.m_BindBone == null)
                        m_KnifeWeapon.m_BindBone = bone.boneName;
                    if (m_SpearWeapon.m_BindBone == null)
                        m_SpearWeapon.m_BindBone = bone.boneName;
                    break;
                }
            }
        }
#if UNITY_EDITOR
        private void OnEnable()
        {
            AutoBindModelData();    
        }
#endif

    }
    
    

    [Serializable]
    public class WeaponPart
    {
        public int m_Id;
        public string m_BindBone;

        [InlineEditor(InlineEditorModes.LargePreview, Expanded = true)] [InlineButton("LoadDefaultView", "缺省模型")]
        public GameObject m_PartView;

        public Vector3 m_OffsetPosition;
        public Vector3 m_OffsetRotation;
        public Vector3 m_OffsetScale;

        public WeaponPart()
        {
            m_OffsetScale = Vector3.one;
        }
#if UNITY_EDITOR
        private void LoadDefaultView()
        {
            switch (m_Id)
            {
                case 1:
                {
                    m_PartView = (GameObject) AssetDatabase.LoadMainAssetAtPath(ConStr.DefaultSword);
                    break;
                }
                case 2:
                {
                    m_PartView = (GameObject) AssetDatabase.LoadMainAssetAtPath(ConStr.DefaultKnife);
                    break;
                }
                case 3:
                {
                    m_PartView = (GameObject) AssetDatabase.LoadMainAssetAtPath(ConStr.DefaultSpear);
                    break;
                }
            }

            AssetDatabase.SaveAssets();
        }
#endif
    }

    public class SwordPart : WeaponPart
    {
        public SwordPart()
        {
            m_Id = 1;
        }
    }

    public class KnifePart : WeaponPart
    {
        public KnifePart()
        {
            m_Id = 2;
        }
    }

    public class SpearPart : WeaponPart
    {
        public SpearPart()
        {
            m_Id = 3;
        }
    }
}