using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public Material material;

    public static ChunkManager Instance;
    
    private void Awake()
    {
        Instance = this;
        
        // CreateEntity(_entityManager, Vector3Int.zero);
        // _entityManager.AddComponentData(entity, new MeshFilter {mesh = new Mesh()});
    }

    public static void CreateEntity(EntityManager entityManager, Vector3Int chunk)
    {
        EntityArchetype archetype = entityManager.CreateArchetype(typeof(ChunkData), 
            typeof(RenderMesh),
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderBounds),
            typeof(LocalToWorld));
        
        Entity entity = entityManager.CreateEntity(archetype);

        entityManager.AddComponentData(entity, new ChunkData {coords = chunk, updated = true});
    }
}
