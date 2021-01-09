using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Set what class this editor will controll (each editor has a target)
[CustomEditor(typeof(scrWaypoint))]
public class scrWaypointEditor : Editor
{
    scrWaypoint waypoint => target as scrWaypoint;

    private void OnSceneGUI()
    {
        Handles.color = Color.red;
        for (int i = 0; i < waypoint.Points.Length; i++)
        {
            EditorGUI.BeginChangeCheck();

            // Create handles
            Vector3 currentWaypointPoint = waypoint.CurrentPosition + waypoint.Points[i];
            Vector3 newWaypointPoint = Handles.FreeMoveHandle(currentWaypointPoint, Quaternion.identity, 0.7f, new Vector3(0.3f, 0.3f, 0.3f), Handles.SphereHandleCap);

            //Create text
            GUIStyle textStyle = new GUIStyle();
            textStyle.fontStyle = FontStyle.Bold;
            textStyle.fontSize = 16;
            textStyle.normal.textColor = Color.yellow;

            //Gives us the bottom right possition
            Vector3 textAllignment = Vector3.down * 0.35f + Vector3.right * 0.35f;
            Handles.Label(waypoint.CurrentPosition + waypoint.Points[i] + textAllignment, $"{i + 1}", textStyle);

            EditorGUI.EndChangeCheck();

            //Update the position of the waypoints as the handles are moved
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Free Move Handle");
                waypoint.Points[i] = newWaypointPoint - waypoint.CurrentPosition;
            }
        }
    }
}
