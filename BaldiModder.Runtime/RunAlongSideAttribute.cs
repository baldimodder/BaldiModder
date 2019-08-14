using System;

namespace BaldiModder.Runtime {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RunAlongSideAttribute : Attribute {

        public Type Type {
            get {
                return Master.VersionData.GetType(TypeString);
            }
        }

        public string TypeString { get; private set; }

        public RunAlongSideAttribute(string type) {
            TypeString = type;
        }

    }
}
