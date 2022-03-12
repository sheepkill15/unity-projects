Shader "Custom/WaterShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _WaveAmplitude("Wave amplitude", Range(-1, 1)) = 0
        _Wavelength("Wavelength", Range(-1.57, 1.57)) = 0
        _WaveSpeed("Wave speed", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alpha vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
            float4 screenPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        sampler2D _CameraDepthTexture;
        float4 _CameraDepthTexture_TexelSize;

        float _WaveAmplitude;
        float _Wavelength;
        float _WaveSpeed;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        float ColorBelowWater(float4 screenPos)
        {
            float2 uv = screenPos.xy / screenPos.w;
            #if UNITY_UV_STARTS_AT_TOP

            if(_CameraDepthTexture_TexelSize.y < 0)
            {
                uv.y = 1 - uv.y;
            }

            #endif
            float backgroundDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv));
            float surfaceDepth = UNITY_Z_0_FAR_FROM_CLIPSPACE(screenPos.z);

            float depthDifference = backgroundDepth - surfaceDepth;
            return depthDifference;
        }

        
        void vert (inout appdata_full v, out Input o) {

            float3 p = v.vertex.xyz;
            float k = 2 * UNITY_PI / _Wavelength;
            float f = k * (p.x - _WaveSpeed * _Time.y);
            p.x += _WaveAmplitude * cos(f);
            p.y += _WaveAmplitude * sin(f);

            float3 tangent = normalize(float3(1 - k * _WaveAmplitude * sin(f), k * _WaveAmplitude * cos(f), 0));
            float3 normal = float3(-tangent.y, tangent.x, 0);
            
            v.vertex.xyz = p;
            v.normal = normal;
            
            UNITY_INITIALIZE_OUTPUT(Input,o);
            o.screenPos = UnityWorldToClipPos(v.vertex);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Albedo = _Color;
            o.Alpha = saturate(ColorBelowWater(IN.screenPos) * _Color.a);
        }


        ENDCG
    }
    FallBack "Diffuse"
}
