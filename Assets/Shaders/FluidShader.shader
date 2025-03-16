Shader "Unlit/FluidShader"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _TopColor("Liquid Top Color", Color) = (1,1,1,1)
        _LiquidColor("Liquid Side Color", Color) = (1,0,0,1)
        _FillRatio("Fillratio", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        // use alpha data
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        LOD 100

        // since we want to render both the front AND back faces of the object, we would usually
        // just set "Cull Off", however since we want the resulting faces to be transparent, we will need
        // to seperate these into different passes

        Pass
        {
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                UNITY_FOG_COORDS(1)
            };

            float4 _TopColor;
            float _FillRatio;

            float3 worldPos;

            uniform float _MinWorldY;
            uniform float _MaxWorldY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                // calculating the in world position of the vertex
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // we lerp between the min and max bounds of the object and decide upon the cutoff height
                float fillHeight = lerp(_MinWorldY, _MaxWorldY, _FillRatio);
                
                // based on this height, we either render the vertex or don't
                if (i.worldPos.y <= fillHeight)
                {
                    return _TopColor;
                }

                // colorless and fully transparent material
                return half4(0, 0, 0, 0);

                UNITY_APPLY_FOG(i.fogCoord, col);
            }
            ENDCG
        }

        Pass
        {
            Tags {"LightMode"="UniversalForward"}

            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                UNITY_FOG_COORDS(1)
            };

            float4 _LiquidColor;
            float _FillRatio;

            float3 worldPos;

            uniform float _MinWorldY;
            uniform float _MaxWorldY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float fillHeight = lerp(_MinWorldY, _MaxWorldY, _FillRatio);

                if (i.worldPos.y <= fillHeight)
                {
                    return _LiquidColor;
                }

                return half4(0, 0, 0, 0);

                UNITY_APPLY_FOG(i.fogCoord, col);
            }
            ENDCG
        }
    }
}
