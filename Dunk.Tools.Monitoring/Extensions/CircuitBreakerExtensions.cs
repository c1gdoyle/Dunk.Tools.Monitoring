using System;
using Dunk.Tools.Monitoring.State;

namespace Dunk.Tools.Monitoring.Extensions
{
    /// <summary>
    /// Provides a series of extension methods for a <see cref="CircuitBreaker"/> instance.
    /// </summary>
    public static class CircuitBreakerExtensions
    {
        /// <summary>
        /// Attempts to move a <see cref="CircuitBreaker"/> into a closed state.
        /// </summary>
        /// <param name="circuitBreaker">The circuit breaker we are trying.</param>
        /// <returns>
        /// A <see cref="bool"/> value indicating if the circuit breaker was successfully
        /// closed or not. <c>true</c> if the circuit breaker is closed; otherwise returns <c>false</c>.
        /// </returns>
        /// <remarks>
        /// If the circuit breaker is already in a closed state or if it is currently in a
        /// half-open state this method will move it to a closed state. If the circuit breaker
        /// is currently in an open state it will move to a half-opens state.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="circuitBreaker"/> was null.</exception>
        public static bool TryClose(this CircuitBreaker circuitBreaker)
        {
            if(circuitBreaker == null)
            {
                throw new ArgumentNullException(nameof(circuitBreaker),
                    $"Unable to Try-Close, {nameof(circuitBreaker)} parameter cannot be null");
            }

            if (circuitBreaker.IsHalfOpen)
            {
                circuitBreaker.Close();
            }
            else if (circuitBreaker.IsOpen)
            {
                circuitBreaker.MoveToHalfOpenState();
            }
            return circuitBreaker.IsClosed;
        }

        /// <summary>
        /// Attempts to move a <see cref="CircuitBreaker"/> into an open state.
        /// </summary>
        /// <param name="circuitBreaker">The circuit breaker we are trying.</param>
        /// <returns>
        /// A <see cref="bool"/> value indicating if the circuit breaker was successfully
        /// opened or not. <c>true</c> if the circuit breaker is open; otherwise returns <c>false</c>.
        /// </returns>
        /// <remarks>
        /// If the circuit breaker is already in a open state or if it is currently in a
        /// half-open state this method will move it to an open state. If the circuit breaker
        /// is currently in a closed state it will move to a half-open state.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="circuitBreaker"/> was null.</exception>
        public static bool TryOpen(this CircuitBreaker circuitBreaker)
        {
            if (circuitBreaker == null)
            {
                throw new ArgumentNullException(nameof(circuitBreaker),
                    $"Unable to Try-Open, {nameof(circuitBreaker)} parameter cannot be null");
            }

            if (circuitBreaker.IsHalfOpen)
            {
                circuitBreaker.Open();
            }
            else if (circuitBreaker.IsClosed)
            {
                circuitBreaker.MoveToHalfOpenState();
            }
            return circuitBreaker.IsOpen;
        }
    }
}
