using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DensityGenerator : MonoBehaviour
{
    [Header ("Noise")]
    public int seed;
    public int numOctaves = 4;
    public float lacunarity = 2;
    public float persistence = .5f;
    public float noiseScale = 1;
    public float noiseWeight = 1;
    public bool closeEdges;
    public float floorOffset = 1;
    public float weightMultiplier = 1;

    public float hardFloorHeight;
    public float hardFloorWeight;

    public Vector4 shaderParams;
    
    private const int ThreadGroupSize = 8;
    public ComputeShader densityShader;

    private List<ComputeBuffer> _buffersToRelease;

    public static DensityGenerator Instance;

    private void Awake()
    {
        Instance = this;
    }

    //private Dictionary<Vector3, Dictionary<int,Vector4[]>> _computedPoints = new Dictionary<Vector3, Dictionary<int,Vector4[]>>();

    public ComputeBuffer Generate(ComputeBuffer pointsBuffer, int numPointsPerAxis, float boundsSize,
        Vector3 worldBounds, Vector3 centre, Vector3 offset, float spacing, int lod, bool isWater, float waterLevel = 0)
    {

        // if (_computedPoints.ContainsKey(centre))
        // {
        //     if (_computedPoints[centre].ContainsKey(lod))
        //     {
        //         pointsBuffer.SetData(_computedPoints[centre][lod]); 
        //         return pointsBuffer;
        //     }
        // }
        //
        _buffersToRelease = new List<ComputeBuffer> ();

        // Noise parameters
        var prng = new System.Random (seed);
        var offsets = new Vector3[numOctaves];
        float offsetRange = 1000;
        for (int i = 0; i < numOctaves; i++) {
            offsets[i] = new Vector3 ((float) prng.NextDouble () * 2 - 1, (float) prng.NextDouble () * 2 - 1, (float) prng.NextDouble () * 2 - 1) * offsetRange;
        }

        var offsetsBuffer = new ComputeBuffer (offsets.Length, sizeof (float) * 3);
        offsetsBuffer.SetData (offsets);
        _buffersToRelease.Add (offsetsBuffer);

        int kernelIndex = densityShader.FindKernel("Density");
        densityShader.SetVector ("centre", new Vector4 (centre.x, centre.y, centre.z));
        densityShader.SetInt ("octaves", Mathf.Max (1, numOctaves));
        densityShader.SetFloat ("lacunarity", lacunarity);
        densityShader.SetFloat ("persistence", persistence);
        densityShader.SetFloat ("noiseScale", noiseScale);
        densityShader.SetFloat ("noiseWeight", noiseWeight);
        densityShader.SetBool ("closeEdges", closeEdges);
        densityShader.SetBuffer (kernelIndex, "offsets", offsetsBuffer);
        densityShader.SetFloat ("floorOffset", floorOffset);
        densityShader.SetFloat ("weightMultiplier", weightMultiplier);
        densityShader.SetFloat ("hardFloor", hardFloorHeight);
        densityShader.SetFloat ("hardFloorWeight", hardFloorWeight);

        densityShader.SetVector ("params", shaderParams);
        int numThreadsPerAxis = Mathf.CeilToInt(numPointsPerAxis / (float) ThreadGroupSize);
        
        densityShader.SetBuffer(kernelIndex, "points", pointsBuffer);
        densityShader.SetInt("numPointsPerAxis", numPointsPerAxis);
        densityShader.SetFloat("boundsSize", boundsSize);
        densityShader.SetVector ("centre", new Vector4 (centre.x, centre.y, centre.z));
        densityShader.SetVector ("offset", new Vector4 (offset.x, offset.y, offset.z));
        densityShader.SetFloat ("spacing", spacing);
        densityShader.SetVector("worldSize", worldBounds);
        
        densityShader.SetBool("useWater", isWater);
        densityShader.SetFloat("waterLevel", waterLevel);
        
        densityShader.Dispatch(kernelIndex, numThreadsPerAxis, !isWater ? numThreadsPerAxis : 1, numThreadsPerAxis);
       // Vector4[] points = new Vector4[numPointsPerAxis * numPointsPerAxis * numPointsPerAxis];
        // pointsBuffer.GetData(points);
        // if (!_computedPoints.ContainsKey(centre))
        // {
        //     _computedPoints.Add(centre, new Dictionary<int, Vector4[]>());
        // }
        // _computedPoints[centre].Add(lod, points);

        if (_buffersToRelease == null) return pointsBuffer;
        foreach (ComputeBuffer buff in _buffersToRelease)
        {
            buff.Release();
        }

        return pointsBuffer;
    }
}
