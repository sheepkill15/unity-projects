using UnityEngine;

public class Shape : MonoBehaviour
{
    private Transform _transform;

    public Transform ShapeTransform => _transform ??= transform;

    public uint shapeType = 1;
    public Color shapeColor = Color.white;
    
    public struct ShapeInfo
    {
        public uint Type;
        public Vector3 Center;
        public Vector3 Size;
        public Vector4 Color;

        public const int Stride = sizeof(uint) + sizeof(float) * 6 + sizeof(float) * 4;
    }

    public ShapeInfo GetInfo() => new ShapeInfo
        {Type = shapeType, Center = ShapeTransform.position, Size = ShapeTransform.localScale / 2f, Color = shapeColor};
}
