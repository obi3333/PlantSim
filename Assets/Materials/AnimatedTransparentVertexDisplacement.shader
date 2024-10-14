Shader "Custom/AnimatedTransparentVertexDisplacement"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DisplacementScale ("Displacement Scale", Range(0, 1)) = 0.1
        _DisplacementFrequency ("Displacement Frequency", Range(0, 10)) = 1.0
        _Color ("Color", Color) = (1,1,1,1)
        _TimeScale ("Time Scale", Range(0, 1)) = 0.000000001
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        CGPROGRAM
        #pragma surface surf Lambert vertex:vert alpha:fade

        sampler2D _MainTex;
        float _DisplacementScale;
        float _DisplacementFrequency;
        fixed4 _Color;
        float _TimeScale;

        struct Input
        {
            float2 uv_MainTex;
        };

        void vert (inout appdata_full v)
        {
            float time = _Time.y * _TimeScale;

            float3 noise = float3(
                frac(sin(dot(v.vertex.xyz + time, float3(12.9898, 78.233, 37.719))) * 43758.5453),
                frac(sin(dot(v.vertex.xyz + time, float3(26.719, 33.379, 48.483))) * 21382.3247),
                frac(sin(dot(v.vertex.xyz + time, float3(19.823, 64.292, 27.172))) * 18732.9817)
            );

            noise = (noise - 0.5) * 2.0;
            v.vertex.xyz += noise * _DisplacementScale;
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb * _Color.rgb;
            o.Alpha = c.a * _Color.a;
        }
        ENDCG
    }
    FallBack "Transparent/Cutout/Diffuse"
}
