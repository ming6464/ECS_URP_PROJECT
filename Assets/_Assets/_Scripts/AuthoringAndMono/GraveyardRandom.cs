using _Assets._Scripts.Math;
using Unity.Entities;
using Unity.Mathematics;

namespace _Assets._Scripts.AuthoringAndMono
{
    public struct GraveyardRandom : IComponentData
    {
        public Random Value;
        public float2 XMinMax;
        public float2 YMinMax;
        public float2 ZMinMax;
        
        
    }
}