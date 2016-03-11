using System;
using System.Runtime.Serialization;

namespace Proximity.Control.Common {
    [Serializable]
    [DataContract]
    public sealed class ApplicationInfo {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Executable { get; set; }
        
        [DataMember]
        public string Arguments { get; set; }

        [DataMember]
        public string WorkingDirectory { get; set; }

        [DataMember]
        public string ExecuteAsUser { get; set; }

        [DataMember]
        public string OutFile { get; set; }

        [DataMember]
        public string ErrorFile { get; set; }

        [DataMember]
        public bool Autorestart { get; set; }
    }
}
