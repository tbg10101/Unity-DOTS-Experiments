using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Experiments.Common.Data {
    [Serializable]
    public struct VelocityComponentData : IComponentData {
        public float3 Value;
    }
}
