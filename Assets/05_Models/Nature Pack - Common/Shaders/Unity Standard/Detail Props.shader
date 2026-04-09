// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ANGRYMESH/Nature Pack/Standard/Detail Props"
{
	Properties
	{
		[Header(Base)]_BaseSmoothness("Base Smoothness", Range( 0 , 1)) = 0.5
		_BaseAOIntensity("Base AO Intensity", Range( 0 , 1)) = 0.5
		_BumpScale("Base Normal Intensity", Range( 0 , 2)) = 1
		_BaseColor("Base Color", Color) = (1,1,1,0)
		[NoScaleOffset]_BaseAlbedoASmoothness("Base Albedo (A Smoothness)", 2D) = "gray" {}
		[Normal][NoScaleOffset]_BaseNormalMap("Base NormalMap", 2D) = "bump" {}
		[NoScaleOffset]_BaseAOANoiseMask("Base AO (A NoiseMask)", 2D) = "white" {}
		[Header(Detail)]_DetailUVScale("Detail UV Scale", Range( 0 , 40)) = 10
		_DetailAlbedoIntensity("Detail Albedo Intensity", Range( 0 , 1)) = 1
		_DetailNormalMapIntensity("Detail NormalMap Intensity", Range( 0 , 2)) = 1
		[NoScaleOffset]_DetailAlbedo("Detail Albedo", 2D) = "gray" {}
		[Normal][NoScaleOffset]_DetailNormalMap("Detail NormalMap", 2D) = "bump" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows dithercrossfade 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _DetailNormalMap;
		uniform half _DetailUVScale;
		uniform half _DetailNormalMapIntensity;
		uniform sampler2D _BaseNormalMap;
		uniform half _BumpScale;
		uniform half4 _BaseColor;
		uniform sampler2D _BaseAlbedoASmoothness;
		uniform sampler2D _DetailAlbedo;
		uniform half _DetailAlbedoIntensity;
		uniform half _BaseSmoothness;
		uniform sampler2D _BaseAOANoiseMask;
		uniform half _BaseAOIntensity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			half2 temp_output_182_0 = ( i.uv_texcoord * _DetailUVScale );
			half2 Detail_UVScale191 = temp_output_182_0;
			float2 uv_BaseNormalMap96 = i.uv_texcoord;
			half3 normalizeResult136 = normalize( BlendNormals( UnpackScaleNormal( tex2D( _DetailNormalMap, Detail_UVScale191 ), _DetailNormalMapIntensity ) , UnpackScaleNormal( tex2D( _BaseNormalMap, uv_BaseNormalMap96 ), _BumpScale ) ) );
			half3 Output_Normal320 = normalizeResult136;
			o.Normal = Output_Normal320;
			float2 uv_BaseAlbedoASmoothness162 = i.uv_texcoord;
			half4 tex2DNode162 = tex2D( _BaseAlbedoASmoothness, uv_BaseAlbedoASmoothness162 );
			half4 temp_output_163_0 = ( _BaseColor * tex2DNode162 );
			half4 blendOpSrc178 = ( tex2D( _DetailAlbedo, temp_output_182_0 ) * 2.0 );
			half4 blendOpDest178 = temp_output_163_0;
			half4 lerpResult187 = lerp( temp_output_163_0 , ( saturate( (( blendOpDest178 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest178 ) * ( 1.0 - blendOpSrc178 ) ) : ( 2.0 * blendOpDest178 * blendOpSrc178 ) ) )) , _DetailAlbedoIntensity);
			half4 Output_Albedo318 = lerpResult187;
			o.Albedo = Output_Albedo318.rgb;
			float AlbedoAlphaSmoothness212 = tex2DNode162.a;
			half Output_Smoothness223 = ( AlbedoAlphaSmoothness212 + (-1.0 + (_BaseSmoothness - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) );
			o.Smoothness = Output_Smoothness223;
			half4 temp_cast_1 = (1.0).xxxx;
			float2 uv_BaseAOANoiseMask200 = i.uv_texcoord;
			half4 lerpResult201 = lerp( temp_cast_1 , tex2D( _BaseAOANoiseMask, uv_BaseAOANoiseMask200 ) , _BaseAOIntensity);
			half4 Output_AO322 = lerpResult201;
			o.Occlusion = Output_AO322.r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
}
