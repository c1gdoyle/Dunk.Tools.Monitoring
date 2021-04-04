using Dunk.Tools.Monitoring.Profiling;
using NUnit.Framework;

namespace Dunk.Tools.Monitoring.Test.Profiling
{
    [TestFixture]
    public class MemoryProfilerTests
    {
        [Test]
        public void MemoryProfilerGeneratesSnapShotForCurrentProcess()
        {
            var memorySnapShot = MemoryProfiler.ProfileCurrentProcess();
            Assert.IsNotNull(memorySnapShot);
        }
    }
}
