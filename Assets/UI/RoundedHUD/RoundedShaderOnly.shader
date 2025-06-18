Shader "UI/CardShapeOnlyRounded"
{
    Properties
    {
        [PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}
        [PerRendererData]_Color ("Tint", Color) = (1,1,1,1)
        _Radius ("Corner Radius", Float) = 32.0
        _BorderWidth ("Border Width", Float) = 0.0
        _BorderColor ("Border Color", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Radius;
            float _BorderWidth;
            fixed4 _BorderColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            // 关键逻辑：uv空间圆角mask
            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 corner = min(uv, 1.0 - uv);
                float dist = min(corner.x, corner.y);

                // 圆角alpha（_Radius请配合UI像素/适配用）
                float radius = _Radius / 512.0; // 512是UI设计参考尺寸
                float alpha = smoothstep(radius, radius - 0.003, dist);

                // 主色直接受Image.color驱动
                fixed4 col = tex2D(_MainTex, uv) * _Color;

                // 边框
                float border = 1.0;
                if (_BorderWidth > 0.001)
                {
                    float borderRadius = (_Radius - _BorderWidth) / 512.0;
                    float borderMask = smoothstep(borderRadius, borderRadius - 0.003, dist);
                    col.rgb = lerp(_BorderColor.rgb, col.rgb, borderMask);
                }

                col.a *= alpha;
                return col;
            }
            ENDCG
        }
    }
    FallBack "UI/Default"
}