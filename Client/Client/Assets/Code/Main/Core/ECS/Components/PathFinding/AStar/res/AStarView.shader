// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Editor/AStar"
{
    Properties
    {
        _Num ("_Num", 2D) = "black" {}
        _StartPos ("_StartPos",Vector) =(0,0,0,0)
        _GridSize ("_GridSize",Vector) =(0,0,0,0)
        _Size ("_Size",Vector) =(1,1,0,0)
        _hexParityType ("_hexParityType",float) = 0
        _hexFacingType ("_hexFacingType",float) = 0
        _Power ("_Power", Range(0, 1)) =0.05

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
                #include "Assets/Res/Shader/common/hex.hlsl"
    
                struct appdata_t
                {
                    float4 vertex : POSITION;
                    fixed4 color : COLOR;
                };
    
                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    fixed4 color : COLOR;
                    float3 localPos : TEXCOORD0;
                };

                #pragma shader_feature GridType_Rect
                #pragma target 4.5
    
                sampler2D _Num;
                float2 _StartPos;
                float2 _GridSize;
                float2 _Size;
#if !GridType_Rect
                float _hexParityType;
                float _hexFacingType;
#endif
                float _Power;
                StructuredBuffer<int> _Data;
                
                static const float sqrt_3 = sqrt(3.0);
                static const float b1_halfwidth=0.05;
                static const float b2_halfwidth=0.15;

                fixed4 getNumColor(int v,float2 uv)
                {
                    v=v%10;
                    fixed4 col = tex2D(_Num,(float2(v*32,0)+ uv*32)/float2(320.0,32));
                    return col;
                }
                fixed4 blendColor(int v,float clamp,float2 xy,fixed4 col)
                {
                    float2 uv=(float2(xy.x-clamp,xy.y-0.45)/(b1_halfwidth*2))%1;  
                    fixed4 c=getNumColor(v,uv);
                    float max_v=step(abs(xy.x-(clamp+b1_halfwidth)),b1_halfwidth)*step(abs(xy.y-(0.85+b1_halfwidth)),b1_halfwidth);
                    return lerp(col,c,max_v*c.a);  
                }  
                fixed4 blendColorCost(int v,float clamp,float2 xy,fixed4 col)
                {
                    float2 uv=(float2(xy.x-clamp,xy.y-0.5)/(b2_halfwidth*2));  
                    fixed4 c=getNumColor(v,uv);
                    float max_v=step(abs(xy.x-(clamp+b2_halfwidth)),b2_halfwidth)*step(abs(xy.y-(0.5+b2_halfwidth)),b2_halfwidth);
                    return lerp(col,c,max_v*c.a*(1-step(v,0.5)));  
                }

                v2f vert (appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.localPos = v.vertex.xyz;
                    o.color = v.color;

                    return o;
                }
                
                fixed4 frag (v2f i) : SV_Target
                {
                    float2 localPos= i.localPos.xz;//局部坐标
#if GridType_Rect
                    int2 nxy=(int2)(localPos/_GridSize+1)-1;//格子坐标
#else 
                    int2 nxy = Hex_GetGridxy(localPos,_GridSize.x,_hexParityType,_hexFacingType);
#endif

                    fixed4 col=0;
                    if(nxy.x<0||nxy.y<0||nxy.x>=_Size.x||nxy.y>=_Size.y)
                        return 0;
                    else
                    {
                        int buffer=_Data[nxy.y*_Size.x+nxy.x];
                       
                        int enable=buffer&1;
                        int cost=(buffer&0xff)>>1;
                        int Occupation=(buffer>>8)&0xff;
                        int PathOccupation=(buffer>>16)&0xff;
                        int path=PathOccupation&0x0f;
                        int path_f=PathOccupation>>4;
                       
                        //阻挡显示
                        col.rg=lerp(float2(1,0),float2(0,1),enable);
                       
                        //搜索路径+执行路径 显示
                        col.rgb=lerp(col.rgb,float3(1,1,0),min(path_f,1));
                        col.rgb=lerp(col.rgb,float3(0,1,1),min(path,1));
                       
#if GridType_Rect
                        float2 gridLocalxy=localPos%_GridSize;//在格子内的局部坐标
                        float2 gridLocalxy01=gridLocalxy/_GridSize;

                        //格子边缘线条
                        float m=min(abs(gridLocalxy.x-_GridSize.x),abs(gridLocalxy.y-_GridSize.y));
                        m=min(m,gridLocalxy.x);
                        m=min(m,gridLocalxy.y);
                        float s=step(m/(_GridSize.x/2),_Power);
                        col.a=lerp(0.5,1,s);
                        
                        //单位占位
                        float dis=step(length(gridLocalxy/_GridSize-0.5),0.3);
 #else

                        float2 center=Hex_GetPositon(nxy,_GridSize.x,_hexParityType,_hexFacingType).xz;
                        float2 xy=localPos-center;//格子内与中点的坐标
                        float2 gridLocalxy01=(lerp(xy.xy,xy.yx,0)+float2(_GridSize.x/2,_GridSize.x*sqrt_3/2))/float2(_GridSize.x,_GridSize.x*2/sqrt_3);

                         //格子边缘线条
                        float2 abs_xy=abs(xy);
                        float rv=rcp(sqrt_3);
                        float2 p1=_GridSize.x*float2(0,rv);
                        p1=lerp(p1.xy,p1.yx,_hexFacingType);
                        float2 p2=_GridSize.x*0.5*float2(1,rv);
                        p2=lerp(p2.xy,p2.yx,_hexFacingType);
                        float m=distancePAB(abs_xy,p1,p2);

                        m=min(_GridSize.x/2-lerp(abs_xy.x,abs_xy.y,_hexFacingType),m);
                        float s=step(m/(_GridSize.x/2),_Power);
                        col.a=lerp(0.5,1,s);

                        //单位占位
                        float dis=step(distance(xy,center),0.3);
 #endif
 
                        //单位占位
                        col.rgba=lerp(col.rgba,float4(1,0,0,0.8f),dis*min(Occupation,1));

                        //格子坐标显示
                        col=blendColor((nxy/1000).x,0.05,gridLocalxy01,col);
                        col=blendColor((nxy/100).x, 0.15,gridLocalxy01,col);
                        col=blendColor((nxy/10).x,  0.25,gridLocalxy01,col);
                        col=blendColor(nxy.x,       0.35,gridLocalxy01,col);
                        col=blendColor((nxy/1000).y,0.55,gridLocalxy01,col);
                        col=blendColor((nxy/100).y, 0.65,gridLocalxy01,col);
                        col=blendColor((nxy/10).y,  0.75,gridLocalxy01,col);
                        col=blendColor(nxy.y,       0.85,gridLocalxy01,col);

                        //格子消耗显示
                        col=blendColorCost(cost/100,0.05,gridLocalxy01,col);
                        col=blendColorCost(cost/10, 0.35,gridLocalxy01,col);
                        col=blendColorCost(cost,    0.65,gridLocalxy01,col);

                        return col;
                    }
                }
            ENDCG
        }
    }
}
