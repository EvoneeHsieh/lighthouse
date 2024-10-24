Shader "Custom/GlowShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _EmissionColor ("Emission Color", Color) = (0,0,0)
        _EmissionStrength ("Emission Strength", Range(0, 10)) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100
        
        Pass
        {
            // Vertex Shader
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            fixed4 _Color;
            fixed4 _EmissionColor;
            float _EmissionStrength;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 基本顏色
                fixed4 col = i.color * _Color;

                // 添加發光顏色
                col += _EmissionColor * _EmissionStrength;

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}