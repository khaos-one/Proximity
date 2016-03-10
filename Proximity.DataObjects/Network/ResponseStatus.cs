using System;

namespace Proximity.DataObjects.Network {
    [Serializable]
    public enum ResponseStatus {
        Unknown = 0,
        Success = 1,

        GenericFailure = 2,
        NotAuthenticated = 3,
        NotFound = 4,
        InvalidCredentials = 5
    }
}
