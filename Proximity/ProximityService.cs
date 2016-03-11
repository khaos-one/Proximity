using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using Proximity.Configuration;
using Proximity.Control.Common;
using Tesla.Logging;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Proximity {
    public partial class ProximityService : ServiceBase {
        private List<IApplication> _applications;
        private ProximityConfig _config;
        private Stream _logStream;
        private Server _server;

        public List<IApplication>  Applications => _applications;
        public ProximityConfig Config => _config;
        public Server Server => _server;

        public ProximityService() {
            InitializeComponent();

            var configPath = "Config.yml";

            if (!File.Exists(configPath)) {
                throw new ConfigurationErrorsException("Cannot find any configuraiton file, aborting.");
            }

            using (var fs = File.OpenText(configPath)) {
                var deserializer = new Deserializer(namingConvention: new HyphenatedNamingConvention());
                _config = deserializer.Deserialize<ProximityConfig>(fs);
            }

            _logStream = !string.IsNullOrWhiteSpace(_config.LogFile)
                ? File.Open(_config.LogFile, FileMode.Append)
                : File.Create("Proximity.log");
            Log.DefaultLogStream = _logStream;

            _applications = new List<IApplication>(_config.Applications.Select(x => new Application(x)));
        }

        protected override void OnStart(string[] args) {
            _applications.ForEach(x => {
                x.Start();
                Log.Entry(Priority.Notice, $"Application {x.Executable} started under supervisor.");
            });

            //_server = new Server(this, IPAddress.Parse(_config.Server.Address), _config.Server.Port);
            //_server.Start();

            Log.Entry(Priority.Notice, "Proximity information server started.");
            Log.Entry(Priority.Notice, "Proximity supervisor started.");
        }

        protected override void OnStop() {
            _applications?.ForEach(x => {
                try {
                    x.Stop();
                    x.Dispose();
                }
                catch {
                    // ignored
                }

                Log.Entry(Priority.Notice, $"Application {x.Executable} stopped.");
            });

            //_server.Stop();
            //_server.Dispose();

            Log.Entry(Priority.Notice, "Proximity infomation server stopped.");
            Log.Entry(Priority.Notice, "Proximity supervisor stopped.");

            _logStream?.Close();
        }
    }
}
