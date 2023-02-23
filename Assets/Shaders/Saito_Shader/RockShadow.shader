Shader "Unlit/RockShadow"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags {
            "Queue"             = "AlphaTest"
            "RenderPipeline"    = "UniversalPipeline"
            "RenderType"        = "TransparentCutout"
        }
        LOD 200

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            // Unityのライト情報による影を受け取るために必要な機能
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex   : POSITION;
                float2 uv       : TEXCOORD0;
                float3 normal   : NORMAL;
            };

            struct v2f
            {
                float2 uv           : TEXCOORD0;
                float4 vertex       : SV_POSITION;
                float3 normal       : NORMAL;
                float3 normalWS     : TEXCOORD1;
                float4 shadowCoord  : TEXCOORD3;
            };

            sampler2D   _MainTex;
            float4      _MainTex_ST;
            half4       _Color;

            Light _SLight;

            v2f vert (appdata v)
            {
                v2f o;
                o.uv        = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal    = TransformObjectToWorldNormal(v.normal);

                //面の法線を取得、ライトの当たる向きを計算（ワールドスペースなど全ての情報を取得）
                VertexNormalInputs normal = GetVertexNormalInputs(v.normal);
                o.normalWS = normal.normalWS;

                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
                o.vertex = vertexInput.positionCS;
                o.shadowCoord = GetShadowCoord(vertexInput);

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv) * _Color;

                //Light.hlslで提供されるUnityのライトを取得する関数
                Light lt = GetMainLight(i.shadowCoord);

                //ライトの向きを計算
                float strength = dot(lt.direction, i.normalWS);
                float4 lightColor = float4(lt.color, 1) * (lt.distanceAttenuation * lt.shadowAttenuation);

                return col * lightColor * strength;
            }
            ENDHLSL
        }
    }
}

/*
* [参考]
* https://redhologerbera.hatenablog.com/entry/2022/11/12/211713#:~:text=URP%E3%81%AEShader%E3%81%A7Unity%E3%81%AE%E3%83%A9%E3%82%A4%E3%83%88%E6%83%85%E5%A0%B1%E3%81%AB%E3%82%88%E3%82%8B%E5%BD%B1%E3%82%92%E5%8F%97%E3%81%91%E5%8F%96%E3%82%8B%E3%81%9F%E3%82%81%E3%81%AB%E3%81%AF%E6%AC%A1%E3%81%AE%E6%A9%9F%E8%83%BD%E3%82%92%E4%BD%BF%E7%94%A8%E3%81%97%E3%81%BE%E3%81%99%E3%80%82%20%23pragma%20multi_compile%20_,_MAIN_LIGHT_SHADOWS%20%23pragma%20multi_compile%20_%20_MAIN_LIGHT_SHADOWS_CASCADE
*/