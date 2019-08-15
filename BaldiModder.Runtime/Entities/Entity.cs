using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using BaldiModder.Data;

using UnityEngine;

namespace BaldiModder.Runtime.Entities {
    public class Entity : MonoBehaviour {

        public RunAlongSideAttribute runAlongSide;
        public EntityDefinitionAttribute entityDefinition;
        public Component baseObject;

        public string ConfigurationPath => $"entity/{entityDefinition.name}";

        [ConfigurableEntityField] public bool ForcePivot { get; set; }
        [ConfigurableEntityField] public Pivot Pivot { get; set; }

        protected virtual void Awake() {
            try {
                runAlongSide = (RunAlongSideAttribute)GetType().GetCustomAttributes(typeof(RunAlongSideAttribute), true)[0];
                baseObject = GetComponent(Master.VersionData.HasClassData(runAlongSide.TypeString) ? runAlongSide.Type : typeof(Transform));
            } catch (Exception e) {
                Debug.Log($"Failed to get the RunAlongSideAttribute or BaseObject of {GetType().Name} on {gameObject.name}!\n{e.ToString()}");
            }

            try {
                entityDefinition = (EntityDefinitionAttribute)GetType().GetCustomAttributes(typeof(EntityDefinitionAttribute), true)[0];
            } catch (Exception e) {
                Debug.Log($"Failed to get the EntityDefinition of {GetType().Name} on {gameObject.name}!\n{e.ToString()}");
            }

            if (!AssetManager.DataExists(ConfigurationPath)) {
                SaveConfig();
            }

            LoadConfig();

            Debug.Log($"Successfully added {GetType().Name} to {gameObject.name}!");
        }

        private void OnApplicationQuit() {
            //Null these out to prevent the game from crashing upon exit.
            runAlongSide = null;
            baseObject = null;
        }

        public List<PropertyInfo> GetAllConfigurableEntityProperties() {
            return GetType().GetProperties().Where(prop => prop.IsDefined(typeof(ConfigurableEntityFieldAttribute), true)).ToList();
        }

        public void SaveConfig() {
            Dictionary<string, object> values = new Dictionary<string, object>();

            foreach (PropertyInfo property in GetAllConfigurableEntityProperties()) {
                try {
                    values[property.Name] = property.GetValue(this, null);
                } catch (Exception e) {
                    Debug.Log($"Failed to save value of {property} in {GetType().Name}!\n{e.ToString()}");
                }
            }

            AssetManager.SaveData(ConfigurationPath, values, saveType: true);
        }

        public void LoadConfig() {
            Dictionary<string, object> values = AssetManager.ReadData<Dictionary<string, object>>(ConfigurationPath, saveType: true);

            foreach (string property in values.Keys) {
                try {
                    PropertyInfo propertyInfo = GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
                    propertyInfo.SetValue(this, Convert.ChangeType(values[property], propertyInfo.PropertyType), null);
                } catch (Exception e) {
                    Debug.Log($"Failed to set value of {property} in {GetType().Name}!\n{e.ToString()}");
                }
            }
        }

        public FieldInfo GetField(string fieldName) {
            return Master.VersionData.ClassData[runAlongSide.TypeString].GetField(fieldName);
        }

        public object GetFieldValue(string fieldName) {
            try {
                return GetField(fieldName).GetValue(baseObject);
            } catch (Exception e) {
                Debug.Log(e.ToString());
                return null;
            }
        }

        public void SetFieldValue(string fieldName, object value) {
            try {
                GetField(fieldName).SetValue(baseObject, value);
            } catch (Exception e) {
                Debug.Log(e.ToString());
            }
        }
    }
}
