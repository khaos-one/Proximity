using Tesla.ServiceProcess;

namespace Proximity {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main() {
            var services = new ServiceList {
                new ProximityService()
            };
            services.RunInteractive();
        }
    }
}
