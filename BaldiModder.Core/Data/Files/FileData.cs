using System;

namespace BaldiModder.Data {
    [Serializable]
    public class FileData {
        public string Path { get; set; }
        public LoadTime WhenToLoad { get; set; }
    }
}