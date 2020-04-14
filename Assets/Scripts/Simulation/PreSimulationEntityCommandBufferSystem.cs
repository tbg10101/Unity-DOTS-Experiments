using Unity.Entities;

namespace Experiments.Simulation {
    [UpdateBefore(typeof(PostSimulationEntityCommandBufferSystem))]
    public class PreSimulationEntityCommandBufferSystem : EntityCommandBufferSystem { }
}
