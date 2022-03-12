using UnityEngine;

[ExecuteInEditMode]
public class AddEffects : MonoBehaviour
{
    private Transform _transform;
    private Camera _camera;
    public Material effectMat;
    public float waterLevel;

    public Transform sun;
    
    private void Start()
    {
        _transform = transform;
        _camera = GetComponent<Camera>();
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        // effectMat.SetVector("cameraPos", _transform.position);
        // effectMat.SetVector("cameraView", _transform.forward);
        // effectMat.SetFloat("waterLevel", waterLevel);
        
        //camera must at least be in depth mode
        _camera.depthTextureMode = DepthTextureMode.DepthNormals;
        // Main eye inverse view matrix
        Matrix4x4 left_world_from_view = _camera.cameraToWorldMatrix;

        // Inverse projection matrices, plumbed through GetGPUProjectionMatrix to compensate for render texture
        Matrix4x4 screen_from_view = _camera.projectionMatrix;
        Matrix4x4 left_view_from_screen = GL.GetGPUProjectionMatrix(screen_from_view, true).inverse;

        // Negate [1,1] to reflect Unity's CBuffer state
        left_view_from_screen[1, 1] *= -1;

        // Store matrices
        effectMat.SetMatrix("_LeftWorldFromView", left_world_from_view);
        effectMat.SetMatrix("_LeftViewFromScreen", left_view_from_screen);
        
        effectMat.SetVector("dirToSun", sun.position);
        effectMat.SetVector("viewDir", _transform.forward);
        Graphics.Blit(src, dest, effectMat);
    }
}
