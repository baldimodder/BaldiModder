using BaldiModder.Data;
using BaldiModder.Runtime.Entities;

namespace BaldiModder.Runtime.BBIEAL.Entities {
    [RunAlongSide("BaldiTutor")]
    [EntityDefinition("BaldiTutor")]
    public class BaldiTutorEntity : AnimatedEntity {

        [ConfigurableEntityField] public string WaveAnimation { get; set; }

        private void Start() {
            if (OverrideAnimation) {
                animationController.PlayAnimation(WaveAnimation, AnimationPlayStyle.GoToIdleState);
            }
        }

    }
}
