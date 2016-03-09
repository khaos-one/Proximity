using System;
using System.Net;
using System.Net.Sockets;
using Tesla;
using Tesla.Logging;
using Tesla.Net;

namespace Proximity {
    public sealed class Server : ThreadedTcpServer {
        public Server(IPAddress ip, int port) : base(ip, port) {
            
        }

        public Server(int port) : this(IPAddress.Any, port) {}

        protected override void HandleConnection(Socket socket) {
            throw new NotImplementedException();
        }

        protected override void HandleException(Socket socket, Exception e) {
            Log.Entry(Priority.Warning, $"[Server] Exception during request processing: {e}.");
        }
    }
}
