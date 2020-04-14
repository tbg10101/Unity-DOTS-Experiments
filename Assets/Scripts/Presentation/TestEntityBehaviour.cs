using Experiments.Common;
using Experiments.Common.Data;
using Unity.Entities;
using UnityEngine;

namespace Experiments.Presentation {
    public class TestEntityBehaviour : EntityMonoBehaviour {
        public bool DestroyMe = false;

        protected override void OnUpdate() {
            base.OnUpdate();

            transform.localPosition = EntityManager.GetComponentData<PositionComponentData>(Entity).PresentationValue;

            if (DestroyMe) {
                if (Entity == Entity.Null) {
                    Debug.LogWarning($"{name} has a null entity when trying to be destroyed!");
                }

                Bootstrapper.Instance.Destroy(Entity);
            }
        }
    }
}
