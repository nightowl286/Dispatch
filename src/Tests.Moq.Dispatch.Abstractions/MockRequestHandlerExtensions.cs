using Moq;
using TNO.Dispatch.Abstractions;
using TNO.Dispatch.Results;

namespace TNO.Tests.Moq.Dispatch.Abstractions;

/// <summary>
/// Contains useful extension methods for the <see cref="Mock"/>&lt;<see cref="IRequestHandler{T,U}"/>&gt; class.
/// </summary>
public static class MockRequestHandlerExtensions
{
   #region Methods
   /// <summary>
   /// Sets up the given <paramref name="mockHandler"/> to return the given
   /// <paramref name="result"/> when the given <paramref name="request"/> is provided.
   /// </summary>
   /// <typeparam name="TOutput">The output type of the handler.</typeparam>
   /// <typeparam name="TRequest">The request type of the handler.</typeparam>
   /// <param name="mockHandler">The mock handler to setup.</param>
   /// <param name="request">The request that will be provided.</param>
   /// <param name="result">The result that should be returned.</param>
   /// <returns>The <paramref name="mockHandler"/> instance that was.</returns>
   public static Mock<IRequestHandler<TOutput, TRequest>> WithResult<TOutput, TRequest>(
      this Mock<IRequestHandler<TOutput, TRequest>> mockHandler,
      TRequest request,
      DispatchResult<TOutput> result)
      where TOutput : notnull
      where TRequest : IDispatchRequest
   {
      mockHandler
         .Setup(h => h.HandleAsync(request, default))
         .ReturnsAsync(result);

      return mockHandler;
   }

   /// <inheritdoc cref="WithResult{TOutput, TRequest}(Mock{IRequestHandler{TOutput, TRequest}}, TRequest, DispatchResult{TOutput})"/>
   public static Mock<IRequestHandler<TOutput, TRequest>> WithResult<TOutput, TRequest>(
      this Mock<IRequestHandler<TOutput, TRequest>> mockHandler,
      TRequest request,
      TOutput result)
      where TOutput : notnull
      where TRequest : IDispatchRequest
   {
      DispatchResult<TOutput> dispathResult = new DispatchResult<TOutput>(result);

      mockHandler
         .Setup(h => h.HandleAsync(request, default))
         .ReturnsAsync(dispathResult);

      return mockHandler;
   }

   /// <summary>
   /// Sets up the given <paramref name="mockHandler"/> to return the given
   /// <paramref name="result"/> for any request that is provided.
   /// </summary>
   /// <inheritdoc cref="WithResult{TOutput, TRequest}(Mock{IRequestHandler{TOutput, TRequest}}, TRequest, DispatchResult{TOutput})"/>
   public static Mock<IRequestHandler<TOutput, TRequest>> WithResult<TOutput, TRequest>(
      this Mock<IRequestHandler<TOutput, TRequest>> mockHandler,
      DispatchResult<TOutput> result)
      where TOutput : notnull
      where TRequest : IDispatchRequest
   {
      mockHandler
         .Setup(h => h.HandleAsync(It.IsAny<TRequest>(), default))
         .ReturnsAsync(result);

      return mockHandler;
   }

   /// <summary>
   /// Sets up the given <paramref name="mockHandler"/> to return the given
   /// <paramref name="result"/> for any request that is provided.
   /// </summary>
   /// <inheritdoc cref="WithResult{TOutput, TRequest}(Mock{IRequestHandler{TOutput, TRequest}}, TRequest, DispatchResult{TOutput})"/>
   public static Mock<IRequestHandler<TOutput, TRequest>> WithResult<TOutput, TRequest>(
      this Mock<IRequestHandler<TOutput, TRequest>> mockHandler,
      TOutput result)
      where TOutput : notnull
      where TRequest : IDispatchRequest
   {
      DispatchResult<TOutput> dispatchResult = new DispatchResult<TOutput>(result);

      mockHandler
         .Setup(h => h.HandleAsync(It.IsAny<TRequest>(), default))
         .ReturnsAsync(dispatchResult);

      return mockHandler;
   }
   #endregion
}
