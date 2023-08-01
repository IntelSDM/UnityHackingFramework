using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace UnityFramework.Helpers
{
    class ColourHelper
    {
        public static void SetUp()
        {
            AddColour("Menu Primary Colour", Color.red);
            AddColour("Menu Secondary Colour", Color.white);

        }
        public static Color32 GetColour(string identifier)
        {
            if (Globals.Config.Colours.ColoursDict.TryGetValue(identifier, out var toret))
                return toret;
            return Color.magenta;
        }

        public static void AddColour(string id, Color32 c)
        {
            // add to the colour list
            if (!Globals.Config.Colours.ColoursDict.ContainsKey(id))
                Globals.Config.Colours.ColoursDict.Add(id, c);
        }

        public static void SetColour(string id, Color32 c)
        {
            Globals.Config.Colours.ColoursDict[id] = c;

        }
    }
}
