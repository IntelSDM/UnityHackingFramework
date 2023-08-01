using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace UnityFramework
{
    /*
    You may see some unity methods just calling a function of the method with a 1 on the end. This is because we are excluding the unity method from obfuscation.
    We need to exclude these to keep them working but we make another function to keep the actual code obfuscated.
     */
    class Globals : MonoBehaviour
    {
        public static Camera MainCamera;
     //   public static Cheat.Configs.Config Config = new Configs.Config();
     
        public static bool IsScreenPointVisible(Vector3 screenpoint)
        {
            return screenpoint.z > 0.01f && screenpoint.x > -5f && screenpoint.y > -5f && screenpoint.x < (float)Screen.width && screenpoint.y < (float)Screen.height;
        }

        public static Vector3 WorldPointToScreenPoint(Vector3 worldPoint)
        {
            Vector3 vector = MainCamera.WorldToScreenPoint(worldPoint);
            vector.y = (float)Screen.height - vector.y;
            return vector;
        }
      
        void Start()
        {
        //    Helpers.ShaderHelper.GetShader();
         //   Helpers.ConfigHelper.CreateEnvironment();
          //  Helpers.ColourHelper.AddColours();
        }
      
    }
}
