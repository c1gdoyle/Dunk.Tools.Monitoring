namespace Dunk.Tools.Monitoring.Profiling
{
    /// <summary>
    /// Represents a snap shot of allocated memory
    /// of a process.
    /// </summary>
    public struct MemorySnapShot
    {
        /// <summary>
        /// Gets or sets the total number of bytes allocated
        /// in Managed Memory.
        /// </summary>
        public long TotalManagedMemory { get; set; }

        /// <summary>
        /// Gets or sets the total number of bytes allocated
        /// for the current process' private working set.
        /// </summary>
        public long TotalPrivateMemory { get; set; }

        /// <summary>
        /// Gets or sets the total number of bytes allocated
        /// for the current process.
        /// </summary>
        public long TotalPhysicalMemory { get; set; }
    }
}
