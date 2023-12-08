using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Jyx2;
using UnityEngine;
using UnityEngine.UI;

namespace ZZY_test
{
    public static class ImageLoadHelper
    {
        async static UniTask<Sprite> GetPicTask(string path)
        {
            var _sprite = await ResLoader.LoadAsset<Sprite>(path);
            return _sprite;
        }

        public static void LoadAsyncForget(this Image image, string path)
        {
            LoadAsync(image, GetPicTask(path)).Forget();
        }

        public static async UniTask LoadAsync(this Image image, UniTask<Sprite> task)
        {
            image.gameObject.SetActive(false);
            image.sprite = await task;
            image.gameObject.SetActive(true);
        }
    }
    
    public class Jyx2ResourceHelper : MonoBehaviour
    {
        public static async UniTask Init()
        {
            //模型池
            var allModles = await ResLoader.LoadAssets<ModelAsset>("Assets/Models");
            if (allModles != null)
            {
                ModelAsset.All = allModles;
            }
            
            //技能池
            // var allSkills = await ResLoader.LoadAsset<>()
        }
    } 
}

