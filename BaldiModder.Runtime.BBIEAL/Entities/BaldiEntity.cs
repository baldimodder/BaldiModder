using BaldiModder.Data;
using BaldiModder.Runtime.Entities;

namespace BaldiModder.Runtime.BBIEAL.Entities {
    [RunAlongSide("Baldi")]
    [EntityDefinition("Baldi")]
    public class BaldiEntity : AnimatedEntity {

        [ConfigurableEntityField] public string SlapAnimation { get; set; }
        [ConfigurableEntityField] public float BaseTime { get => (float)GetFieldValue("BaseTime"); set => SetFieldValue("BaseTime", value); }
        [ConfigurableEntityField] public float Speed { get => (float)GetFieldValue("Speed"); set => SetFieldValue("Speed", value); }

        public float MoveFrames { get => (float)GetFieldValue("MoveFrames"); set => SetFieldValue("MoveFrames", value); }
        public float WaitTime { get => (float)GetFieldValue("WaitTime"); set => SetFieldValue("WaitTime", value); }
        public float TimeToMove { get => (float)GetFieldValue("TimeToMove"); set => SetFieldValue("TimeToMove", value); }

        public float Anger { get => (float)GetFieldValue("Anger"); set => SetFieldValue("Anger", value); }
        public float TempAnger { get => (float)GetFieldValue("TempAnger"); set => SetFieldValue("TempAnger", value); }
        public float AngerRate { get => (float)GetFieldValue("AngerRate"); set => SetFieldValue("AngerRate", value); }
        [ConfigurableEntityField] public float AngerRateRate { get => (float)GetFieldValue("AngerRateRate"); set => SetFieldValue("AngerRateRate", value); }
        [ConfigurableEntityField] public float AngerFrequency { get => (float)GetFieldValue("AngerFrequency"); set => SetFieldValue("AngerFrequency", value); }
        public float TimeToAnger { get => (float)GetFieldValue("TimeToAnger"); set => SetFieldValue("TimeToAnger", value); }

        [ConfigurableEntityField] public float SpeedScale { get => (float)GetFieldValue("SpeedScale"); set => SetFieldValue("SpeedScale", value); }

        public float Priority { get => (float)GetFieldValue("Priority"); set => SetFieldValue("Priority", value); }

        public bool AntiHearing { get => (bool)GetFieldValue("AntiHearing"); set => SetFieldValue("AntiHearing", value); }
        public float AntiHearingTime { get => (float)GetFieldValue("AntiHearingTime"); set => SetFieldValue("AntiHearingTime", value); }

        private void Update() {
            if (animator.GetBool(Master.VersionData.GetClassData("Baldi").Fields["SlapAnimationParameter"].Name) && OverrideAnimation) {
                animationController.PlayAnimation(SlapAnimation, AnimationPlayStyle.PingPongToIdleState);
            }
        }
    }
}
