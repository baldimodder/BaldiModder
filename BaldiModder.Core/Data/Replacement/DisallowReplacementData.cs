using System;

using Newtonsoft.Json;

namespace BaldiModder.Data {
    [Serializable]
    public struct DisallowReplacementData {

        [JsonProperty(PropertyName = "r")]
        public DisallowReplacementReason Reason { get; set; }

        [JsonProperty(PropertyName = "am", NullValueHandling = NullValueHandling.Ignore)]
        public string AlternativeMessage { get; set; }

    }
}
