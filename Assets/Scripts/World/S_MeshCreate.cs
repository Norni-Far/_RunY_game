using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))] // тут то, без чего не запустится скрипт 
public class S_MeshCreate : MonoBehaviour
{
    //
    //

    private Mesh Mesh;
    private Vector3[] Vertices;
    private List<int> Triangles = new List<int>();
    private int[] TrianglesForMesh;

    public void Start_S_CreateMesh(Vector3[] VerrticesOfPoligon)
    {
        Vertices = VerrticesOfPoligon;

        Mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = Mesh;
        
        CreateMesh();
        UpdateMesh();
    }

    private void CreateMesh()
    {
        int a = 0;

        for (int i = 0; i < Vertices.Length; i++)
        {
            a++;
            if (a != 3)
            {
                Triangles.Add(i);
            }
            else
            {
                a = 0;
                Triangles.Add(Vertices.Length - 1);

                if (i != Vertices.Length - 1)
                    i -= 2;
            }
        }

        TrianglesForMesh = new int[Triangles.Count];

        for (int i = 0; i < Triangles.Count; i++)
        {
            TrianglesForMesh[i] = Triangles[i];
        }
    }

    private void UpdateMesh()
    {
        Mesh.Clear();

        Mesh.vertices = Vertices;
        Mesh.triangles = TrianglesForMesh;
    }

}
