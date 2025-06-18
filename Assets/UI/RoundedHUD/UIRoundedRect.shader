Shader "UI/PureRoundedRect_AutoAspect"
{
    Properties
    {
        _Color ("Fill Color", Color) = (0.157, 0.176, 0.275, 1)
        _Radius ("Corner Radius", Range(0, 1)) = 0.1
        _Aspect ("Aspect Ratio", Float) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        ZWrite Off
        Cull Off
        Lighting Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float _Radius;
            float _Aspect;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // 修正横向缩放，防止圆角形变
                float2 centeredUV = uv - 0.5;
                centeredUV.x *= _Aspect;

                float2 border = float2(0.5 * _Aspect - _Radius, 0.5 - _Radius);
                float2 edge = (abs(centeredUV) - border) / _Radius;

                float dist = length(max(edge, 0.0));
                float mask = smoothstep(1.0, 0.97, dist);

                return _Color * mask;
            }
            ENDCG
        }
    }
}
