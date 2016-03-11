using System;
using System.IO;
using Proximity.Control.Common;
using YamlDotNet.Serialization;

namespace Proximity.Configuration {
    [Serializable]
    public sealed class ApplicationConfig {
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

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

        public ApplicationInfo ToInfo() {
            var result = new ApplicationInfo {
                Name = Name,
                Executable = Path.GetFullPath(Executable),
                Arguments = Arguments,
                WorkingDirectory = WorkingDirectory,
                ExecuteAsUser = ExecuteAsUser,
                OutFile = OutFile,
                ErrorFile = ErrorFile,
                Autorestart = Autorestart
            };

            if (!string.IsNullOrWhiteSpace(result.WorkingDirectory)) {
                result.WorkingDirectory = Path.GetFullPath(result.WorkingDirectory);
            }

            return result;
        }
    }
}
