using System;

namespace Dunk.Tools.Monitoring.State
{
    /// <summary>
    /// A super class of <see cref="CircuitBreakerStateBase"/> that represents a closed state.
    /// </summary>
    public sealed class ClosedState : CircuitBreakerStateBase
    {
        /// <summary>
        /// Initialises a new instance of <see cref="ClosedState"/> with a
        /// specified Circuit Breaker.
        /// </summary>
        /// <param name="circuitBreaker">The Circuit Breaker.</param>
        public ClosedState(CircuitBreaker circuitBreaker)
            : base(circuitBreaker)
        {
            CircuitBreaker.ResetFailureCount();
        }

        /// <summary>
        /// Gets an enum representation of the state of the remote
        /// service or resource.
        /// 
        /// In this case <see cref="CircuitBreakerState.Close"/>
        /// </summary>
        public override CircuitBreakerState State
        {
            get { return CircuitBreakerState.Close; }
        }

        /// <summary>
        /// Acts on an <see cref="Exception"/> raised by an unsuccessful
        /// attempt to call the operation represented by the CircuitBreaker.
        /// </summary>
        /// <param name="error">The exception raised by the unsuccessful operation.</param>
        public override void ActOnExpection(Exception error)
        {
            base.ActOnExpection(error);
            if (CircuitBreaker.IsRetryLimitReached)
            {
                CircuitBreaker.MoveToOpenState();
            }
        }
    }
}
