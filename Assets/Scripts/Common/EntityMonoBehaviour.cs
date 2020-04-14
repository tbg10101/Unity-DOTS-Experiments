using System.Collections.Generic;
using Unity.Entities;

#if UNITY_EDITOR && ENTITY_NAME_SYNC
using UnityEngine.Profiling;
#endif

namespace Experiments.Common {
    public abstract class EntityMonoBehaviour : ManagedMonoBehaviour {
        private static readonly Dictionary<Entity, EntityMonoBehaviour> Instances = new Dictionary<Entity, EntityMonoBehaviour>();

        private Entity _entity = Entity.Null;
        public Entity Entity {
            get => _entity;
            set {
                Instances.Remove(_entity);

                if (value != Entity.Null) {
                    Instances[value] = this;
                }

                _entity = value;
            }
        }

        public EntityManager EntityManager = null;

#if UNITY_EDITOR && ENTITY_NAME_SYNC
        private string _oldName = null;
#endif

        protected virtual void Awake() {
#if UNITY_EDITOR && ENTITY_NAME_SYNC
            _oldName = name;
#endif
        }

        public virtual void OnDestroy() {
            Instances.Remove(_entity);
        }

        protected override void OnUpdate() {
#if UNITY_EDITOR && ENTITY_NAME_SYNC
            Profiler.BeginSample("NameChange");
            if (name != _oldName) {
                EntityManager.SetName(_entity, name);
            }
            Profiler.EndSample();
#endif
        }

        public static EntityMonoBehaviour Get(Entity entity) {
            return Instances[entity];
        }

        public override void Destroy() {
            Entity = Entity.Null;
            EntityManager = null;

            base.Destroy();
        }
    }
}
