using System;

namespace Dunk.Tools.Monitoring.State
{
    /// <summary>
    /// An abstract base class for classes that implement <see cref="ICircuitBreakerState"/>.
    /// </summary>
    public abstract class CircuitBreakerStateBase : ICircuitBreakerState
    {
        private readonly CircuitBreaker _circuitBreaker;

        /// <summary>
        /// Initialises an instance of <see cref="CircuitBreakerStateBase"/> with a specified
        /// Circuit Breaker.
        /// </summary>
        /// <param name="circuitBreaker">The Circuit Breaker</param>
        protected CircuitBreakerStateBase(CircuitBreaker circuitBreaker)
        {
            _circuitBreaker = circuitBreaker;
        }

        #region ICircuitBreakerState Members
        /// <inheritdoc />
        public CircuitBreaker CircuitBreaker
        {
            get { return _circuitBreaker; }
        }

        /// <inheritdoc />
        public abstract CircuitBreakerState State { get; }

        /// <inheritdoc />
        public virtual void OperationHasBeenCalled()
        {
            //by default do nothing
        }

        /// <inheritdoc />
        public virtual void OperationIsAboutToBeCalled()
        {
            //by default do nothing
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("csharpsquid", "CA1716:Allow parameter name error")]
        public virtual void ActOnExpection(Exception error)
        {
            //by default just increment the failure encounter
            CircuitBreaker.IncrementFailureCount();
        }

        /// <inheritdoc />
        public virtual void UpdateState()
        {
            //by default do nothing
        }
        #endregion ICircuitBreakerState Members
    }
}
