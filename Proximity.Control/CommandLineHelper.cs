using System;
using System.IO;

namespace Proximity.Control {
    public static class CommandLineHelper {
        public static Func<string> GetUsageFunc { get; set; }
        public static Action OnFail { get; set; }
        public static Action OnExit { get; set; }

        public static void Fail(string message = null, int exitCode = 1, bool dontExit = false) {
            if (!string.IsNullOrWhiteSpace(message)) {
                WriteErrorLine($"ERROR: {message}", ConsoleColor.Red);
            }
            else {
                if (GetUsageFunc != null) {
                    WriteLine(GetUsageFunc());
                }
            }

            OnFail?.Invoke();

            if (!dontExit) {
                OnExit?.Invoke();
                Environment.Exit(exitCode);
            }
        }

        public static void WriteErrorLine(string message, ConsoleColor? foregroundColor = null,
            ConsoleColor? backgroundColor = null) {
            Write(message + Environment.NewLine, foregroundColor, backgroundColor, Console.Error);
        }

        public static void WriteError(string message, ConsoleColor? foregroundColor = null,
            ConsoleColor? backgroundColor = null) {
            Write(message + Environment.NewLine, foregroundColor, backgroundColor, Console.Error);
        }

        public static void WriteLine(string message, ConsoleColor? foregroundColor = null,
            ConsoleColor? backgroundColor = null, TextWriter writer = null) {
            Write(message + Environment.NewLine, foregroundColor, backgroundColor, writer);
        }

        public static void Write(string message, ConsoleColor? foregroundColor = null,
            ConsoleColor? backgroundColor = null, TextWriter writer = null) {
            var fcol = Console.ForegroundColor;
            var bcol = Console.BackgroundColor;

            if (foregroundColor != null) {
                Console.ForegroundColor = foregroundColor.Value;
            }
            if (backgroundColor != null) {
                Console.BackgroundColor = backgroundColor.Value;
            }

            if (writer == null) {
                writer = Console.Out;
            }

            writer.Write(message);

            if (foregroundColor != null) {
                Console.ForegroundColor = fcol;
            }
            if (backgroundColor != null) {
                Console.BackgroundColor = bcol;
            }
        }

        public static void Pause() {
            WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}
