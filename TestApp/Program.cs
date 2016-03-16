using System;
using System.Threading;

namespace TestApp {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Some test app string we normally see in console.");
            Console.ReadLine();
            Console.WriteLine("ReadLine() exited.");
            Thread.Sleep(Timeout.Infinite);
            Console.WriteLine("Program exit.");
        }
    }
}
