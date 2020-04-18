using Software10101.DOTS.Utils;
using UnityEngine;

namespace Software10101.DOTS.Example.MonoBehaviours {
    public class ResetDotsOnDestroy : MonoBehaviour{
        private void OnDestroy() {
            DotsUtil.Reset();
        }
    }
}
