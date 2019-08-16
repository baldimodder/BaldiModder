using BaldiModder.Runtime.Entities;

namespace BaldiModder.Runtime.BBIEAL.Entities {
    [RunAlongSide("Bully")]
    [EntityDefinition("Bully")]
    public class BullyEntity : AnimatedEntity {

        public float ActiveTime { get => (float)GetFieldValue("ActiveTime"); set => SetFieldValue("ActiveTime", value); }
        public float WaitTime { get => (float)GetFieldValue("WaitTime"); set => SetFieldValue("WaitTime", value); }
        public float Guilt { get => (float)GetFieldValue("Guilt"); set => SetFieldValue("Guilt", value); }

        public bool Active { get => (bool)GetFieldValue("Active"); }
        public bool HasSpoken { get => (bool)GetFieldValue("HasSpoken"); }

    }
}
