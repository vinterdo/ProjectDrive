using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* https://www.youtube.com/watch?v=KZaz7OqyTHQ */

[RequireComponent (typeof(Vehicle))]
public class Jump : MonoBehaviour
{
    public float IdleForce = 7000;
    public float JumpForce = 100000;
    public float IdleRayDist = 0.4f;
    public float JumpRayDist = 1;
    public float Colddown = 10;
    public float ForceTime = 1;
    Vehicle V;
    int _cd;
    float _timeLeft;

    void Start()
    {
        V = GetComponent<Vehicle>();
        IdleForce = V.ForceValue;
        IdleRayDist = V.MaxRayDist;
    }

    void Update()
    {
        if (Input.GetAxis("Jump") > 0)
        {
            _timeLeft = ForceTime;
             
        }

        if (_timeLeft > 0)
        {
            V.ForceValue = JumpForce;
            V.MaxRayDist = JumpRayDist;
            _timeLeft -= Time.deltaTime;
        }
        else
        {
            V.ForceValue = IdleForce;
            V.MaxRayDist = IdleRayDist;
        }
    }

}
