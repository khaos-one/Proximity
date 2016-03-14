using System;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using Proximity.Configuration;
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

            // Local WCF communication setup.
            LocalCommunicationService = new ServiceHost(typeof (WcfLocalCommunicationService));
            var pipe = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            LocalCommunicationService.AddServiceEndpoint(typeof (ILocalCommunicationService), pipe,
                Service.Config.LocalComm.Pipe);
            LocalCommunicationService.BeginOpen(null, null);

            // Network WCF communication setup.
            var netBinding = new NetTcpBinding {
                Security = {
                    Mode = SecurityMode.TransportWithMessageCredential,
                    Message = {
                        ClientCredentialType = MessageCredentialType.UserName,
                        AlgorithmSuite = SecurityAlgorithmSuite.Basic128Sha256Rsa15
                    },
                    Transport = new TcpTransportSecurity {
                        ClientCredentialType = TcpClientCredentialType.None,
                        ProtectionLevel = ProtectionLevel.EncryptAndSign,
                        SslProtocols = SslProtocols.Tls
                    }
                }
            };
            NetCommuncationService = new ServiceHost(typeof (WcfNetCommunicationService)) {
                Credentials = {
                    ServiceCertificate = {
                        Certificate = new X509Certificate2(Service.Config.NetComm.ServerCertificate)
                    },
                    ClientCertificate = {
                        Authentication = {
                            CertificateValidationMode = X509CertificateValidationMode.None,
                            RevocationMode = X509RevocationMode.NoCheck
                        }
                    },
                    UserNameAuthentication = {
                        UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom,
                        CustomUserNamePasswordValidator = new NetUserValidator()
                    }
                }
            };
            NetCommuncationService.AddServiceEndpoint(typeof (INetCommunicationService), netBinding,
                Service.Config.NetComm.Address);
            NetCommuncationService.BeginOpen(null, null);

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
