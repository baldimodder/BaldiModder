using System.Collections.Generic;
using System.IO;

namespace BaldiModder.Data { 
    public class FileRepository {
        //public IniFile IniFile { get; set; }

        public string Base { get; set; }

        public Dictionary<string, FileData> Files { get; set; }

        public string GetPath(string entry) {
            FileData data = GetFileData(entry);
            return Path.Combine(Base, data.Path);
        }

        public string GetPathInModFolder(string entry) {
            FileData data = GetFileData(entry);
            return AssetManager.GetPathInModFolder(GetPath(entry));
        }

        public FileData GetFileData(string entry) {
            return Files[entry];
        }
    }
}