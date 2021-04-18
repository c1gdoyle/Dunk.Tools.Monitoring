using System;

namespace Dunk.Tools.Monitoring.State
{
    /// <summary>
    /// An interface that represents state information on a remote
    /// service or resource.
    /// </summary>
    public interface ICircuitBreakerState
    {
        /// <summary>
        /// Gets the <see cref="CircuitBreaker"/> instance associated
        /// with this state.
        /// </summary>
        CircuitBreaker CircuitBreaker { get; }

        /// <summary>
        /// Gets an enum representation of the state of the remote
        /// service or resource.
        /// </summary>
        CircuitBreakerState State { get; }

        /// <summary>
        /// Invoked when the operation represented by the
        /// CircuitBreaker is about to be called.
        /// </summary>
        void OperationIsAboutToBeCalled();

        /// <summary>
        /// Invoked when the operation represented by the
        /// CircuitBreaker has been called.
        /// </summary>
        void OperationHasBeenCalled();

        /// <summary>
        /// Acts on an <see cref="Exception"/> raised by an unsuccessful
        /// attempt to call the operation represented by the CircuitBreaker.
        /// </summary>
        /// <param name="error">The exception raised by the unsuccessful operation.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("csharpsquid", "CA1716:Allow parameter name error")]
        void ActOnExpection(Exception error);

        /// <summary>
        /// Updates the state of the CircuitBreaker.
        /// </summary>
        void UpdateState();
    }
}
