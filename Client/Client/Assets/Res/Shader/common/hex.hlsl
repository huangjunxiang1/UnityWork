
#define hex_HexWidth 1.5

static const int2 Hex_QuadHalfSize = int2(10, 10);
static const int2 Hex_QuadSize = Hex_QuadHalfSize * 2 + 1;
static const int Hex_GridCount = Hex_QuadSize.x * Hex_QuadSize.y;
static const float Hex_sqrt_3 = sqrt(3);

int2 Hex_GetGridxy(float3 pos)
{
    float yf = pos.z / (hex_HexWidth * Hex_sqrt_3 / 2.0);
    int y = (int) round(yf);

    int parity = y & 1;
    float xf = (pos.x + parity * hex_HexWidth / 2.0) / hex_HexWidth;
    int x = (int) round(xf);

    return int2(x, y);
}
float3 Hex_GetPositon(int2 xy)
{
    return float3(xy.x * hex_HexWidth - (xy.y & 1) * hex_HexWidth / 2.0, 0, xy.y * hex_HexWidth * Hex_sqrt_3 / 2.0);
}

int2 GetCenterGrid(int2 xy)
{
    xy += Hex_QuadHalfSize;
    int2 dis = Hex_QuadHalfSize * 2 + 1;
    int2 n = xy / dis;
    return int2(xy.x < 0 ? n.x - 1 : n.x, xy.y < 0 ? n.y - 1 : n.y) * dis;
}
static int2 GetQuadLocalxy(int2 xy)
{
    xy += Hex_QuadHalfSize;
    return xy % Hex_QuadSize;
}