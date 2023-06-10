Shader "PolySoft/Ant_tint_Shader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_ColorTint1 ("Color tint 1", Color) = (1,1,1,1)
		_ColorTint2 ("Color tint 2", Color) = (1,1,1,1)
		_ColorTint3 ("Color tint 3", Color) = (1,1,1,1)
		[HDR]_EmissionColor1 ("Emission color 1", Color) = (0,0,0,1)
		[HDR]_EmissionColor2 ("Emission color 2", Color) = (0,0,0,1)
		[HDR]_EmissionColor3 ("Emission color 3", Color) = (0,0,0,1)
		_Glossiness ("Smoothness", Range(0,1)) = 1.0
		_Metallicness ("Metallic power", Range(0,1)) = 1.0
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Mask ("Mask (R color 1, G color 2, B color 3)", 2D) = "black" {}
        _Metallic ("Metallic", 2D) = "black" {}
		_NormalMap ("Normal Map", 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _Metallic;
		sampler2D _Mask;
		sampler2D _NormalMap;
        struct Input
        {
            float2 uv_MainTex;
			float2 uv_NormalMap;
        };

        half _Glossiness;
		half _Metallicness;
        fixed4 _Color;
		fixed4 _ColorTint1;
		fixed4 _ColorTint2;
		fixed4 _ColorTint3;
		fixed4 _EmissionColor1;
		fixed4 _EmissionColor2;
		fixed4 _EmissionColor3;
        UNITY_INSTANCING_BUFFER_START(Props)

        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 m = tex2D (_Metallic, IN.uv_MainTex);
			fixed4 mask = tex2D (_Mask, IN.uv_MainTex);
			o.Normal = UnpackNormal (tex2D (_NormalMap, IN.uv_NormalMap));
			o.Albedo = lerp(c.rgb * _ColorTint1.rgb, c.rgb * _ColorTint2.rgb, mask.g);
            o.Albedo = lerp(o.Albedo, c.rgb * _ColorTint3.rgb, mask.b) * _Color;
            o.Metallic = m.r * _Metallicness;
            o.Smoothness = m.a * _Glossiness;
			o.Emission = mask.b * _EmissionColor3.rgb +  mask.r * _EmissionColor1.rgb +  mask.g * _EmissionColor2.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
