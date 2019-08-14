namespace BaldiModder.Data.IO {
    public interface IFileHandler {
        bool FileExists(string path);
        bool FolderExists(string path);

        void CreateDirectory(string path);

        string ReadAllText(string path);
        string[] ReadAllLines(string path);
        byte[] ReadAllBytes(string path);

        void WriteAllText(string path, string data);
        void WriteAllLines(string path, string[] data);
        void WriteAllBytes(string path, byte[] data);
    }
}