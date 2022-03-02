Shader "Unlit/realTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _redOffset("red offset", Range(-1,1)) = 0
        _greenOffset("green offset", Range(-1,1)) = 0
        _blueOffset("blue offset", Range(-1,1)) = 0
        _Color("Color", Color) = (0, 1, 1, 1)

            _Frequency("Frequency", float) = 10
        _DisplAmount("Displacement amount", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            half _redOffset;
            half _greenOffset;
            half _blueOffset;

            float _Frequency;
            float _DisplAmount;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex.x += sin(v.vertex.y * _Frequency) * _DisplAmount;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 c;
                c.r = tex2D(_MainTex, i.uv + _redOffset).r;
                c.g = tex2D(_MainTex, i.uv + _greenOffset).g;
                c.b = tex2D(_MainTex, i.uv + _blueOffset).b;
                c.a = tex2D(_MainTex, i.uv).a;

                //fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, c);
                
                return c * _Color;
            }
            ENDCG
        }
    }
}
