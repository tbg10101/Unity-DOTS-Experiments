using Experiments.Common;
using Experiments.Common.Data;
using Unity.Entities;
using Unity.Mathematics;

namespace Experiments.Presentation {
    [UpdateBefore(typeof(PreUpdatePresentationEntityCommandBufferSystem))]
    public class PositionPresentationSystem : SystemBase {
        protected override void OnUpdate() {
            float presentationFraction = TimeUtil.PresentationTimeFraction; // this is done just once instead of once per instance

            Entities
                .ForEach((ref PositionComponentData component) => {
                    component.PresentationValue = math.lerp(component.PreviousValue, component.NextValue, presentationFraction);
                })
                .ScheduleParallel();
        }
    }
}
