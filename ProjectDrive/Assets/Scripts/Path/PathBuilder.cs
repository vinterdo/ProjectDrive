using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathBuilder : MonoBehaviour 
{
    BezierSpline spline;
    MeshFilter filter;

    public float width = 2;
    public float distance = 5;
    public float Xstr = 3;
    public float Zstr = 1.2f;
    public float offroad = 0.1f;
    public float offUV = 0.2f;
    public float step = 0.01f;

	// Use this for initialization
	void Start () 
    {
        spline = GetComponent<BezierSpline>();
        filter = GetComponent<MeshFilter>();

        Mesh m = filter.mesh;
        m.Clear();

        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        for(int i = 0;i<1/step;i++)
        {
            Vector3 v = spline.GetPoint(i * step);
            Vector3 next;
            if(i<1)
                next = spline.GetPoint((i + 1) * step);
            else
                next = v;

            float angle = Vector3.Angle(v, next);

            Vector3 temp;
            float x,y,z;

            if (i == 0)
                y = 0;
            else
                y = i * distance;

            x = v.x * Xstr;
            z = v.z * Zstr;
            
            temp = new Vector3(x - width / 2, y, z);
            temp = RotatePointAroundPivot(temp, v, Quaternion.Euler(0, angle, 0));
            verts.Add(temp);

            temp = new Vector3(x - (width / 2) + offroad, y, z);
            temp = RotatePointAroundPivot(temp, v, Quaternion.Euler(0, angle, 0));
            verts.Add(temp);

            temp = new Vector3(x + (width / 2) - offroad, y, z);
            temp = RotatePointAroundPivot(temp, v, Quaternion.Euler(0, angle, 0));
            verts.Add(temp);

            temp = new Vector3(x + width / 2, y, z);
            temp = RotatePointAroundPivot(temp, v, Quaternion.Euler(0, angle, 0));
            verts.Add(temp);
        }

        m.vertices = verts.ToArray();

        Debug.Log(verts.Count);

        for(int i = 1;i<verts.Count / 4;i++)
        {
            //OFFROAD LEFT
            tris.AddRange(new int[] { i * 4, (i * 4) + 1, ((i - 1) * 4) + 1, ((i - 1) * 4), i * 4, ((i - 1) * 4) + 1 });

            //MIDDLE
            tris.AddRange(new int[] { (i * 4) + 1, (i * 4) + 2, ((i - 1) * 4) + 2, ((i - 1) * 4) + 1, (i * 4) + 1, ((i - 1) * 4) + 2 });

            //OFFROAD RIGHT
            tris.AddRange(new int[] { (i * 4) + 2, (i * 4) + 3, ((i - 1) * 4) + 3, ((i - 1) * 4) + 2, (i * 4) + 2, ((i - 1) * 4) + 3 });
        }

        m.triangles = tris.ToArray();

        m.RecalculateNormals();
        m.RecalculateBounds();
        m.Optimize();
	}

    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angle)
    {
        return angle * (point - pivot) + pivot;
    }
}
