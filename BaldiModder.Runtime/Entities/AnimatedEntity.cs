using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BaldiModder.Runtime.Entities {
    public class AnimatedEntity : Entity {
        [ConfigurableEntityField] public bool OverrideAnimation { get; set; }
        [ConfigurableEntityField] public string IdleAnimation { get; set; }

        protected Animator animator;
        protected AnimationController animationController;

        protected override void Awake() {
            base.Awake();

            animator = GetComponentInChildren<Animator>();

            if (OverrideAnimation) {
                animationController = animator.gameObject.AddComponent<AnimationController>();

                animationController.idleAnimationName = IdleAnimation;

                //Clear all the Unity Animator's AnimationClips.
                foreach (AnimatorClipInfo clipInfo in animator.GetCurrentAnimatorClipInfo(0)) {
                    clipInfo.clip.ClearCurves();
                }

                SetPivot();
            }
        }

        protected void SetPivot() {
            if (ForcePivot) {
                try {
                    animationController.pivot = Pivot.AsVector2;
                    animationController.forcePivot = true;
                } catch {
                    if (animationController == null) Debug.Log($"Failed to set pivot of {gameObject.name} because it's missing an AnimationController.");
                }
            }
        }
    }
}
