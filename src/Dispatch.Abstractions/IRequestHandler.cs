using System.Threading;
using System.Threading.Tasks;
using TNO.Dispatch.Abstractions.Results;

namespace TNO.Dispatch.Abstractions
{
   public interface IRequestHandler<TOutput, in TRequest>
      where TOutput : notnull
      where TRequest : IDispatchRequest
   {
      #region Methods
      ValueTask<IDispatchResult<TOutput>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
      #endregion
   }
}
