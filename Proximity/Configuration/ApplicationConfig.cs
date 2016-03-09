using System;
using Proximity.DataObjects;
using YamlDotNet.Serialization;

namespace Proximity.Configuration {
    [Serializable]
    public sealed class ApplicationConfig {
        [YamlMember(Alias = "executable")]
        public string Executable { get; set; }

        [YamlMember(Alias = "args")]
        public string Arguments { get; set; }

        [YamlMember(Alias = "working-dir")]
        public string WorkingDirectory { get; set; }

        [YamlMember(Alias = "exec-as-user")]
        public string ExecuteAsUser { get; set; }

        [YamlMember(Alias = "out-file")]
        public string OutFile { get; set; }

        [YamlMember(Alias = "error-file")]
        public string ErrorFile { get; set; }

        [YamlMember(Alias = "autorestart")]
        public bool Autorestart { get; set; }

        public SupervisorApplicationInfo ToInfo() {
            return new SupervisorApplicationInfo {
                Executable = Executable,
                Arguments = Arguments,
                WorkingDirectory = WorkingDirectory,
                ExecuteAsUser = ExecuteAsUser,
                OutFile = OutFile,
                ErrorFile = ErrorFile,
                Autorestart = Autorestart
            };
        }
    }
}
