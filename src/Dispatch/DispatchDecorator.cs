using System.Diagnostics;
using TNO.Dispatch.Abstractions;
using TNO.Dispatch.Abstractions.Results;

namespace TNO.Dispatch
{
   public abstract class DispatchDecorator<TOutput, TRequest> : IDispatchDecorator<TOutput, TRequest>
      where TOutput : notnull
      where TRequest : notnull, IDispatchRequest
   {
      #region Properties
      public IRequestHandler<TOutput, TRequest> InnerHandler { get; set; } = null!;
      #endregion

      #region Methods
      public abstract ValueTask<IDispatchResult<TOutput>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
      public ValueTask<IDispatchResult<TOutput>> HandleInnerAsync(TRequest request, CancellationToken cancellationToken = default)
      {
         Debug.Assert(InnerHandler is not null);
         cancellationToken.ThrowIfCancellationRequested();

         return InnerHandler.HandleAsync(request, cancellationToken);
      }
      #endregion
   }
}
