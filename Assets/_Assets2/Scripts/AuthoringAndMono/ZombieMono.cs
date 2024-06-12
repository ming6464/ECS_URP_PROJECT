using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class ZombieMono : MonoBehaviour
{
    public int numberSpawn;
    public float speed;
    public int seedRandom;
    public GameObject zombiePrefab;
    public Transform XPointRandom;
    public Transform YPointRandom;
    public Transform OPointRandom;
}

public class Baker : Baker<ZombieMono>
{
    public override void Bake(ZombieMono authoring)
    {
        Entity zombieEntity = GetEntity(TransformUsageFlags.Dynamic);
        
        AddComponent(zombieEntity,new ZombieProperties
        {
            numberSpawn = authoring.numberSpawn,
            speed = authoring.speed,
            entity = GetEntity(authoring.zombiePrefab,TransformUsageFlags.Dynamic),
        });

        Vector3 pointO = authoring.OPointRandom.localPosition;
        Vector3 pointX = authoring.XPointRandom.localPosition;
        Vector3 pointY = authoring.YPointRandom.localPosition;

        float2 xMinMax = new float2(pointO.x, pointX.x);
        float2 yMinMax = new float2(pointO.y, pointY.y);
        float2 zMinMax = new float2(pointX.z, pointY.z);
        Random random = new Random((uint)authoring.seedRandom);
        
        AddComponent(zombieEntity,new ZombieRandom()
        {
            value = random,
            XMinMax = xMinMax,
            YMinMax = yMinMax,
            ZMinMax = zMinMax,
        });
    }
}