using System;
using System.ServiceModel;
using Proximity.Control.Common;
using Tesla.ServiceProcess;

namespace Proximity {
    internal static class Program {
        internal static ProximityService Service { get; private set; }
        internal static ServiceHost LocalCommunicationService { get; private set; }
        internal static ServiceHost NetCommuncationService { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        internal static void Main() {
            Service = new ProximityService();

            LocalCommunicationService = new ServiceHost(typeof (WcfLocalCommunicationService));
            var pipe = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            LocalCommunicationService.AddServiceEndpoint(typeof (ILocalCommunicationService), pipe,
                Service.Config.LocalComm.Pipe);
            LocalCommunicationService.BeginOpen(null, null);

            RunServices();
        }

        internal static void RunServices() {
            var services = new ServiceList {
                Service
            };
            services.RunInteractive();
        }
    }
}
