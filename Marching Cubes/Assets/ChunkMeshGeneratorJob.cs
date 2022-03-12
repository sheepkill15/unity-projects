using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct ChunkMeshGeneratorJob : IJob
{
    [ReadOnly]
    public Vector3Int ChunkCoords;
    [ReadOnly]
    public NativeArray<Generator.Triangle> Input;

    public NativeArray<Vector3> MeshVertices;
    public NativeArray<int> MeshTriangles;
    public NativeArray<Vector3> MeshNormals;
    
    public void Execute()
    {
        int numTris = Input.Length;
        for (int i = 0; i < numTris; i++)
        {
            Vector3 norm = Input[i].GetNormal();
            for (int j = 0; j < 3; j++)
            {
                MeshTriangles[i * 3 + j] = i*3 + j;
                MeshVertices[i * 3 + j] = Input[i][j];
                MeshNormals[i * 3 + j] = norm;
            }
        }
    }
}