﻿using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Proximity.Configuration {
    [Serializable]
    public sealed class NetCommConfig {
        [YamlMember(Alias = "pipe")]
        public string Pipe { get; set; }

        [YamlMember(Alias = "server-cert")]
        public string ServerCertificate { get; set; }

        [YamlMember(Alias = "users")]
        public List<ServerUserConfig> Users { get; set; }
    }
}