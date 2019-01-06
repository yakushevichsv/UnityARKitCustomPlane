using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARGeneratePlane : MonoBehaviour {

    public GameObject planePrefab;
    private ARAnchorManager customARAnchorManager;

    // Use this for initialization
    void Start()
    {
        customARAnchorManager = new ARAnchorManager();
        ARUtility.InitializePlanePrefab(planePrefab);
    }

    void OnDestroy()
    {
        customARAnchorManager.Destroy();
    }

    void OnGUI()
    {
        /*IEnumerable<ARPlaneAnchorGameObject> arpags = unityARAnchorManager.GetCurrentPlaneAnchors();
        foreach (var planeAnchor in arpags)
        {
            //ARPlaneAnchor ap = planeAnchor;
            //GUI.Box (new Rect (100, 100, 800, 60), string.Format ("Center: x:{0}, y:{1}, z:{2}", ap.center.x, ap.center.y, ap.center.z));
            //GUI.Box(new Rect(100, 200, 800, 60), string.Format ("Extent: x:{0}, y:{1}, z:{2}", ap.extent.x, ap.extent.y, ap.extent.z));
        }*/
    }
}
