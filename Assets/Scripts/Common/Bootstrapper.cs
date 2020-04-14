using System;
using Experiments.Common.Data;
using Experiments.Presentation;
using Experiments.Simulation;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using PresentationSystemGroup = Experiments.Presentation.PresentationSystemGroup;
using SimulationSystemGroup = Experiments.Simulation.SimulationSystemGroup;
using Random = Unity.Mathematics.Random;

namespace Experiments.Common {
    public class Bootstrapper : MonoBehaviour {
        private static Bootstrapper _instance = null;
        public static Bootstrapper Instance => _instance;

        [SerializeField]
        private WorldBehaviour _world = null;

        public ushort EntitiesToGenerate = 1;

        public float SpeedMultiplier = 1.0f;

        [SerializeField]
        private PrefabAndArchetype[] _prefabs = new PrefabAndArchetype[0];

        private void Awake() {
            _instance = this;
        }

        private void Start() {
            if (_world == null) {
                throw new Exception($"{nameof(Bootstrapper)} requires a world.");
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            SimulationSystemGroup simGroup = _world.GetOrCreateSystem<SimulationSystemGroup>(typeof(FixedUpdate));
            _world.GetOrCreateSystem<SimulationDestroySystem>(simGroup);
            _world.GetOrCreateSystem<PrefabSpawnSystem>(simGroup);
            _world.GetOrCreateSystem<PreSimulationEntityCommandBufferSystem>(simGroup);
            _world.GetOrCreateSystem<ApplyVelocitySystem>(simGroup);
            _world.GetOrCreateSystem<PostSimulationEntityCommandBufferSystem>(simGroup);

            PresentationSystemGroup presGroup = _world.GetOrCreateSystem<PresentationSystemGroup>(typeof(Update));
            _world.GetOrCreateSystem<PositionPresentationSystem>(presGroup);
            _world.GetOrCreateSystem<PreUpdatePresentationEntityCommandBufferSystem>(presGroup);
            _world.GetOrCreateSystem<ManagedMonoBehaviourUpdateSystem>(presGroup);
            _world.GetOrCreateSystem<PostUpdatePresentationEntityCommandBufferSystem>(presGroup);
            _world.GetOrCreateSystem<PresentationDestroySystem>(presGroup);
            // TODO add a new buffer system here to process the component removals

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            EntityManager entityManager = _world.EntityManager;

            foreach (PrefabAndArchetype p in _prefabs) {
                p.Archetype = p.ArchetypeProducer.Produce(entityManager);
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            Random r = new Random();
            r.InitState();

            for (ushort i = 0; i < EntitiesToGenerate; i++) {
                (Entity entity, EntityCommandBuffer ecb) = Create(0);
                ecb.SetComponent(entity, new VelocityComponentData {
                    Value = SpeedMultiplier * math.normalize(2.0f * (r.NextFloat3() - 0.5f)) * r.NextFloat()
                });
            }
        }

        private void OnDestroy() {
            if (ReferenceEquals(_instance, this)) {
                _instance = null;
            }
        }

        public EntityMonoBehaviour GetPrefab(int prefabIndex) {
            return _prefabs[prefabIndex].Prefab;
        }

        public (Entity, EntityCommandBuffer) Create(int prefabIndex)
        {
            EntityCommandBuffer entityCommandBuffer = _world
                .GetExistingSystem<PostUpdatePresentationEntityCommandBufferSystem>()
                .CreateCommandBuffer();

            Entity entity = entityCommandBuffer.CreateEntity(_prefabs[prefabIndex].Archetype);
            entityCommandBuffer.AddComponent(entity, new InitComponentData {
                PrefabIndex = prefabIndex
            });

            return (entity, entityCommandBuffer);
        }

        public void Destroy(Entity entity)
        {
            EntityCommandBuffer entityCommandBuffer = _world
                .GetExistingSystem<PostUpdatePresentationEntityCommandBufferSystem>()
                .CreateCommandBuffer();

            entityCommandBuffer.AddComponent(entity, new DestroyFlagComponentData());
        }

        /*
        public (Entity, EntityMonoBehaviour) CreateImmediate(
            EntityArchetype archetype,
            EntityMonoBehaviour prefab,
            string newName = null)
        {
            EntityManager entityManager = _world.EntityManager;

            Entity entity = entityManager.CreateEntity(archetype);
            EntityMonoBehaviour instance = Instantiate(prefab);

            if (newName != null) {
                entityManager.SetName(entity, newName);
                instance.name = newName;
            }

            entityManager.AddComponentData(entity, new GameObjectReferenceComponentData());
            instance.Entity = entity;
            instance.World = _world;

            return (entity, instance);
        }
        */

        [Serializable]
        private class PrefabAndArchetype {
            public ArchetypeProducer ArchetypeProducer = null;
            public EntityMonoBehaviour Prefab = null;
            [NonSerialized]
            public EntityArchetype Archetype;
        }
    }
}
