using System;
using System.Threading;
using Dunk.Tools.Monitoring.Extensions;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Dunk.Tools.Monitoring.Test")]
namespace Dunk.Tools.Monitoring.State
{
    /// <summary>
    /// A Circuit Breaker class that serves as a proxy
    /// for operations that might fail.
    /// </summary>
    /// <remarks>
    /// See
    /// https://docs.microsoft.com/en-us/azure/architecture/patterns/circuit-breaker
    /// </remarks>
    public class CircuitBreaker
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        private ICircuitBreakerState _currentState;
        private Exception _exceptionFromLastAttempt;

        /// <summary>
        /// Initialises a new instance of <see cref="CircuitBreaker"/> with a specified
        /// retry limit and time-out.
        /// </summary>
        /// <param name="retryLimit">The number of retries to attempt a failed operation.</param>
        /// <param name="timeOut">The time to wait when the circuit is open, in milli-seconds.</param>
        public CircuitBreaker(int retryLimit, int timeOut)
        {
            if (retryLimit < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(retryLimit), "Retry limit should be greater than 0.");
            }
            if (timeOut < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(timeOut), "Timeout should be greater than 0.");
            }
            RetryLimt = retryLimit;
            Timeout = timeOut;
            MoveToClosedState();
        }

        /// <summary>
        /// Gets or sets the retry limit for this <see cref="CircuitBreaker"/>
        /// to attempt a failed operation.
        /// </summary>
        public int RetryLimt { get; private set; }

        /// <summary>
        /// Gets or sets the time to wait when the circuit is open, in milli-seconds.
        /// </summary>
        public int Timeout { get; private set; }

        /// <summary>
        /// Gets or sets the number of failed attempt operations.
        /// </summary>
        public int FailureCount { get; private set; }

        /// <summary>
        /// Gets whether or not this <see cref="CircuitBreaker"/> is currently in a closed state.
        /// </summary>
        public bool IsClosed { get { return _currentState.State == CircuitBreakerState.Close; } }

        /// <summary>
        /// Gets whether or not this <see cref="CircuitBreaker"/> is currently in an open state.
        /// </summary>
        public bool IsOpen { get { return _currentState.State == CircuitBreakerState.Open; } }

        /// <summary>
        /// Gets whether or not this <see cref="CircuitBreaker"/> is currently in a half-open state.
        /// </summary>
        public bool IsHalfOpen { get { return _currentState.State == CircuitBreakerState.HalfOpen; } }

        /// <summary>
        /// Gets whether or not the retry limit has been reached.
        /// </summary>
        public bool IsRetryLimitReached { get { return FailureCount > RetryLimt; } }

        /// <summary>
        /// Gets the exception raised by the last failed opeartion, if any.
        /// </summary>
        public Exception ExceptionFromLastAttempt { get { return _exceptionFromLastAttempt; } }

        /// <summary>
        /// Attempts to execute a specified operation and updates this circuit breaker's state
        /// depending on if the operation completed successfully or threw an exception.
        /// </summary>
        /// <param name="operation">The operation to execute.</param>
        /// <returns>
        /// The circuit breaker instance after attempting to execute the specified operation.
        /// If action threw an exception it will be stored in the <see cref="CircuitBreaker.ExceptionFromLastAttempt"/>.
        /// </returns>
        public CircuitBreaker AttemptCall(Action operation)
        {
            _exceptionFromLastAttempt = null;
            using (_lock.WriteLock())
            {
                _currentState.OperationIsAboutToBeCalled();
                if (_currentState.State == CircuitBreakerState.Open)
                {
                    //stop execution of the protected code
                    return this;
                }
            }

            try
            {
                operation();
            }
            catch (Exception e)
            {
                _exceptionFromLastAttempt = e;
                using (_lock.WriteLock())
                {
                    _currentState.ActOnExpection(e);
                }
            }

            using (_lock.WriteLock())
            {
                _currentState.OperationHasBeenCalled();
            }
            return this;
        }

        /// <summary>
        /// Moves this <see cref="CircuitBreaker"/> into a Closed state.
        /// </summary>
        public void Close()
        {
            MoveToClosedState();
        }

        /// <summary>
        /// Moves this <see cref="CircuitBreaker"/> into a Open state.
        /// </summary>
        public void Open()
        {
            MoveToOpenState();
        }

        internal void ResetFailureCount()
        {
            FailureCount = 0;
        }

        internal void IncrementFailureCount()
        {
            FailureCount++;
        }

        internal void MoveToClosedState()
        {
            using (_lock.WriteLock())
            {
                _currentState = new ClosedState(this);
            }
        }

        internal void MoveToOpenState()
        {
            using (_lock.WriteLock())
            {
                _currentState = new OpenState(this);
            }
        }

        internal void MoveToHalfOpenState()
        {
            using (_lock.WriteLock())
            {
                _currentState = new HalfOpenState(this);
            }
        }
    }
}
