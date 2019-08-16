using BaldiModder.Runtime.Entities;

namespace BaldiModder.Runtime.BBIEAL.Entities {
    [RunAlongSide("FirstPrize")]
    [EntityDefinition("FirstPrize")]
    public class FirstPrizeEntity : Entity {

        [ConfigurableEntityField] public float TurnSpeed { get => (float)GetFieldValue("TurnSpeed"); set => SetFieldValue("TurnSpeed", value); }
        [ConfigurableEntityField] public float NormalSpeed { get => (float)GetFieldValue("NormalSpeed"); set => SetFieldValue("NormalSpeed", value); }
        [ConfigurableEntityField] public float RunSpeed { get => (float)GetFieldValue("RunSpeed"); set => SetFieldValue("RunSpeed", value); }

        public float CurrentSpeed { get => (float)GetFieldValue("CurrentSpeed"); set => SetFieldValue("CurrentSpeed", value); }
        public float Speed { get => (float)GetFieldValue("Speed"); set => SetFieldValue("Speed", value); }

        public float Acceleration { get => (float)GetFieldValue("Acceleration"); set => SetFieldValue("Acceleration", value); }
        
        public float AngleDifference { get => (float)GetFieldValue("AngleDifference"); }
        
        public float AutoBrakeCool { get => (float)GetFieldValue("AutoBrakeCool"); set => SetFieldValue("AutoBrakeCool", value); }
        public float CoolDown { get => (float)GetFieldValue("CoolDown"); set => SetFieldValue("CoolDown", value); }

        public float CrazyTime { get => (float)GetFieldValue("CrazyTime"); set => SetFieldValue("CrazyTime", value); }

        public bool PlayerSeen { get => (bool)GetFieldValue("PlayerSeen"); }
        public bool HugAnnounced { get => (bool)GetFieldValue("HugAnnounced"); }

        //TODO: Add animation support for FirstPrize.

    }
}
