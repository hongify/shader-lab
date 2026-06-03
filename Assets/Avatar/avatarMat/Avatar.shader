Shader "Custom/Avatar"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf ToonLighting fullforwardshadows

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;

        half4 LightingToonLighting(SurfaceOutput s, half3 lightDir, half atten)
        {
            half NdotL = dot(s.Normal, lightDir);  
            NdotL = saturate(NdotL);              
            half toonShade = lerp(0.3, 0.7, NdotL); 
            return half4(s.Albedo * toonShade, s.Alpha) * atten;
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = albedo.rgb;  
            o.Alpha = albedo.a;   
        }
        ENDCG
    }
    FallBack "Diffuse"
}