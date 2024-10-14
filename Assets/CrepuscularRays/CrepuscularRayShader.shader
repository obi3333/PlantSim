Shader "Custom/CrepuscularRays"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _LightPos("Light Position", Vector) = (0, 0, 0)
        _NumSamples("Number of Samples", Range(0, 1024)) = 128
        _Density("Density", Range(0, 1)) = 1.0
        _Weight("Weight", Range(0, 1)) = 1.0
        _Decay("Decay", Range(0, 1)) = 1.0
        _Exposure("Exposure", Range(0, 1)) = 1.0
        _RayColor("Ray Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float4 _LightPos;
            float _NumSamples;
            float _Density;
            float _Weight;
            float _Decay;
            float _Exposure;
            float4 _RayColor;

            half4 frag(v2f_img i) : SV_Target
            {
                // Calculate vector from pixel to light source in screen space.
                float2 deltaTexCoord = (i.uv - _LightPos.xy);
                // Divide by number of samples and scale by control factor.
                deltaTexCoord *= _Density / _NumSamples;
                // Store initial sample.
                float2 uv = i.uv;
                half3 color = tex2D(_MainTex, uv).rgb;
                // Set up illumination decay factor.
                half illuminationDecay = 1.0;
                // Evaluate summation from Equation 3 NUM_SAMPLES iterations.
                for (int j = 0; j < _NumSamples; j++)
                {
                    // Step sample location along ray.
                    uv -= deltaTexCoord;
                    // Retrieve sample at new location.
                    half3 sample = tex2D(_MainTex, uv).rgb;
                    // Apply sample attenuation scale/decay factors.
                    sample *= illuminationDecay * (_Weight / _NumSamples);
                    // Accumulate combined color.
                    color += sample;
                    // Update exponential decay factor.
                    illuminationDecay *= _Decay;
                }
                // Tint the rays with the specified color and output final color with a further scale control factor.
                return half4(color * _Exposure * _RayColor.rgb, 1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
