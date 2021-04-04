using System;

namespace Dunk.Tools.Monitoring.State
{
    /// <summary>
    /// A super class of <see cref="CircuitBreakerStateBase"/> that represents a half-open state.
    /// </summary>
    public sealed class HalfOpenState : CircuitBreakerStateBase
    {
        /// <summary>
        /// Initialises a new instance of <see cref="HalfOpenState"/> with a
        /// specified Circuit Breaker.
        /// </summary>
        /// <param name="circuitBreaker">The Circuit Breaker.</param>
        public HalfOpenState(CircuitBreaker circuitBreaker)
            : base(circuitBreaker)
        {
        }

        /// <summary>
        /// Gets an enum representation of the state of the remote
        /// service or resource.
        /// 
        /// In this case <see cref="CircuitBreakerState.HalfOpen"/>
        /// </summary>
        public override CircuitBreakerState State
        {
            get { return CircuitBreakerState.HalfOpen; }
        }

        public override void OperationHasBeenCalled()
        {
            base.OperationHasBeenCalled();
            CircuitBreaker.MoveToClosedState();
        }

        public override void ActOnExpection(Exception error)
        {
            base.ActOnExpection(error);
            CircuitBreaker.MoveToOpenState();
        }
    }
}
