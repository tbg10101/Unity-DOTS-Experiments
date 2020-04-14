using Experiments.Common.Data;
using Unity.Entities;

namespace Experiments.Simulation {
    [UpdateBefore(typeof(PrefabSpawnSystem))]
    [UpdateBefore(typeof(PreSimulationEntityCommandBufferSystem))]
    public class SimulationDestroySystem : SystemBase {
        protected override void OnUpdate() {
            Entities
                .WithAll<DestroyFlagComponentData>()
                .WithNone<GameObjectFlagComponentData>()
                .WithStructuralChanges()
                .ForEach((Entity entity) => {
                    EntityManager.DestroyEntity(entity);
                })
                .Run();
        }
    }
}
