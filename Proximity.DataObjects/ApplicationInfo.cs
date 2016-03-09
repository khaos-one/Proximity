using System;
using ProtoBuf;

namespace Proximity.DataObjects {
    [Serializable]
    [ProtoContract]
    public sealed class ApplicationInfo
    {
        [ProtoMember(1)]
        public string Executable { get; set; }

        [ProtoMember(2)]
        public string Arguments { get; set; }

        [ProtoMember(3)]
        public string WorkingDirectory { get; set; }

        [ProtoMember(4)]
        public string ExecuteAsUser { get; set; }

        [ProtoMember(5)]
        public string OutFile { get; set; }

        [ProtoMember(6)]
        public string ErrorFile { get; set; }

        [ProtoMember(7)]
        public bool Autorestart { get; set; }
    }
}
