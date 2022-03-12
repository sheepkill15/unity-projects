// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Raymarcher"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _Steps("Steps", int) = 10
        _Center("Center of sphere", Vector) = (0,0,0)
        _Radius("Radius of sphere", float) = 5

        _MinDistance("Minimum distance", float) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION; // Clip space
                float3 wPos : TEXCOORD1; // World position
            };

            // Vertex function
            v2f vert(appdata_full v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.wPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }
            int _Steps;
            float3 _Center;
            float _Radius;
            float _MinDistance;
            fixed4 _Color;

            float sphereDistance(float3 p)
            {
                return distance(p, _Center) - _Radius;
            }

            float map(float3 p)
            {
                return sphereDistance(p);
            }

            #include "Lighting.cginc"
            fixed4 simpleLambert(fixed3 normal)
            {
                fixed3 lightDir = _WorldSpaceLightPos0.xyz; // Light direction
                fixed3 lightCol = _LightColor0.rgb; // Light color
                fixed NdotL = max(dot(normal, lightDir), 0);
                fixed4 c;
                c.rgb = _Color * lightCol * NdotL;
                c.a = 1;
                return c;
            }

            float3 normal(float3 p)
            {
                const float eps = 0.01;
                return normalize
                (float3
                    (map(p + float3(eps, 0, 0)) - map(p - float3(eps, 0, 0)),
                     map(p + float3(0, eps, 0)) - map(p - float3(0, eps, 0)),
                     map(p + float3(0, 0, eps)) - map(p - float3(0, 0, eps))
                    )
                );
            }



            fixed4 renderSurface(float3 p)
            {
                float3 n = normal(p);
                return simpleLambert(n);
            }

            fixed4 raymarch(float3 position, float3 direction)
            {
                for (int i = 0; i < _Steps; i++)
                {
                    float dist = sphereDistance(position);
                    if (dist < _MinDistance)
                    {
                        return renderSurface(position);
                    }
                    position += direction * dist;
                }
                return fixed4(1,1,1,1);
            }

            // Fragment function
            fixed4 frag(v2f i) : SV_Target
            {
                float3 worldPosition = i.wPos;
                float3 viewDirection = normalize(i.wPos - _WorldSpaceCameraPos);
                return raymarch(worldPosition, viewDirection);
            }
            ENDCG
        }
    }
}