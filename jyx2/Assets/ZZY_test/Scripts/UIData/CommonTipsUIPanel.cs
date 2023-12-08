using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

namespace ZZY_test
{
    public partial class CommonTipsUIPanel
    {
        private RectTransform CommonTips_RectTransform;
        private RectTransform PopInfoParent_RectTransform;
        private RectTransform MidllTopMessageSuggest_RectTransform;
        private Text MiddleText_Text;

        public void InitTrans()
        {
            CommonTips_RectTransform = transform.Find("CommonTips").GetComponent<RectTransform>();
            PopInfoParent_RectTransform = transform.Find("CommonTips/PopInfoParent").GetComponent<RectTransform>();
            MidllTopMessageSuggest_RectTransform =
                transform.Find("MiddleTopMessageSuggest").GetComponent<RectTransform>();
            MiddleText_Text = transform.Find("MiddleTopMessageSuggest/Bg/MiddleText").GetComponent<Text>();
        }
    }
}