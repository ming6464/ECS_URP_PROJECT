using Unity.Entities;
using Unity.Mathematics;

public struct GraveyardAndProperties : IComponentData
{
    public float2 FieldDimensions;
    public int NumberTombstonesToSpawn;
    public Entity TombstonePrefab;
}