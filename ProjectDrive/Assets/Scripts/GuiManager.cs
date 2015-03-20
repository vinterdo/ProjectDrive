using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GuiManager : MonoBehaviour
{
    public GameObject PlayerVehicle;
    public GameObject RaceManager;

    public GameObject PositionCanvas;

    void OnGUI()
    {
        RaceManager RM = RaceManager.GetComponent<RaceManager>();
        PositionCanvas.GetComponent<Text>().text = RM.GetVehiclePos(PlayerVehicle).ToString() + " / " + RM.Vehicles.Count.ToString();
    }
}