using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZED.CustomControl
{
    public class Singleton<T> where T : class, new()
    {
        private static T _Instance = default(T);

        public static T GetInstance()
        {
            if (Singleton<T>._Instance == null)
            {
                Interlocked.CompareExchange<T>(ref Singleton<T>._Instance, Activator.CreateInstance<T>(), default(T));
            }
            return Singleton<T>._Instance;
        }
    }
}
