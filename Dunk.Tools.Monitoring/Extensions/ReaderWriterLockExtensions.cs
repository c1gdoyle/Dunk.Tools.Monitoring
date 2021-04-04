using System;
using System.Threading;

namespace Dunk.Tools.Monitoring.Extensions
{
    /// <summary>
    /// Provides a series of extension methods for a <see cref="ReaderWriterLockSlim"/> instance.
    /// </summary>
    internal static class ReaderWriterLockExtensions
    {
        /// <summary>
        /// Enters the lock in write mode and returns an <see cref="IDisposable"/> wrapper.
        /// </summary>
        /// <param name="sync">The lock.</param>
        /// <returns>
        /// An <see cref="IDisposable"/> wrapper over the lock, when dispoed the underlying lock will
        /// exit write mode.
        /// </returns>
        internal static IDisposable WriteLock(this ReaderWriterLockSlim sync)
        {
            if (sync == null)
            {
                throw new ArgumentNullException(nameof(sync), 
                    $"Write-Lock {nameof(sync)} parameter cannot be null");
            }
            return new WriteLockToken(sync);
        }

        private sealed class WriteLockToken : IDisposable
        {
            private readonly ReaderWriterLockSlim _sync;

            public WriteLockToken(ReaderWriterLockSlim sync)
            {
                _sync = sync;
                _sync.EnterWriteLock();
            }

            public void Dispose()
            {
                if (_sync != null)
                {
                    _sync.ExitWriteLock();
                }
            }
        }
    }
}
