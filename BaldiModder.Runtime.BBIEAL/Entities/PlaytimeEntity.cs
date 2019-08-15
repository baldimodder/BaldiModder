using BaldiModder.Data;
using BaldiModder.Runtime.Entities;

namespace BaldiModder.Runtime.BBIEAL.Entities {
    [RunAlongSide("Playtime")]
    [EntityDefinition("Playtime")]
    public class PlaytimeEntity : AnimatedEntity {

        [ConfigurableEntityField] public string DisappointedAnimation { get; set; }

        public bool PlayerSeen { get => (bool)GetFieldValue("PlayerSeen"); }
        public bool IsDisappointed { get => (bool)GetFieldValue("IsDisappointed"); }

        public int AudioValue { get => (int)GetFieldValue("AudioValue"); set => SetFieldValue("AudioValue", value); }

        public float CoolDown { get => (float)GetFieldValue("CoolDown"); set => SetFieldValue("CoolDown", value); }
        public float PlayCooldown { get => (float)GetFieldValue("PlayCooldown"); set => SetFieldValue("PlayCooldown", value); }

        public bool PlayerSpotted { get => (bool)GetFieldValue("PlayerSpotted"); }
        public bool JumpRopeStarted { get => (bool)GetFieldValue("JumpRopeStarted"); }

        private void Update() {
            if (Master.VersionData.Version.Minor >= 3 && OverrideAnimation && animator.GetBool("disappointed")) {
                animationController.PlayAnimation(DisappointedAnimation, AnimationPlayStyle.GoToIdleState);
            }
        }

    }
}
