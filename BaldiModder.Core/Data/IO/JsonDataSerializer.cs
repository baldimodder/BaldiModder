using System.IO;

using Newtonsoft.Json;

namespace BaldiModder.Data.IO {
    public class JsonDataSerializer : IDataSerializer {
        public string GetPath(string path) {
            return path.ToLower().EndsWith(".json") ? path : $"{path}.json";
        }

        public bool DataExists(string path) {
            path = GetPath(path);
            return File.Exists(path);
        }

        public T ReadData<T>(string path, bool saveType = false) {
            path = GetPath(path);

            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path), new JsonSerializerSettings {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = saveType ? TypeNameHandling.Auto : TypeNameHandling.None
            });
        }

        public void SaveData(string path, object obj, bool saveType = false) {
            path = GetPath(path);
            try {
                AssetManager.EnsureFileDirectoryExists(path);
            } catch { }

            File.WriteAllText(path, JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = saveType ? TypeNameHandling.Auto : TypeNameHandling.None
            }));
        }
    }
}