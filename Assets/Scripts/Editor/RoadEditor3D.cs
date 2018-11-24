using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoadCreator3D))]
public class RoadEditor : Editor
{

    RoadCreator3D creator;

    private void OnSceneGUI()
    {
        if (creator.autoUpdate )//&& Event.current.type == EventType.Repaint)
        {
            creator.UpdateRoad();
            //divCreator.UpdateDiv();
        }
    }

    private void OnEnable()
    {
        creator = (RoadCreator3D)target;
    }
}
