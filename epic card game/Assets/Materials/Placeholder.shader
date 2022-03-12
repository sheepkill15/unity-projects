Shader "Unlit/Placeholder"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _WarpScale ("Warp scale", float) = 1
        _InversePosScale ("Divide position with", float) = 1
        _Unique ("Unique", int) = 0
        _Zoom ("Zoom factor", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "QUEUE"="Transparent" "RenderType"="Transparent" "CanUseSpriteAtlas"="true" }
        LOD 100
        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 world : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _WarpScale;
            float _InversePosScale;
            int _Unique;
            float _Zoom;

            const float PHI = 1.61803398874989484820459;  // Φ = Golden Ratio   
            
            float gold_noise(in float2 xy, in float seed){
                   return frac(tan(distance(xy*PHI, xy)*seed)*xy.x);
            }
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.world = mul(unity_ObjectToWorld, v.vertex).xy;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 tex_coords = i.uv;
                int offset = floor(i.world.x / _InversePosScale);
                if(_Unique)
                {
                    offset = 10;
                }
                else if(offset == 0)
                {
                    offset = 100;
                }
                const float nudge = gold_noise(i.vertex.xy, offset);
                tex_coords.x -= sin(tex_coords.y * 10 + _Time[2] + nudge + offset) * 0.1 * (0.5 - abs(tex_coords.y - 0.5)) * _WarpScale;
                tex_coords.y -= sin(i.uv.x * 10 + _Time[2] + nudge + offset) * 0.1 * (0.5 - abs(i.uv.x - 0.5)) * _WarpScale;
                // sample the texture
                fixed4 col = tex2D(_MainTex, tex_coords * _Zoom);
                col.a = clamp(0.5, 1, col.r);
                
                return col * _Color;
            }
            ENDCG
        }
    }
}
