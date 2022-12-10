using TNO.Dispatch.Abstractions.Results;
using TNO.Dispatch.Abstractions.Workflows;
using TNO.Dispatch.Decorators;
using TNO.Tests.Moq.DependencyInjection.Abstractions;

namespace TNO.Dispatch.Tests.Decorators
{
   [TestClass]
   [TestCategory(Category.Dispatch)]
   [TestCategory(Category.Decorator)]
   public class LazyDecoratorTests
   {
      #region Test Methods
      [TestMethod]
      public async Task HandleAsync_NoInstance_BuildsAndDecoratesInstance()
      {
         // Arrange (Handler)
         object handlerResult = new object();
         Mock<IRequestHandler<object, IDispatchRequest>> mockHandler = CreateMockHandler(handlerResult);
         Type handlerType = typeof(IRequestHandler<object, IDispatchRequest>);
         IDispatchRequest mockRequest = Mock.Of<IDispatchRequest>();

         // Arrange (Workflow)
         Mock<IDispatchWorkflow> workflowMock = CreateMockWorkflow(mockHandler.Object);

         // Arrange (Builder)
         Mock<IServiceBuilder> builderMock = new Mock<IServiceBuilder>(MockBehavior.Strict)
            .WithBuild(handlerType, mockHandler.Object);

         // Arrange (Sut)
         LazyDecorator<object, IDispatchRequest> sut =
            new LazyDecorator<object, IDispatchRequest>(builderMock.Object, workflowMock.Object, handlerType);

         // Pre-Act Assert (make sure constructor does not call anything)
         workflowMock.VerifyNever();
         builderMock.VerifyNever(b => b.Build(It.IsAny<Type>()));

         // Act
         IDispatchResult<object> result = await sut.HandleAsync(mockRequest);

         // Assert (Result)
         Assert.IsTrue(result.Successful);
         Assert.AreSame(handlerResult, result.Output);

         // Assert (Builder)
         builderMock.VerifyOnce(b => b.Build(handlerType));

         // Assert (Workflow)
         workflowMock.VerifyOnce(mockHandler.Object);

         // Assert (Handler)
         mockHandler.VerifyOnce(h => h.HandleAsync(mockRequest, default));

         // Assert (No other calls)
         Assert.That.NoOtherCalls(builderMock, workflowMock, mockHandler);
      }

      [TestMethod]
      public async Task HandleAsync_WithInstance_ReusesInstance()
      {
         // Arrange (Handler)
         object handlerResult = new object();
         Mock<IRequestHandler<object, IDispatchRequest>> mockHandler = CreateMockHandler(handlerResult);
         Type handlerType = typeof(IRequestHandler<object, IDispatchRequest>);
         IDispatchRequest mockRequest = Mock.Of<IDispatchRequest>();

         // Arrange (Workflow)
         Mock<IDispatchWorkflow> workflowMock = CreateMockWorkflow(mockHandler.Object);

         // Arrange (Builder)
         Mock<IServiceBuilder> builderMock = new Mock<IServiceBuilder>(MockBehavior.Strict)
            .WithBuild(handlerType, mockHandler.Object);

         // Arrange (Sut)
         LazyDecorator<object, IDispatchRequest> sut =
            new LazyDecorator<object, IDispatchRequest>(builderMock.Object, workflowMock.Object, handlerType);
         IDispatchResult<object> result = await sut.HandleAsync(mockRequest);

         // Arrange Assert (Result)
         Assert.IsTrue(result.Successful);
         Assert.AreSame(handlerResult, result.Output);

         // Act
         result = await sut.HandleAsync(mockRequest);

         // Assert (Result)
         Assert.IsTrue(result.Successful);
         Assert.AreSame(handlerResult, result.Output);

         // Assert (Builder)
         builderMock.VerifyOnce(b => b.Build(handlerType));

         // Assert (Workflow)
         workflowMock.VerifyOnce(mockHandler.Object);

         // Assert (Handler)
         mockHandler.Verify(h => h.HandleAsync(mockRequest, default), Times.Exactly(2));

         // Assert (No other calls)
         Assert.That.NoOtherCalls(builderMock, workflowMock, mockHandler);

      }
      #endregion

      #region Helpers
      private static Mock<IRequestHandler<object, IDispatchRequest>> CreateMockHandler(object result)
      {
         Mock<IRequestHandler<object, IDispatchRequest>> mockHandler =
            new Mock<IRequestHandler<object, IDispatchRequest>>(MockBehavior.Strict)
            .WithResult(result);

         return mockHandler;
      }
      private static Mock<IDispatchWorkflow> CreateMockWorkflow(IRequestHandler<object, IDispatchRequest> handler)
      {
         return new Mock<IDispatchWorkflow>(MockBehavior.Strict)
            .With(handler);
      }
      #endregion
   }
}
