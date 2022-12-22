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
			"Queue"				= "AlphaTest"			// �`��̗D��x�iBackground��Geometry��AlphaTest��Transparent��Overlay�j
			"RenderPipeline"	= "UniversalPipeline"
			"RenderType"		= "TransparentCutout"
			"IgnoreProjector"	= "True"				// �g�Ɏg��
		}
		LOD 200

		// ���ʂ�`�悵�����ꍇ�̋L�q
		Cull Off

		Pass
		{
			Name "ShadowCaster"
			Tags { 
				"LightMode" = "ShadowCaster" 
			}
			ZWrite On	// �����̕`��(�f�v�X�o�b�t�@�������݃��[�h)

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
				o.pos		= TransformObjectToHClip(v.vertex.xyz);	// 3D��ԍ��W��2D��ʏ��ɕϊ�
				o.uv		= TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos	= mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			// �e�̕`��
			half4 frag(v2f i) : SV_Target
			{
				// �g����
				float2 nUv = i.uv;
				nUv.y += _Time.x * _Speed;
				float4 uvNoise = 2 * tex2D(_NoiseTex, nUv) - 1;	//0 - 1���W��-1 - 1�ɕϊ�
				i.uv += uvNoise.xy * _NoiseAmount;

				// ���C���`�敔���Ɠ��l�ɓ����x������...
				// ���̕��������_UnderColor�������ƁA���̉e��������
				float alpha = tex2D(_MainTex, i.uv).a * _UnderColor;

				// �����x�ɉ�����_DitherMaskLOD����Ԋ|���͗l���T���v�����O��...
				/*
				* 0.25		: �r�ׂ̍���
				* 0.9375	: �A���t�@
				*/
				float alphaRef = tex3D(_DitherMaskLOD, float3(i.pos.xy * 0.25, alpha * 0.9375)).a;

				// �V���h�E�}�b�v��Ԋ|���͗l�ɂ��邱�Ƃœ����x�ɉ������e����点��
				clip(alphaRef - 0.01);

				// ShadowCaster�p�X�ł̓f�v�X���ǂ��Ȃ邩���d�v�ŁA�Ԃ��F�͂Ȃ�ł�����
				return 0.0;
			}
			ENDHLSL
		}
	}
		Fallback "Transparent/Diffuse" // �����V�F�[�_�[
		//Fallback "Transpaent/Cutout/Diffuse" // �A���t�@�e�X�g�V�F�[�_�[
}
