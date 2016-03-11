using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Proximity.Configuration {
    [Serializable]
    public sealed class ProximityConfig {
        [YamlMember(Alias = "log-file")]
        public string LogFile { get; set; }

        [YamlMember(Alias = "local-comm")]
        public LocalCommConfig LocalComm { get; set; }

        [YamlMember(Alias = "net-comm")]
        public NetCommConfig NetComm { get; set; }

        [YamlMember(Alias = "apps")]
        public List<ApplicationConfig> Applications { get; set; }
    }
}
