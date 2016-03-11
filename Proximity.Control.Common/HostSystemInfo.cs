using System;
using System.Runtime.Serialization;

namespace Proximity.Control.Common {
    [Serializable]
    [DataContract]
    public sealed class HostSystemInfo {
        [DataMember]
        public string SystemName { get; set; }

        [DataMember]
        public float TotalCpuLoadPercent { get; set; }

        [DataMember]
        public float TotalAvailableRamMb { get; set; }

        [DataMember]
        public float TotalRamMb { get; set; }
    }
}
