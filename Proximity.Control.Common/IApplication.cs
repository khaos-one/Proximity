using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Proximity.Control.Common {
    [ServiceContract]
    public interface IApplication : IDisposable {
        string Name { get; }
        string Executable { get; }
        ApplicationInfo Info { get; }

        [OperationContract]
        void Start();

        [OperationContract]
        void Stop();

        [OperationContract]
        void Restart();
    }
}
