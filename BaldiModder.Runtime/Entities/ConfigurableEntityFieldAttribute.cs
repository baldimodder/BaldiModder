using System;

namespace BaldiModder.Runtime.Entities {
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConfigurableEntityFieldAttribute : Attribute {

        public ConfigurableEntityFieldAttribute() { }

    }
}
