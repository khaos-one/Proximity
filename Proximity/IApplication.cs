using System;
using System.Security.Cryptography.X509Certificates;

namespace Proximity {
    public interface IApplication : IDisposable {
        string Executable { get; }

        void Start();
        void Stop();
        void Restart();
    }
}
