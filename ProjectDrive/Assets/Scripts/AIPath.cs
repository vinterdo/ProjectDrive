using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPath : MonoBehaviour 
{

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Transform[] Waypoints = GetComponentsInChildren<Transform>();

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

}
