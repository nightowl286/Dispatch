using System.Threading;
using System.Threading.Tasks;
using TNO.Dispatch.Abstractions.Results;

namespace TNO.Dispatch.Abstractions
{
   public interface IRequestDispatcher<in TRequestConstraint>
      where TRequestConstraint : notnull, IDispatchRequest
   {
      #region Methods
      bool CanDispatch<TOutput, TRequest>()
         where TOutput : notnull
         where TRequest : TRequestConstraint;

      ValueTask<IDispatchResult<TOutput>> DispatchAsync<TOutput, TRequest>(TRequest request, CancellationToken cancellationToken = default)
         where TOutput : notnull
         where TRequest : TRequestConstraint;
      #endregion
   }
}
