using System;

namespace Dunk.Tools.Monitoring.Profiling
{
    /// <summary>
    /// Provides ehlper methods for getting a memory snap shot of a process.
    /// </summary>
    public static class MemoryProfiler
    {
        /// <summary>
        /// Gets a snap shot of the memory currently held by this process.
        /// </summary>
        /// <returns>
        /// A <see cref="MemorySnapShot"/> detailing the memory held by the current process.
        /// </returns>
        public static MemorySnapShot ProfileCurrentProcess()
        {
            long totalBytesGC = GC.GetTotalMemory(false);

            var currentProcess = System.Diagnostics.Process.GetCurrentProcess();

            long totalPrivateBytes = currentProcess.PrivateMemorySize64;
            long totalPhysicalBytes = currentProcess.WorkingSet64;

            return new MemorySnapShot
            {
                TotalManagedMemory = totalBytesGC,
                TotalPrivateMemory = totalPrivateBytes,
                TotalPhysicalMemory = totalPhysicalBytes
            };
        }
    }
}
