using Unity.Entities;
using UnityEngine;

namespace Experiments.Common {
    public abstract class ArchetypeProducer : ScriptableObject {
        public abstract EntityArchetype Produce(EntityManager entityManager);
    }
}
