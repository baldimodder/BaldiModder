using System.Reflection;

using BaldiModder.Data;

namespace BaldiModder.Runtime {
    public class Master {

        public static bool VerboseMode { get; set; }
        public static bool DebugMode { get; set; }

        public static Assembly RuntimeAssembly { get; set; }
        public static VersionData VersionData { get; set; }

        static Master() {
            VerboseMode = false;
            DebugMode = false;
            VersionData = new VersionData();
        }

        public static void Inject() {
            //TODO: Implement injection.
        }

    }
}
