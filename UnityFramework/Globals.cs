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
    class Globals : MonoBehaviour
    {
        public static Camera MainCamera;
        public static Configs.Config Config = new Configs.Config();
        
        public static bool IsScreenPointVisible(Vector3 screenpoint)
        {
            // check if esp is on screen, dont render
            return screenpoint.z > 0.01f && screenpoint.x > -5f && screenpoint.y > -5f && screenpoint.x < (float)Screen.width && screenpoint.y < (float)Screen.height;
        }

        public static Vector3 WorldPointToScreenPoint(Vector3 worldPoint)
        {
            // work to screen, flip height
            Vector3 vector = MainCamera.WorldToScreenPoint(worldPoint);
            vector.y = (float)Screen.height - vector.y;
            return vector;
        }
      
        void Start()
        {
            Helpers.ShaderHelper.SetUp();
            Helpers.ConfigHelper.SetUp();
            Helpers.ColourHelper.SetUp();
        }
      
    }
}
