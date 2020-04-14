using Experiments.Common.Data;
using Unity.Entities;
using UnityEngine;

namespace Experiments.Common.Archetypes {
    [CreateAssetMenu(menuName = "Archetypes/" + nameof(TestArchetypeProducer))]
    public class TestArchetypeProducer : ArchetypeProducer {
        public override EntityArchetype Produce(EntityManager entityManager) {
            return entityManager.CreateArchetype(
                new ComponentType(typeof(PositionComponentData)),
                new ComponentType(typeof(VelocityComponentData), ComponentType.AccessMode.ReadOnly));
        }
    }
}
