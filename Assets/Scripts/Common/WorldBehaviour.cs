using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Experiments.Common {
    public class WorldBehaviour : MonoBehaviour {
        private WorldWrapper _world = null;

        public EntityManager EntityManager => _world.EntityManager;

        private void Awake() {
            _world = new WorldWrapper(name);
        }

        private void OnDestroy() {
            _world?.Dispose();
        }

        public T GetExistingSystem<T>() where T : ComponentSystemBase {
            return _world.GetExistingSystem<T>();
        }

        public T GetOrCreateSystem<T>(Type parent) where T : ComponentSystemBase {
            return _world.GetOrCreateSystem<T>(parent);
        }

        public T GetOrCreateSystem<T>(ComponentSystemGroup group) where T : ComponentSystemBase {
            return _world.GetOrCreateSystem<T>(group);
        }
    }

    public class WorldWrapper : IDisposable {
        private readonly World _world;

        private readonly Dictionary<ComponentSystemBase, Type> _topLevelSystems = new Dictionary<ComponentSystemBase, Type>();

        public EntityManager EntityManager => _world.EntityManager;

        public WorldWrapper(string name) {
            _world = new World(name);
        }

        public T GetExistingSystem<T>() where T : ComponentSystemBase {
            return _world.GetExistingSystem<T>();
        }

        public T GetOrCreateSystem<T>(Type parent) where T : ComponentSystemBase {
            bool hasSystem = _world.GetExistingSystem<T>() != null;

            T system = _world.GetOrCreateSystem<T>();

            if (!hasSystem) {
                _topLevelSystems[system] = parent;
                PlayerLoopUtil.AddSubSystem(parent, system);
            }

            return system;
        }

        public T GetOrCreateSystem<T>(ComponentSystemGroup group) where T : ComponentSystemBase {
            T system = _world.GetOrCreateSystem<T>();

            group.AddSystemToUpdateList(system);

            return system;
        }

        public void Dispose() {
            foreach (KeyValuePair<ComponentSystemBase,Type> systemEntry in _topLevelSystems) {
                PlayerLoopUtil.RemoveSubSystem(systemEntry.Value, systemEntry.Key);
            }

            _world.Dispose();
        }
    }
}
