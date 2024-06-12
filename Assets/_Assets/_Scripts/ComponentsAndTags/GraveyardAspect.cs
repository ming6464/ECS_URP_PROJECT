using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


public struct UniformTransform
{
    public float3 Position;
    public quaternion Rotation;
    public float Scale;

    public float4x4 Value()
    {
        return MathExt.TRSToMatrix(Position, Rotation, new float3(1, 1, 1) * Scale);
    }
}

public readonly partial struct GraveyardAspect : IAspect
{
    public readonly Entity entity;
    private readonly RefRO<GraveyardAndProperties> _graveyardAndProperties;
    private readonly RefRW<GraveyardRandom> _graveyardRandom;
    public readonly RefRO<LocalToWorld> worldTransform;
    public readonly RefRO<LocalTransform> localTransform;

    public int GetNumberSpawn => _graveyardAndProperties.ValueRO.NumberTombstonesToSpawn;
    public Entity GetEntityPrefab => _graveyardAndProperties.ValueRO.TombstonePrefab;

    public UniformTransform GetRandomTransform(int i, bool check)
    {
        return (new UniformTransform
        {
            Position = check ? GetRandomPosition1() : localTransform.ValueRO.Position,
            Rotation = quaternion.identity,
            Scale = 1,
        });
    }

    public float3 GetRandomPosition(int seed)
    {
        return MathExt.GetRandomRange((uint)seed, MinCorner, MaxCorner);
    }

    public float3 GetRandomPosition1()
    {
        return localTransform.ValueRO.Position + new float3(3, 3, 3);
    }

    private float3 MinCorner => new float3(_graveyardRandom.ValueRO.XMinMax.x, _graveyardRandom.ValueRO.ZMinMax.x,
        _graveyardRandom.ValueRO.ZMinMax.x);

    private float3 MaxCorner => new float3(_graveyardRandom.ValueRO.XMinMax.y, _graveyardRandom.ValueRO.YMinMax.y,
        _graveyardRandom.ValueRO.ZMinMax.y);
}