using System.Collections.Generic;
using System.IO;

namespace BaldiModder.Data {
    public class ImageFileRepository : FileRepository {
        public new Dictionary<string, ImageFileData> Files { get; set; }

        public new string GetPath(string entry) {
            ImageFileData data = GetFileData(entry);
            return Path.Combine(Base, data.Path);
        }

        public new string GetPathInModFolder(string entry) {
            ImageFileData data = GetFileData(entry);
            return AssetManager.GetPathInModFolder(GetPath(entry));
        }

        public new ImageFileData GetFileData(string entry) {
            return Files[entry];
        }
    }
}