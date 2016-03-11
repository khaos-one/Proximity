using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Proximity.Control.Common;

namespace Proximity {
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public sealed class WcfLocalCommunicationService : ILocalCommunicationService {
        public void Start() {
            Program.RunServices();
        }

        public void Stop() {
            Program.Service.Stop();
        }

        public void Restart() {
            Stop();
            Start();
        }

        public IList<IApplication> GetApplications() {
            return Program.Service.Applications;
        }

        public IApplication GetApplication(string name) {
            return Program.Service.Applications.FirstOrDefault(x => x.Name == name);
        }
    }
}
