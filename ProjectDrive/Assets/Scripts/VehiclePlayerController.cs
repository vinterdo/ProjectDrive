using UnityEngine;
using System.Collections;

public class VehiclePlayerController : MonoBehaviour 
{
    public GameObject VehicleObject;

	void Update () 
    {
        Vehicle v = VehicleObject.GetComponent<Vehicle>();

        v.ControllForward = Input.GetAxis("Vertical");
        v.ControllSides = Input.GetAxis("Horizontal");
	}
}
