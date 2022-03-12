using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[Serializable]
public struct ChunkData : IComponentData
{
    public Vector3Int coords;
    public bool updated;

    [Serializable]
    public struct ChunkMesh
    {
        public NativeArray<int> triangles;
        public NativeArray<Vector3> vertices;
        public NativeArray<Vector3> normals;
    }

    public ChunkMesh meshData;
}
