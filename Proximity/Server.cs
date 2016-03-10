using System;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using ProtoBuf;
using Proximity.Configuration;
using Proximity.DataObjects;
using Proximity.DataObjects.Network;
using Tesla.Logging;
using Tesla.Net;

namespace Proximity {
    public sealed class Server : ThreadedSslServer {
        private ProximityService _service;

        public Server(ProximityService service, IPAddress ip, int port) : base(new SslOptions {
            ServerCertificate = new X509Certificate2("ServerCertificate.pfx"),
            Protocols = SslProtocols.Tls,
            CheckRevocation = false,
            RequireClientCertificate = false
        }, ip, port) {
            _service = service;
        }

        public Server(ProximityService service, int port) : this(service, IPAddress.Any, port) {}

        protected override void HandleSslClient(SslStream stream) {
            Request request;
            ServerUserConfig user = null;

            do {
                try {
                    request = ProtoSerializer.DeserializeNetwork<Request>(stream);
                }
                catch (ProtoException e) {
                    Log.Entry(Priority.Error,
                        $"[Server] Protocol Buffers failed to deserialize incoming message: {e.Message}.");
                    continue;
                }
                catch (OutOfMemoryException) {
                    Log.Entry(Priority.Error, $"[Server] Corrupt TCP stream.");
                    return;
                }
                catch {
                    break;
                }

                // Check if request is null.
                if (request == null) {
                    Log.Entry(Priority.Warning, $"[Server] Incoming request was null.");
                    continue;
                }

                // If user is not authenticated, but request something other than auth, deny.
                if (user == null && request.Type != RequestType.Authenticate) {
                    ProtoSerializer.SerializeNetwork(stream, new Response {
                        Status = ResponseStatus.NotAuthenticated
                    });
                    continue;
                }

                try {
                    switch (request.Type) {
                        case RequestType.Authenticate:
                            var authinfo = ProtoSerializer.Deserialize<UserLoginCredentials>(request.Payload);
                            user =
                                _service.Config.Server.Users.FirstOrDefault(
                                    x =>
                                        x.UserName.Equals(authinfo.UserName) &&
                                        x.PasswordSha1.Equals(authinfo.PasswordSha1));

                            if (user == null) {
                                ProtoSerializer.SerializeNetwork(stream, new Response {
                                    Status = ResponseStatus.InvalidCredentials
                                });
                            }
                            break;

                        case RequestType.Unknown:
                            break;
                        case RequestType.GetHostSystemInfo:
                            // TODO: FUTURE: Should also check user access level.
                            // TODO: Finish later.
                            //ProtoSerializer.SerializeNetwork(stream, new Response {
                            //    Status = ResponseStatus.Success,
                            //    Payload = ProtoSerializer.Serialize(new HostSystemInfo {
                            //        SystemName = Environment.MachineName,
                            //        TotalCpuLoadPercent = 
                            //    })
                            //});
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (Exception e) {
                    Log.Entry(Priority.Error, $"[Server] Failed to process user request: {e}.");
                }
            } while (true);
        }
    }
}
