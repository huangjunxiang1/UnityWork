Shader "Hidden/ftProjection"
{
    SubShader {

        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard vertex:vert noinstancing

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _BakeryProjectionMap;
        float3 _BakeryProjectionDir;
        float4 _BakeryProjectionTilingOffset;

        struct Input {
            float3 worldPos;
            //float4 screenPos;
        };

        struct vinput
        {
            float4 vertex : POSITION;
            float2 texcoord1 : TEXCOORD1;
            float2 texcoord2 : TEXCOORD2;
            float3 normal : NORMAL0;
            float2 texcoord : TEXCOORD0;
            float4 tangent : TANGENT;
        };

        void vert (inout vinput v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            //o.screenPos = mul(UNITY_MATRIX_VP, float4(o.worldPos,1));
        }

        float2 BoxIntersection(float3 ro, float3 rd, float3 boxSize)
        {
            float3 m = 1.0/rd; // can precompute if traversing a set of aligned boxes
            float3 n = m*ro;   // can precompute if traversing a set of aligned boxes
            float3 k = abs(m)*boxSize;
            float3 t1 = -n - k;
            float3 t2 = -n + k;
            float tN = max( max( t1.x, t1.y ), t1.z );
            float tF = min( min( t2.x, t2.y ), t2.z );
            if( tN>tF || tF<0.0) return -1.0; // no intersection
            return float2( tN, tF );
        }

        void surf (Input IN, inout SurfaceOutputStandard o) {
            o.Albedo = 0.0001f;
            o.Smoothness = 0;

            float3 normal = _BakeryProjectionDir;
            float3 binormal, tangent;
            if(abs(normal.x) > abs(normal.z))
            {
                binormal.x = -normal.y;
                binormal.y =  normal.x;
                binormal.z =  0;
            }
            else
            {
                binormal.x =  0;
                binormal.y = -normal.z;
                binormal.z =  normal.y;
            }
            binormal = normalize(binormal);
            tangent = cross(binormal, normal);

            float U = dot(IN.worldPos, tangent)  * _BakeryProjectionTilingOffset.x + _BakeryProjectionTilingOffset.z;
            float V = dot(IN.worldPos, binormal) * _BakeryProjectionTilingOffset.y + _BakeryProjectionTilingOffset.w;

            float2 uv = float2(U, 1-V);
            float2 dx = ddx(uv);
            float2 dy = ddy(uv);

            float3 pos = tangent * -_BakeryProjectionTilingOffset.z/_BakeryProjectionTilingOffset.x + binormal * -_BakeryProjectionTilingOffset.w/_BakeryProjectionTilingOffset.y;
            pos += tangent * 0.5/_BakeryProjectionTilingOffset.x;
            pos += binormal * 0.5/_BakeryProjectionTilingOffset.y;

            tangent *= _BakeryProjectionTilingOffset.x * 2;
            binormal *= _BakeryProjectionTilingOffset.y * 2;
            float depth = 1000.0f;
            normal /= depth * 0.5f;

            float3x3 rotation = float3x3(tangent.x, tangent.y, tangent.z,
                                        binormal.x, binormal.y, binormal.z,
                                        normal.x, normal.y, normal.z);

            float3 viewDir = normalize(IN.worldPos - _WorldSpaceCameraPos);
            float3 ro = mul(rotation, _WorldSpaceCameraPos - pos);
            float3 rd = mul(rotation, viewDir);
            float2 box = BoxIntersection(ro, rd, 1);

            float2 inside = saturate((1-abs(uv*2-1))*1000);

            o.Emission = tex2Dgrad(_BakeryProjectionMap, frac(uv), dx, dy) *  max(max(saturate(max(box.x, box.y)) * 0.5f, 0.25f), inside.x*inside.y);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
