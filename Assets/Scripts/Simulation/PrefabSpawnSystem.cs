using Experiments.Common;
using Experiments.Common.Data;
using Unity.Entities;
using UnityEngine;

namespace Experiments.Simulation {
    [UpdateAfter(typeof(SimulationDestroySystem))]
    [UpdateBefore(typeof(PreSimulationEntityCommandBufferSystem))]
    public class PrefabSpawnSystem : SystemBase {
        protected override void OnUpdate() {
            EntityCommandBuffer ecb = World.GetExistingSystem<PreSimulationEntityCommandBufferSystem>().CreateCommandBuffer();

            Entities
                .WithoutBurst()
                .ForEach((Entity entity, in InitComponentData component) => {
                    // TODO use a pool instead
                    EntityMonoBehaviour instance = Object.Instantiate(Bootstrapper.Instance.GetPrefab(component.PrefabIndex));

                    instance.Entity = entity;
                    instance.EntityManager = EntityManager;

                    EntityManager.SetName(entity, instance.name);

                    ecb.RemoveComponent<InitComponentData>(entity);
                    ecb.AddComponent(entity, new GameObjectFlagComponentData());
                })
                .Run(); // must be on the main thread
        }
    }
}
