using UnityEngine;

[ExecuteInEditMode]
public class RaymarchManager : MonoBehaviour
{
    public ComputeShader raymarchShader;
    private RenderTexture _target;

    private Camera _camera;
    private Camera MainCamera => _camera ??= GetComponent<Camera>();

    private Shape[] _shapes;
    private Shape[] AllShapes => _shapes ??= FindObjectsOfType<Shape>();

    public Transform worldLightTransform;
    public Light worldLight;
    
    [Header("Raymarching settings")]
    public int steps = 10;

    public float maxDistance = 80;
    public float minDistance = 1;
    public float specularPower = 1;
    public float specularGloss = 1;
    public float blendingFactor = 32;
    public float mandelPower = 1;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        InitRenderTexture();
        int kernelHandle = raymarchShader.FindKernel("CSMain");
        raymarchShader.SetTexture(kernelHandle, "Source", src);
        raymarchShader.SetTexture(kernelHandle, "Result", _target);
        int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
        SetShaderParameters();
        
        // Set shape buffer
        Shape.ShapeInfo[] shapeInfos = new Shape.ShapeInfo[AllShapes.Length];
        for(int i = 0; i < AllShapes.Length; i++)
        {
            shapeInfos[i] = AllShapes[i].GetInfo();
        }

        ComputeBuffer shapeBuffer = new ComputeBuffer(Mathf.Max(AllShapes.Length, 1), Shape.ShapeInfo.Stride);
        shapeBuffer.SetData(shapeInfos);
        
        raymarchShader.SetBuffer(kernelHandle, "Shapes", shapeBuffer);
        raymarchShader.SetInt("ShapeCount", AllShapes.Length);
        
        raymarchShader.Dispatch(kernelHandle, threadGroupsX, threadGroupsY, 1);
        shapeBuffer.Dispose();
        Graphics.Blit(_target, dest);
    }
    
    private void InitRenderTexture()
    {
        if (_target == null || _target.width != Screen.width || _target.height != Screen.height)
        {
            // Release render texture if we already have one
            if (_target != null)
                _target.Release();

            // Get a render target for Ray Tracing
            _target = new RenderTexture(Screen.width, Screen.height, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear) {enableRandomWrite = true};
            _target.Create();
        }
    }

    private void SetShaderParameters()
    {
        raymarchShader.SetVector("_WorldSpaceLightPos0", -worldLightTransform.forward);
        raymarchShader.SetVector("_LightColor0", worldLight.color);
        raymarchShader.SetFloat("_SpecularPower", specularPower);
        raymarchShader.SetFloat("_Gloss", specularGloss);
        raymarchShader.SetFloat("_MandelPower", mandelPower);
        raymarchShader.SetInt("_Steps", steps);
        raymarchShader.SetFloat("_BlendK", blendingFactor);
        raymarchShader.SetFloat("_MinDistance", minDistance);
        raymarchShader.SetFloat("_MaxDistance", maxDistance);
        raymarchShader.SetMatrix("_CameraToWorld", MainCamera.cameraToWorldMatrix);
        raymarchShader.SetMatrix("_CameraInverseProjection", MainCamera.projectionMatrix.inverse);
    }
}
