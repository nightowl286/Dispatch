using System.Diagnostics;
using TNO.Dispatch.Abstractions;
using TNO.Dispatch.Results;

namespace TNO.Dispatch.Decorators;

/// <inheritdoc/>
public abstract class DispatchDecorator<TOutput, TRequest> : IDispatchDecorator<TOutput, TRequest>
   where TOutput : notnull
   where TRequest : notnull, IDispatchRequest
{
   #region Properties
   /// <inheritdoc/>
   public IRequestHandler<TOutput, TRequest> InnerHandler { get; set; } = null!;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public abstract ValueTask<DispatchResult<TOutput>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);

   /// <summary>Asynchronously handles the given <paramref name="request"/>, using the <see cref="InnerHandler"/>.</summary>
   /// <inheritdoc cref="HandleAsync(TRequest, CancellationToken)"/>
   protected ValueTask<DispatchResult<TOutput>> HandleInnerAsync(TRequest request, CancellationToken cancellationToken = default)
   {
      Debug.Assert(InnerHandler is not null);
      cancellationToken.ThrowIfCancellationRequested();

      return InnerHandler.HandleAsync(request, cancellationToken);
   }
   #endregion
}
