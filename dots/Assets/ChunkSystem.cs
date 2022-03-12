using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

public class ChunkSystem : SystemBase
{
    private EntityManager _entityManager;

    public Vector3Int ChunkSize = Vector3Int.one * 5;


    private Transform _player;
    private Generator _meshGenerator;

    private EndSimulationEntityCommandBufferSystem _commandBufferSystem;

    protected override void OnCreate()
    {
        _commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        _meshGenerator = GameObject.Find("ChunkManager").GetComponent<Generator>();
        _player = GameObject.Find("Plane").GetComponent<Transform>();
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
        NativeArray<Vector3> required =
            new NativeArray<Vector3>(ChunkSize.x * ChunkSize.y * ChunkSize.z, Allocator.Temp);
        AssertChunks(ref required);

        foreach (Vector3 coord in required)
        {
            ChunkManager.CreateEntity(_entityManager, new Vector3Int(Mathf.FloorToInt(coord.x), Mathf.FloorToInt(coord.y),Mathf.FloorToInt( coord.z)));
        }

        required.Dispose();
    }

    protected override void OnUpdate()
    {
        Vector3Int chunkSize = new Vector3Int(ChunkSize.x, ChunkSize.y, ChunkSize.z);
        NativeArray<Vector3> required =
            new NativeArray<Vector3>(chunkSize.x * chunkSize.y * chunkSize.z, Allocator.TempJob);
        AssertChunks(ref required);
        Entities.ForEach((ref ChunkData data) =>
        {
            for (int i = 0; i < required.Length; i++)
            {
                if (required[i] == data.coords)
                {
                    required[i] = Vector3.negativeInfinity;
                    return;
                }
            }

            Vector3 last = Vector3.zero;
            for (int i = 0; i < required.Length; i++)
            {
                if (required[i] != Vector3.negativeInfinity)
                {
                    last = required[i];
                    required[i] = Vector3.negativeInfinity;
                    break;
                }
            }
            data.coords = new Vector3Int(Mathf.FloorToInt(last.x), Mathf.FloorToInt(last.y), Mathf.FloorToInt(last.z));
            data.updated = true;
        }).Run();
        required.Dispose();

        EntityCommandBuffer buffer = _commandBufferSystem.CreateCommandBuffer();
        Entities.ForEach((Entity entity, ref ChunkData data, ref RenderBounds bounds) =>
        {
            if (!data.updated) return;
            Generator.Instance.GenerateMesh(ref data.meshData, ref data.coords, new Vector3Int(chunkSize.x, chunkSize.y, chunkSize.z));
            data.updated = false;
            Debug.Log(data.meshData.vertices.Length);
        }).Schedule();
    }

    private void AssertChunks(ref NativeArray<Vector3> required)
    {
        var position = _player.position;
        Vector3Int playerPos = new Vector3Int(Mathf.FloorToInt(position.x / _meshGenerator.boundsSize),
            0,
            Mathf.FloorToInt(position.z / _meshGenerator.boundsSize));
        int i = 0;
        for (int x = -ChunkSize.x / 2; x <= ChunkSize.x / 2; x++)
        {
            for (int y = -ChunkSize.y / 2; y <= ChunkSize.y / 2; y++)
            {
                for (int z = -ChunkSize.z / 2; z <= ChunkSize.z / 2; z++)
                {
                    Vector3 coord = new Vector3(x + playerPos.x, y + playerPos.y, z + playerPos.z);
                    required[i] = coord;
                }
            }
        }
    }
}