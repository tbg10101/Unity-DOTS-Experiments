using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Experiments.Common.Data {
    [Serializable]
    public struct PositionComponentData : IComponentData {
        public float3 PreviousValue;
        public float3 PresentationValue;
        public float3 NextValue;
    }
}
