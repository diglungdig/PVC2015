Shader "Aubergine/Object/Surf/Sample/Diffuse-Hologram" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}
		/* HOLOGRAM SHADER PROPERTIES */
		_HoloTex("Hologram Texture", 2D) = "white" { }
		_Power("Power", Range (0.0, 1)) = 0.5
		_Speed("Speed", Range (0.0, 100)) = 0.5
		_Thickness("Thickness", Range (0.0, 1)) = 0.5
		_Luminance("Luminance", Range (0.0, 1)) = 0.5
		_Darkness("Darkness", Range (0.0, 1)) = 0.5
	}

	SubShader {
		Tags { "RenderType" = "Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma exclude_renderers xbox360 ps3 flash
		#pragma surface surf Lambert

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG

		/* HOLOGRAM SHADER PASS */
		UsePass "Aubergine/Object/BaseFX/Hologram/BASE"
	} 

	FallBack "Diffuse"
}