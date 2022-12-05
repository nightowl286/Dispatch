using Moq;
using TNO.Dispatch.Abstractions;
using TNO.Dispatch.Abstractions.Workflows;

namespace TNO.Tests.Moq.Dispatch.Abstractions
{
   /// <summary>
   /// Contains useful extension methods for the <see cref="Mock"/>&lt;<see cref="IDispatchWorkflow"/>&gt; class.
   /// </summary>
   public static class MockDispatchWorkflowExtensions
   {
      #region Methods
      /// <summary>
      /// Sets up the given <paramref name="mockWorkflow"/> to return the <paramref name="returned"/>
      /// handler, when the <paramref name="requested"/> handler is requested.
      /// </summary>
      /// <typeparam name="TOutput">The output type of the handler.</typeparam>
      /// <typeparam name="TRequest">The request type of the handler.</typeparam>
      /// <param name="mockWorkflow">The mock workflow to setup.</param>
      /// <param name="requested">The handler that will be requested.</param>
      /// <param name="returned">The handler that should be returned.</param>
      /// <returns>The <paramref name="mockWorkflow"/> instance that was given.</returns>
      public static Mock<IDispatchWorkflow> With<TOutput, TRequest>(
         this Mock<IDispatchWorkflow> mockWorkflow,
         IRequestHandler<TOutput, TRequest> requested,
         IRequestHandler<TOutput, TRequest> returned)
         where TOutput : notnull
         where TRequest : IDispatchRequest
      {
         mockWorkflow.Setup(w => w.Build(requested)).Returns(returned);
         return mockWorkflow;
      }

      /// <summary>
      /// Sets up the given <paramref name="mockWorkflow"/> to return the 
      /// <paramref name="requestedAndReturned"/> handler, whenever it is requested.
      /// </summary>
      /// <param name="mockWorkflow">The mock workflow to setup.</param>
      /// <param name="requestedAndReturned">The handler that will be requested and should be returned.</param>
      /// <inheritdoc cref="With{TOutput, TRequest}(Mock{IDispatchWorkflow}, IRequestHandler{TOutput, TRequest}, IRequestHandler{TOutput, TRequest})"/>
      public static Mock<IDispatchWorkflow> With<TOutput, TRequest>(
         this Mock<IDispatchWorkflow> mockWorkflow,
         IRequestHandler<TOutput, TRequest> requestedAndReturned)
         where TOutput : notnull
         where TRequest : IDispatchRequest
      {
         mockWorkflow.Setup(w => w.Build(requestedAndReturned)).Returns(requestedAndReturned);
         return mockWorkflow;
      }

      /// <summary>Verifies that the given <paramref name="handler"/> was only requested to be built once.</summary>
      /// <param name="mockWorkflow">The mock workflow to verify.</param>
      /// <param name="handler">The handler that should be checked.</param>
      /// <inheritdoc cref="With{TOutput, TRequest}(Mock{IDispatchWorkflow}, IRequestHandler{TOutput, TRequest})"/>
      public static Mock<IDispatchWorkflow> VerifyOnce<TOutput, TRequest>(this Mock<IDispatchWorkflow> mockWorkflow, IRequestHandler<TOutput, TRequest> handler)
         where TOutput : notnull
         where TRequest : IDispatchRequest
      {
         mockWorkflow.VerifyOnce(w => w.Build(handler));
         return mockWorkflow;
      }

      /// <summary>Verifies that the given <paramref name="handler"/> was never requested to be built.</summary>
      /// <inheritdoc cref="VerifyOnce{TOutput, TRequest}(Mock{IDispatchWorkflow}, IRequestHandler{TOutput, TRequest})"/>
      public static Mock<IDispatchWorkflow> VerifyNever<TOutput, TRequest>(this Mock<IDispatchWorkflow> mockWorkflow, IRequestHandler<TOutput, TRequest> handler)
         where TOutput : notnull
         where TRequest : IDispatchRequest
      {
         mockWorkflow.VerifyNever(w => w.Build(handler));
         return mockWorkflow;
      }

      /// <summary>Verifies that no handler was requested to be built.</summary>
      /// <inheritdoc cref="VerifyOnce{TOutput, TRequest}(Mock{IDispatchWorkflow}, IRequestHandler{TOutput, TRequest})"/>
      public static Mock<IDispatchWorkflow> VerifyNever<TOutput, TRequest>(this Mock<IDispatchWorkflow> mockWorkflow)
         where TOutput : notnull
         where TRequest : IDispatchRequest
      {
         mockWorkflow.VerifyNever(w => w.Build(It.IsAny<IRequestHandler<TOutput, TRequest>>()));
         return mockWorkflow;
      }

      /// <inheritdoc cref="VerifyNever{TOutput, TRequest}(Mock{IDispatchWorkflow})"/>
      /// <remarks>This will assume the request type to be <see cref="IDispatchRequest"/>.</remarks>
      public static Mock<IDispatchWorkflow> VerifyNever<TOutput>(this Mock<IDispatchWorkflow> mockWorkflow)
        where TOutput : notnull
      {
         mockWorkflow.VerifyNever(w => w.Build(It.IsAny<IRequestHandler<TOutput, IDispatchRequest>>()));
         return mockWorkflow;
      }

      /// <inheritdoc cref="VerifyNever{TOutput, TRequest}(Mock{IDispatchWorkflow})"/>
      /// <remarks>
      /// This will assume the output type to be <see cref="object"/>, and the request type to be <see cref="IDispatchRequest"/>.
      /// </remarks>
      public static Mock<IDispatchWorkflow> VerifyNever(this Mock<IDispatchWorkflow> mockWorkflow)
      {
         mockWorkflow.VerifyNever(w => w.Build(It.IsAny<IRequestHandler<object, IDispatchRequest>>()));
         return mockWorkflow;
      }
      #endregion
   }
}
