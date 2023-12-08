using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Steamworks.Data;
using UnityEngine;

namespace ZZY_test.Middleware 
{
    public class Tools
    {

        #region 数学方法
        private static System.Random rnd = new System.Random();

        public static double GetRandom(double a, double b)
        {
            double k = rnd.NextDouble();
            double tmp = 0;
            if (b > a)
            {
                tmp = a;
                a = b;
                b = tmp;
            }

            return b + (a - b) * k;
        }

        public static int GetRandomInt(int a,int b)
        {
            int k = (int) GetRandom(a, b + 1);
            if (k >= b && b >= a)
            {
                k = b;
            }

            return k;
        }

        #endregion

        public static T GetRandomElement<T>(IEnumerable<T> list)
        {
            return GetRandomElementInList<T>(list.ToList());
        }

        public static T GetRandomElementInList<T>(List<T> list)
        {
            if (list.Count == 0) return default;
            return list[GetRandomInt(0, list.Count - 1)];
        }
    }
}

