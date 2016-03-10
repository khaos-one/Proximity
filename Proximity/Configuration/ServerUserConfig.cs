using System;
using Proximity.DataObjects;
using YamlDotNet.Serialization;

namespace Proximity.Configuration {
    [Serializable]
    public sealed class ServerUserConfig {
        [YamlMember(Alias = "user")]
        public string UserName { get; set; }

        [YamlMember(Alias = "password-sha1")]
        public string PasswordSha1 { get; set; }

        [YamlMember(Alias = "access")]
        public UserAccessLevel Access { get; set; }
    }
}
