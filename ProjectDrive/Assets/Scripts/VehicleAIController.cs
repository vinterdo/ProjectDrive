using UnityEngine;
using System.Collections;

public class VehicleAIController : MonoBehaviour 
{
    public GameObject PathObject;
    public GameObject VehicleObject;
    public float PathDistance = 10;
    public float SlowingDist = 5;
    public float MaxSpeed = 0.8f;
    public float MinSpeed = 0.1f;

    public int WaypointId;

    Transform[] Waypoints;
    float TurnAngle = -1;

    void Start()
    {
        AssignClosestWaypoint();
    }

    void Update()
    {
        Waypoints = PathObject.GetComponentsInChildren<Transform>();
        if (Vector3.Distance(transform.position, Waypoints[WaypointId].position) < PathDistance)
        {
            WaypointId = (WaypointId + 1) % Waypoints.Length;
        }

        Vector3 steerVec = transform.InverseTransformPoint(new Vector3(Waypoints[WaypointId].position.x, transform.position.y, Waypoints[WaypointId].position.z));
        float steer = steerVec.x / steerVec.magnitude;

        VehicleObject.GetComponent<Vehicle>().ControllSides = steer;

        float DistToClosestWayp = Vector3.Distance(transform.position, Waypoints[FindClosestWaypointId()].position);

        if (DistToClosestWayp < SlowingDist)
        {
            TurnAngle = GetClosestTurnAngle();
            float diff = TurnAngle / 180f;
            VehicleObject.GetComponent<Vehicle>().ControllForward = MinSpeed + (MaxSpeed - MinSpeed) * (1f - diff) * (1f - Mathf.Abs(steer));
        }
        else
        {
            VehicleObject.GetComponent<Vehicle>().ControllForward = MaxSpeed * (1f - Mathf.Abs(steer));
        }
    }

    void AssignClosestWaypoint()
    {
        WaypointId = FindClosestWaypointId();
    }

    int FindClosestWaypointId()
    {
        Waypoints = PathObject.GetComponentsInChildren<Transform>();
        int id = 0;
        float minDist = float.MaxValue;
        for (int i = 0; i < Waypoints.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, Waypoints[i].position);
            if (dist < minDist)
            {
                minDist = dist;
                id = i;
            }
        }

        return id;
    }

    float GetNextTurnAngle()
    {
        Vector3 diff1 = (Waypoints[(WaypointId + 1) % Waypoints.Length].position - Waypoints[WaypointId].position).normalized;
        Vector3 diff2 = (Waypoints[(WaypointId + 2) % Waypoints.Length].position - Waypoints[(WaypointId + 1) % Waypoints.Length].position).normalized;

        return Vector3.Angle(diff2, diff1);
    }

    float GetClosestTurnAngle()
    {
        //Debug.Log(FindClosestWaypointId());
        Vector3 diff1 = (Waypoints[TranslateId(FindClosestWaypointId(), 0)].position - Waypoints[TranslateId(FindClosestWaypointId(), -1)].position).normalized;
        Vector3 diff2 = (Waypoints[TranslateId(FindClosestWaypointId(), 1)].position - Waypoints[TranslateId(FindClosestWaypointId(), 0)].position).normalized;

        //Debug.Log(Vector3.Angle(diff2, diff1));

        return Vector3.Angle(diff2, diff1);
    }

    int TranslateId(int id, int trans)
    {
        return (id + trans + Waypoints.Length) % Waypoints.Length;
    }
}
