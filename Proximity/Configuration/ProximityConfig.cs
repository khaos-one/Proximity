using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Proximity.Configuration {
    [Serializable]
    public sealed class ProximityConfig {
        [YamlMember(Alias = "log-file")]
        public string LogFile { get; set; }

        [YamlMember(Alias = "server")]
        public ServerConfig Server { get; set; }

        [YamlMember(Alias = "apps")]
        public List<ApplicationConfig> Applications { get; set; }
    }
}
