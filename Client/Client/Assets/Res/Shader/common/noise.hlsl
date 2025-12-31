
#include "math.hlsl"

static float noise_snoise(float2 v)
{
    float4 C = float4(0.211324865405187f, // (3.0-math.sqrt(3.0))/6.0
                                  0.366025403784439f, // 0.5*(math.sqrt(3.0)-1.0)
                                 -0.577350269189626f, // -1.0 + 2.0 * C.x
                                  0.024390243902439f); // 1.0 / 41.0
            // First corner
    float2 i = floor(v + dot(v, C.yy));
    float2 x0 = v - i + dot(i, C.xx);

            // Other corners
    float2 i1;
            //i1.x = math.step( x0.y, x0.x ); // x0.x > x0.y ? 1.0 : 0.0
            //i1.y = 1.0 - i1.x;
    i1 = (x0.x > x0.y) ? float2(1.0f, 0.0f) : float2(0.0f, 1.0f);
            // x0 = x0 - 0.0 + 0.0 * C.xx ;
            // x1 = x0 - i1 + 1.0 * C.xx ;
            // x2 = x0 - 1.0 + 2.0 * C.xx ;
    float4 x12 = x0.xyxy + C.xxzz;
    x12.xy -= i1;

            // Permutations
    i = mod289(i); // Avoid truncation effects in permutation
    float3 p = permute(permute(i.y + float3(0.0f, i1.y, 1.0f)) + i.x + float3(0.0f, i1.x, 1.0f));

    float3 m = max(0.5f - float3(dot(x0, x0), dot(x12.xy, x12.xy), dot(x12.zw, x12.zw)), 0.0f);
    m = m * m;
    m = m * m;

            // Gradients: 41 points uniformly over a line, mapped onto a diamond.
            // The ring size 17*17 = 289 is close to a multiple of 41 (41*7 = 287)

    float3 x = 2.0f * frac(p * C.www) - 1.0f;
    float3 h = abs(x) - 0.5f;
    float3 ox = floor(x + 0.5f);
    float3 a0 = x - ox;

            // Normalise gradients implicitly by scaling m
            // Approximation of: m *= inversemath.sqrt( a0*a0 + h*h );
    m *= 1.79284291400159f - 0.85373472095314f * (a0 * a0 + h * h);

            // Compute final noise value at P

    float gx = a0.x * x0.x + h.x * x0.y;
    float2 gyz = a0.yz * x12.xz + h.yz * x12.yw;
    float3 g = float3(gx, gyz);

    return 130.0f * dot(m, g);
}

 
static float2 noise_cellular(float2 P)
{
    const float K = 0.142857142857f; // 1/7
    const float Ko = 0.428571428571f; // 3/7
    const float jitter = 1.0f; // Less gives more regular pattern

    float2 Pi = mod289(floor(P));
    float2 Pf = frac(P);
    float3 oi = float3(-1.0f, 0.0f, 1.0f);
    float3 of = float3(-0.5f, 0.5f, 1.5f);
    float3 px = permute(Pi.x + oi);
    float3 p = permute(px.x + Pi.y + oi); // p11, p12, p13
    float3 ox = frac(p * K) - Ko;
    float3 oy = mod7(floor(p * K)) * K - Ko;
    float3 dx = Pf.x + 0.5f + jitter * ox;
    float3 dy = Pf.y - of + jitter * oy;
    float3 d1 = dx * dx + dy * dy; // d11, d12 and d13, squared
    p = permute(px.y + Pi.y + oi); // p21, p22, p23
    ox = frac(p * K) - Ko;
    oy = mod7(floor(p * K)) * K - Ko;
    dx = Pf.x - 0.5f + jitter * ox;
    dy = Pf.y - of + jitter * oy;
    float3 d2 = dx * dx + dy * dy; // d21, d22 and d23, squared
    p = permute(px.z + Pi.y + oi); // p31, p32, p33
    ox = frac(p * K) - Ko;
    oy = mod7(floor(p * K)) * K - Ko;
    dx = Pf.x - 1.5f + jitter * ox;
    dy = Pf.y - of + jitter * oy;
    float3 d3 = dx * dx + dy * dy; // d31, d32 and d33, squared
            // Sort out the two smallest distances (F1, F2)
    float3 d1a = min(d1, d2);
    d2 = max(d1, d2); // Swap to keep candidates for F2
    d2 = min(d2, d3); // neither F1 nor F2 are now in d3
    d1 = min(d1a, d2); // F1 is now in d1
    d2 = max(d1a, d2); // Swap to keep candidates for F2
    d1.xy = (d1.x < d1.y) ? d1.xy : d1.yx; // Swap if smaller
    d1.xz = (d1.x < d1.z) ? d1.xz : d1.zx; // F1 is in d1.x
    d1.yz = min(d1.yz, d2.yz); // F2 is now not in d2.yz
    d1.y = min(d1.y, d1.z); // nor in  d1.z
    d1.y = min(d1.y, d2.x); // F2 is in d1.y, we're done.
    return sqrt(d1.xy);
}
