using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Experiments.Common {
    public abstract class ManagedMonoBehaviour : MonoBehaviour {
        private static readonly List<ManagedMonoBehaviour> Instances = new List<ManagedMonoBehaviour>();

        private bool _destroyed = false;

        protected virtual void OnEnable() {
            if (_destroyed) {
                return;
            }

            Instances.Add(this);
        }

        protected virtual void OnDisable() {
            if (_destroyed) {
                return;
            }

            Instances.Remove(this);
        }

        protected abstract void OnUpdate();

        public static void DoUpdate() {
            int i;
            int count = Instances.Count;

            for (i = 0; i < count; i++) {
                Profiler.BeginSample("ManagedMonoBehaviour.OnUpdate()");

                if (Instances[i]._destroyed) {
                    Debug.LogWarning($"{Instances[i].name} was destroyed but is still on this list of objects to update!");
                    continue;
                }

                Instances[i].OnUpdate();
                Profiler.EndSample();
            }
        }

        public virtual void Destroy() {
            _destroyed = true;
            Instances.Remove(this);
            Object.Destroy(gameObject);
        }
    }
}
