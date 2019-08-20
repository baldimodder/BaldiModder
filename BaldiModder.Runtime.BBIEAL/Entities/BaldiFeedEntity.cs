using BaldiModder.Data;
using BaldiModder.Runtime.Entities;

using UnityEngine;

namespace BaldiModder.Runtime.BBIEAL.Entities {
    [RunAlongSide("BaldiFeed")]
    [EntityDefinition("BaldiFeed")]
    public class BaldiFeedEntity : AnimatedEntity {

        [ConfigurableEntityField] public string TalkAnimation { get; set; }
        [ConfigurableEntityField] public string AngryAnimation { get; set; }

        private bool angry = false;

        private void Update() {
            if (!OverrideAnimation) return;

            //TODO: For some reason, the angry animation won't go past one or two frames. I'm too annoyed with this issue to fix it right now.
            //TODO: Implement the talk animation.

            if (animator.GetBool("angry")) {
                animationController.PlayAnimation(AngryAnimation, AnimationPlayStyle.StayOnLastFrame, false);
            }
        }

    }
}
