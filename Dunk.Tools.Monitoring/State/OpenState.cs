using System;

namespace Dunk.Tools.Monitoring.State
{
    /// <summary>
    /// A super class of <see cref="CircuitBreakerStateBase"/> that represents an open state.
    /// </summary>
    public sealed class OpenState : CircuitBreakerStateBase
    {
        private readonly DateTime _openDateTime;

        /// <summary>
        /// Initialises a new instance of <see cref="OpenState"/> with a
        /// specified Circuit Breaker.
        /// </summary>
        /// <param name="circuitBreaker">The Circuit Breaker.</param>
        public OpenState(CircuitBreaker circuitBreaker)
            : base(circuitBreaker)
        {
            _openDateTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets an enum representation of the state of the remote
        /// service or resource.
        /// 
        /// In this case <see cref="CircuitBreakerState.Open"/>
        /// </summary>
        public override CircuitBreakerState State
        {
            get { return CircuitBreakerState.Open; }
        }

        public override void OperationIsAboutToBeCalled()
        {
            base.OperationIsAboutToBeCalled();
            UpdateState();
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if (DateTime.UtcNow >= _openDateTime.AddMilliseconds(CircuitBreaker.Timeout))
            {
                CircuitBreaker.MoveToHalfOpenState();
            }
        }
    }
}
