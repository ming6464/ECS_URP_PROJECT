using System.Security.Cryptography.X509Certificates;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct ZombieMoveSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ZombieProperties>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        ZombieProperties zombieProperties = SystemAPI.GetComponentRO<ZombieProperties>(SystemAPI.GetSingletonEntity<ZombieProperties>()).ValueRO;
        // EntityManager entityManager = state.EntityManager;
        // NativeArray<Entity> entities = entityManager.GetAllEntities();
        //
        // foreach (Entity entity in entities)
        // {
        //     if (entityManager.HasComponent<LocalTransform>(entity) && entityManager.HasComponent<ZombieMoveDirection>(entity))
        //     {
        //         ZombieMoveDirection moveDirection = entityManager.GetComponentData<ZombieMoveDirection>(entity);
        //         LocalTransform lt = entityManager.GetComponentData<LocalTransform>(entity);
        //         lt.Position += moveDirection.direction * zombieProperties.speed * deltaTime;
        //         entityManager.SetComponentData(entity, lt);
        //     }
        // }

        // foreach (var (moveDirection, lt) in SystemAPI.Query<RefRO<ZombieMoveDirection>, RefRW<LocalTransform>>())
        // {
        //     lt.ValueRW.Position += moveDirection.ValueRO.direction * zombieProperties.speed * deltaTime;
        // }

        MoveZombieJob job = new MoveZombieJob()
        {
            deltaTime = deltaTime,
            speed = zombieProperties.speed,
        };
        state.Dependency = job.ScheduleParallel(state.Dependency);

    }
    
    [BurstCompile]
    public partial struct MoveZombieJob : IJobEntity
    {
        [ReadOnly] public float speed;
        [ReadOnly] public float deltaTime;
        
        public void Execute(ref LocalTransform lt,in ZombieMoveDirection moveDirection)
        {
            lt.Position += moveDirection.direction * speed * deltaTime;
        }
    }
    
}