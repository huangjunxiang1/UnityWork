// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Editor/AStar"
{
    Properties
    {
        _MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
        _Size ("_Size",Vector) =(1,1,0,0)
        _Power ("_Power",float) =0.05

        _BlendSrcFactor ("Blend SrcFactor", Float) = 5
        _BlendDstFactor ("Blend DstFactor", Float) = 10
    }
    
    SubShader
    {
        LOD 100

        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }
        
        Cull Off
        Lighting Off
        ZWrite Off
        Fog { Mode Off }
        Blend [_BlendSrcFactor] [_BlendDstFactor], One One

        Pass
        {
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                
                #include "UnityCG.cginc"
    
                struct appdata_t
                {
                    float4 vertex : POSITION;
                    fixed4 color : COLOR;
                    float4 texcoord : TEXCOORD0;
                };
    
                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    float4 worldPos : TEXCOORD1;
                    fixed4 color : COLOR;
                    float4 texcoord : TEXCOORD0;
                };
    
                sampler2D _MainTex;
                float2 _Size;
                float _Power;
                
                v2f vert (appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyzw;
                    o.texcoord = v.texcoord;
                    o.color = v.color;

                    return o;
                }
                
                fixed4 frag (v2f i) : SV_Target
                {
                    float2 uv= i.texcoord.xy;
                    fixed4 col = tex2D(_MainTex, i.texcoord.xy / i.texcoord.w) * i.color;
                    float2 cube=1/_Size;
                    float m=min(abs(uv.x%cube.x-cube.x),abs(uv.y%cube.y-cube.y));
                    m=min(m,uv.x%cube.x);
                    m=min(m,uv.y%cube.y);
                    float s=step(m,_Power*min(cube.x,cube.y));
                    col.a=lerp(col.a,1,s);

                    return col;
                }
            ENDCG
        }
    }
}
