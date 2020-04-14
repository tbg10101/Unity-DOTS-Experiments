using Experiments.Common;
using Experiments.Common.Data;
using Unity.Entities;

namespace Experiments.Simulation {
    [UpdateBefore(typeof(PostSimulationEntityCommandBufferSystem))]
    public class ApplyVelocitySystem : SystemBase {
        protected override void OnUpdate() {
            float dt = TimeUtil.FixedDeltaTime;

            Entities
                .WithNone<InitComponentData>()
                .ForEach((ref PositionComponentData component, in VelocityComponentData rate) => {
                    component.PreviousValue = component.NextValue;
                    component.NextValue += dt * rate.Value;
                })
                .ScheduleParallel();
        }
    }
}
