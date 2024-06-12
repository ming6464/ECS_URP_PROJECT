using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct ZombieSystem : ISystem
{
    private float _finishSpawn;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _finishSpawn = 0;
        state.RequireForUpdate<ZombieProperties>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (_finishSpawn > 0) return;

        if (!SystemAPI.TryGetSingletonEntity<ZombieProperties>(out Entity entitytry)) return;
        var entity = SystemAPI.GetSingletonEntity<ZombieProperties>();
        ZombieProperties zombieProperties = SystemAPI.GetComponentRO<ZombieProperties>(entity).ValueRO;
        ZombieRandom zombieRandom = SystemAPI.GetComponentRO<ZombieRandom>(entity).ValueRO;
        LocalTransform localTf = SystemAPI.GetComponentRO<LocalTransform>(entity).ValueRO;
        float3 localPos = localTf.Position;
        int numberSpawn = zombieProperties.numberSpawn;
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        for (int i = 0; i < numberSpawn; i++)
        {
            var entityNew = ecb.Instantiate(zombieProperties.entity);
            ecb.AddComponent(entityNew, new LocalTransform()
            {
                Position = GetPositionRandom(i, zombieRandom, localPos),
                Rotation = localTf.Rotation,
                Scale = localTf.Scale,
            });
            ecb.AddComponent(entityNew, new ZombieMoveDirection
            {
                direction = new float3(0, 0, 1) // Hướng di chuyển mặc định là phía trước (trục Z)
            });
        }

        ecb.Playback(state.EntityManager);

        _finishSpawn = 1;
    }


    private float3 GetPositionRandom(float i, ZombieRandom ro, float3 localPos)
    {
        float3 min = localPos - new float3(ro.XMinMax.x, ro.YMinMax.x, ro.ZMinMax.x);
        float3 max = localPos + new float3(ro.XMinMax.y, ro.YMinMax.y, ro.ZMinMax.y);
        return ro.value.NextFloat3(min - i, max + i);
    }
}