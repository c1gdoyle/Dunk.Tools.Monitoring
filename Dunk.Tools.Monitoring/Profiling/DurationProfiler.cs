using System;

namespace Dunk.Tools.Monitoring.Profiling
{
    /// <summary>
    /// Provides methods for profiling the duration of an operation.
    /// </summary>
    public static class DurationProfiler
    {
        /// <summary>
        /// Executes a specified action and returns the time taken to complete in milli-seconds.
        /// </summary>
        /// <param name="operation">The operation to execute.</param>
        /// <returns>
        /// The duration of the action in milli-seconds.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="operation"/> was null.</exception>
        public static long GetOperationDurationInMilliSeconds(Action operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation),
                    $"Unable to profile duration, {nameof(operation)} parameter cannot be null.");
            }

            var sw = System.Diagnostics.Stopwatch.StartNew();
            operation();
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        /// <summary>
        /// Executes a specified action and returns the time taken to complete as a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="operation">The operation to execute.</param>
        /// <returns>
        /// The duration of the action as a <see cref="TimeSpan"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="operation"/> was null.</exception>
        public static TimeSpan GetOperationDurationTimespan(Action operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation),
                    $"Unable to profile duration, {nameof(operation)} parameter cannot be null.");
            }

            var sw = System.Diagnostics.Stopwatch.StartNew();
            operation();
            sw.Stop();
            return sw.Elapsed;
        }

        /// <summary>
        /// Executes a specified unsafe action and returns the time taken to complete in milli-seconds
        /// and any exception that might have occurred.
        /// </summary>
        /// <param name="operation">The operation to execute.</param>
        /// <returns>
        /// A <see cref="Tuple{T1, T2}"/> representing the duration of attempting to execute the acction.
        /// <see cref="Tuple{T1, T2}.Item1"/> represents the duration for the action to complete or fail in milli-seconds.
        /// <see cref="Tuple{T1, T2}.Item2"/> represents any exception that occurred whilst attempting to complete the action.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="operation"/> was null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("csharpsquid", "CA1031:Support generic catch for operation")]
        public static Tuple<long, Exception> GetUnsafeOperationDurationInMilliSeconds(Action operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation),
                    $"Unable to profile duration, {nameof(operation)} parameter cannot be null.");
            }

            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                operation();
                sw.Stop();
                return new Tuple<long, Exception>(sw.ElapsedMilliseconds, null);
            }
            catch (Exception ex)
            {
                sw.Stop();
                return new Tuple<long, Exception>(sw.ElapsedMilliseconds, ex);
            }
        }


        /// <summary>
        /// Executes a specified unsafe action and returns the time taken to complete as a <see cref="TimeSpan"/>
        /// and any exception that might have occurred.
        /// </summary>
        /// <param name="operation">The operation to execute.</param>
        /// <returns>
        /// A <see cref="Tuple{T1, T2}"/> representing the duration of attempting to execute the acction.
        /// <see cref="Tuple{T1, T2}.Item1"/> represents the duration for the action to complete or fail as a <see cref="TimeSpan"/>.
        /// <see cref="Tuple{T1, T2}.Item2"/> represents any exception that occurred whilst attempting to complete the action.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="operation"/> was null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("csharpsquid", "CA1031:Support generic catch for operation")]
        public static Tuple<TimeSpan, Exception> GetUnsafeOperationDurationTimeSpan(Action operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation),
                    $"Unable to profile duration, {nameof(operation)} parameter cannot be null.");
            }

            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                operation();
                sw.Stop();
                return new Tuple<TimeSpan, Exception>(sw.Elapsed, null);
            }
            catch (Exception ex)
            {
                sw.Stop();
                return new Tuple<TimeSpan, Exception>(sw.Elapsed, ex);
            }
        }
    }
}
