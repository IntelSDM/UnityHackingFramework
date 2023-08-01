using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityFramework
{
    public class Loader
    {
        public static void Init()
        {
            UnityEngine.Object.DontDestroyOnLoad(Object);
        }
        public static GameObject Object = new GameObject();

    }
}
