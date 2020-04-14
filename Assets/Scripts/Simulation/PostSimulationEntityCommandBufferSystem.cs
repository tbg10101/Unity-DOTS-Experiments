using Unity.Entities;

namespace Experiments.Simulation {
    [UpdateAfter(typeof(PreSimulationEntityCommandBufferSystem))]
    public class PostSimulationEntityCommandBufferSystem : EntityCommandBufferSystem { }
}
