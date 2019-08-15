using System;
using System.Collections;
using System.IO;

using BaldiModder.Data;
using BaldiModder.Data.IO;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BaldiModder.Runtime {
    public class LoadingScreen : MonoBehaviour {

        private const string modPathStart = "mod:";

        private bool failed = false;
        private Text text;

        private WWW www = null;

        private void Awake() {
            text = GetComponentInChildren<Text>();

            text.text = "";
            Master.VersionData = AssetManager.ReadData<VersionData>("version", false);
        }

        private void Start() {
            try {
                StartCoroutine(LoadAssets());
            } catch (Exception e) {
                HandleException(e);
            }
        }

        private void Update() {
            if (failed && Input.anyKey) {
                if (Input.GetKey(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                else Application.Quit();
            }
        }

        private IEnumerator LoadAssets() {
            yield return null;

            try {
                LoadingMessage(""); //Set the loading message.

                AssetManager.DataSerializer = new JsonDataSerializer();

                foreach (string arg in Environment.GetCommandLineArgs()) {
                    string toLower = arg.ToLower();

                    if (toLower.StartsWith(modPathStart)) {
                        AssetManager.FilePath = toLower.TrimStart(modPathStart.ToCharArray());
                    }

                    switch (toLower) {
                        case "verbose":
                            Master.VerboseMode = true;
                            break;
                        case "debug":
                            Master.DebugMode = true;
                            break;
                    }
                }

                try {
                    Debug.Log($"Current Directory: {Directory.GetCurrentDirectory()}");
                } catch (IndexOutOfRangeException) { }

                if (!Directory.Exists(AssetManager.FilePath)) AssetManager.FilePath = "default_mod";
                if (!Directory.Exists(AssetManager.FilePath)) throw LoadingException.modIsInvalid; //If the mod is invalid, throw an exception.

                Debug.Log("Mod folder is " + AssetManager.FilePath);

                AssetManager.Config = AssetManager.ReadData<Config>(AssetManager.configFilePath); //Get the config data.

                AssetManager.ModInfo = AssetManager.ReadData<ModInfo>(AssetManager.modInfoPath); //Get the mod info.

                if (AssetManager.Config.Cursor.UseSystemCursor) {
                    AssetManager.UpdateCursor();
                    Cursor.visible = true;
                } else {
                    Cursor.visible = false;
                }

                AssetManager.AudioRepository = AssetManager.ReadData<AudioFileRepository>(AssetManager.audioFileRepositoryPath); //Get the audio repository.
                AssetManager.ImageRepository = AssetManager.ReadData<ImageFileRepository>(AssetManager.imageFileRepositoryPath); //Get the image repository.
                AssetManager.AnimationRepository = AssetManager.ReadData<AnimationFileRepository>(AssetManager.animationFileRepositoryPath); //Get the animation repository.

                AssetManager.CheckSoundReplacements(); //Check the sound replacements.
                AssetManager.CheckTextureReplacements(); //Check the texture replacements.
                AssetManager.CheckSpriteReplacements(); //Check the sprite replacements.

                foreach (string soundReplacement in AssetManager.SoundReplacements.Keys) {
                    SoundReplacement data = AssetManager.SoundReplacements[soundReplacement];
                    if (Master.VersionData.NonReplaceableAudioFiles.ContainsKey(soundReplacement)) {
                        throw LoadingException.DisallowedReplacement(soundReplacement, Master.VersionData.NonReplaceableAudioFiles[soundReplacement]);
                    }
                }
            } catch (Exception e) {
                HandleException(e);
                yield break;
            }

            foreach (string audioFile in AssetManager.AudioRepository.Files.Keys) {
                string path;
                FileData data;

                try {
                    data = AssetManager.AudioRepository.Files[audioFile];
                } catch (Exception e) {
                    HandleException(e);
                    yield break;
                }

                yield return LoadFile(AssetManager.AudioRepository.GetPath(audioFile));

                try {
                    PreparingMessage(data.Path);
                    while (www.GetAudioClip(false).loadState != AudioDataLoadState.Loaded) { }
                    AssetManager.LoadedAudioFiles[audioFile] = www.GetAudioClip();
                } catch (Exception e) {
                    HandleException(e);
                    yield break;
                }
            }

            foreach (string imageFile in AssetManager.ImageRepository.Files.Keys) {
                string path;
                FileData data;

                try {
                    data = AssetManager.ImageRepository.Files[imageFile];
                } catch (Exception e) {
                    HandleException(e);
                    yield break;
                }

                yield return LoadFile(AssetManager.ImageRepository.GetPath(imageFile));

                try {
                    PreparingMessage(data.Path);

                    Texture2D tex = www.texture;

                    tex.filterMode = AssetManager.ImageRepository.Files[imageFile].FilterMode;
                    AssetManager.LoadedTextures[imageFile] = tex;

                    if (AssetManager.Config.Cursor.CursorImage == imageFile) {
                        AssetManager.UpdateCursor();
                        Cursor.visible = true;
                    }
                } catch (Exception e) {
                    HandleException(e);
                    yield break;
                }
            }

            foreach (string animation in AssetManager.AnimationRepository.Files.Keys) {
                FileData data;

                try {
                    AssetManager.LoadedAnimations[animation] = AssetManager.ReadData<Data.Animation>(AssetManager.AnimationRepository.GetPathInModFolder(animation), false);
                } catch (Exception e) {
                    HandleException(e);
                    yield break;
                }
            }

            try {
                text.text = AssetManager.Config.WarningText;

                GameObject controller = new GameObject("BaldiModder");
                controller.AddComponent<GameController>();

                AssetManager.UpdateCursor();
                Cursor.visible = true;

                gameObject.AddComponent(Master.VersionData.GetType("WarningScreen"));
            } catch (Exception e) {
                HandleException(e);
                yield break;
            }
        }

        private IEnumerator LoadFile(string path) {

            try {
                string fullPath = AssetManager.GetPathInModFolder(path);
                if (!AssetManager.FileExists(path)) {
                    throw LoadingException.FileDoesntExist(path);
                }
                LoadingMessage(Path.GetFileName(path)); //Set the loading message.
                Debug.Log("Loading " + Path.GetFileName(path) + "...");
                www = new WWW("file://" + Path.Combine(Directory.GetCurrentDirectory(), fullPath));
            } catch (Exception e) {
                HandleException(e);
                yield break;
            }

            yield return www;
        }

        private void LoadingMessage(string loading) {
            if (String.IsNullOrEmpty(loading) || !Master.VerboseMode) text.text = "Loading...";
            else text.text = "Loading " + loading + "...";
        }

        private void PreparingMessage(string loading) {
            LoadingMessage(loading);
        }

        private void HandleException(Exception e) {
            StopAllCoroutines();

            text.fontSize = 24;
            text.text = $"An error has occured:\n{e.Message}\n\nPress R to retry. Press anything else to quit.";
            Directory.CreateDirectory("errors");
            File.WriteAllText(Path.Combine("errors", string.Format("text-{0:yyyy-MM-dd_hh-mm-ss-tt}.log", DateTime.Now)), e.ToString());
            failed = true;
        }

    }

    public class LoadingException : Exception {

        public static readonly LoadingException modIsInvalid = new LoadingException("Mod is invalid.");

        public static LoadingException FileDoesntExist(string file) {
            return new LoadingException(file + " doesn't exist.");
        }

        public static LoadingException DisallowedReplacement(string name, DisallowReplacementData data) {
            //return new LoadingException(file + " doesn't exist.");
            string message = $"{name} cannot be replaced because ";

            switch (data.Reason) {
                case DisallowReplacementReason.AlternativeIsAvailable:
                    message += $"an alternative is available: {data.AlternativeMessage}";
                    break;
            }

            LoadingException loadingException = new LoadingException(message);
            return loadingException;
        }

        public LoadingException() : base() { }

        public LoadingException(string message) : base(message) { }

    }

}
