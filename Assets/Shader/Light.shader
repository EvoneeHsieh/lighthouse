Shader "Custom/Light"
{
    Properties
    {
        _MainColor("MainColor", Color) = (0,0,0,1)
        _InSideRimColor("InSideRimColor", Color) = (1,1,1,1)
        _InSideRimPower("InSideRimPower", Range(0.0,5)) = 0 
        _InSideRimIntensity("InSideRimIntensity", Range(0.0, 10)) = 0  
 
        _OutSideRimColor("OutSideRimColor", Color) = (1,1,1,1)
        _OutSideRimSize("OutSideRimSize", Float) = 0 
        _OutSideRimPower("OutSideRimPower", Range(0.0,5)) = 0 
        _OutSideRimIntensity("OutSideRimIntensity", Range(0.0, 10)) = 0   
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
 
            uniform float4 _MainColor;
            uniform float4 _InSideRimColor;
            uniform float  _InSideRimPower;
            uniform float _InSideRimIntensity;
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
 
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float4 vertexWorld : TEXCOORD2;
 
            };
            v2f vert(appdata v)
            {
                v2f o;
                o.normal = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertexWorld = mul(unity_ObjectToWorld, v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            fixed4 frag(v2f i) : SV_Target
            {
                i.normal = normalize(i.normal);
                float3 worldViewDir = normalize(_WorldSpaceCameraPos.xyz - i.vertexWorld.xyz);
                half NdotV = max(0, dot(i.normal, worldViewDir));
                NdotV = 1.0 - NdotV;
                float fresnel = pow(NdotV,_InSideRimPower) * _InSideRimIntensity;
                float3  Emissive = _InSideRimColor.rgb * fresnel; 
                return _MainColor + float4(Emissive,1);
            }
            ENDCG
        }
 
        Pass
        {
            Cull Front
            Blend SrcAlpha One
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
 
             uniform float4 _OutSideRimColor;
             uniform float  _OutSideRimSize;
             uniform float  _OutSideRimPower;
             uniform float  _OutSideRimIntensity;
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
 
                float4 tangent : TANGENT;
 
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float4 vertexWorld : TEXCOORD2;
 
            };
            v2f vert(appdata v)
            {
                v2f o;
                o.normal = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                v.vertex.xyz += v.normal * _OutSideRimSize;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertexWorld = mul(unity_ObjectToWorld, v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            fixed4 frag(v2f i) : SV_Target
            {
                i.normal = normalize(i.normal);
                float3 worldViewDir = normalize(i.vertexWorld.xyz - _WorldSpaceCameraPos.xyz);
                half NdotV = dot(i.normal, worldViewDir);
                float fresnel = pow(saturate(NdotV),_OutSideRimPower) * _OutSideRimIntensity;
                return float4(_OutSideRimColor.rgb,fresnel);
            }
            ENDCG
        }
    }
}
