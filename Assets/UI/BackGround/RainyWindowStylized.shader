
Shader "Custom/RealisticRainyWindow"
{
    Properties
    {
        _BaseColor("Base Glass Color", Color) = (0.086, 0.094, 0.129, 1)
        _RainSpeed("Rain Drop Speed", Range(0.01, 0.3)) = 0.12
        _CarLightColor1("Car Light Color 1", Color) = (1, 0.4, 0, 1)
        _CarLightColor2("Car Light Color 2", Color) = (1, 1, 0, 1)
        _CarLightColor3("Car Light Color 3", Color) = (0.6, 0.2, 1, 1)
        _CarLightColor4("Car Light Color 4", Color) = (0.3, 1, 0.9, 1)
        _TowerColor1("Front Tower Color", Color) = (1, 0.3, 0.2, 1)
        _TowerColor2("Back Tower Color", Color) = (0.2, 0.6, 1, 1)
        _DropDensity("Rain Drop Density", Range(5, 30)) = 15.0
        _TowerBreathSpeed("Tower Breath Speed", Range(0.5, 3)) = 1.5
        _CarLightSpeed("Car Light Speed", Range(0.1, 0.8)) = 0.4
        _RainDistortion("Rain Distortion", Range(0, 0.1)) = 0.03
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _BaseColor;
            float _RainSpeed, _DropDensity, _TowerBreathSpeed, _CarLightSpeed, _RainDistortion;
            float4 _CarLightColor1, _CarLightColor2, _CarLightColor3, _CarLightColor4;
            float4 _TowerColor1, _TowerColor2;

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            // ���õ��������
            float random(float2 st) 
            {
                return frac(sin(dot(st.xy, float2(12.9898, 78.233))) * 43758.5453123);
            }

            float2 random2(float2 st) 
            {
                st = float2(dot(st, float2(127.1, 311.7)), dot(st, float2(269.5, 183.3)));
                return -1.0 + 2.0 * frac(sin(st) * 43758.5453123);
            }

            // ��������������Ȼ����ι켣
            float noise(float2 st) 
            {
                float2 i = floor(st);
                float2 f = frac(st);
                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(lerp(dot(random2(i + float2(0.0, 0.0)), f - float2(0.0, 0.0)),
                               dot(random2(i + float2(1.0, 0.0)), f - float2(1.0, 0.0)), u.x),
                           lerp(dot(random2(i + float2(0.0, 1.0)), f - float2(0.0, 1.0)),
                               dot(random2(i + float2(1.0, 1.0)), f - float2(1.0, 1.0)), u.x), u.y);
            }

            // ��ʵ�����·�� - ���ǲ�����������������
            float2 rainDropPath(float2 uv, float time, float2 seed)
            {
                float dropTime = time + seed.y * 10.0;
                float gravity = dropTime * _RainSpeed;
                
                // ����������ɵ����ͣ�ٺͼ���
                float tension = sin(dropTime * 8.0 + seed.x * 20.0) * 0.3 + 0.7;
                gravity *= tension;
                
                // ���������΢С��б�Ͳ�ƽ��
                float surface_noise = noise(uv * 50.0 + seed * 10.0) * 0.008;
                float zigzag = noise(float2(uv.y * 30.0 + dropTime * 2.0, seed.x)) * 0.015;
                
                return float2(surface_noise + zigzag, gravity);
            }

            // ����ʵ�������״ - ģ����ʵˮ�εı�������
            float waterDrop(float2 uv, float2 pos, float size, float trail)
            {
                float2 diff = uv - pos;
                
                // ��Ҫˮ���� - Բ�ε���΢����
                float dropBody = length(diff / float2(size, size * 1.2));
                float mainDrop = 1.0 - smoothstep(0.0, 1.0, dropBody);
                
                // ˮ��β�� - ֻ���·�
                float tailIntensity = 0.0;
                if (diff.y > 0.0) 
                {
                    float tailDist = abs(diff.x) / (size * 0.3) + diff.y / (trail * size);
                    tailIntensity = exp(-tailDist * 8.0) * 0.4;
                }
                
                return max(mainDrop, tailIntensity);
            }

            // ���ϵͳ - ���ٵ�����ʵ�����
            float drawRain(float2 uv, float time)
            {
                float rain = 0.0;
                
                // ��������������������
                for (int i = 0; i < 12; i++)
                {
                    float dropId = float(i);
                    float2 seed = float2(dropId / _DropDensity, random(float2(dropId, 42.0)));
                    
                    // ÿ������в�ͬ����������
                    float dropTime = time + random(seed + dropId) * 15.0;
                    float2 startPos = float2(seed.x, 1.0 + random(seed) * 0.2);
                    
                    float2 movement = rainDropPath(uv, dropTime, seed);
                    float2 currentPos = startPos + movement;
                    
                    // ѭ���߽紦��
                    currentPos.y = frac(currentPos.y + 1.0) - 0.2;
                    currentPos.x = frac(currentPos.x + 0.5);
                    
                    // ��δ�С��ǿ�ȵ�����仯
                    float dropSize = 0.005 + random(seed + 1.0) * 0.008;
                    float dropTrail = 2.0 + random(seed + 2.0) * 3.0;
                    float dropAlpha = 0.6 + random(seed + 3.0) * 0.4;
                    
                    float drop = waterDrop(uv, currentPos, dropSize, dropTrail);
                    rain += drop * dropAlpha;
                }
                
                return saturate(rain);
            }

            // Բ�κ����޺����
            float breathingTower(float2 uv, float2 center, float time, float phase, float baseSize, float breathIntensity)
            {
                float breathCycle = sin(time * _TowerBreathSpeed + phase) * 0.5 + 0.5;
                float currentSize = baseSize * (0.7 + breathCycle * 0.6);
                
                float dist = distance(uv, center);
                float tower = exp(-pow(dist / currentSize, 1.5) * 3.0);
                
                // ����ʱ��ǿ�ȱ仯
                float intensity = breathIntensity * (0.5 + breathCycle * 0.8);
                
                return tower * intensity;
            }

            // ��͵ĳ��ƹ켣
            float smoothCarLight(float2 uv, float time, float yPos, float speed, float size)
            {
                float x = frac(time * speed * _CarLightSpeed);
                float2 lightPos = float2(x, yPos);
                
                float dist = distance(uv, lightPos);
                float light = exp(-pow(dist / size, 1.2) * 4.0);
                
                // ��Ե����Ч�� - ����"�ƶ�������"�ĸо�
                float edgeFade = smoothstep(0.0, 0.1, x) * smoothstep(1.0, 0.9, x);
                
                return light * edgeFade;
            }

            // ����͸��Ч��
            float atmosphericPerspective(float2 uv, float depth)
            {
                float fog = exp(-depth * 2.0);
                float distance_fade = 1.0 - smoothstep(0.3, 1.0, uv.y);
                return fog * distance_fade;
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float t = _Time.y;
                float2 uv = i.uv;
                
                // ����������ɫ
                float3 color = _BaseColor.rgb;
                
                // ���Ч��
                float rain = drawRain(uv, t);
                
                // ��ζԹ��ߵ�����Ч��
                float2 distortedUV = uv + rain * _RainDistortion * float2(
                    noise(uv * 20.0 + t) - 0.5,
                    noise(uv * 15.0 + t + 10.0) - 0.5
                );
                
                // ����ò�����΢����������
                color = lerp(color, color * 1.15 + float3(0.05, 0.05, 0.08), rain * 0.8);
                
                // ����¥ - ��Զ���ϰ����ϴ�ĺ���Ч��
                float backTower = breathingTower(distortedUV, float2(0.25, 0.75), t, 0.0, 0.08, 0.4);
                color += _TowerColor2.rgb * backTower * atmosphericPerspective(uv, 1.5);
                
                // ǰ����¥ - �Ͻ�����������С����ǿ�ҵĺ���Ч��
                float frontTower = breathingTower(distortedUV, float2(0.75, 0.65), t, 3.14159, 0.06, 0.8);
                color += _TowerColor1.rgb * frontTower * atmosphericPerspective(uv, 0.8);
                
                // �����ĳ��� - ����͵Ĺ켣
                float car1 = smoothCarLight(distortedUV, t, 0.15, 1.2, 0.025);
                float car2 = smoothCarLight(distortedUV, t * 0.7, 0.12, 0.8, 0.03);
                float car3 = smoothCarLight(distortedUV, t * 1.3, 0.18, 1.5, 0.022);
                float car4 = smoothCarLight(distortedUV, t * 0.9, 0.21, 1.0, 0.035);
                
                color += _CarLightColor1.rgb * car1 * 0.8;
                color += _CarLightColor2.rgb * car2 * 0.6;
                color += _CarLightColor3.rgb * car3 * 0.7;
                color += _CarLightColor4.rgb * car4 * 0.5;
                
                // �������Ч��
                float vignette = 1.0 - smoothstep(0.4, 1.2, length(uv - 0.5));
                color *= vignette;
                
                // ���ϸ΢�Ļ�����
                color += float3(0.01, 0.015, 0.02) * (1.0 - uv.y * 0.5);
                
                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}