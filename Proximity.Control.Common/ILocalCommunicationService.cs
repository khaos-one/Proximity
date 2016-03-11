using System.Collections.Generic;
using System.ServiceModel;

namespace Proximity.Control.Common {
    [ServiceContract]
    public interface ILocalCommunicationService {
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
        void StartApplication(string name);

        [OperationContract]
        void StopApplication(string name);

        [OperationContract]
        void RestartApplication(string name);
    }
}
