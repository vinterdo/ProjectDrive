using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AIPath))]
public class AIPathEditor : Editor
{
    bool edit = false;
    int ID = "AIPathEditor".GetHashCode();

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AIPath path = (AIPath)target;
        if (GUILayout.Button("Snap down"))
        {
            path.SnapDown();
        }

        if (GUILayout.Button("Snap up"))
        {
            path.SnapUp();
        }

        if (GUILayout.Button("Up"))
        {
            path.Up();
        }

        if (GUILayout.Button("Down"))
        {
            path.Down();
        }

        if (GUILayout.Button("Moar"))
        {
            path.Moar();
        }

        if(edit)
        {
            GUI.color = Color.red;
        }
        else
        {
            GUI.color = Color.white;
        }

        if(GUILayout.Button("Edit Mode"))
        {
            edit = !edit;
            if (GUIUtility.hotControl != 0) GUIUtility.hotControl = 0;
        }

        if(Event.current.shift)
        {
            edit = !edit;
            if (GUIUtility.hotControl != 0) GUIUtility.hotControl = 0;
        }
    }

    void OnSceneGUI()
    {
        AIPath path = (AIPath)target;
        if (edit)
        {
            if (Event.current.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(ID);
            }

            if (Event.current.type == EventType.MouseDown && HandleUtility.nearestControl == ID)
            {
                Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hitInfo;
                // Shoot this ray. check in a distance of 10000.
                if (Physics.Raycast(worldRay, out hitInfo, 10000))
                {
                    if (path.empty)
                    {
                        GameObject g = (GameObject)Instantiate(path.empty, hitInfo.point, Quaternion.identity);
                        g.name = "Waypoint" + path.count++;
                        g.transform.parent = path.transform;
                        EditorUtility.SetDirty(g);
                        HandleUtility.Repaint();
                    }
                    else
                    {
                        Debug.LogError("There's no 'empty' object !");
                    }
                }
            }
            Event.current.Use();
        }
    }
}