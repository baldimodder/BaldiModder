using BaldiModder.Data;
using BaldiModder.Runtime.Entities;

namespace BaldiModder.Runtime.BBIEAL.Entities {
    [RunAlongSide("ArtsAndCrafters")]
    [EntityDefinition("ArtsAndCrafters")]
    public class ArtsAndCraftersEntity : AnimatedEntity {

        public bool Angry { get => (bool)GetFieldValue("Angry"); set => SetFieldValue("Angry", value); }
        public bool GettingAngry { get => (bool)GetFieldValue("GettingAngry"); set => SetFieldValue("GettingAngry", value); }

        public float Anger { get => (float)GetFieldValue("Anger"); set => SetFieldValue("Anger", value); }

        public float ForceShowTime { get => (float)GetFieldValue("ForceShowTime"); set => SetFieldValue("ForceShowTime", value); }

        //TODO: Angry animation.

    }
}
