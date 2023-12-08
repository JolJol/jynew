using System;
using System.IO;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace ZZY_test
{
    public class GameStart : MonoBehaviour
    {
        public CanvasGroup introPanel;

        void Start()
        {
            FixSaves();
            StartAsync().Forget();
        }
        void FixSaves()
        {
#if true
            try
            {
                if (!PlayerPrefs.HasKey("save_fixed_20221204"))
                {
                    DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }

                    var moveDir = Path.Combine(dir.Parent.FullName, "wuxia_launch");
                    if (Directory.Exists(moveDir))
                    {
                        CopyDirectory(moveDir, dir.FullName);
                    }

                    PlayerPrefs.SetInt("save_fixed_20221204", 1);
                    PlayerPrefs.Save();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
#endif
        }

        static void CopyDirectory(string srcDir, string tgtDir)
        {
            DirectoryInfo source = new DirectoryInfo(srcDir);
            DirectoryInfo target = new DirectoryInfo(tgtDir);

            if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录");
            }

            if (!source.Exists)
            {
                return;
            }

            if (!target.Exists)
            {
                target.Create();
            }

            FileInfo[] files = source.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                File.Copy(files[i].FullName, target.FullName + @"\" + files[i].Name, true);
            }

            DirectoryInfo[] dirs = source.GetDirectories();

            for (int j = 0; j < dirs.Length; j++)
            {
                CopyDirectory(dirs[j].FullName, target.FullName + @"\" + dirs[j].Name);
            }
        }

        async UniTask StartAsync()
        {
            introPanel.gameObject.SetActive(true);

            introPanel.alpha = 0;
            await introPanel.DOFade(1, 1f).SetEase(Ease.Linear);
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            await introPanel.DOFade(0, 1f).SetEase(Ease.Linear).OnComplete(() => { Destroy(introPanel.gameObject); });

            Application.logMessageReceived += OnErrorMsg;
            
        }

        private void OnErrorMsg(string condition, string stackTrace, LogType logType)
        {
            if (logType == LogType.Exception)
            {
                Debug.LogWarningFormat("Exception版本：{0},触发时间：{1}", Application.version, DateTime.Now);
            }
            else if (logType == LogType.Error)
            {
                Debug.LogWarningFormat("Error版本：{0}，触发时间：{1}", Application.version, DateTime.Now);
            }
        }
    }
}