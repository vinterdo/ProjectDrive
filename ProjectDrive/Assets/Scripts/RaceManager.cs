using UnityEngine;
using System;
using System.Collections.Generic;

public class RaceManager : MonoBehaviour 
{
    public GameObject PathObject;
    public List<GameObject> Vehicles;
    public float CheckpointDist;
    public float MaxDistFromCheckpoint;
	
	public Dictionary<GameObject, GameObject> Checkpoints; // Checkpoints<Vehicle> -> Checkpoint

    public enum State
    {
        Overview,
        Countdown,
        Race,
        Finish
    }

    public State CurrentState;
    public float MaxCountdownTime = 3;
    public float CurrentCountdownTime = 0;
    public GameObject OverwievCamera;
    public GameObject PlayerCamera;

    public Vector3 OverviewCenter = Vector3.zero;
    public float OverviewRadius = 500;
    public float OverviewHeight = 500;
    public float OverviewSpeed = 1;

    public List<GameObject> FinishedVehicles;

    public float RaceStart;
    

	void Start () 
    {
        Checkpoints = new Dictionary<GameObject, GameObject>();
        GameObject FirstCheckpoint = PathObject.GetComponentsInChildren<Transform>()[0].gameObject;
        foreach (GameObject V in Vehicles)
        {
            Checkpoints.Add(V, FirstCheckpoint);
            V.GetComponent<Vehicle>().Lock = true;
        }

        OverwievCamera.SetActive(true);
        PlayerCamera.SetActive(false);
        CurrentState = State.Overview;

        FinishedVehicles = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (CurrentState)
        {
            case State.Countdown:
                CountdownUpdate();
                break;
            case State.Finish:
                if (Input.GetKey(KeyCode.Space)) Application.LoadLevel(Application.loadedLevel);
                break;
            case State.Overview:
                OverwievUpdate();
                break;
            case State.Race:
                RaceUpdate();
                break;
        }
    }

    private void OverwievUpdate()
    {
        OverwievCamera.transform.position = OverviewCenter + new Vector3(Mathf.Sin(Time.realtimeSinceStartup * OverviewSpeed) * OverviewRadius, OverviewHeight, Mathf.Cos(Time.realtimeSinceStartup * OverviewSpeed) * OverviewRadius);
        OverwievCamera.transform.LookAt(OverviewCenter);


        if (Input.GetKey(KeyCode.Space))
        {
            CurrentState = State.Countdown;
            OverwievCamera.SetActive(false);
            PlayerCamera.SetActive(true);
        }
    }

    private void CountdownUpdate()
    {
        CurrentCountdownTime += Time.deltaTime;
        if (CurrentCountdownTime > MaxCountdownTime)
        {
            CurrentCountdownTime = 0;
            CurrentState = State.Race;

            RaceStart = Time.timeSinceLevelLoad;

            foreach (GameObject V in Vehicles)
            {
                V.GetComponent<Vehicle>().Lock = false;
            }
        }
    }

    private void RaceUpdate()
    {
        for (int i = 0; i < Vehicles.Count; i++)
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

        if(FinishedVehicles.Count == Vehicles.Count)
        {
            CurrentState = State.Finish;
        }
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

    public bool IsOnLastCheckpoint(GameObject Vehicle)
    {
        GameObject CheckPoint = Checkpoints[Vehicle];

        Transform[] path = PathObject.GetComponentsInChildren<Transform>();
        if (path[path.Length - 1] == CheckPoint.transform) return true;
        return false;
    }
}