using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private const int ThreadGroupSize = 8;

    [Header("General Settings")] public DensityGenerator densityGenerator;
    public float waterLevel;
    public bool generateClouds;

    [Space] public ComputeShader shader;

    [Header("Voxel Settings")] public float isoLevel;

    public float boundsSize = 1;
    public Vector3 offset = Vector3.zero;

    [Range(2, 100)] public int numPointsPerAxis = 30;

    // Chunk spawning
    public Transform viewer;

    [Range(1, 10)] public int lod;

    [Range(1, 10)] public int lodDivider;

    public static Generator Instance;

    private void Awake()
    {
        Instance = this;
    }


    private Vector3 ChunkPosFromCoord(Vector3Int coord)
    {
        return new Vector3(coord.x * boundsSize, coord.y * boundsSize, coord.z * boundsSize);
    }
    

    public void GenerateMesh(ref ChunkData.ChunkMesh mesh, ref Vector3Int chunk, Vector3Int numChunks)
    {
        int chunkLod = 1;
        
        int perAxis = numPointsPerAxis / chunkLod;
        int numVoxelsPerAxis = perAxis - 1;
        int numThreadsPerAxis = Mathf.CeilToInt(numVoxelsPerAxis / (float) ThreadGroupSize);
        float pointSpacing = boundsSize / (perAxis - 1);
        
        int numPoints = numPointsPerAxis / chunkLod * numPointsPerAxis / chunkLod * numPointsPerAxis / chunkLod;
        int numVoxels = numVoxelsPerAxis * numVoxelsPerAxis * numVoxelsPerAxis;
        int maxTriangleCount = numVoxels * 5;

    // Always create buffers in editor (since buffers are released immediately to prevent memory leak)
        // Otherwise, only create if null or if size has changed
        ComputeBuffer triangleBuffer = new ComputeBuffer(maxTriangleCount, sizeof(float) * 3 * 3, ComputeBufferType.Append);
        ComputeBuffer pointsBuffer = new ComputeBuffer(numPoints, sizeof(float) * 4);
        ComputeBuffer triCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);

        //_waterTriangleBuffer = new ComputeBuffer(maxTriangleCount, sizeof(float) * 3 * 3, ComputeBufferType.Append);
        //_waterTriCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);

        Vector3 centre = ChunkPosFromCoord(chunk);
        Vector3 worldBounds = new Vector3(numChunks.x, numChunks.y, numChunks.z) * boundsSize;

        densityGenerator.Generate(pointsBuffer, perAxis, boundsSize, worldBounds, centre, offset, pointSpacing,
            chunkLod, false);

        triangleBuffer.SetCounterValue(0);
        int kernelIndex = shader.FindKernel("March");
        shader.SetBuffer(kernelIndex, "points", pointsBuffer);
        shader.SetBuffer(kernelIndex, "triangles", triangleBuffer);
        shader.SetInt("numPointsPerAxis", perAxis);
        shader.SetFloat("isoLevel", isoLevel);
        shader.SetBool("useWaterLevel", false);

        shader.Dispatch(kernelIndex, numThreadsPerAxis, numThreadsPerAxis, numThreadsPerAxis);

        // Get number of triangles in the triangle buffer
        ComputeBuffer.CopyCount(triangleBuffer, triCountBuffer, 0);
        int[] triCountArray = {0};
        triCountBuffer.GetData(triCountArray);
        int numTris = triCountArray[0];

        // Get triangle data from shader
        Triangle[] tris = new Triangle[numTris];
        triangleBuffer.GetData(tris, 0, 0, numTris);

        var vertices = new Vector3[numTris * 3];
        var triangles = new int[numTris * 3];
        var normals = new Vector3[numTris * 3];

        for (int i = 0; i < numTris; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                vertices[i * 3 + j] = tris[i][j];
                triangles[i * 3 + j] = i * 3 + j;
                normals[i * 3 + j] = tris[i].GetNormal();
            }
        }

        // mesh.Clear();
        mesh.vertices = new NativeArray<Vector3>(vertices, Allocator.Persistent);
        mesh.triangles = new NativeArray<int>(triangles, Allocator.Persistent);
        mesh.normals = new NativeArray<Vector3>(normals, Allocator.Persistent);
        // mesh.RecalculateBounds();
        
        triangleBuffer.Release();
        pointsBuffer.Release();
        triCountBuffer.Release();
    }

    public struct Triangle
    {
        public Vector3 A;
        public Vector3 B;
        public Vector3 C;

        public Vector3 this[int i]
        {
            get
            {
                return i switch
                {
                    0 => A,
                    1 => B,
                    _ => C
                };
            }
        }

        public Vector3 GetNormal()
        {
            return Vector3.Cross(B - A, C - A).normalized;
        }
    }
}