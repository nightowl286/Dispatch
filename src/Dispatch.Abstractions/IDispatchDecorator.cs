using TNO.Dispatch.Abstractions.Workflows;

namespace TNO.Dispatch.Abstractions;

/// <summary>
/// Denotes a dispatch handler decorator.
/// </summary>
/// <typeparam name="TOutput">The output that this decorator returns.</typeparam>
/// <typeparam name="TRequest">The request that this decorator can handle.</typeparam>
public interface IDispatchDecorator<TOutput, TRequest> : IRequestHandler<TOutput, TRequest>
   where TOutput : notnull
   where TRequest : notnull, IDispatchRequest
{
   #region Properties
   /// <summary>The decorated handler.</summary>
   /// <remarks>This will usually be set by the <see cref="IDispatchWorkflow"/>.</remarks>
   IRequestHandler<TOutput, TRequest> InnerHandler { get; set; }
   #endregion
}
