using CommandLine;

namespace Proximity.Control {
    internal sealed class CommandLineOptions {
        [ValueList(typeof (string), MaximumElements = 2)]
        public string Command { get; set; }
    }
}
