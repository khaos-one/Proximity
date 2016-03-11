using System;

namespace Proximity.Control.Common {
    [Serializable]
    public sealed class HostSystemInfo {
        public string SystemName { get; set; }
        public float TotalCpuLoadPercent { get; set; }
        public double TotalMemoryUsageMb { get; set; }
    }
}
