using System;
using System.Diagnostics;

namespace Wikiled.Common.Logging
{
    public class PerformanceTrace : IDisposable
    {
        private readonly Action<string> log;

        private readonly string message;

        private readonly Stopwatch stopwatch;

        public PerformanceTrace(Action<string> log, string message)
        {
            this.log = log;
            log($"Starting: {message}");
            stopwatch = new Stopwatch();
            stopwatch.Start();
            this.message = message;
        }

        public void Dispose()
        {
            stopwatch.Stop();
            log($"{message} in {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
