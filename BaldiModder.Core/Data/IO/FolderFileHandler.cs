using System.IO;

namespace BaldiModder.Data.IO {
    public class FolderFileHandler : IFileHandler {

        public bool FileExists(string path) {
            return File.Exists(path);
        }

        public bool FolderExists(string path) {
            return Directory.Exists(path);
        }

        public void CreateDirectory(string path) {
            Directory.CreateDirectory(path);
        }

        public byte[] ReadAllBytes(string path) {
            return File.ReadAllBytes(path);
        }

        public string[] ReadAllLines(string path) {
            return File.ReadAllLines(path);
        }

        public string ReadAllText(string path) {
            return File.ReadAllText(path);
        }

        public void WriteAllBytes(string path, byte[] data) {
            File.WriteAllBytes(path, data);
        }

        public void WriteAllLines(string path, string[] data) {
            File.WriteAllLines(path, data);
        }

        public void WriteAllText(string path, string data) {
            File.WriteAllText(path, data);
        }

    }
}
