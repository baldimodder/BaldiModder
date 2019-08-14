using System.Collections.Generic;

using Newtonsoft.Json;

namespace BaldiModder.Data {
    public class Animation {

        public float Interval { get; set; }

        public List<AnimationFrame> Frames { get; set; }

        [JsonIgnore]
        public int NumberOfFrames {
            get {
                return Frames.Count - 1;
            }
        }

    }
}
