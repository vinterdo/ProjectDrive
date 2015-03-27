using UnityEngine;
using System.Collections;

public class Finish : MonoBehaviour
{
    public GameObject RaceManager;

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.GetComponent<Vehicle>()!= null)
        {
            var RM = RaceManager.GetComponent<RaceManager>();
            if (!RM.FinishedVehicles.Contains(col.gameObject) && RM.IsOnLastCheckpoint(col.gameObject))
            {
                RM.FinishedVehicles.Add(col.gameObject);
                col.gameObject.GetComponent<Vehicle>().Lock = true;
            }
        }
    }
	
}
