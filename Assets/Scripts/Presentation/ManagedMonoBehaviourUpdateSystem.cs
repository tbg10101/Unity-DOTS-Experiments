using Experiments.Common;
using Unity.Entities;

namespace Experiments.Presentation {
    [UpdateAfter(typeof(PreUpdatePresentationEntityCommandBufferSystem))]
    public class ManagedMonoBehaviourUpdateSystem : SystemBase {
        protected override void OnUpdate() {
            ManagedMonoBehaviour.DoUpdate();
        }
    }
}
