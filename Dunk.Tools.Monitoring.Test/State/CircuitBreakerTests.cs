using System;
using Dunk.Tools.Monitoring.State;
using NUnit.Framework;

namespace Dunk.Tools.Monitoring.Test.State
{
    [TestFixture]
    public class CircuitBreakerTests
    {
        [Test]
        public void CircuitBreakerInitialises()
        {
            using (var circuitBreaker = new CircuitBreaker(5, 60000))
            {
                Assert.IsNotNull(circuitBreaker);
            }
        }

        [Test]
        public void CircuitBreakerThrowsIfRetryLimitIsLessThanOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CircuitBreaker(0, 60000));
        }

        [Test]
        public void CircuitBreakerThrowsIfTimeoutIsLessThanOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CircuitBreaker(5, 0));
        }

        [Test]
        public void CircuitBreakerStateIsClosedByDefault()
        {
            using (var circuitBreaker = new CircuitBreaker(5, 60000))
            {
                Assert.IsTrue(circuitBreaker.IsClosed);
            }
        }

        [Test]
        public void CircuitBreakerAttemptCallThrowsIfOperationIsNull()
        {
            using(var circuitBreaker = new CircuitBreaker(5, 60000))
            {
                Assert.Throws<ArgumentNullException>(() => circuitBreaker.AttemptCall(null));
            }
        }

        [Test]
        public void CircuitBreakerIncrementsFailureCountOnExpection()
        {
            const string customErrorMessage = "CircuitBreaker operation failed";

            Action failedOperation = () =>
            {
                throw new Exception(customErrorMessage);
            };

            using (var circuitBreaker = new CircuitBreaker(5, 60000))
            {
                circuitBreaker.AttemptCall(failedOperation);
                circuitBreaker.AttemptCall(failedOperation);
                circuitBreaker.AttemptCall(failedOperation);

                Assert.AreEqual(3, circuitBreaker.FailureCount);
            }
        }

        [Test]
        public void CircuitBreakerCapturesExceptionFromLastAttempt()
        {
            const string customErrorMessage = "CircuitBreaker operation failed";

            Action failedOperation = () =>
            {
                throw new Exception(customErrorMessage);
            };

            using (var circuitBreaker = new CircuitBreaker(5, 60000))
            {
                circuitBreaker.AttemptCall(failedOperation);

                var error = circuitBreaker.ExceptionFromLastAttempt;

                Assert.IsNotNull(error);
                Assert.AreEqual(customErrorMessage, error.Message);
            }
        }

        [Test]
        public void CircuitBreakerResetsFailureCountWhenMovedToClosedState()
        {
            const string customErrorMessage = "CircuitBreaker operation failed";

            Action failedOperation = () =>
            {
                throw new Exception(customErrorMessage);
            };

            using (var circuitBreaker = new CircuitBreaker(5, 60000))
            {
                circuitBreaker.AttemptCall(failedOperation);
                circuitBreaker.AttemptCall(failedOperation);
                circuitBreaker.AttemptCall(failedOperation);

                circuitBreaker.Close();

                Assert.AreEqual(0, circuitBreaker.FailureCount);
            }
        }

        [Test]
        public void CircuitBreakerMovesStateToOpenIfRetryLimitReached()
        {
            const string customErrorMessage = "CircuitBreaker operation failed";

            Action failedOperation = () =>
            {
                throw new Exception(customErrorMessage);
            };

            using (var circuitBreaker = new CircuitBreaker(5, 60000))
            {
                circuitBreaker.AttemptCall(failedOperation);
                circuitBreaker.AttemptCall(failedOperation);
                circuitBreaker.AttemptCall(failedOperation);
                circuitBreaker.AttemptCall(failedOperation);
                circuitBreaker.AttemptCall(failedOperation);
                circuitBreaker.AttemptCall(failedOperation);

                Assert.IsTrue(circuitBreaker.IsOpen);
            }
        }

        [Test]
        public void CircuitBreakerInOpenStateDoesNotAttemptOperation()
        {
            bool operationWasAttempted = false;

            Action successfulOperation = () =>
            {
                operationWasAttempted = true;
            };

            using (var circuitBreaker = new CircuitBreaker(5, 60000))
            {
                circuitBreaker.Open();
                circuitBreaker.AttemptCall(successfulOperation);

                Assert.IsFalse(operationWasAttempted);
            }
        }

        [Test]
        public void CircuitBreakerInHalfOpenStateDoesAttemptOperation()
        {
            bool operationWasAttempted = false;

            Action successfulOperation = () =>
            {
                operationWasAttempted = true;
            };

            using (var circuitBreaker = new CircuitBreaker(5, 100))
            {
                circuitBreaker.Open();
                System.Threading.Thread.Sleep(200);

                circuitBreaker.AttemptCall(successfulOperation);

                Assert.IsTrue(operationWasAttempted);
            }
        }

        [Test]
        public void CircuitBreakerInHalfOpenStateMovesToOpenStateIfOperationFails()
        {
            const string customErrorMessage = "CircuitBreaker operation failed";

            Action failedOperation = () =>
            {
                throw new Exception(customErrorMessage);
            };

            using (var circuitBreaker = new CircuitBreaker(5, 100))
            {
                circuitBreaker.Open();
                System.Threading.Thread.Sleep(200);

                circuitBreaker.AttemptCall(failedOperation);

                Assert.IsTrue(circuitBreaker.IsOpen);
            }
        }

        [Test]
        public void CircuitBreakerInHalfOpenStateMovesToClosedStateIfOperationSucceeds()
        {
            Action successfulOperation = () =>
            {
            };

            using (var circuitBreaker = new CircuitBreaker(5, 100))
            {
                circuitBreaker.Open();
                System.Threading.Thread.Sleep(200);

                circuitBreaker.AttemptCall(successfulOperation);

                Assert.IsTrue(circuitBreaker.IsClosed);
            }
        }
    }
}
