namespace Dunk.Tools.Monitoring.State
{
    /// <summary>
    /// An enum that represents the current state of a 
    /// remote service or resource.
    /// </summary>
    public enum CircuitBreakerState
    {
        /// <summary>
        /// Denotes that the service or resource is in
        /// a Closed state
        /// 
        /// Requests for the resource will be routed to the operation.
        /// </summary>
        Close = 0,
        /// <summary>
        /// Denotes that the service or resource is in
        /// an Open state.
        /// 
        /// Requests for the resource will fail immediately.
        /// </summary>
        Open = 1,
        /// <summary>
        /// Denotes that the service or resource is in
        ///  Half-Open state.
        ///  
        /// A limited number of requests from the application are allowed to
        /// pass through and invoke the operation.
        /// </summary>
        HalfOpen = 2
    }
}
