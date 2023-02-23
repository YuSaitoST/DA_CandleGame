Shader "Unlit/WatersSurface_Noise"
{
	Properties
	{
		_UpperColor			("Upper Color"		, Color		) = (1,1,1,0)
		_UnderColor			("Under Color"		, Color		) = (1,1,1,1)
		_MainTex			("Albedo (RGB)"		, 2D		) = "white" {}
		_Glossiness			("Smoothness"		, Range(0,1)) = 0.5
		_Metallic			("Metallic"			, Range(0,1)) = 0.0
		_SeparationHeight	("Separation Height", Float		) = 0.0

		_NoiseTex("Noise", 2D) = "white" {}
		_Time("Time", Float) = 0.0
		_Speed("Speed", Range(0, 1)) = 0.7
		_NoiseAmount("NoiseAmount", Range(0, 0.05)) = 0.025
		_Brightness("Brightness", Range(1, 10)) = 5
		_Color("Color", Color) = (1, 1, 1, 1)
	}

	SubShader
	{
		Tags {
			"Queue"				= "AlphaTest"			// 描画の優先度（Background→Geometry→AlphaTest→Transparent→Overlay）
			"RenderPipeline"	= "UniversalPipeline"
			"RenderType"		= "TransparentCutout"
			"IgnoreProjector"	= "True"				// 波に使う
		}
		LOD 200

		// 両面を描画したい場合の記述
		Cull Off

		Pass
		{
			Name "ShadowCaster"
			Tags { 
				"LightMode" = "ShadowCaster" 
			}
			ZWrite On	// 裏側の描画(デプスバッファ書き込みモード)

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			struct appdata
			{
				float4 vertex	: POSITION;
				float2 uv		: TEXCOORD0;
			};

			struct v2f
			{
				float4 pos		: SV_POSITION;
				float2 uv		: TEXCOORD0;
				float3 worldPos : TEXCOORD1;
			};

			sampler3D _DitherMaskLOD;
			sampler2D _MainTex;
			half4 _UpperColor;
			half4 _UnderColor;
			float _SeparationHeight;

			half4 _Color;

			float _Speed;
			float4 _MainTex_ST;
			sampler2D _NoiseTex;
			float _NoiseAmount;
			float _Brightness;

			v2f vert(appdata v)
			{
				v2f o;
				o.pos		= TransformObjectToHClip(v.vertex.xyz);	// 3D空間座標を2D画面情報に変換
				o.uv		= TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos	= mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			// 影の描画
			half4 frag(v2f i) : SV_Target
			{
				// 波処理
				float2 nUv = i.uv;
				nUv.y += _Time.x * _Speed;
				float4 uvNoise = 2 * tex2D(_NoiseTex, nUv) - 1;	//0 - 1座標を-1 - 1に変換
				i.uv += uvNoise.xy * _NoiseAmount;

				// メイン描画部分と同様に透明度を求め...
				// 後ろの封印解いて_UnderColorを消すと、裏の影が消える
				float alpha = tex2D(_MainTex, i.uv).a * _UnderColor;

				// 透明度に応じて_DitherMaskLODから網掛け模様をサンプリングし...
				/*
				* 0.25		: 荒の細かさ
				* 0.9375	: アルファ
				*/
				float alphaRef = tex3D(_DitherMaskLOD, float3(i.pos.xy * 0.25, alpha * 0.9375)).a;

				// シャドウマップを網掛け模様にすることで透明度に応じた影を作らせる
				clip(alphaRef - 0.01);

				// ShadowCasterパスではデプスがどうなるかが重要で、返す色はなんでもいい
				return 0.0;
			}
			ENDHLSL
		}
	}
		Fallback "Transparent/Diffuse" // 透明シェーダー
		//Fallback "Transpaent/Cutout/Diffuse" // アルファテストシェーダー
}
