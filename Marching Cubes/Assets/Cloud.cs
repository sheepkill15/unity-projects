using UnityEngine;

public class Cloud : Chunk
{
    public override void Init()
    {
        chunkFilter.mesh = new Mesh();
    }

    public override void UpdateMesh(Vector3[] vertices, int[] meshTriangles, Vector3[] meshNormals, bool water)
    {
        if (!water)
        {
            base.UpdateMesh(vertices, meshTriangles,meshNormals, false);
        }
    }
}
