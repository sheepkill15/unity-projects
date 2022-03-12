using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct WaterMeshGeneratorJob : IJob
{
    [ReadOnly]
    public Vector3Int ChunkCoords;
    [ReadOnly]
    public NativeArray<Generator.Triangle> Input;

    public NativeArray<Vector3> MeshVertices;
    public NativeArray<int> MeshTriangles;
    public NativeArray<Vector3> MeshNormals;

    public NativeArray<int> Result;
    
    public void Execute()
    {
        int numTris = Input.Length;
        var addedVertices = new Dictionary<Vector3, int>(numTris * 3);

        var vertices = new List<Vector3>(numTris * 3);
        var meshNormals = new List<Vector3>(numTris * 3);
        for (int i = 0; i < numTris; i++)
        {
            Vector3 norm = Input[i].GetNormal();
            for (int j = 0; j < 3; j++)
            {
                if (addedVertices.ContainsKey(Input[i][j]))
                {
                    MeshTriangles[i * 3 + j] = addedVertices[Input[i][j]];
                    continue;
                }

                MeshTriangles[i * 3 + j] = vertices.Count;
                vertices.Add(Input[i][j]);
                addedVertices.Add(Input[i][j], vertices.Count - 1);
                meshNormals.Add(norm);
            }
        }

        for (int i = 0; i < vertices.Count; i++)
        {
            MeshVertices[i] = vertices[i];
            MeshNormals[i] = meshNormals[i];
        }

        Result[0] = vertices.Count;
    }
}
