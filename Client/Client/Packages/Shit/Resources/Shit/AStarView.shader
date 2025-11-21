// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Editor/AStar"
{
    Properties
    {
        _Num ("_Num", 2D) = "black" {}
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
                    fixed4 color : COLOR;
                    float4 texcoord : TEXCOORD0;
                };
    
                sampler2D _Num;
                float2 _Size;
                float _Power;
                StructuredBuffer<int> _Data;
                
                static float xOffset[4] = {0,10.0/40,21.0/40,31.0/40};
                static float yOffset[3] = {0,13.0/37,26.0/37};

                fixed4 getNumColor(int v,float2 uv)
                {
                    v=(v+9)%10;
                    float2 numStartUV=float2(xOffset[v%4],yOffset[2-v/4]);
                    fixed4 col = tex2D(_Num, numStartUV+uv*float2(9.0/40,11.0/37)); 
                    return col;
                }
                fixed4 blendColor(int v,float clamp,float2 xy,float2 cube, fixed4 col)
                {
                    float numLerp;
                    if(xy.x>cube.x*clamp&&xy.x<cube.x*(clamp+0.1)&&xy.y>cube.y*0.85&&xy.y<cube.y*0.95)
                          numLerp=1;
                    else
                          numLerp=0;
                    float2 uv=(float2(xy.x-clamp*cube.x,xy.y-0.45*cube.y)/(0.1*cube))%1;  
                    fixed4 c=getNumColor(v,uv);
                    return lerp(col,c,numLerp);  
                }  
                fixed4 blendColorCost(int cost,int v,float clamp,float2 xy,float2 cube, fixed4 col)
                {
                    float numLerp;
                    if(xy.x>cube.x*clamp&&xy.x<cube.x*(clamp+0.3)&&xy.y>cube.y*0.3&&xy.y<cube.y*0.6)
                          numLerp=1;
                    else
                          numLerp=0;
                    float2 uv=(float2(xy.x-clamp*cube.x,xy.y-0.3*cube.y)/(0.3*cube))%1;  
                    fixed4 c=getNumColor(v,uv);
                    return lerp(col,c,numLerp*(1-step(cost,0.5))*(1-step(v,0.5)));  
                }

                v2f vert (appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.texcoord = v.texcoord;
                    o.color = v.color;

                    return o;
                }
                
                fixed4 frag (v2f i) : SV_Target
                {
                    float2 uv= i.texcoord.xy;
                    fixed4 col = 0;
                    float2 cube=1/_Size;
                    float2 xy=float2(uv.x%cube.x,uv.y%cube.y);
                    int2 nxy=uv*_Size;
                    int buffer=_Data[nxy.y*_Size.x+nxy.x];

                    int enable=buffer&1;
                    int cost=(buffer&0xff)>>1;
                    int Occupation=(buffer>>8)&0xff;
                    int PathOccupation=(buffer>>16)&0xff;
                    int path=PathOccupation&0x0f;
                    int path_f=PathOccupation>>4;

                    float m=min(abs(xy.x-cube.x),abs(xy.y-cube.y));
                    m=min(m,xy.x);
                    m=min(m,xy.y);
                    float s=step(m,_Power*min(cube.x,cube.y));

                    //阻挡显示
                    col.rg=lerp(float2(1,0),float2(0,1),enable);

                    //单位占位
                    col.rgb=lerp(col.rgb,float3(col.r,0,1),min(Occupation,1));//占位(只显示可行走区域)

                    //搜索路径+执行路径
                    col.rgb=lerp(col.rgb,float3(1,1,0),min(path_f,1));
                    col.rgb=lerp(col.rgb,float3(0,1,1),min(path,1));

                    //格子边缘线条
                    col.a=lerp(0.5,1,s);
                    
                    col=blendColor((nxy/1000).x,0.05,xy,cube,col);
                    col=blendColor((nxy/100).x,0.15,xy,cube,col);
                    col=blendColor((nxy/10).x,0.25,xy,cube,col);
                    col=blendColor(nxy.x,0.35,xy,cube,col);
                    col=blendColor((nxy/1000).y,0.55,xy,cube,col);
                    col=blendColor((nxy/100).y,0.65,xy,cube,col);
                    col=blendColor((nxy/10).y,0.75,xy,cube,col);
                    col=blendColor(nxy.y,0.85,xy,cube,col);

                    col=blendColorCost(cost,cost/100,0.05,xy,cube,col);
                    col=blendColorCost(cost,cost/10,0.35,xy,cube,col);
                    col=blendColorCost(cost,cost,0.65,xy,cube,col);

                    return col;
                }
            ENDCG
        }
    }
}
