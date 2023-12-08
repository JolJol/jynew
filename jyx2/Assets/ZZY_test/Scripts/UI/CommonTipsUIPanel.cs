using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ZZY_test
{
    public enum TipsType
    {
        Common = 0,
        MiddleTop = 1
    }
    

    public partial class CommonTipsUIPanel : Jyx2_UIBase
    {
        public override UILayer Layer => UILayer.PopupUI;

        protected override void OnCreate()
        {
            InitTrans();
            MidllTopMessageSuggest_RectTransform.gameObject.SetActive(false);
        }

        private const float POPINFO_FADEOUT_TIME = 1f;

        protected override void OnShowPanel(params object[] allParams)
        {
            base.OnShowPanel(allParams);

            TipsType currentType = (TipsType) allParams[0];
            string text = allParams[1] as string;
            float duration = 2f;
            if (allParams.Length > 2)
            {
                duration = (float) allParams[2];
            }

            switch (currentType)
            {
                case TipsType.Common:
                    break;
                case TipsType.MiddleTop:
                    break;
            }
        }

        async UniTaskVoid ShowInfo(string msg, float duration)
        {
            //初始化
            // var popinfoItem =
        }
    } 
}

