Shader "Aubergine/Object/BaseFX/Hologram" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}
		_HoloTex("Hologram Texture", 2D) = "white" { }
		_Power("Power", Range (0.0, 1)) = 0.5
		_Speed("Speed", Range (0.0, 100)) = 0.5
		_Thickness("Thickness", Range (0.0, 1)) = 0.5
		_Luminance("Luminance", Range (0.0, 1)) = 0.5
		_Darkness("Darkness", Range (0.0, 1)) = 0.5
	}

	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True"}
		LOD 100

		Pass {
			Name "BASE"
			Tags { "LightMode" = "Always" }

			Fog { Mode off }
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma exclude_renderers xbox360 ps3 flash
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			sampler2D _MainTex, _HoloTex;
			fixed _Power;
			float _Speed;
			float _Thickness;
			float _Luminance;
			float _Darkness;

			struct a2v {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed3 eye : TEXCOORD1;
				fixed3 norm : TEXCOORD2;
			};

			v2f vert(a2v v) {
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord.xy;
				o.eye = normalize(ObjSpaceViewDir(v.vertex));
				o.norm = normalize(v.normal);
				return o;
			}

			fixed4 frag(v2f i) : COLOR {
				//For simple meshes, this should be calculated in vert
				fixed EdotN = dot(i.eye, i.norm);
				EdotN = clamp(EdotN, 0.0, 1.0);
				float2 uv = EdotN;
				fixed3 hol = tex2D(_HoloTex, uv).rgb;
				fixed3 col = tex2D(_MainTex, i.uv).rgb;
				fixed alpha = 1.0 - EdotN;
				fixed4 result = fixed4(lerp(col, hol, _Power), alpha);
				float cycle = sin((i.uv.y / 0.005 - _Time.y * _Speed) * _Thickness);
				result *= _Darkness + _Luminance * cycle;
				return result;
			}
			ENDCG 
		}
	}

	Fallback Off
}