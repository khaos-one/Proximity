using System.Collections.Generic;
using System.ServiceModel;

namespace Proximity.Control.Common {
    [ServiceContract]
    public interface INetCommunicationService {
        [OperationContract]
        void Ping();

        [OperationContract]
        void Start();

        [OperationContract]
        void Stop();

        [OperationContract]
        void Restart();

        [OperationContract]
        IEnumerable<ApplicationInfo> GetApplications();

        [OperationContract]
        ApplicationInfo GetApplication(string name);

        [OperationContract]
        HostSystemInfo GetSystemInfo();
    }
}
