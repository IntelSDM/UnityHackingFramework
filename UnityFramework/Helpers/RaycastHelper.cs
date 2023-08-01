using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace UnityFramework.Helpers
{
    class RaycastHelper
    {
        private static RaycastHit RaycastHit;
        private static readonly LayerMask Mask;

        public static LayerMask MaskFinder()
        {
            try
            {
             

                Physics.Linecast(
                    Globals.MainCamera.transform.position,
                    Globals.MainCamera.transform.position - Globals.MainCamera.transform.up * 1000f,
                    out RaycastHit,
                    Mask);

                return RaycastHit.collider.gameObject.layer;
            }
            catch
            {
                return RaycastHit.collider.gameObject.layer;
            }
        }
        public static bool IsPointVisible(GameObject player, Vector3 BonePos)
        {

            return (Physics.Linecast(
                    Globals.MainCamera.transform.position,
                    BonePos,
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.transform.root.gameObject);

        }
      
    }
}
