using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARRayManager
{
    private static ARRaycastManager arRay;
    private static List<ARRaycastHit> hitInfos = new List<ARRaycastHit>();
    static ARRayManager()
    {
        arRay = GameObject.FindObjectOfType<ARRaycastManager>();
    }

    public static bool Raycast(Vector2 screenPosition, out Pose pose)
    {
        //레이캐스트를 쏘아 평면이면 공간정보를 hitInfos에 가져온다.
        if (arRay.Raycast(screenPosition, hitInfos, TrackableType.Planes))
        {
            pose = hitInfos[0].pose;
            return true;
        }
        else
        {
            pose = Pose.identity;
            return false;
        }
    }
}
