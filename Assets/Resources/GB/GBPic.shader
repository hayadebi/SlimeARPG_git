// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/GBPic"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}							// 加工前画像
		_ResolutionRatioP("ResolutionRatio", Float) = 0.5				// ターゲットとなる解像度の比率
		_DitherP("Dither", Float) = 1.0									// ディザ加工するか？
		_ScanningLineP("ScanningLine", Float) = 1.0						// 走査線演出を入れるか？
		_ScanningLineThresholdP("ScanningLineThreshold", Float) = 0.35	// 走査線演出の閾値
		_ContrastP ("Contrast", Float) = 1.2							// コントラスト調整
		_ColorFilterP ("ColorFilter", Color) = (0.650, 0.901, 0.3, 1.0)	// カラーフィルター
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float _ResolutionRatioP;
			float _DitherP;
			float _ScanningLineP;
			float _ScanningLineThresholdP;
			float _ContrastP;
			fixed4 _ColorFilterP;

			fixed4 frag (v2f i) : SV_Target
			{
				// ターゲット解像度（160*144に合わせると初代GBの解像度になる）
				float2 target_resolution = _ScreenParams.xy * _ResolutionRatioP;

				// UVの精度を下げて低解像度サンプリングする（この時点でモザイクエフェクトとしても使える）
				// Todo : 段階的に解像度を下げていくか、多点サンプリングするかした方が良好な結果を得られるかもしれない
				float2 uv_low = i.uv * target_resolution;
				uv_low = floor(uv_low);
				uv_low /= target_resolution;
				fixed4 color = tex2D(_MainTex, uv_low);

				// サンプリングした色をグレースケール化する（NTSC規格の加重平均）
				float shade = color.x * 0.2989 + color.y * 0.5866 + color.z * 0.1144;\

				// グレースケールの明暗を適当に調整（コントラストをいじるイメージ）
				shade = pow(shade, _ContrastP);

				// グレースケールを4階調に変換する
				// また、市松模様のドットパターンも仕込み、疑似的に7階調に見せかける（ディザ加工とも言う）
				shade *= 4.0;
				float checkered_pattern = step(frac(shade), 0.5);
				checkered_pattern *= step(0.5, fmod(uv_low.x * target_resolution.x + step(0.5, fmod(uv_low.y * target_resolution.y, 2.0)), 2.0));
				shade = max(shade - checkered_pattern * _DitherP, 0.1);
				shade = ceil(shade);
				shade /= 4.0;

				// ドットの格子模様を重ね掛け（走査線風演出？）
				float2 grid = step(1.0 / _ResolutionRatioP - 1.0, fmod(i.uv * _ScreenParams, 1.0 / _ResolutionRatioP));
				shade = max(shade, max(grid.x, grid.y) * _ScanningLineThresholdP * _ScanningLineP);

				// グレースケールにフィルターカラーを適用
				color.xyz = shade * _ColorFilterP.xyz;\

				return color;
			}
			ENDCG
		}
	}
}

