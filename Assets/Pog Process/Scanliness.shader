Shader "Hidden/PostProcess/Scanline"
{
    HLSLINCLUDE
    #include "Packages/com.yetman.render-pipelines.universal.postprocess/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

    TEXTURE2D_X(_MainTex);

    int _Rows;
	int _Lines;

    float4 GrayscaleFragmentProgram (PostProcessVaryings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        float2 uv = UnityStereoTransformScreenSpaceTex(input.texcoord);
        float4 color = LOAD_TEXTURE2D_X(_MainTex, uv * _ScreenSize.xy);
        float4 ogColor = LOAD_TEXTURE2D_X(_MainTex, uv * _ScreenSize.xy);

		float rows = _Rows;
		float lines = _Lines;
		float otherChannelCoefficient = 0.33;
		float curScanning = lines - fmod(floor(_Time.y*lines/5), lines);
		
		color = LOAD_TEXTURE2D_X(_MainTex, float2(uv.x, floor(uv.y*lines)/lines) * _ScreenSize.xy);

		if(fmod(floor(uv.x*rows), 3) == 0) {
			color = float4(color.r * otherChannelCoefficient, color.g * otherChannelCoefficient, color.b, color.a);
		}
		if(fmod(floor(uv.x*rows+1), 3) == 0) {
			color = float4(color.r, color.g * otherChannelCoefficient, color.b * otherChannelCoefficient, color.a);
		}
		if(fmod(floor(uv.x*rows+2), 3) == 0) {
			color = float4(color.r * otherChannelCoefficient, color.g, color.b * otherChannelCoefficient, color.a);
		}

		//color = float4(floor(uv.y*lines)/lines, floor(uv.y*lines)/lines, floor(uv.y*lines)/lines, 1);

		int EPSILON = 2; // I fucking hate this stupid shit it should not be this broken
		if(floor(uv.y*lines) > curScanning - EPSILON && floor(uv.y*lines) < curScanning + EPSILON) {
			color *= 4;
		}


        return ogColor * 0.75f + color * 0.25f;
    }
    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            HLSLPROGRAM
            #pragma vertex FullScreenTrianglePostProcessVertexProgram
            #pragma fragment GrayscaleFragmentProgram
            ENDHLSL
        }
    }
    Fallback Off
}