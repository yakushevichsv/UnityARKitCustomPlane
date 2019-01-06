using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class ARUtility : MonoBehaviour {

        private MeshCollider meshCollider; //declared to avoid code stripping of class
        private MeshFilter meshFilter; //declared to avoid code stripping of class
        private static GameObject planePrefab = null;

        public static void InitializePlanePrefab(GameObject go)
        {
            planePrefab = go;
        }
        
        public static GameObject CreatePlaneInScene(ARPlaneAnchor arPlaneAnchor)
        {
            var plane = planePrefab != null ? Instantiate(planePrefab) : new GameObject(); 

            plane.name = arPlaneAnchor.identifier;

            var apmr = plane.GetComponent<ARKitPlaneCustomMeshRender> ();
            apmr?.InitiliazeMesh (arPlaneAnchor);

            return UpdatePlaneWithAnchorTransform(plane, arPlaneAnchor);

        }

        public static GameObject UpdatePlaneWithAnchorTransform(GameObject plane, ARPlaneAnchor arPlaneAnchor)
        {
            //do coordinate conversion from ARKit to Unity
            plane.transform.position = UnityARMatrixOps.GetPosition (arPlaneAnchor.transform);
            plane.transform.rotation = UnityARMatrixOps.GetRotation (arPlaneAnchor.transform);

            var apmr = plane.GetComponent<ARKitPlaneCustomMeshRender> ();
            apmr?.UpdateMesh (arPlaneAnchor);



            MeshFilter mf = plane.GetComponentInChildren<MeshFilter> ();

            if (mf != null) {
                if (apmr == null) {
                    //since our plane mesh is actually 10mx10m in the world, we scale it here by 0.1f
                    mf.gameObject.transform.localScale = new Vector3 (arPlaneAnchor.extent.x * 0.1f, arPlaneAnchor.extent.y * 0.1f, arPlaneAnchor.extent.z * 0.1f);

                    //convert our center position to unity coords
                    mf.gameObject.transform.localPosition = new Vector3(arPlaneAnchor.center.x,arPlaneAnchor.center.y, -arPlaneAnchor.center.z);
                }

            }

            return plane;
        }


}
