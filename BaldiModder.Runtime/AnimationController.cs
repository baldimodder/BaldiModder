﻿using System;
using System.Collections;

using BaldiModder.Data;

using UnityEngine;
using UnityEngine.UI;

using Animation = BaldiModder.Data.Animation;

namespace BaldiModder.Runtime {
    public class AnimationController : MonoBehaviour {

        public string idleAnimationName;
        public bool forcePivot = false;
        public Vector2 pivot = Vector2.zero;

        public string CurrentAnimationName { get; private set; }
        public Animation CurrentAnimation { get; private set; }
        public int CurrentFrame { get; private set; }

        private SpriteRenderer spriteRenderer;
        private Image image;
        private Coroutine coroutine;

        private void Start() {
            CurrentAnimationName = "";

            if (GetComponent<SpriteRenderer>() != null) {
                spriteRenderer = GetComponent<SpriteRenderer>();
            } else if (GetComponentInParent<SpriteRenderer>() != null) {
                spriteRenderer = GetComponentInParent<SpriteRenderer>();
            } else if (GetComponentInChildren<SpriteRenderer>() != null) {
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            } else {
                Debug.Log($"No sprite renderer found on {gameObject.name}.");

                image = GetComponent<Image>();
            }
        }

        private void LateUpdate() {
            try {
                Texture2D spriteTexture;
                try {
                    spriteTexture = AssetManager.GetTexture(CurrentAnimation.Frames[CurrentFrame].ImageName);
                } catch {
                    string final = "";

                    foreach (string entry in AssetManager.ImageRepository.Files.Keys) {
                        final += $"{AssetManager.ImageRepository.Files[entry].Path} loaded as {entry}.";
                    }
                    return;
                }

                Vector2 originalPivot = Vector2.zero;

                try {
                    originalPivot = AssetManager.GetSpritePivot(GetCurrentSprite());
                } catch { }

                SetSprite(AssetManager.GetSprite(CurrentAnimation.Frames[CurrentFrame].ImageName,
                    forcePivot ? pivot : originalPivot,
                    AssetManager.GeneratePixelsPerUnit(GetCurrentSprite().pixelsPerUnit, spriteRenderer? spriteRenderer.size : GetCurrentSprite().rect.size,
                    new Vector2(spriteTexture.width, spriteTexture.height)))); //Get the sprite.
            } catch (Exception e) {
                Debug.Log(e.ToString());
            }
        }

        public Sprite GetCurrentSprite() {
            if (spriteRenderer != null) {
                return spriteRenderer.sprite;
            }

            if (image != null) {
                return image.sprite;
            }

            return null;
        }

        public void SetSprite(Sprite sprite) {
            if (spriteRenderer != null) {
                spriteRenderer.sprite = sprite;
            }

            if (image != null) {
                image.sprite = sprite;
            }
        }

        public void PlayAnimation(string animationName, AnimationPlayStyle playStyle, bool replayAnimation = false) {
            if (CurrentAnimationName == animationName && !replayAnimation) return;

            try {
                StopCoroutine(coroutine); //Stop the coroutine.
            } catch { }

            coroutine = StartCoroutine(PlayAnimationCoroutine(animationName, playStyle)); //Start the coroutine.
        }

        public void PlayIdleAnimation() {
            PlayAnimation(idleAnimationName, AnimationPlayStyle.Loop);
        }

        private IEnumerator PlayAnimationCoroutine(string animationName, AnimationPlayStyle playStyle) {
            Animation animation;

            try {
                animation = AssetManager.GetAnimation(animationName);
            } catch {
                Debug.Log($"Failed to get animation {animationName}!");
                yield break;
            }

            CurrentAnimation = animation;
            CurrentAnimationName = animationName;
            CurrentFrame = 0;

            int currentFrame = 0;
            int increment = 1;

            while (true) {
                try {
                    spriteRenderer.sprite = AssetManager.GetSprite(animation.Frames[currentFrame].ImageName, AssetManager.GetSpritePivot(GetCurrentSprite()), GetCurrentSprite().pixelsPerUnit); //Get the sprite.
                } catch { }
                currentFrame = Mathf.Clamp(currentFrame + increment, 0, animation.NumberOfFrames);

                CurrentFrame = currentFrame;

                if (currentFrame == animation.NumberOfFrames && increment == 1) {
                    switch (playStyle) {
                        case AnimationPlayStyle.Loop:
                            currentFrame = 0;
                            break;
                        case AnimationPlayStyle.StayOnLastFrame:
                            yield break;
                        default:
                            increment = -1; //Go back.
                            break;
                    }
                } else if (currentFrame == 0 && increment == -1) {
                    switch (playStyle) {
                        case AnimationPlayStyle.Loop:
                            increment = 1; //Loop the animation.
                            break;
                        case AnimationPlayStyle.PingPongToIdleState:
                            PlayIdleAnimation(); //Play the idle animation.
                            break;
                        case AnimationPlayStyle.GoToIdleState:
                            PlayIdleAnimation();
                            break;
                    }
                }

                yield return new WaitForSeconds(animation.Interval); //Wait.
            }

            yield return null;
        }

    }
}
