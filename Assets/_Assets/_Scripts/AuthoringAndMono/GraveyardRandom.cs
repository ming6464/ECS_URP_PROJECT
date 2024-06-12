using Unity.Entities;
using Unity.Mathematics;

public struct GraveyardRandom : IComponentData
{
    public Random Value;
    public float2 XMinMax;
    public float2 YMinMax;
    public float2 ZMinMax;
}