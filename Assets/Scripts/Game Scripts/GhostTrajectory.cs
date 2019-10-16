﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTrajectory : MonoBehaviour
{
    Mesh mesh;
    public GameObject TrajectoryLine;
 
    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    public void CopyTrajectory()
    {
        //copy position and rotation
        transform.position = TrajectoryLine.GetComponent<Transform>().position;
        transform.rotation = TrajectoryLine.GetComponent<Transform>().rotation;

        //copy mesh info
        if (mesh)
        {
            mesh.Clear();

            
            mesh.vertices = TrajectoryLine.GetComponent<MeshFilter>().mesh.vertices;
            mesh.triangles = TrajectoryLine.GetComponent<MeshFilter>().mesh.triangles;
        }
    }
}
