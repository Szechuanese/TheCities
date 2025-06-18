Shader "UI/RoundedRect_AspectFixed"
{
    Properties
    {
        _Color ("Fill Color", Color) = (0.157, 0.176, 0.275, 1)
        _Radius ("Corner Radius", Range(0, 0.5)) = 0.1
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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // �� uv �ռ����Ļ�����У�� x �������
                float2 pos = uv - 0.5;
                pos.x *= _Aspect;

                // �ھ�������
                float2 box = float2(0.5 * _Aspect - _Radius, 0.5 - _Radius);
                float2 dist = (abs(pos) - box) / _Radius;

                float d = length(max(dist, 0.0));
                float mask = smoothstep(1.0, 0.97, d);

                return _Color * mask;
            }
            ENDCG
        }
    }
}
