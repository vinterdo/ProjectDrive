using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GuiManager : MonoBehaviour
{
    public GameObject PlayerVehicle;
    public GameObject RaceManager;

    public GameObject PositionCanvas;
    public GameObject WrongWayCanvas;
    public GameObject CountdownCanvas;
    public GameObject HighScoreCanvas;
    public GameObject TimerCanvas;

    void OnGUI()
    {
        RaceManager RM = RaceManager.GetComponent<RaceManager>();
        PositionCanvas.GetComponent<Text>().text = RM.CurrentState == global::RaceManager.State.Race ? RM.GetVehiclePos(PlayerVehicle).ToString() + " / " + RM.Vehicles.Count.ToString() : "";
        WrongWayCanvas.GetComponent<Text>().text = RM.IsWrongWay(PlayerVehicle) ? "Wrong Way" : "";
        CountdownCanvas.GetComponent<Text>().text = RM.CurrentState == global::RaceManager.State.Countdown ? ((int)(RM.MaxCountdownTime - RM.CurrentCountdownTime) + 1).ToString() : "";
        TimerCanvas.GetComponent<Text>().text = RM.CurrentState == global::RaceManager.State.Race ? (Mathf.Round((Time.timeSinceLevelLoad - RM.RaceStart)* 10f) / 10f).ToString() : "";

        if(RM.CurrentState == global::RaceManager.State.Finish)
        {
            HighScoreCanvas.GetComponent<Text>().text = "";

            foreach (GameObject V in RM.FinishedVehicles)
            {
                HighScoreCanvas.GetComponent<Text>().text += V.name + "\n";
            }
        }
        else
        {
            HighScoreCanvas.GetComponent<Text>().text = "";
        }
    }
}