using System.Collections.Generic;
using System.ServiceModel;

namespace Proximity.Control.Common {
    [ServiceContract]
    public interface INetCommunicationService {
        [OperationContract]
        void Ping();

        [OperationContract]
        IEnumerable<ApplicationInfo> GetApplications();

        [OperationContract]
        ApplicationInfo GetApplication(string name);

        [OperationContract]
        void StartApplication(string name);

        [OperationContract]
        void StopApplication(string name);

        [OperationContract]
        void RestartApplication(string name);

        [OperationContract]
        HostSystemInfo GetSystemInfo();
    }
}
