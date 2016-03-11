using System;
using System.Collections.Generic;
using System.ServiceModel;
using Proximity.Control.Common;
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
                }
            }, defaultAction: () => {
                Fail("No such command.");
            });
        }
    }
}
