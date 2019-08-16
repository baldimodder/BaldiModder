using BaldiModder.Runtime.Entities;

namespace BaldiModder.Runtime.BBIEAL.Entities {
    [RunAlongSide("Sweep")]
    [EntityDefinition("Sweep")]
    public class SweepEntity : AnimatedEntity {

        [ConfigurableEntityField] public float TimeUntilActive { get; set; } = -1;

        public float CoolDown { get => (float)GetFieldValue("CoolDown"); set => SetFieldValue("CoolDown", value); }
        public float WaitTime { get => (float)GetFieldValue("WaitTime"); set => SetFieldValue("WaitTime", value); }

        public float Wanders { get => (float)GetFieldValue("Wanders"); set => SetFieldValue("Wanders", value); }

        public float Active { get => (float)GetFieldValue("Active"); set => SetFieldValue("Active", value); }

        private bool overridenCooldown = false;

        private void LateUpdate() {
            if (!overridenCooldown && TimeUntilActive != -1) {
                WaitTime = TimeUntilActive;
                overridenCooldown = true;
            }
        }

    }
}
