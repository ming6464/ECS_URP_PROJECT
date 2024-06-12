using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class GraveyardMono : MonoBehaviour
{
    public float2 FiledDimensions;
    public int NumberTombstonesToSpawn;
    public GameObject TombstonePrefab;
    public Transform PositionY;
    public Transform PositionO;
    public Transform PositionX;
}

public class Graveyard : Baker<GraveyardMono>
{
    public override void Bake(GraveyardMono authoring)
    {
        Entity tombstoneEntity = GetEntity(authoring.TombstonePrefab, TransformUsageFlags.Dynamic);

        AddComponent(new GraveyardAndProperties
        {
            FieldDimensions = authoring.FiledDimensions,
            NumberTombstonesToSpawn = authoring.NumberTombstonesToSpawn,
            TombstonePrefab = tombstoneEntity,
        });


        float2 XMinMax = new float2(authoring.PositionO.position.x, authoring.PositionX.position.x);
        float2 YMinMax = new float2(authoring.PositionX.position.y, authoring.PositionY.position.y);
        float2 ZMinMax = new float2(authoring.PositionO.position.z, authoring.PositionY.position.z);

        AddComponent(new GraveyardRandom
        {
            Value = Unity.Mathematics.Random.CreateFromIndex(MathExt.GetSeedWithTime()),
            XMinMax = XMinMax,
            YMinMax = YMinMax,
            ZMinMax = ZMinMax,
        });
    }
}