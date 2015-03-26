using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour 
{
    public float MaxRayDist = 1;
    public float ForceValue = 1;
    public float Acceleration = 1;
    public float TurningForce = 1;
    public float Traction = 0.1f;

    float _ControllForward = 0, _ControllSides = 0;
    public float ControllForward
    {
        get
        {
            return _ControllForward;
        }
        set
        {
            if (!Lock) _ControllForward = value;
        }
    }
    public float ControllSides
    {
        get
        {
            return _ControllSides ;
        }
        set
        {
            if (!Lock) _ControllSides = value;
        }
    }

    public bool Lock = false;

	void Start () 
    {
        //rigidbody.centerOfMass = new Vector3(0, -1, 0);
	}
	
	void FixedUpdate () 
    {
        Ray ray1 = new Ray(transform.position + transform.rotation * new Vector3(0.51f, -0.20f, 1f), -transform.up);
        Ray ray2 = new Ray(transform.position + transform.rotation * new Vector3(0.51f, -0.20f, -1f), -transform.up);
        Ray ray3 = new Ray(transform.position + transform.rotation * new Vector3(-0.51f, -0.20f, 1f), -transform.up);
        Ray ray4 = new Ray(transform.position + transform.rotation * new Vector3(-0.51f, -0.20f, -1f), -transform.up);

        RaycastHit HitInfo1, HitInfo2, HitInfo3, HitInfo4;

        bool OnGround = false;

        if (Physics.Raycast(ray1, out HitInfo1, MaxRayDist) && HitInfo1.distance < MaxRayDist)
        {
            float BackForce = (1f - HitInfo1.distance / MaxRayDist) * ForceValue;
            rigidbody.AddForceAtPosition(BackForce * transform.up, transform.position + transform.rotation * new Vector3(0.5f, -0.25f, 1f));
        }
        if (Physics.Raycast(ray2, out HitInfo2, MaxRayDist) && HitInfo2.distance < MaxRayDist)
        {
            float BackForce = (1f - HitInfo2.distance / MaxRayDist) * ForceValue;
            rigidbody.AddForceAtPosition(BackForce * transform.up, transform.position + transform.rotation * new Vector3(0.5f, -0.25f, -1f));
            OnGround = true;
        }
        if (Physics.Raycast(ray3, out HitInfo3, MaxRayDist) && HitInfo3.distance < MaxRayDist)
        {
            float BackForce = (1f - HitInfo3.distance / MaxRayDist) * ForceValue;
            rigidbody.AddForceAtPosition(BackForce * transform.up, transform.position + transform.rotation * new Vector3(-0.5f, -0.25f, 1f));
        }
        if (Physics.Raycast(ray4, out HitInfo4, MaxRayDist) && HitInfo4.distance < MaxRayDist)
        {
            float BackForce = (1f - HitInfo4.distance / MaxRayDist) * ForceValue;
            rigidbody.AddForceAtPosition(BackForce * transform.up, transform.position + transform.rotation * new Vector3(-0.5f, -0.25f, -1f)); 
            OnGround = true;
        }

        if (OnGround)
        {
            Vector3 AccForce = Acceleration * transform.forward;

            rigidbody.AddForceAtPosition(ControllForward * AccForce, transform.position + transform.rotation * new Vector3(0, -0.15f, 0.1f));
            rigidbody.AddTorque(new Vector3(0, ControllSides * TurningForce, 0));

            rigidbody.drag = 1.2f;

            ApplyTraction();
        }
        else
        {
            rigidbody.drag = 0.3f;
        } 
        
        if (transform.up.y < 0.1)
        {
            rigidbody.centerOfMass = new Vector3(0, -1, 0);
        }
        else
        {
            rigidbody.centerOfMass = new Vector3(0, -0.2f, 0);
        }

        //Camera.main.fov = 60f * (1 + rigidbody.velocity.magnitude / 50f);
	}

    private void ApplyTraction()
    {
        Vector3 Inversed = Quaternion.Inverse(transform.rotation) * rigidbody.velocity;

        //Debug.DrawLine(transform.position, transform.position + rigidbody.rotation * new Vector3(Inversed.x, 0, 0), Color.red);

        rigidbody.AddForce(rigidbody.rotation * new Vector3(Inversed.x, 0, 0) * -Traction);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(new Ray(transform.position + transform.rotation * new Vector3(0.5f, -0.25f, 1f), -transform.up));
        Gizmos.DrawRay(new Ray(transform.position + transform.rotation * new Vector3(0.5f, -0.25f, -1f), -transform.up));
        Gizmos.DrawRay(new Ray(transform.position + transform.rotation * new Vector3(-0.5f, -0.25f, 1f), -transform.up));
        Gizmos.DrawRay(new Ray(transform.position + transform.rotation * new Vector3(-0.5f, -0.25f, -1f), -transform.up));

    }
}
