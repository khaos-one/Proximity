using System;
using ProtoBuf;

namespace Proximity.DataObjects {
    [Serializable]
    [ProtoContract]
    public sealed class HostSystemInfo {
        [ProtoMember(1)]
        public string SystemName { get; set; }

        [ProtoMember(2)]
        public float TotalCpuLoadPercent { get; set; }

        [ProtoMember(3)]
        public double TotalMemoryUsageMb { get; set; }
    }
}
