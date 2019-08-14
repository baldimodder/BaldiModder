using System;
using System.Collections.Generic;
using System.Reflection;

namespace BaldiModder.Data {
    [Serializable]
    public class ClassData {

        public Type Type { get; set; }

        public Dictionary<string, VariableData> Fields { get; set; }
        public Dictionary<string, VariableData> Properties { get; set; }
        public Dictionary<string, MethodData> Methods { get; set; }

        public ClassData(Type type) {
            Type = type;
            Fields = new Dictionary<string, VariableData>();
            Properties = new Dictionary<string, VariableData>();
            Methods = new Dictionary<string, MethodData>();
        }

        public void AddField(string entry, string name) {
            Fields[entry] = new VariableData(name);
        }

        public void AddField(string entry, string name, VariableAccessLevel accessLevel) {
            Fields[entry] = new VariableData(name, accessLevel);
        }

        public void AddProperty(string entry, string name) {
            Properties[entry] = new VariableData(name);
        }

        public void AddProperty(string entry, string name, VariableAccessLevel accessLevel) {
            Properties[entry] = new VariableData(name, accessLevel);
        }

        public void AddMethod(string entry, string name) {
            Methods[entry] = new MethodData(name);
        }

        public void AddMethod(string entry, string name, VariableAccessLevel accessLevel) {
            Methods[entry] = new MethodData(name, accessLevel);
        }

        public FieldInfo GetField(string name) {
            FieldInfo field;
            
            try {
                field = Type.GetField(Fields[name].Name, Fields[name].AccessLevelBindingFlags);
                if (field == null) throw new NullReferenceException();
            } catch {
                field = Type.GetField(Fields[name].Name, Fields[name].AccessLevelBindingFlags | BindingFlags.Instance);                 
            }

            return field;
        }

        public MethodInfo GetMethod(string name) {
            MethodInfo method;

            try {
                method = Type.GetMethod(Methods[name].Name, Methods[name].AccessLevelBindingFlags);
                if (method == null) throw new NullReferenceException();
            } catch {
                method = Type.GetMethod(Methods[name].Name, Methods[name].AccessLevelBindingFlags | BindingFlags.Instance);
            }

            return method;
        }

        public object InvokeMethod(string name, object obj, object[] parameters) {
            return GetMethod(name).Invoke(obj, parameters);
        }

    }
}
