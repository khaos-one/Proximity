using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Proximity.Control.Common;

namespace Proximity {
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public sealed class WcfLocalCommunicationService : ILocalCommunicationService {
        public void Ping() {
            return;
        }

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

        public IEnumerable<ApplicationInfo> GetApplications() {
            return Program.Service.Applications.Select(x => x.Info);
        }

        public ApplicationInfo GetApplication(string name) {
            return Program.Service.Applications.FirstOrDefault(x => x.Name == name)?.Info;
        }

        public void StartApplication(string name) {
            var app = Program.Service.Applications.FirstOrDefault(x => x.Name == name);

            if (app == null) {
                throw new FaultException("No such application.");
            }

            try {
                app.Start();
            }
            catch (InvalidOperationException e) {
                throw new FaultException(e.Message);
            }
        }

        public void StopApplication(string name) {
            var app = Program.Service.Applications.FirstOrDefault(x => x.Name == name);

            if (app == null) {
                throw new FaultException("No such application.");
            }

            try {
                app.Stop();
            }
            catch (InvalidOperationException e) {
                throw new FaultException(e.Message);
            }
        }

        public void RestartApplication(string name) {
            var app = Program.Service.Applications.FirstOrDefault(x => x.Name == name);

            if (app == null) {
                throw new FaultException("No such application.");
            }

            try {
                app.Restart();
            }
            catch (InvalidOperationException e) {
                throw new FaultException(e.Message);
            }
        }
    }
}
