Shader "Custom/GrassToon"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Scale ("Scale", float) = 1.0
        _Angle ("Angle", Range(0,90)) = 85
    }

    SubShader
    {
        Tags{ "RenderType"="Opaque" }
        LOD 200
        Cull Off
        
        CGPROGRAM
        #pragma surface surf Standard vertex:vert addshadow fullforwardshadows
        #pragma instancing_options procedural:setup

        sampler2D _MainTex;
        
        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
            float3 worldNormal;
            INTERNAL_DATA
        };

        fixed4 _Color;
        
        float _Scale;           
        float _Angle;    
        float _Trample;         
        float3 _Position;  

        #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
        struct GrassClump
        {
            float3 position;
            float trample;
            float noise;
        };
        StructuredBuffer<GrassClump> clumpsBuffer; 
        #endif

        void setup()
        {
            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
                GrassClump clump = clumpsBuffer[unity_InstanceID];
                _Trample = clump.trample;
                _Position = clump.position;
            #endif
        }

        void vert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);

            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED

                v.vertex.xyz *= _Scale;

                if (_Trample > 0.0)
                {
                    float angleRad = radians(_Angle);
                    float c = cos(angleRad);
                    float s = sin(angleRad);

                    float3 original = v.vertex.xyz;
                    float rotatedY = original.y * c - original.z * s;
                    float rotatedZ = original.y * s + original.z * c;
                    v.vertex.y = rotatedY;
                    v.vertex.z = rotatedZ;
                }
                v.vertex.xyz += _Position; 
            #endif
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            fixed3 normal = normalize(IN.worldNormal);                   
            fixed3 lightDir = normalize(UnityWorldSpaceLightDir(IN.worldPos)); 

            float NdotL = saturate(dot(normal, lightDir)); 
            float toonFloat = step(0.5, NdotL) * 0.5 + step(0.8, NdotL) * 0.5;

            o.Albedo = c.rgb * toonFloat;
            o.Alpha = c.a;
            clip(c.a - 0.4);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
