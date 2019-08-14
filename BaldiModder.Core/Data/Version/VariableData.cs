using System;
using System.Reflection;

using Newtonsoft.Json;

namespace BaldiModder.Data {
    [Serializable]
    public class VariableData {

        public string Name { get; set; }

        public VariableAccessLevel AccessLevel { get; set; }

        [JsonIgnore]
        public BindingFlags AccessLevelBindingFlags {
            get {
                return (AccessLevel == VariableAccessLevel.Public) ? BindingFlags.Public : BindingFlags.NonPublic;
            }
        }

        public VariableData() { }

        public VariableData(string name) : this(name, VariableAccessLevel.Public) { }

        public VariableData(string name, VariableAccessLevel accessLevel) {
            Name = name;
            AccessLevel = accessLevel;
        }

    }
}
