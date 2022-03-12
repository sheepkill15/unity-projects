using UnityEngine;

public class Chunk : MonoBehaviour
{
    public MeshFilter chunkFilter;

    public MeshFilter waterFilter;

    public bool updated;

    public int lod;
    
    public virtual void Init()
    {
        chunkFilter.mesh = new Mesh();
        waterFilter.mesh = new Mesh();
    }

    public virtual void UpdateMesh(Vector3[] vertices, int[] meshTriangles, Vector3[] meshNormals, bool water)
    {
        Mesh mesh = water ? waterFilter.sharedMesh : chunkFilter.sharedMesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = meshTriangles;
        mesh.normals = meshNormals;
    }
}
