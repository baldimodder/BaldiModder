using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

namespace BaldiModder.Runtime {
    public class MaterialReplacer : MonoBehaviour {

        private new MeshRenderer renderer;

        private bool disabled = false;

        private void Start() {
            if (GetComponents<MaterialReplacer>().Length > 1) {
                Destroy(this);
            }

            int materialsToChange = 0;
            bool failed = false;
            List<string> materials = new List<string>();

            try {
                renderer = GetComponent<MeshRenderer>();

                foreach (Material material in renderer.materials) {
                    try {
                        if (AssetManager.TextureReplacements.ContainsKey(material.mainTexture.name)) {
                            material.mainTexture = AssetManager.ReplaceTexture((Texture2D)material.mainTexture);
                            materialsToChange++;
                            materials.Add($"Renderer:{material.mainTexture.name}");
                        }
                    } catch { }
                }

                foreach (Component component in GetComponents<Component>()) {
                    foreach (FieldInfo field in component.GetType().GetFields()) {
                        try {
                            if (field.FieldType == typeof(Material)) {
                                Material baseMaterial = (Material)field.GetValue(component);
                                if (AssetManager.TextureReplacements.ContainsKey(baseMaterial.mainTexture.name)) {
                                    baseMaterial.mainTexture = AssetManager.ReplaceTexture((Texture2D)baseMaterial.mainTexture);
                                    materialsToChange++;
                                    materials.Add($"Field:{component.GetType().Name}:{field.FieldType.Name}:{baseMaterial.mainTexture}");
                                    field.SetValue(component, baseMaterial);
                                }
                            }
                        } catch { }
                    }
                }
            } catch {
                failed = true;
            }

            Destroy(this);

            if (failed || materialsToChange <= 0) {
                string text = $"Destroying Material Replacer. New Material Replacer Count: {FindObjectsOfType<MaterialReplacer>().Length - 1}";

                foreach (string str in materials) {
                    text += $"\n{str}";
                }

                disabled = true;
            }
        }
    }
}
