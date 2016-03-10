using System;

namespace Proximity.DataObjects.Network {
    [Serializable]
    public enum RequestType {
        Unknown = 0,
        Authenticate = 1,
        GetHostSystemInfo = 10
    }
}
