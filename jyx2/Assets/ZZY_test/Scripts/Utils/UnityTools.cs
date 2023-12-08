using UnityEngine;

namespace ZZY_test
{
    public static class UnityTools
    {


        public static Transform DeepFindChild(Transform root, string childName)
        {
            Transform result;
            result = root.Find(childName);
            if (result == null)
            {
                foreach (Transform transform in root)
                {
                    result = DeepFindChild(transform, childName);
                    if (result != null)
                        return result;
                }
            }

            return result;
        }
    }
}