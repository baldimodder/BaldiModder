using System;
using System.Reflection;

using UnityEngine;

namespace BaldiModder.Runtime {
    public class FieldReplacer {

        public string ObjectTypeKey { get; set; }

        public Type ObjectType {
            get {
                return Master.VersionData.ClassData[ObjectTypeKey].Type;
            }
        }

        public FieldReplacer(string type) {
            ObjectTypeKey = type;
        }

        public void ReplaceAll(object obj) {
            foreach (string field in Master.VersionData.ClassData[ObjectTypeKey].Fields.Keys) {
                try {
                    Replace(obj, field);
                } catch (Exception e) {
                    Debug.Log($"Failed to replace the field {field} ({Master.VersionData.ClassData[ObjectTypeKey].Fields[field].Name}) in {ObjectTypeKey}! {e.ToString()}");
                }
            }
        }

        public void Replace(object obj, string fieldName) {
            FieldInfo field = ObjectType.GetField(fieldName);
            object value = field.GetValue(obj);

            /* 
             * Another case of "if, else if, else if" because Type objects are not constant.
             * However, it isn't even that big of an issue anyway, so I think we can get away with this.
             */

            if (field.GetType() == typeof(AudioClip)) {
                AudioClip audioClip = (AudioClip)field.GetValue(obj);
                field.SetValue(obj, AssetManager.ReplaceAudioClip(audioClip));
                return;
            } else if (field.GetType() == typeof(AudioClip[])) {
                AudioClip[] audioClips = (AudioClip[])field.GetValue(obj);

                for (int i = 0; i < audioClips.Length; i++) {
                    audioClips[i] = AssetManager.ReplaceAudioClip(audioClips[i]);
                }

                field.SetValue(obj, audioClips);
                return;
            }
        }

    }
}
