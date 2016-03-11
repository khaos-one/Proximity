using System.Collections.Generic;
using System.ServiceModel;

namespace Proximity.Control.Common {
    [ServiceContract]
    public interface INetCommunicationService {
        [OperationContract]
        void Stop();

        [OperationContract]
        IList<IApplication> GetApplications();

        [OperationContract]
        IApplication GetApplication(string name);

        [OperationContract]
        HostSystemInfo GetSystemInfo();
    }
}
