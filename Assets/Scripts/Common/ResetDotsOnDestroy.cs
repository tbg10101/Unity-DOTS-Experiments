using UnityEngine;

namespace Experiments.Common {
    public class ResetDotsOnDestroy : MonoBehaviour{
        private void OnDestroy() {
            DotsUtil.Reset();
        }
    }
}
