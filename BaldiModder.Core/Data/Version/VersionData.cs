using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace BaldiModder.Data {
    [Serializable]
    public class VersionData {

        public Version Version { get; set; }

        public Dictionary<string, string> GameObjectNames { get; set; }

        public Dictionary<string, ClassData> ClassData { get; set; }

        public Dictionary<string, string> SceneNames { get; set; }

        public Dictionary<string, DisallowReplacementData> NonReplaceableAudioFiles { get; set; }

        public VersionData() {
            NonReplaceableAudioFiles = new Dictionary<string, DisallowReplacementData>();
            GameObjectNames = new Dictionary<string, string>();
            ClassData = new Dictionary<string, ClassData>();
            SceneNames = new Dictionary<string, string>();
        }

        public void AddClassData(string name, Type type) {
            ClassData[name] = new ClassData(type);
        }

        public ClassData GetClassData(string name) {
            return ClassData[name];
        }

        public Type GetType(string name) {
            return ClassData[name].Type;
        }

        /// <summary>
        /// Check the name of the game object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool GameObjectNameIs(GameObject obj, string name) {
            return obj.name.ToLower() == GameObjectNames[name].ToLower();
        }

        /// <summary>
        /// Check the name of the scene.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool SceneNameIs(Scene scene, string name) {
            return scene.name == SceneNames[name];
        }

    }
}
