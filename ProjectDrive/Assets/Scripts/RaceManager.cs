using UnityEngine;
using System.Collections.Generic;

public class RaceManager : MonoBehaviour 
{
    public GameObject PathObject;
    public List<GameObject> Vehicles;
    public float CheckpointDist;
    public float MaxDistFromCheckpoint;
    public GameObject Finish;

    List<int> VehicleCheckpoints; // index is index of vehicle, stored number is current checkpoint for given vehicle

	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        for(int i=0; i < Vehicles.Count; i++)
        {
            if(Vector3.Magnitude(Vehicles[i].transform.position, 
        }
	}

    public int GetVehiclePos(GameObject Vehicle)
    {

    }

    public void BeginRace()
    {
    }

    public void EndRace()
    {
    }
}
