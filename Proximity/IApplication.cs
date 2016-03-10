using System;
using System.Security.Cryptography.X509Certificates;
using Proximity.DataObjects;

namespace Proximity {
    public interface IApplication : IDisposable {
        string Name { get; }
        string Executable { get; }
        ApplicationInfo Info { get; }

        void Start();
        void Stop();
        void Restart();
    }
}
