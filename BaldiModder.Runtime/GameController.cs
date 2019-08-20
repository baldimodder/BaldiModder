using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using BaldiModder.Data;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BaldiModder.Runtime {
    public class GameController : MonoBehaviour {

        #region Variables

        public static GameController Instance { get; private set; }

        public List<int> AddedMaterialReplacerTo { get; set; }
        public List<int> HandledObjects { get; set; }

        private float lastBaldiWaitTime = 0.0f;
        private float lastBaldiWaitTimeMultiplied = 0.0f;

        private int oldCount = 0;

        private List<Type> runAlongSideTypes = GetTypesWithRunAlongSideAttribute(Master.RuntimeAssembly);

        #endregion

        #region Unity Events

        private void Start() {
            AssetManager.UpdateCursor();

            AddedMaterialReplacerTo = new List<int>();
            HandledObjects = new List<int>();

            for (int i = 0; i < SceneManager.sceneCount; i++) {
                Debug.Log(SceneManager.GetSceneAt(i).name);
            }

            if (FindObjectsOfType<GameController>().Length > 1) {
                Debug.Log("There is already one GameController.");
                Destroy(gameObject);
            }

            Instance = this;

            Debug.Log("GameController has been instantiated as " + name + ".");

            DontDestroyOnLoad(gameObject);

            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        private void Update() {
            if (Master.DebugMode) {
                if (Master.VersionData.SceneNameIs(SceneManager.GetActiveScene(), "School")) {
                    if (Input.GetKey(KeyCode.O)) {
                        try {
                            GetNotebook();
                        } catch (Exception e) {
                            Debug.Log($"FAILED TO OPEN NOTEBOOK!\n{e.ToString()}");
                        }
                    }

                    try {
                        if (Input.GetKeyDown(KeyCode.F11)) Master.VersionData.ClassData["Playtime"].InvokeMethod("TargetPlayer", FindObjectOfType(Master.VersionData.GetType("Playtime")), new object[] { });
                    } catch { }

                    if (Input.GetKeyDown(KeyCode.U)) Master.VersionData.ClassData["GameController"].InvokeMethod("CollectItem", FindObjectOfType(Master.VersionData.GetType("GameController")), new object[] { 9 });
                }

                if (Input.GetKey(KeyCode.P)) {
                    SceneManager.LoadScene(Master.VersionData.SceneNames["Win"]);
                }

                if (Input.GetKey(KeyCode.I)) {
                    SceneManager.LoadScene(Master.VersionData.SceneNames["GameOver"]);
                }
            }
        }

        private void LateUpdate() {
            int count = FindObjectsOfType<GameObject>().Length; //Get the number of GameObjects.
            if (count != oldCount) { //Compare the counts.
                HandleAllObjects(); //Handle the objects.
                oldCount = count; //Set the old count.
            }
        }

        private void SceneManager_activeSceneChanged(Scene before, Scene after) {
            Debug.Log(after.name);

            AddedMaterialReplacerTo.Clear();

            /*
             * I really wish I could use switch statements for this,
             * but even though the scene names are the same in every build,
             * we gotta prepare for changes.
             */

            if (Master.VersionData.SceneNameIs(after, "MainMenu")) {
                foreach (AudioSource source in FindObjectsOfType<AudioSource>()) {
                    Debug.Log(source.name + " " + source.clip.name);
                    if (Master.VersionData.GameObjectNameIs(source.gameObject, "IntroMusic")) {
                        source.Stop(); //Stop the audio source.
                        source.clip = AssetManager.GetAudioClip(AssetManager.Config.TitleScreenMusic.AudioFile);
                        source.loop = AssetManager.Config.TitleScreenMusic.Loop;
                        source.Play();
                    }

                    if (Master.VersionData.GameObjectNameIs(source.gameObject, "BaldiIntro") && !AssetManager.Config.TitleScreenMusic.BaldiIntro) {
                        source.enabled = false;
                    }
                }
            } else if (Master.VersionData.SceneNameIs(after, "School")) {
                //Lock the cursor.
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            } else if (Master.VersionData.SceneNameIs(after, "Win")) {
                //This used to have a purpose, now it doesn't, it's only here for reference.
            } else if (Master.VersionData.SceneNameIs(after, "GameOver")) {
                if (AssetManager.Config.GameOver.ReplaceDefaultGameOver) {
                    foreach (object obj in GetObjectsOfType("GameOver")) {
                        ((MonoBehaviour)obj).gameObject.AddComponent<GameOver>();
                        Destroy((MonoBehaviour)obj);
                    }
                }
            }
        }

        #endregion

        #region Events

        private void GetNotebook() {
            ClassData notebookClass = Master.VersionData.ClassData["Notebook"];
            ClassData mathGameClass = Master.VersionData.ClassData["MathGame"];

            if (FindObjectOfType(mathGameClass.Type) != null) return;

            foreach (Component notebook in FindObjectsOfType(notebookClass.Type)) {
                if ((bool)notebookClass.GetField("Up").GetValue(notebook)) {
                    notebook.transform.position = new Vector3(0f, -20f, 0f);
                    notebookClass.GetField("Up").SetValue(notebook, false);

                    notebookClass.GetField("RespawnTime").SetValue(notebook, 120f);
                    Master.VersionData.ClassData["GameController"].InvokeMethod("CollectNotebook", FindObjectOfType(Master.VersionData.GetType("GameController")), new object[] { });

                    GameObject learningGame = Instantiate<GameObject>((GameObject)notebookClass.GetField("LearningGame").GetValue(notebook));
                    Component mathGame = learningGame.GetComponent(mathGameClass.Type);
                    mathGameClass.GetField("GameController").SetValue(mathGame, notebookClass.GetField("GameController").GetValue(notebook));
                    mathGameClass.GetField("Baldi").SetValue(mathGame, notebookClass.GetField("Baldi").GetValue(notebook));
                    mathGameClass.GetField("PlayerPosition").SetValue(mathGame, ((Transform)notebookClass.GetField("Player").GetValue(notebook)).position);
                    return;
                }
            }
        }

        private void HandleObject(GameObject gameObj) {
            foreach (Component obj in gameObj.GetComponents<Component>()) {

                if (obj.GetType() == typeof(AudioSource)) {
                    try {
                        ReplaceAudioSourceClip((AudioSource)obj);
                    } catch { }
                }

                if (obj.GetType() == typeof(SpriteRenderer)) {
                    //try {
                    SpriteRenderer spriteRenderer = (SpriteRenderer)obj;
                    spriteRenderer.sprite = AssetManager.ReplaceSprite(spriteRenderer.sprite);
                    //} catch { }
                }

                if (obj.GetType() == typeof(MeshRenderer)) {
                    AddMaterialReplaceToObject(obj.gameObject);
                }

                if (obj.GetType() == typeof(Image)) {
                    try {
                        Image image = (Image)obj;
                        image.sprite = AssetManager.ReplaceSprite(image.sprite);
                    } catch { }
                }

                if (obj.GetType() == typeof(RawImage)) {
                    try {
                        RawImage image = (RawImage)obj;
                        image.texture = AssetManager.ReplaceTexture(image.texture);
                    } catch { }
                }

                foreach (Type type in runAlongSideTypes) {
                    foreach (RunAlongSideAttribute runAlongSide in (RunAlongSideAttribute[])type.GetCustomAttributes(typeof(RunAlongSideAttribute), true)) {
                        if (obj.GetComponent(type) != null) continue;

                        if (Master.VersionData.GameObjectNameIs(gameObj, runAlongSide.TypeString)) {
                            obj.gameObject.AddComponent(type);
                        } else if (obj.GetType() == runAlongSide.Type) {
                            obj.gameObject.AddComponent(type);
                        }
                    }
                }

                foreach (FieldInfo field in obj.GetType().GetFields()) {
                    HandleField(field, obj);
                }

                foreach (FieldInfo field in obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)) {
                    HandleField(field, obj);
                }
            }
        }

        private void HandleAllObjects() {
            foreach (GameObject obj in FindObjectsOfType<GameObject>()) {
                if (HandledObjects.Contains(obj.GetInstanceID())) continue;
                HandledObjects.Add(obj.GetInstanceID());
                HandleObject(obj);
            }
        }

        private void AddTypeToObject(Component obj, string type, Type objectToAdd) {
            if (obj.GetType() == Master.VersionData.GetType(type) && obj.GetComponent(objectToAdd) == null) {
                obj.gameObject.AddComponent(objectToAdd);
            }
        }

        private void BaldiSpeedMultiplier(Component baldi) {
            FieldInfo field = Master.VersionData.ClassData["Baldi"].GetField("WaitTime");
            float waitTime = (float)field.GetValue(baldi);

            if (waitTime != lastBaldiWaitTime && waitTime != lastBaldiWaitTimeMultiplied) {
                lastBaldiWaitTime = waitTime;
                lastBaldiWaitTimeMultiplied = waitTime * AssetManager.Config.BaldiSpeedMultiplier;

                waitTime = lastBaldiWaitTimeMultiplied;

                field.SetValue(baldi, waitTime);
            }
        }

        #endregion

        private static List<Type> GetTypesWithRunAlongSideAttribute(Assembly assembly) {
            return assembly.GetTypes().Where(type => type.IsDefined(typeof(RunAlongSideAttribute), false)).ToList();
        }

        private void HandleField(FieldInfo field, object obj) {
            try {
                if (field.FieldType == typeof(AudioClip)) {
                    field.SetValue(obj, AssetManager.ReplaceAudioClip((AudioClip)field.GetValue(obj)));
                }

                if (field.FieldType == typeof(AudioClip[])) {
                    AudioClip[] clips = (AudioClip[])field.GetValue(obj);

                    for (int i = 0; i < clips.Length; i++) {
                        clips[i] = AssetManager.ReplaceAudioClip(clips[i]);
                    }

                    field.SetValue(obj, clips);
                }
            } catch { }
        }

        public void AddMaterialReplaceToObject(GameObject obj) {
            if (obj.GetComponent<MaterialReplacer>() == null && !AddedMaterialReplacerTo.Contains(obj.GetInstanceID())) {
                obj.AddComponent<MaterialReplacer>();
                AddedMaterialReplacerTo.Add(obj.gameObject.GetInstanceID());
            }
        }

        public static UnityEngine.Object[] GetObjectsOfType(string classKey) {
            return FindObjectsOfType(Master.VersionData.GetType(classKey));
        }

        public static void ReplaceAudioSourceClip(AudioSource audioSource) {
            bool wasPlaying = audioSource.isPlaying; //Set the AudioSource's isPlaying value to a variable.
            audioSource.clip = AssetManager.ReplaceAudioClip(audioSource.clip); //Set the clip.

            if (wasPlaying && !audioSource.isPlaying) audioSource.Play();
            if (!wasPlaying && audioSource.isPlaying) audioSource.Play();
        }

    }
}
