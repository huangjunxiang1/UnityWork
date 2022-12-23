Shader "CS/GPUDraw"
{
    Properties
    {
        _Color("_Color",Color) = (1,1,1,1)
        [HDR] _EColor("_EColor",Color) = (1,1,1,1)
        _EPower("_EPower",float) = 1
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
            #pragma multi_compile_fog
            #pragma   multi_compile_instancing

            #include "UnityCG.cginc"
            #include "Assets/Res/Shader/GPUDrawPosGet.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                uint instanceID:SV_InstanceID;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                    float4 vertex :  SV_POSITION;
            };

            float4 _Color;
            float4 _EColor;
            float _EPower;

            v2f vert (appdata v)
            {
                v2f o;

                float3 Pos;
                GetPos_float(v.instanceID, Pos);
                o.vertex = UnityObjectToClipPos(v.vertex + Pos);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            { 
                return _EColor* _EPower;
            }
            ENDCG
        }
    }
}
