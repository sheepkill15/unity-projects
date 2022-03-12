Shader "Custom/ColorShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _SteepnessScale("Steepness scale", Range(0, 5)) = 1
        
        _GrassHeight("Grass height", float) = 0.0
        _GrassElevation("Max grass elevation", float) = 0.0
        _GrassColor("Grass color", Color) = (0,1,0,1)
        
        _MountainHeight("Mountain height", float) = 0.0
        _MountainElevation("Max Mountain elevation", float) = 0.0
        _MountainColor("Mountain color", Color) = (1,1,0,1)
        
        _SnowHeight("Snow height", float) = 0.0
        _SnowElevation("Max Snow elevation", float) = 0.0
        _SnowColor("Snow color", Color) = (1,1,1,1)
        
        _SmoothingFactor("Smoothing factor", float) = 1
        
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 normal;
            float3 vertPos;
        };

        half _Glossiness;
        half _Metallic;
        half _SteepnessScale;
        fixed4 _Color;

        float _GrassHeight;
        float _GrassElevation;
        fixed4 _GrassColor;
        float _MountainHeight;
        float _MountainElevation;
        fixed4 _MountainColor;
        float _SnowHeight;
        float _SnowElevation;
        fixed4 _SnowColor;

        float _SmoothingFactor;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
        
        void vert (inout appdata_full v, out Input o) {
          UNITY_INITIALIZE_OUTPUT(Input,o);
            o.normal = v.normal;
            o.vertPos = v.vertex;
        }
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            float steepness = dot(IN.normal, float3(0, IN.normal.y, 0));
            steepness = saturate(steepness / _SteepnessScale);
            float3 color = lerp(_GrassColor, _MountainColor, steepness * steepness);
            
            o.Albedo = color;
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
