using System;
using Dunk.Tools.Monitoring.Extensions;
using Dunk.Tools.Monitoring.State;
using NUnit.Framework;

namespace Dunk.Tools.Monitoring.Test.Extensions
{
    [TestFixture]
    public class CircuitBreakerExtensionsTests
    {
        [Test]
        public void CircuitBreakerTryCloseThrowsIfCircuitBreakerIsNull()
        {
            CircuitBreaker circuitBreaker = null;
            Assert.Throws<ArgumentNullException>(() => circuitBreaker.TryClose());
        }

        [Test]
        public void CircuitBreakerTryCloseMovesToClosedStateIfCurrentlyClosed()
        {
            using (var circuitBreaker = new CircuitBreaker(5, 100))
            {
                circuitBreaker.Close();

                circuitBreaker.TryClose();

                Assert.IsTrue(circuitBreaker.IsClosed);
            }
        }

        [Test]
        public void CircuitBreakerTryCloseMovesToClosedStateIfCurrentlyHalfOpen()
        {
            using (var circuitBreaker = new CircuitBreaker(5, 100))
            {
                circuitBreaker.MoveToHalfOpenState();

                circuitBreaker.TryClose();

                Assert.IsTrue(circuitBreaker.IsClosed);
            }
        }

        [Test]
        public void CircuitBreakerTryCloseMovesToHalfOpenStateIfCurrentlyOpen()
        {
            using (var circuitBreaker = new CircuitBreaker(5, 100))
            {
                circuitBreaker.Open();

                circuitBreaker.TryClose();

                Assert.IsTrue(circuitBreaker.IsHalfOpen);
            }
        }

        [Test]
        public void CircuitBreakerTryCloseReturnsTrueIfStateIsCurrentlyClsoed()
        {
            using (var circuitBreaker = new CircuitBreaker(5, 100))
            {
                circuitBreaker.Close();

                Assert.IsTrue(circuitBreaker.TryClose());
            }
        }

        [Test]
        public void CircuitBreakerTryCloseReturnsTrueIfStateIsCurrentlyHalfOpen()
        {
            using (var circuitBreaker = new CircuitBreaker(5, 100))
            {
                circuitBreaker.MoveToHalfOpenState();

                Assert.IsTrue(circuitBreaker.TryClose());
            }
        }

        [Test]
        public void CircuitBreakerTryCloseReturnsFalseIfStateIsCurrentlyOpen()
        {
            using (var circuitBreaker = new CircuitBreaker(5, 100))
            {
                circuitBreaker.Open();

                Assert.IsFalse(circuitBreaker.TryClose());
            }
        }

        [Test]
        public void CircuitBreakerTryOpenThrowsIfCircuitBreakerIsNull()
        {
            CircuitBreaker circuitBreaker = null;
            Assert.Throws<ArgumentNullException>(() => circuitBreaker.TryOpen());
        }

        [Test]
        public void CircuitBreakerTryOpenMovesToHalfOpenStateIfCurrentlyClosed()
        {
            using (var circuitBreaker = new CircuitBreaker(5, 100))
            {
                circuitBreaker.Close();

                circuitBreaker.TryOpen();

                Assert.IsTrue(circuitBreaker.IsHalfOpen);
            }
        }

        [Test]
        public void CircuitBreakerTryOpenMovesToOpenStateIfCurrentlyHalfOpen()
        {
            using (var circuitBreaker = new CircuitBreaker(5, 100))
            {
                circuitBreaker.MoveToHalfOpenState();

                circuitBreaker.TryOpen();

                Assert.IsTrue(circuitBreaker.IsOpen);
            }
        }

        [Test]
        public void CircuitBreakerTryOpenMovesToOpenStateIfCurrentlyOpen()
        {
            using (var circuitBreaker = new CircuitBreaker(5, 100))
            {
                circuitBreaker.Open();

                circuitBreaker.TryOpen();

                Assert.IsTrue(circuitBreaker.IsOpen);
            }
        }

        [Test]
        public void CircuitBreakerTryOpenReturnsFalseIfStateIsCurrentlyClsoed()
        {
            using (var circuitBreaker = new CircuitBreaker(5, 100))
            {
                circuitBreaker.Close();

                Assert.IsFalse(circuitBreaker.TryOpen());
            }
        }

        [Test]
        public void CircuitBreakerTryOpenReturnsTrueIfStateIsCurrentlyHalfOpen()
        {
            using (var circuitBreaker = new CircuitBreaker(5, 100))
            {
                circuitBreaker.MoveToHalfOpenState();

                Assert.IsTrue(circuitBreaker.TryOpen());
            }
        }

        [Test]
        public void CircuitBreakerTryOpenReturnsTrueIfStateIsCurrentlyOpen()
        {
            using (var circuitBreaker = new CircuitBreaker(5, 100))
            {
                circuitBreaker.Open();

                Assert.IsTrue(circuitBreaker.TryOpen());
            }
        }
    }
}
