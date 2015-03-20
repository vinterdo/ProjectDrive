using UnityEngine;
using System;
using System.Collections.Generic;

public class RaceManager : MonoBehaviour 
{
    public GameObject PathObject;
    public List<GameObject> Vehicles;
    public float CheckpointDist;
    public float MaxDistFromCheckpoint;
    public GameObject Finish;
	
	public Dictionary<GameObject, GameObject> Checkpoints; // Checkpoints<Vehicle> -> Checkpoint

	void Start () 
    {
        Checkpoints = new Dictionary<GameObject, GameObject>();
        GameObject FirstCheckpoint = PathObject.GetComponentsInChildren<Transform>()[0].gameObject;
        foreach (GameObject V in Vehicles)
        {
            Checkpoints.Add(V, FirstCheckpoint); 
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        for(int i=0; i < Vehicles.Count; i++)
        {
            if (Vector3.Magnitude(Vehicles[i].transform.position - GetNextCheckpoint(Checkpoints[Vehicles[i]]).transform.position) < CheckpointDist)
			{
				Checkpoints[Vehicles[i]] = GetNextCheckpoint(Checkpoints[Vehicles[i]]);
				//Debug.Log("switched checpoint");
			}
        }


        foreach (GameObject V in Vehicles)
        {
            if (Vector3.Distance(V.transform.position, Checkpoints[V].transform.position) > 2 * MaxDistFromCheckpoint)
            {
                V.transform.position = Checkpoints[V].transform.position;
            }
        }
        //Debug.Log(GetVehiclePos(Vehicles[0]));
	}

    public int GetVehiclePos(GameObject Vehicle)
    {
        Transform[] path = PathObject.GetComponentsInChildren<Transform>();
        int pos = Vehicles.Count;
        
        for (int i = 0; i < path.Length; i++)
        {
            List<GameObject> AtCheckpoint = new List<GameObject>();

            foreach (GameObject V in Vehicles)
            {
                if (Checkpoints[V] == path[i].gameObject)
                {
                    AtCheckpoint.Add(V);
                }
            }

            if (AtCheckpoint.Contains(Vehicle))
            {
                float VehicleDist = Vector3.Distance(Vehicle.transform.position, path[i].position);
                foreach (GameObject V in AtCheckpoint)
                {
                    if (Vector3.Distance(V.transform.position, path[i].position) > VehicleDist)
                    {
                        pos--;
                    }
                }

                return pos;
            }
            else
            {
                pos -= AtCheckpoint.Count;
            }

        }
        
        return pos;
    }
	
	public GameObject GetNextCheckpoint(GameObject Checkpoint)
	{
		int index = Array.IndexOf(PathObject.GetComponentsInChildren<Transform>(), Checkpoint.transform);
		Transform[] path = PathObject.GetComponentsInChildren<Transform>();
		return path[Mathf.Clamp(index + 1,0, path.Length - 1)].gameObject;
	}

    public GameObject GetPrivCheckpoint(GameObject Checkpoint)
    {
        int index = Array.IndexOf(PathObject.GetComponentsInChildren<Transform>(), Checkpoint.transform);
        Transform[] path = PathObject.GetComponentsInChildren<Transform>();
        return path[Mathf.Clamp(index - 1, 0, path.Length - 1)].gameObject;
    }
	
    public void BeginRace()
    {
    }

    public void EndRace()
    {
    }

    public bool IsWrongWay(GameObject Vehicle)
    {
        if (Vehicle.rigidbody.velocity.magnitude > 10)
        {
            if (Vector3.Distance(Vehicle.transform.position, Checkpoints[Vehicle].transform.position) > MaxDistFromCheckpoint)
            {
                return true;
            }
        }

        return false;
    }
}