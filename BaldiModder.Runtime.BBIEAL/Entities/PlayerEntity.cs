using BaldiModder.Runtime.Entities;

namespace BaldiModder.Runtime.BBIEAL.Entities {
    [RunAlongSide("Player")]
    [EntityDefinition("Player")]
    public class PlayerEntity : Entity {

        public bool IsPlayingJumpRope { get => (bool)GetFieldValue("JumpRope"); }
        public bool IsBeingSweeped { get => (bool)GetFieldValue("Sweeping"); }
        public bool IsBeingHugged { get => (bool)GetFieldValue("Hugging"); }

        public float MouseSensitivity { get => (float)GetFieldValue("MouseSensitivity"); }

        public float Speed { get => (float)GetFieldValue("Speed"); set => SetFieldValue("Speed", value); }
        [ConfigurableEntityField] public float WalkSpeed { get => (float)GetFieldValue("WalkSpeed"); set => SetFieldValue("WalkSpeed", value); }
        [ConfigurableEntityField] public float RunSpeed { get => (float)GetFieldValue("RunSpeed"); set => SetFieldValue("RunSpeed", value); }
        [ConfigurableEntityField] public float SlowSpeed { get => (float)GetFieldValue("SlowSpeed"); set => SetFieldValue("SlowSpeed", value); }

        public float Stamina { get => (float)GetFieldValue("Stamina"); set => SetFieldValue("Stamina", value); }
        [ConfigurableEntityField] public float MaxStamina { get => (float)GetFieldValue("MaxStamina"); set => SetFieldValue("MaxStamina", value); }
        [ConfigurableEntityField] public float StaminaRate { get => (float)GetFieldValue("StaminaRate"); set => SetFieldValue("StaminaRate", value); }

        [ConfigurableEntityField] public float InitialGuilt { get => (float)GetFieldValue("InitGuilt"); set => SetFieldValue("InitGuilt", value); }
        public float Guilt { get => (float)GetFieldValue("Guilt"); set => SetFieldValue("Guilt", value); }

    }
}
