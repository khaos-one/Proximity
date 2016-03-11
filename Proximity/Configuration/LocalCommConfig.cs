using System;
using YamlDotNet.Serialization;

namespace Proximity.Configuration {
    [Serializable]
    public sealed class LocalCommConfig {
        [YamlMember(Alias = "pipe")]
        public string Pipe { get; set; }
    }
}
