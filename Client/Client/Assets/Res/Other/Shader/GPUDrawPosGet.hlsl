
StructuredBuffer<float2> _pCb;

void GetPos_float(float InstanceID, out float3 Pos)
{
	int index = round(InstanceID);
	float2 v2 = _pCb[index];
	Pos = float3(v2.x, 0, v2.y);
}