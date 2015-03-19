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
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        for(int i=0; i < Vehicles.Count; i++)
        {
            if(Vector3.Magnitude(Vehicles[i].transform.position - GetNextCheckpoint(Checkpoints[Vehicles[i]]).transform.position) < MaxDistFromCheckpoint)
			{
				Checkpoints[Vehicles[i]] = GetNextCheckpoint(Checkpoints[Vehicles[i]]);
				Debug.Log("switched checpoint");
			}
        }
	}

    public int GetVehiclePos(GameObject Vehicle)
    {
        return 0;
    }
	
	public GameObject GetNextCheckpoint(GameObject Checkpoint)
	{
		int index = Array.IndexOf(PathObject.GetComponentsInChildren<Transform>(), Checkpoint.transform);
		Transform[] path = PathObject.GetComponentsInChildren<Transform>();
		return path[Mathf.Clamp(index + 1,0, path.Length)].gameObject;
	}
	
    public void BeginRace()
    {
    }

    public void EndRace()
    {
    }
}