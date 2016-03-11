using System;

namespace Proximity.Control.Common {
    [Serializable]
    public sealed class ApplicationInfo {
        public string Executable { get; set; }
        public string Arguments { get; set; }
        public string WorkingDirectory { get; set; }
        public string ExecuteAsUser { get; set; }
        public string OutFile { get; set; }
        public string ErrorFile { get; set; }
        public bool Autorestart { get; set; }
    }
}
