using UnityEngine;

namespace BaldiModder.Data {
    public class ImageFileData : FileData {
        public LoadImageAs LoadImageAs { get; set; }

        public FilterMode FilterMode { get; set; }
    }
}