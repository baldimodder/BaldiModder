using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace BaldiModder.Runtime {
    public class GameOver : MonoBehaviour {

        private AudioSource audioSource;

        private bool canPressAnyKeyToContinue = false;

        private void Start() {
            if (AssetManager.Config.GameOver.EnableMusic) {
                AudioSource existingAudioSource = GetComponent<AudioSource>();
                audioSource = existingAudioSource ?? gameObject.AddComponent<AudioSource>();
                audioSource.clip = AssetManager.GetAudioClip(AssetManager.Config.GameOver.MusicToPlay);
                audioSource.loop = AssetManager.Config.GameOver.LoopMusic;
                audioSource.Play();
            }

            StartCoroutine(GoToTitleScreenRoutine());
            StartCoroutine(CanPressAnyKeyToContinueRoutine());
        }

        private void Update() {
            if (AssetManager.Config.GameOver.AnyKeyGoesToTitleScreen && canPressAnyKeyToContinue && Input.anyKey) {
                GoToTitleScreen();
            }
        }

        private IEnumerator CanPressAnyKeyToContinueRoutine() {
            yield return new WaitForSeconds(1);
            canPressAnyKeyToContinue = true;
        }

        private IEnumerator GoToTitleScreenRoutine() {
            yield return new WaitForSeconds(AssetManager.Config.GameOver.AmountOfTimeToShow);
            GoToTitleScreen();
        }

        private void GoToTitleScreen() {
            SceneManager.LoadScene(Master.VersionData.SceneNames["MainMenu"]);
        }

    }
}
