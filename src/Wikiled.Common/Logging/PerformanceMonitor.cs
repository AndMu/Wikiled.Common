using System;
using System.Diagnostics;
using System.Threading;

namespace Wikiled.Core.Utility.Logging
{
    public class PerformanceMonitor
    {
        private readonly Stopwatch timer = new Stopwatch();

        private readonly Stopwatch timerAll = new Stopwatch();

        private readonly long initial;

        private long lastStep;

        private long manualCount;

        private long current;

        public PerformanceMonitor(long total)
        {
            initial = total;
            lastStep = 0;
            timer.Start();
            timerAll.Start();
        }

        public void ManualyCount()
        {
            Interlocked.Increment(ref manualCount);
        }

        public void Increment()
        {
            Interlocked.Increment(ref current);
        }

        public override string ToString()
        {
            var time = timer.Elapsed;
            var allTime = timerAll.Elapsed;
            var manual = Volatile.Read(ref manualCount);
            var total = manual > initial ? manual : initial;

            var currentStep = Volatile.Read(ref current);
            total = total < currentStep ? currentStep : total;
            timer.Restart();

            var step = currentStep - lastStep;
            Volatile.Write(ref lastStep, currentStep);
            if (currentStep == 0)
            {
                return $"Processed: {current}/{total} Operations per second: 0";
            }

            var speed = step / (time.TotalMilliseconds + 1);
            var speedAll = currentStep / (allTime.TotalMilliseconds + 1);

            var left = speedAll == 0 ? (TimeSpan?)null : TimeSpan.FromMilliseconds((total - current) / speedAll);
            var completeTime = left.HasValue ? DateTime.UtcNow.Add(left.Value) : (DateTime?)null;

            return string.Format(
                left?.Days > 0
                    ? "Processed: {0}/{1} Operations per second: [{2:F2}] Average: [{6:F2}] Took:[{4:hh\\:mm\\:ss}] Left:[{3:dd\\.hh\\:mm\\:ss}] Until:[{5:hh\\:mm\\:ss}]"
                    : "Processed: {0}/{1} Operations per second: [{2:F2}] Average: [{6:F2}] Took:[{4:hh\\:mm\\:ss}] Left:[{3:hh\\:mm\\:ss}] Until:[{5:HH\\:mm\\:ss}]",
                currentStep,
                total,
                speed * 1000,
                left,
                time,
                completeTime,
                speedAll * 1000);
        }
    }
}
