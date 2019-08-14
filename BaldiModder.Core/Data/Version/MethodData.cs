namespace BaldiModder.Data {
    public class MethodData : VariableData {
        
        private MethodData() { }

        public MethodData(string name) : base(name) { }

        public MethodData(string name, VariableAccessLevel accessLevel) : base(name, accessLevel) { }

    }
}
