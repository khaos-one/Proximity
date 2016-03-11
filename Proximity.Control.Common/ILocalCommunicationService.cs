using System.Collections.Generic;
using System.ServiceModel;

namespace Proximity.Control.Common {
    [ServiceContract]
    public interface ILocalCommunicationService {
        [OperationContract]
        void Start();

        [OperationContract]
        void Stop();

        [OperationContract]
        void Restart();

        [OperationContract]
        IList<IApplication> GetApplications();

        [OperationContract]
        IApplication GetApplication(string name);
    }
}
