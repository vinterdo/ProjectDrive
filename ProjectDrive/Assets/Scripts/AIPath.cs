using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPath : MonoBehaviour 
{
    Transform[] Waypoints;
    public GameObject empty;
    public int count = 0;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Waypoints = GetComponentsInChildren<Transform>();

        for (int i = 0; i < Waypoints.Length; i++)
        {
            Gizmos.DrawSphere(Waypoints[i].position, 0.3f);
        }

        for (int i = 0; i < Waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(Waypoints[i].position, Waypoints[i + 1].position);
        }

        Gizmos.DrawLine(Waypoints[Waypoints.Length - 1].position, Waypoints[0].position);        
    }

    public void SnapDown()
    {
        RaycastHit c;

        for (int i = 0; i < Waypoints.Length; i++)
        {
            if (Physics.Raycast(Waypoints[i].transform.position, -Vector3.up, out c))
            {
                Waypoints[i].transform.position = c.point + Vector3.up;
            }
        }
    }

    public void SnapUp()
    {
        RaycastHit c;

        for (int i = 0; i < Waypoints.Length; i++)
        {
            if (Physics.Raycast(Waypoints[i].transform.position, Vector3.up, out c))
            {
                Waypoints[i].transform.position = c.point + Vector3.up;
            }
        }
    }

    public void Up()
    {
        for (int i = 0; i < Waypoints.Length; i++)
        {
            Waypoints[i].transform.position += Vector3.up;
        }
    }

    public void Down()
    {
        for (int i = 0; i < Waypoints.Length; i++)
        {
            Waypoints[i].transform.position -= Vector3.up;
        }
    }

    public void Moar()
    {
        if (empty)
        {
            Transform[] waypoints = GetComponentsInChildren<Transform>();
            List<Vector3> newW = new List<Vector3>();

            for (int i = 0; i < waypoints.Length - 1; i++)
            {
                newW.Add(waypoints[i].position);

                Vector3 p = (waypoints[i].position + waypoints[i + 1].position) / 2;

                newW.Add(p);
            }

            newW.Add((waypoints[waypoints.Length - 1].position + transform.position) / 2);

            int children = transform.childCount;
            for (int i = 0; i < children; i++)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }

            int x = 0;
            foreach (Vector3 v in newW)
            {
                GameObject t = (GameObject)Instantiate(empty, v, Quaternion.identity);
                t.name = "Waypoint" + x++;
                t.transform.parent = transform;
            }

            count = newW.Count;
        }
        else
        {
            Debug.LogError("There's no 'empty' object !");
        }
    }

}
