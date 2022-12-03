using System.Threading;
using System.Threading.Tasks;
using TNO.Dispatch.Abstractions.Results;

namespace TNO.Dispatch.Abstractions
{
   /// <summary>
   /// Denotes a common dispatch request handler.
   /// </summary>
   /// <typeparam name="TOutput">The type of the output that the <typeparamref name="TRequest"/> can return.</typeparam>
   /// <typeparam name="TRequest">The type of the <see cref="IDispatchRequest"/> this handler can handle.</typeparam>
   public interface IRequestHandler<TOutput, in TRequest>
      where TOutput : notnull
      where TRequest : IDispatchRequest
   {
      #region Methods
      /// <summary>Asynchronously handles the given <paramref name="request"/>.</summary>
      /// <param name="request">The request to handle.</param>
      /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel this operation.</param>
      /// <returns>The result of handling the given <paramref name="request"/>.</returns>
      ValueTask<IDispatchResult<TOutput>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
      #endregion
   }
}
