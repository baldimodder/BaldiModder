using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaldiModder.Runtime.Entities {
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityDefinitionAttribute : Attribute {

        public string name;

        public EntityDefinitionAttribute(string name) {
            this.name = name;
        }

    }
}
