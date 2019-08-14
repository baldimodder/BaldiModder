namespace BaldiModder.Data.IO {
    public interface IDataSerializer {
        T ReadData<T>(string path, bool saveType = false);
        void SaveData(string path, object obj, bool saveType = false);
        bool DataExists(string path);
    }
}