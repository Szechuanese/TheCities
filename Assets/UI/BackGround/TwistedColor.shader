Shader "Custom/TwistedColor"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (1, 0, 0, 1)
        _Color2 ("Color 2", Color) = (0, 0, 1, 1)
        _Color3 ("Blend Color", Color) = (0, 0, 0, 1)//衔接颜色
        _Color4 ("Edge Color", Color) = (1, 1, 1, 1)//起始/终点颜色
        _Contrast ("Contrast Iterations", Range(0, 10)) = 5//过度强度
        _Gradual ("Blend Gradual", Range(0, 2)) = 2.0//渐变
        _Width1 ("Inner Width", Range(0.01, 1)) = 0.04//中间色强度
        _Width2 ("Middle Width", Range(0.01, 1)) = 0.1//外边缘色强度
        _Scale1 ("UV Scale 1", Range(0, 100)) = 10.0//缩放1
        _Scale2 ("UV Scale 2", Range(0, 10)) = 1.0//缩放2
        _Offset ("Offset", Vector) = (0,0,0,0)//偏移
        _Intensity ("Distort Intensity", Range(0, 4)) = 0.2//扭曲强度
        _SpinSpeed ("Spin Speed", Range(0, 10)) = 0.2//旋转速度
        _SpinAmount ("Spin Amount", Range(0, 10)) = 1.5//旋转量
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color1, _Color2, _Color3, _Color4;
            float _Contrast, _Gradual, _Width1, _Width2;
            float _Scale1, _Scale2, _Intensity;
            float4 _Offset;
            float _SpinSpeed, _SpinAmount;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // screen centered
                float aspect = _ScreenParams.x / _ScreenParams.y;
                uv -= float2(0.5 * aspect, 0.5);
                uv *= 2.0;
                uv += _Offset.xy;

                float len_uv = length(uv);
                float angle = atan2(uv.y, uv.x);
                angle -= _SpinAmount * len_uv;
                angle += _Time.y * _SpinSpeed;

                uv = float2(cos(angle), sin(angle)) * len_uv * _Scale2;
                uv *= _Scale1;
                
                float2 uv2 = float2(uv.x + uv.y, uv.x - uv.y);

                for (int j = 0; j < (int)_Contrast; j++) {
                    uv2 += sin(uv);
                    uv += float2(cos(_Intensity * uv2.y + _Time.y), sin(_Intensity * uv2.x - _Time.y));
                    uv -= cos(uv.x + uv.y) - sin(uv.x - uv.y);
                }

                float paint_res = smoothstep(0, _Gradual, length(uv) / _Scale1);
                float c3p = 1.0 - min(_Width2, abs(paint_res - 0.5)) * (1.0 / _Width2);
                float c_out = max(0.0, (paint_res - (1.0 - _Width1))) * (1.0 / _Width1);
                float c_in = max(0.0, -(paint_res - _Width1)) * (1.0 / _Width1);
                float c4p = c_out + c_in;

                fixed3 color = lerp(_Color1.rgb, _Color2.rgb, paint_res);
                color = lerp(color, _Color3.rgb, c3p);
                color = lerp(color, _Color4.rgb, c4p);

                return fixed4(color, 1);
            }
            ENDCG
        }
    }
}

