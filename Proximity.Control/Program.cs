using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Selectors;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using Proximity.Control.Common;
using Tesla.Extensions;
using static Proximity.Control.CommandLineHelper;

namespace Proximity.Control {
    static class Program {
        static ILocalCommunicationService Channel;

        static string Usage() {
            return "Proximity.Control.exe <command> [application]";
        }

        static void Main(string[] args) {
            GetUsageFunc = Usage;

            if (args.Length < 1) {
                Fail();
            }

            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            var ep = new EndpointAddress("net.pipe://localhost/proximity/local");

            try {
                Channel = ChannelFactory<ILocalCommunicationService>.CreateChannel(binding, ep);
                Channel.Ping();
            }
            catch (Exception e) {
                Fail($"Cannot establish connection with local Proximity server ({e.Message}).");
            }

            CommandsMapper.Map(args[0], new Dictionary<string, Action> {
                {
                    "list", () => {
                        var list = Channel.GetApplications();

                        foreach (var app in list) {
                            WriteLine($"    {app.Name} ({app.Executable})   unknown");
                        }
                    }
                }, {
                    "status", () => {
                        if (args.Length < 2) {
                            Fail("Application name required.");
                        }

                        var app = Channel.GetApplication(args[1]);

                        if (app == null) {
                            Fail("No such application.");
                        }

                        WriteLine($"{app.Name} ({app.Executable})\n" +
                                  $"    working dir: {app.WorkingDirectory}\n" +
                                  $"");
                        // TODO: Rest of the code here.
                    }
                }, {
                    "start", () => {
                        if (args.Length < 2) {
                            Fail("Application name required.");
                        }

                        try {
                            Channel.StartApplication(args[1]);
                        }
                        catch (FaultException e) {
                            Fail(e.Message);
                        }
                    }
                }, {
                    "stop", () => {
                        if (args.Length < 2) {
                            Fail("Application name required.");
                        }

                        try {
                            Channel.StopApplication(args[1]);
                        }
                        catch (FaultException e) {
                            Fail(e.Message);
                        }
                    }
                }, {
                    "restart", () => {
                        if (args.Length < 2) {
                            Fail("Application name required.");
                        }

                        try {
                            Channel.RestartApplication(args[1]);
                        }
                        catch (FaultException e) {
                            Fail(e.Message);
                        }
                    }
                },
                { "network-test", () => {
                    if (args.Length < 3) {
                        Fail("You must specify host, username and password for connectivity check.");
                    }

                    var host = args[1];
                    var username = args[2];
                    string passwordDigest;

                    using (var sha1 = SHA1.Create()) {
                        passwordDigest = sha1.ComputeHash(Encoding.Default.GetBytes(args[3])).ToHexString();
                    }

                    var netBinding = new NetTcpBinding(SecurityMode.TransportWithMessageCredential) {
                        Security = {
                            Mode = SecurityMode.TransportWithMessageCredential,
                            Message = {
                                ClientCredentialType = MessageCredentialType.UserName,
                                AlgorithmSuite = SecurityAlgorithmSuite.Basic128Sha256Rsa15
                            },
                            Transport = new TcpTransportSecurity {
                                ClientCredentialType = TcpClientCredentialType.Certificate,
                                ProtectionLevel = ProtectionLevel.EncryptAndSign
                            }
                        }
                    };
                    var netep = new EndpointAddress(new Uri($"net.tcp://{host}:25001/proximity/net"),
                        EndpointIdentity.CreateX509CertificateIdentity(new X509Certificate2("ServerCertificate.pfx")));
                    var netChannelFactory = new ChannelFactory<INetCommunicationService>(netBinding, netep) {
                        Credentials = {
                            UserName = {
                                UserName = username,
                                Password = passwordDigest
                            },
                            ServiceCertificate = {
                                Authentication = {
                                    CertificateValidationMode = X509CertificateValidationMode.Custom,
                                    RevocationMode = X509RevocationMode.NoCheck,
                                    CustomCertificateValidator = new CustomCertificateValidator()
                                }
                            },
                            //ClientCertificate = {
                            //    Certificate = new X509Certificate2("ClientCertificate.pfx")
                            //}
                        }
                    };

                    try {
                        var netChannel = netChannelFactory.CreateChannel();
                        netChannel.Ping();

                        WriteLine("SUCCESS: Proximity service is reachable by network.",
                            foregroundColor: ConsoleColor.Green);
                    }
                    catch (Exception e) {
                        Fail($"Proximity service is unreachable or username/password pair is incorrect: {e.Message}.");
                    }
                } }
            }, defaultAction: () => {
                Fail("No such command.");
            });
        }
    }

    class CustomCertificateValidator : X509CertificateValidator {
        public override void Validate(X509Certificate2 certificate) {
            return;
        }
    }
}
