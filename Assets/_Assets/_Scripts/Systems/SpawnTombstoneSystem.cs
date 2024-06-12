using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnTombstoneSystem : ISystem
{
    //Structs

    private struct InfoCheckAvoid
    {
        public Entity entity;
        public float3 position;
        public float radius;
    }

    //Structs

    private bool _finishInstantiate;
    private NativeArray<InfoCheckAvoid> _infoCheckAvoids;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GraveyardAndProperties>(); // có ít nhất 1 GraveyardAndProperties thì mới chạy update
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        if (_finishInstantiate)
        {
            _infoCheckAvoids.Dispose();
        }
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!_finishInstantiate)
        {
            bool check = false;
            var graveyardEntity = SystemAPI.GetSingletonEntity<GraveyardAndProperties>(); // tìm entity 
            var graveyard = SystemAPI.GetAspect<GraveyardAspect>(graveyardEntity);
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            _infoCheckAvoids = new NativeArray<InfoCheckAvoid>(graveyard.GetNumberSpawn, Allocator.Persistent);
            int number = graveyard.GetNumberSpawn;
            for (int i = 0; i < number; i++)
            {
                // Tạo entity mới
                Entity entityNew = ecb.Instantiate(graveyard.GetEntityPrefab);
                var transRandom = graveyard.GetRandomTransform(i, check);
                ecb.SetComponent(entityNew, new LocalTransform()
                {
                    Position = transRandom.Position,
                    Rotation = transRandom.Rotation,
                    Scale = transRandom.Scale,
                });
                check = true;
            }


            ecb.Playback(state.EntityManager);
            ecb.Dispose();
            _finishInstantiate = true;
            return;
        }
    }
}