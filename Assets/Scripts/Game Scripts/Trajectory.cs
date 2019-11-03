using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Trajectory : MonoBehaviour
{
    Mesh mesh;
    public float meshWidth;

    //public float Fvelocity;
    //public float Fangle;
    public int resolution = 10;


    float g;
    float radianAngle;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        g = Mathf.Abs(Physics.gravity.y);

        ResetTrajectory();

    }

    public void ResetTrajectory()
    {
        transform.rotation = Quaternion.identity;
        transform.position = GameObject.FindGameObjectWithTag("Ball").transform.position;
    }

    public void DrawArc(float velocity, float angle)
    {
        MakeArkMesh(CalcArcArr(velocity, angle));
    }


    void MakeArkMesh(Vector3[] arcVerts)
    {
        if (mesh) 
        { 
            mesh.Clear(); 

            Vector3[] verticies = new Vector3[(resolution + 1) * 2];
            int[] tri = new int[resolution * 12];

            for (int i = 0; i <= resolution; i++)
            {
                //set verts
                verticies[i * 2] = new Vector3(meshWidth * 0.5f, arcVerts[i].y, arcVerts[i].x);
                verticies[i * 2 + 1] = new Vector3(meshWidth * -0.5f, arcVerts[i].y, arcVerts[i].x);

                //set up tris
                if (i != resolution)
                {
                    tri[i * 12] = i * 2;
                    tri[i * 12 + 1] = tri[i * 12 + 4] = i * 2 + 1;
                    tri[i * 12 + 2] = tri[i * 12 + 3] = (i + 1) * 2;
                    tri[i * 12 + 5] = (i + 1) * 2 + 1;

                    tri[i * 12 + 6] = i * 2;
                    tri[i * 12 + 7] = tri[i * 12 + 10] = (i + 1) * 2;
                    tri[i * 12 + 8] = tri[i * 12 + 9] = i * 2 + 1;
                    tri[i * 12 + 11] = (i + 1) * 2 + 1;

                }
            }

            mesh.vertices = verticies;
            mesh.triangles = tri;
            SetUVs();
        }
    }

    private void SetUVs()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(i, i);
        }
        mesh.uv = uvs;
    }

    Vector3[] CalcArcArr(float velocity, float angle)
    {

        Vector3[] arr = new Vector3[resolution + 1];

        radianAngle = Mathf.Deg2Rad * angle;

        //formula from wiki (https://en.wikipedia.org/wiki/Range_of_a_projectile)
        float maxDist = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / g;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arr[i] = CalcArcPoint(t, maxDist, velocity);
        }

        return arr;
    }

    Vector3 CalcArcPoint(float t, float maxDist, float velocity)
    {
        float x = t * maxDist;
        float y = x * Mathf.Tan(radianAngle) - ((g * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));

        return new Vector3(x, y);
    }

}
