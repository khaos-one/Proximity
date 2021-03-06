﻿using System;
using System.Diagnostics;
using System.IO;
using Proximity.Configuration;
using Proximity.Control.Common;
using Tesla.Logging;

namespace Proximity {
    public sealed class Application : IApplication {
        private readonly ProcessStartInfo _startInfo;
        private Process _process;
        private StreamWriter _stdErr;
        private StreamWriter _stdOut;
        private bool _stopping = true;
        private bool _plannedStop;
        private bool _isBroken = false;

        public string Name { get; }
        public string Executable { get; }
        public ApplicationInfo Info { get; }

        public Application(ApplicationConfig config) {
            if (config == null) {
                throw new ArgumentNullException(nameof(config));
            }

            if (string.IsNullOrWhiteSpace(config.Executable)) {
                throw new ArgumentException(nameof(config.Executable));
            }

            Info = config.ToInfo();
            Executable = config.Executable;
            Name = config.Name;
            _startInfo = new ProcessStartInfo(Path.GetFullPath(config.Executable)) {
                UseShellExecute = false,
                CreateNoWindow = true
            };

            if (!string.IsNullOrWhiteSpace(config.Arguments)) {
                _startInfo.Arguments = config.Arguments;
            }

            if (!string.IsNullOrWhiteSpace(config.ExecuteAsUser)) {
                _startInfo.UserName = config.ExecuteAsUser;
            }

            if (!string.IsNullOrWhiteSpace(config.WorkingDirectory)) {
                _startInfo.WorkingDirectory = Path.GetFullPath(config.WorkingDirectory);
            }

            if (!string.IsNullOrEmpty(config.ErrorFile)) {
                _stdErr = !File.Exists(config.ErrorFile)
                    ? File.CreateText(config.ErrorFile)
                    : new StreamWriter(config.ErrorFile, true);
                _startInfo.RedirectStandardError = true;
            }

            if (!string.IsNullOrEmpty(config.OutFile)) {
                _stdOut = !File.Exists(config.OutFile)
                    ? File.CreateText(config.OutFile)
                    : new StreamWriter(config.OutFile, true);
                _startInfo.RedirectStandardOutput = true;
            }
        }

        public void Start() {
            if (_isBroken) {
                throw new InvalidOperationException("Process is broken, aborting start.");
            }

            if (_stopping == false) {
                throw new InvalidOperationException("Application is already started.");
            }

            _stopping = false;
            _process = new Process {StartInfo = _startInfo, EnableRaisingEvents = true};
            _process.Exited += Process_Exited;

            if (_stdErr != null)
                _process.ErrorDataReceived += Process_ErrorDataReceived;

            if (_stdOut != null)
                _process.OutputDataReceived += Process_OutputDataReceived;

            try {
                _process.Start();
            }
            catch {
                _isBroken = true;
                throw;
            }

            if (_stdErr != null)
                _process.BeginErrorReadLine();

            if (_stdOut != null)
                _process.BeginOutputReadLine();
        }

        public void Stop() {
            if (_isBroken) {
                throw new InvalidOperationException("Process is broken, aborting stop.");
            }

            if (_stopping) {
                throw new InvalidOperationException("Application is already stopped.");
            }

            if (_process == null) {
                throw new InvalidOperationException("Process was detached from service.");
            }

            _stopping = true;

            //if (_timer != null)
            //    _timer.Change(Timeout.Infinite, Timeout.Infinite);

            _plannedStop = true;

            if (_stdErr != null)
                _process.CancelErrorRead();

            if (_stdOut != null)
                _process.CancelOutputRead();

            if (!_process.HasExited) {
                // TODO: Implement graceful stop if possible.
                _process.Kill();
                _process.WaitForExit();
                _plannedStop = false;
            }

            _process = null;

            if (_stdErr != null) {
                _stdErr.Close();
                _stdErr.Dispose();
                _stdErr = null;
            }

            if (_stdOut != null) {
                _stdOut.Close();
                _stdOut.Dispose();
                _stdOut = null;
            }
        }

        public void Restart() {
            if (_isBroken) {
                throw new InvalidOperationException("Process is broken, aborting restart.");
            }

            Stop();
            Start();
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            if (_stopping)
                return;

            if (!string.IsNullOrEmpty(e.Data))
                _stdErr.WriteLine(e.Data);
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            if (_stopping)
                return;

            if (!string.IsNullOrEmpty(e.Data))
                _stdErr.WriteLine(e.Data);
        }

        private void Process_Exited(object sender, EventArgs e) {
            if (_isBroken) {
                return;
            }

            if (_stopping) {
                return;
            }

            if (!_plannedStop) {
                Log.Entry(Priority.Warning, "Application `{0}` unexpectedly exited with code {1}, restarting.",
                    _startInfo.FileName, _process.ExitCode);
            }

            _stopping = true;
            Start();
        }

        public void Dispose() {
            if (_process != null) {
                Stop();
            }
        }
    }
}
