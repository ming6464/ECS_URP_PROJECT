using Unity.Entities;
using Unity.Mathematics;

// Thành phần lưu trữ thuộc tính của zombie
public struct ZombieProperties : IComponentData
{
    public int numberSpawn;
    public float speed;
    public Entity entity;
}

// Thành phần lưu trữ dữ liệu ngẫu nhiên cho zombie
public struct ZombieRandom : IComponentData
{
    public Random value;
    public float2 XMinMax;
    public float2 YMinMax;
    public float2 ZMinMax;
}

// Thành phần lưu trữ hướng di chuyển của zombie
public struct ZombieMoveDirection : IComponentData
{
    public float3 direction;
}