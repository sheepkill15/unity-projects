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

    public Vector3Int numChunks = Vector3Int.one;
    public float waterLevel;
    public bool generateClouds;
    public float cloudLevel;

    [Space] public ComputeShader shader;

    [Header("Voxel Settings")] public float isoLevel;

    public float boundsSize = 1;
    public Vector3 offset = Vector3.zero;

    [Range(2, 100)] public int numPointsPerAxis = 30;

    public GameObject chunkPrefab;

    // Chunk spawning
    public Transform viewer;

    [Range(1, 10)] public int lod;

    [Range(1, 10)] public int lodDivider;

    private Dictionary<Vector3Int, Chunk> _chunks;

    private Queue<Chunk> _freeChunks;
    private ComputeBuffer _pointsBuffer;
    private Transform _transform;

    // Buffers
    private ComputeBuffer _triangleBuffer;
    private ComputeBuffer _triCountBuffer;

    private ComputeBuffer _waterTriangleBuffer;
    private ComputeBuffer _waterTriCountBuffer;

    private void Awake()
    {
        _transform = transform;
        _chunks = new Dictionary<Vector3Int, Chunk>(numChunks.x * numChunks.y * numChunks.z);
        _freeChunks = new Queue<Chunk>(numChunks.x * numChunks.y * numChunks.z);
    }

    private void Start()
    {
        StartCoroutine(GeneratorUpdate());
    }

    void OnDestroy()
    {
        if (Application.isPlaying)
        {
            ReleaseBuffers();
        }

        StopAllCoroutines();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            MarkAllAsUpdated();
        }
    }

    private Vector3 ChunkPosFromCoord(Vector3Int coord)
    {
        return new Vector3(coord.x * boundsSize, coord.y * boundsSize, coord.z * boundsSize);
    }

    private void AssertChunks()
    {
        Vector3 position = viewer.position;
        Vector3Int currLocation = new Vector3Int(Mathf.FloorToInt(position.x / boundsSize),
            0,
            Mathf.FloorToInt(position.z / boundsSize));

        for (int x = -numChunks.x / 2; x <= numChunks.x / 2; x++)
        {
            for (int y = -numChunks.y / 2; y <= numChunks.y / 2; y++)
            {
                for (int z = -numChunks.z / 2; z <= numChunks.z / 2; z++)
                {
                    Vector3Int chunkCoord = currLocation + new Vector3Int(x, y, z);
                    if (!_chunks.ContainsKey(chunkCoord))
                    {
                        Chunk chunkComp;
                        if (_freeChunks.Count > 0)
                        {
                            chunkComp = _freeChunks.Dequeue();
                        }
                        else
                        {
                            GameObject go = Instantiate(chunkPrefab, Vector3.zero, Quaternion.identity, _transform);
                            chunkComp = go.GetComponent<Chunk>();
                        }

                        chunkComp.Init();
                        chunkComp.updated = true;
                        int chunkLod = Mathf.Clamp((currLocation - chunkCoord).sqrMagnitude / lodDivider, 1, lod);
                        chunkComp.lod = chunkLod;
                        _chunks.Add(chunkCoord, chunkComp);
                    }
                }
            }
        }
    }

    private void CheckForFreeChunks()
    {
        Vector3 position = viewer.position;
        Vector3Int currLocation = new Vector3Int(Mathf.FloorToInt(position.x / boundsSize),
            0,
            Mathf.FloorToInt(position.z / boundsSize));
        List<Vector3Int> toRemove = new List<Vector3Int>(_chunks.Count);
        foreach (var chunk in _chunks)
        {
            if (Mathf.Abs(currLocation.x - chunk.Key.x) > numChunks.x / 2 ||
                Mathf.Abs(currLocation.y - chunk.Key.y) > numChunks.y / 2 ||
                Mathf.Abs(currLocation.z - chunk.Key.z) > numChunks.z / 2)
            {
                toRemove.Add(chunk.Key);
                _freeChunks.Enqueue(chunk.Value);
            }
            else
            {
                int chunkLod = Mathf.Clamp((currLocation - chunk.Key).sqrMagnitude / lodDivider, 1, lod);
                if (chunkLod != chunk.Value.lod)
                {
                    chunk.Value.lod = chunkLod;
                    chunk.Value.updated = true;
                }
            }
        }

        foreach (var chunkToRemove in toRemove)
        {
            _chunks.Remove(chunkToRemove);
        }
    }

    public void MarkAllAsUpdated()
    {
        return;
/*
        if (_chunks == null) return;
        foreach (KeyValuePair<Vector3Int,Chunk> chunk in _chunks)
        {
            chunk.Value.updated = true;
        }
*/
    }

    private IEnumerator GeneratorUpdate()
    {
        Dictionary<JobHandle, ChunkMeshGeneratorJob> scheduledChunks =
            new Dictionary<JobHandle, ChunkMeshGeneratorJob>();
        Dictionary<JobHandle, WaterMeshGeneratorJob> scheduledWaters =
            new Dictionary<JobHandle, WaterMeshGeneratorJob>();
        while (true)
        {
            CheckForFreeChunks();
            AssertChunks();
            foreach (var chunk in _chunks)
            {
                if (chunk.Value.updated)
                {
                    (JobHandle chunkHandle, ChunkMeshGeneratorJob chunkJob, JobHandle waterHandle, WaterMeshGeneratorJob waterJob) = GenerateMesh(chunk.Key);
                    scheduledChunks.Add(chunkHandle, chunkJob);
                    scheduledWaters.Add(waterHandle, waterJob);
                    chunk.Value.updated = false;
                    yield return null;
                }
            }

            foreach (var scheduledChunk in scheduledChunks)
            {
                if (scheduledChunk.Key.IsCompleted)
                {
                    
                    _chunks[scheduledChunk.Value.ChunkCoords].UpdateMesh(scheduledChunk.Value.MeshVertices.ToArray(), scheduledChunk.Value.MeshTriangles.ToArray(), scheduledChunk.Value.MeshNormals.ToArray(),
                        false);

                    scheduledChunk.Value.MeshVertices.Dispose();
                    scheduledChunk.Value.MeshTriangles.Dispose();
                    scheduledChunk.Value.MeshNormals.Dispose();
                    
                    // scheduledChunks.Remove(scheduledChunk);
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    void CreateBuffers(int chunkLod)
    {
        int numPoints = numPointsPerAxis / chunkLod * numPointsPerAxis / chunkLod * numPointsPerAxis / chunkLod;
        int numVoxelsPerAxis = numPointsPerAxis - 1;
        int numVoxels = numVoxelsPerAxis * numVoxelsPerAxis * numVoxelsPerAxis;
        int maxTriangleCount = numVoxels * 5;

        // Always create buffers in editor (since buffers are released immediately to prevent memory leak)
        // Otherwise, only create if null or if size has changed
        if (_pointsBuffer == null || numPoints != _pointsBuffer.count)
        {
            ReleaseBuffers();
            _triangleBuffer = new ComputeBuffer(maxTriangleCount, sizeof(float) * 3 * 3, ComputeBufferType.Append);
            _pointsBuffer = new ComputeBuffer(numPoints, sizeof(float) * 4);
            _triCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);

            _waterTriangleBuffer = new ComputeBuffer(maxTriangleCount, sizeof(float) * 3 * 3, ComputeBufferType.Append);
            _waterTriCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
        }
    }

    public (JobHandle, ChunkMeshGeneratorJob, JobHandle, WaterMeshGeneratorJob) GenerateMesh(Vector3Int chunk)
    {
        int chunkLod = _chunks[chunk].lod;
        CreateBuffers(chunkLod);
        int perAxis = numPointsPerAxis / chunkLod;
        int numVoxelsPerAxis = perAxis - 1;
        int numThreadsPerAxis = Mathf.CeilToInt(numVoxelsPerAxis / (float) ThreadGroupSize);
        float pointSpacing = boundsSize / (perAxis - 1);

        Vector3 centre = ChunkPosFromCoord(chunk) + new Vector3(0, cloudLevel, 0);
        Vector3 worldBounds = new Vector3(numChunks.x, numChunks.y, numChunks.z) * boundsSize;

        densityGenerator.Generate(_pointsBuffer, perAxis, boundsSize, worldBounds, centre, offset, pointSpacing,
            chunkLod, false);

        _triangleBuffer.SetCounterValue(0);
        int kernelIndex = shader.FindKernel("March");
        shader.SetBuffer(kernelIndex, "points", _pointsBuffer);
        shader.SetBuffer(kernelIndex, "triangles", _triangleBuffer);
        shader.SetInt("numPointsPerAxis", perAxis);
        shader.SetFloat("isoLevel", isoLevel);
        shader.SetBool("useWaterLevel", false);

        shader.Dispatch(kernelIndex, numThreadsPerAxis, numThreadsPerAxis, numThreadsPerAxis);

        // Get number of triangles in the triangle buffer
        ComputeBuffer.CopyCount(_triangleBuffer, _triCountBuffer, 0);
        int[] triCountArray = {0};
        _triCountBuffer.GetData(triCountArray);
        int numTris = triCountArray[0];

        // Get triangle data from shader
        Triangle[] tris = new Triangle[numTris];
        _triangleBuffer.GetData(tris, 0, 0, numTris);

        NativeArray<Triangle> chunkInput = new NativeArray<Triangle>(tris, Allocator.TempJob);
        NativeArray<Vector3> chunkSharedVerts = new NativeArray<Vector3>(numTris * 3, Allocator.TempJob);
        NativeArray<int> chunkSharedTris = new NativeArray<int>(numTris * 3, Allocator.TempJob);
        NativeArray<Vector3> chunkSharedNormals = new NativeArray<Vector3>(numTris * 3, Allocator.TempJob);

        ChunkMeshGeneratorJob genJob = new ChunkMeshGeneratorJob
        {
            Input = chunkInput,
            MeshVertices = chunkSharedVerts,
            MeshTriangles = chunkSharedTris,
            MeshNormals = chunkSharedNormals,
            ChunkCoords = chunk
        };

        JobHandle chunkJobHandle = genJob.Schedule();
        _waterTriangleBuffer.SetCounterValue(0);
        shader.SetBuffer(kernelIndex, "triangles", _waterTriangleBuffer);
        shader.SetBool("useWaterLevel", true);
        shader.SetFloat("waterLevel", waterLevel);
        shader.Dispatch(kernelIndex, numThreadsPerAxis, numThreadsPerAxis, numThreadsPerAxis);

        ComputeBuffer.CopyCount(_waterTriangleBuffer, _waterTriCountBuffer, 0);
        int[] waterTriCountArray = {0};
        _waterTriCountBuffer.GetData(waterTriCountArray);
        int waterTriCount = waterTriCountArray[0];
        // Get triangle data from shader
        Triangle[] waterTris = new Triangle[waterTriCount];
        _waterTriangleBuffer.GetData(waterTris, 0, 0, waterTriCount);

        NativeArray<Triangle> input = new NativeArray<Triangle>(waterTris, Allocator.TempJob);
        NativeArray<Vector3> sharedVerts = new NativeArray<Vector3>(waterTriCount * 3, Allocator.TempJob);
        NativeArray<int> sharedTris = new NativeArray<int>(waterTriCount * 3, Allocator.TempJob);
        NativeArray<Vector3> sharedNormals = new NativeArray<Vector3>(waterTriCount * 3, Allocator.TempJob);
        NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);

        WaterMeshGeneratorJob waterMeshGeneratorJob = new WaterMeshGeneratorJob
        {
            Input = input,
            MeshVertices = sharedVerts,
            MeshTriangles = sharedTris,
            MeshNormals = sharedNormals,
            ChunkCoords = chunk,
            Result = result
        };

        JobHandle waterJobHandle = waterMeshGeneratorJob.Schedule();
        chunkJobHandle.Complete();
        waterJobHandle.Complete();

        _chunks[chunk].UpdateMesh(sharedVerts.GetSubArray(0, result[0]).ToArray(), sharedTris.ToArray(),
            sharedNormals.GetSubArray(0, result[0]).ToArray(), true);


        chunkInput.Dispose();
        chunkSharedVerts.Dispose();
        chunkSharedTris.Dispose();
        chunkSharedNormals.Dispose();

        input.Dispose();
        sharedVerts.Dispose();
        sharedTris.Dispose();
        sharedNormals.Dispose();
        result.Dispose();

        return (chunkJobHandle, genJob, waterJobHandle, waterMeshGeneratorJob);
    }

    void ReleaseBuffers()
    {
        if (_triangleBuffer != null)
        {
            _triangleBuffer.Release();
            _pointsBuffer.Release();
            _triCountBuffer.Release();
            _waterTriangleBuffer.Release();
            _waterTriCountBuffer.Release();
        }
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