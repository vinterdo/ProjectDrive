using UnityEngine;
using System.Collections;

public class CameraPositioner : MonoBehaviour
{
    public GameObject CameraObject;
    public Vector3 OriginOffset;
    public float dist;

    void Start()
    {
    }

    void Update()
    {
        Vector3 dir = -CameraObject.transform.forward * dist;

        Ray r = new Ray(transform.position + OriginOffset, dir);
        RaycastHit HitInfo;
        Physics.Raycast(r, out HitInfo, dist);

        Debug.DrawLine(transform.position + OriginOffset, transform.position + OriginOffset + dir, Color.red);

        if (HitInfo.collider != null)
        {
            CameraObject.transform.position = (CameraObject.transform.position * 19 + HitInfo.point) / 20f;
        }
        else
        {
            CameraObject.transform.position = (CameraObject.transform.position * 19 + transform.position + OriginOffset + dir) / 20f;
        }
    }
}
