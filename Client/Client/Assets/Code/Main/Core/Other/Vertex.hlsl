

void Vertex_float(float3 worldPosition, float2 waveSpeed, float waveHeight, float waveInterval, out float3 outWorldPosition,out float3 normal)
{
    float time = _Time.y * waveSpeed.x;
    float3 totalOffset = float3(0, 0, 0);
    float3 grad = float3(0, 0, 0);
		
    for (int i = 0; i < 10; i++)
    {
        float goldenAngle = 2.39996323; // radians
        float angle = i * goldenAngle;
        float2 dir = float2(cos(angle), sin(angle));

        float k = 2 * 3.1415926 / waveInterval * pow(1.18, i - 1);
        float a = 0.2 * waveHeight * pow(0.82, i - 1) / k;
        float c = sqrt(9.8 / k);

        float f = k * (dot(dir, worldPosition.xz) - c * time);

        totalOffset.x += dir.x * a * cos(f);
        totalOffset.y += a * sin(f);
        totalOffset.z += dir.y * a * cos(f);

        grad.x += -dir.x * a * k * cos(f);
        grad.z += -dir.y * a * k * cos(f);
    }
    outWorldPosition = worldPosition + totalOffset;
    
    normal = normalize(float3(grad.x, 1.0, grad.z));
}