using System;
using Dunk.Tools.Monitoring.Profiling;
using NUnit.Framework;

namespace Dunk.Tools.Monitoring.Test.Profiling
{
    [TestFixture]
    public class DurationProfilerTests
    {
        [Test]
        public void DurationProfilerReturnsDurationInMilliSeconds()
        {
            const int operationTime = 100;
            Action operation = () =>
            {
                System.Threading.Thread.Sleep(operationTime * 2);
            };

            long duration = DurationProfiler.GetOperationDurationInMilliSeconds(operation);

            Assert.IsTrue(duration > operationTime);
        }

        [Test]
        public void DurationProfilerReturnsDurationAsTimeSpan()
        {
            const int operationTime = 100;
            Action operation = () =>
            {
                System.Threading.Thread.Sleep(operationTime * 2);
            };

            TimeSpan duration = DurationProfiler.GetOperationDurationTimespan(operation);

            Assert.IsTrue(duration.Milliseconds > operationTime);
        }

        [Test]
        public void DurationProfilerReturnsDurationInMilliSecondsForUnsafeOperation()
        {
            const int operationTime = 100;
            Action operation = () =>
            {
                System.Threading.Thread.Sleep(operationTime * 2);
            };

            Tuple<long, Exception> duration = DurationProfiler.GetUnsafeOperationDurationInMilliSeconds(operation);

            Assert.IsTrue(duration.Item1 > operationTime);
        }

        [Test]
        public void DurationProfilerReturnsDurationInAsTimeSpanForUnsafeOperation()
        {
            const int operationTime = 100;
            Action operation = () =>
            {
                System.Threading.Thread.Sleep(operationTime * 2);
            };

            Tuple<TimeSpan, Exception> duration = DurationProfiler.GetUnsafeOperationDurationTimeSpan(operation);

            Assert.IsTrue(duration.Item1.Milliseconds > operationTime);
        }

        [Test]
        public void DurationProfilerReturnsExceptionForUnsafeOperation()
        {
            const int operationTime = 100;
            const string errorMsg = "Error!";

            Action operation = () =>
            {
                System.Threading.Thread.Sleep(operationTime * 2);
                throw new Exception(errorMsg);
            };

            Tuple<long, Exception> duration1 = DurationProfiler.GetUnsafeOperationDurationInMilliSeconds(operation);
            Tuple<TimeSpan, Exception> duration2 = DurationProfiler.GetUnsafeOperationDurationTimeSpan(operation);

            Assert.AreEqual(errorMsg, duration1.Item2.Message);
            Assert.AreEqual(errorMsg, duration2.Item2.Message);
        }

        [Test]
        public void DurationProfilerGetOperationDurationInMilliSecondsThrowsIfOperationIsNull()
        {
            Action operation = null;

            Assert.Throws<ArgumentNullException>(() => DurationProfiler.GetOperationDurationInMilliSeconds(operation));
        }

        [Test]
        public void DurationProfilerGetOperationDurationTimespanThrowsIfOperationIsNull()
        {
            Action operation = null;

            Assert.Throws<ArgumentNullException>(() => DurationProfiler.GetOperationDurationTimespan(operation));
        }

        [Test]
        public void DurationProfilerGetUnsafeOperationDurationInMilliSecondsThrowsIfOperationIsNull()
        {
            Action operation = null;

            Assert.Throws<ArgumentNullException>(() => DurationProfiler.GetUnsafeOperationDurationInMilliSeconds(operation));
        }

        [Test]
        public void DurationProfilerGetUnsafeOperationDurationTimeSpanThrowsIfOperationIsNull()
        {
            Action operation = null;

            Assert.Throws<ArgumentNullException>(() => DurationProfiler.GetUnsafeOperationDurationTimeSpan(operation));
        }
    }
}
