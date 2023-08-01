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
            Loader.Object.AddComponent<Globals>();
            Loader.Object.AddComponent<DrawMenu>();
            UnityEngine.Object.DontDestroyOnLoad(Object);
        }
        public static GameObject Object = new GameObject();

    }
}
