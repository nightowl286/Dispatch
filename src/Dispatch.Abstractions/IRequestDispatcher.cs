using System.Threading;
using System.Threading.Tasks;
using TNO.Dispatch.Results;

namespace TNO.Dispatch.Abstractions;

/// <summary>
/// Denotes a common request dispatcher.
/// </summary>
/// <typeparam name="TRequestConstraint">
/// The type of <see cref="IDispatchRequest"/> that this dispatcher will allow.
/// </typeparam>
public interface IRequestDispatcher<in TRequestConstraint>
   where TRequestConstraint : notnull, IDispatchRequest
{
   #region Methods
   /// <summary>
   /// Checks whether the <typeparamref name="TOutput"/>/<typeparamref name="TRequest"/>
   /// combination can be dispatched.
   /// </summary>
   /// <typeparam name="TOutput">The expected output type.</typeparam>
   /// <typeparam name="TRequest">The type of the request.</typeparam>
   /// <returns>
   /// <see langword="true"/> if the <typeparamref name="TOutput"/>/<typeparamref name="TRequest"/>
   /// combination can be dispatched, <see langword="false"/> otherwise.
   /// </returns>
   bool CanDispatch<TOutput, TRequest>()
      where TOutput : notnull
      where TRequest : TRequestConstraint;

   /// <summary>
   /// Asynchronously dispatches the given <paramref name="request"/> with an 
   /// expected output of the type <typeparamref name="TOutput"/>.
   /// </summary>
   /// <typeparam name="TOutput">The expected output type.</typeparam>
   /// <typeparam name="TRequest">The type of the <paramref name="request"/>.</typeparam>
   /// <param name="request">The request to dispatch.</param>
   /// <param name="cancellationToken">
   /// A <see cref="CancellationToken"/> that can be used to cancel this operation.
   /// </param>
   /// <returns>The dispatch result returned by the handler.</returns>
   ValueTask<DispatchResult<TOutput>> DispatchAsync<TOutput, TRequest>(TRequest request, CancellationToken cancellationToken = default)
      where TOutput : notnull
      where TRequest : TRequestConstraint;
   #endregion
}
