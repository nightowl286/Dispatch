using TNO.Dispatch.Abstractions.Results;
using TNO.Dispatch.Abstractions.Workflows;
using TNO.Dispatch.Decorators;

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
         IRequestHandler<object, IDispatchRequest> handler =
            new Mock<IRequestHandler<object, IDispatchRequest>>(MockBehavior.Strict)
            .WithResult(handlerResult)
            .Object;

         Type handlerType = typeof(IRequestHandler<object, IDispatchRequest>);

         // Arrange (Workflow)
         Mock<IDispatchWorkflow> workflowMock =
            new Mock<IDispatchWorkflow>(MockBehavior.Strict)
            .With(handler);

         // Arrange (Builder)
         Mock<IServiceBuilder> builderMock = new Mock<IServiceBuilder>(MockBehavior.Strict);
         builderMock.Setup(b => b.Build(handlerType)).Returns(handler);

         // Arrange (Sut)
         LazyDecorator<object, IDispatchRequest> sut =
            new LazyDecorator<object, IDispatchRequest>(
               builderMock.Object,
               workflowMock.Object,
               handlerType);

         // Pre-Act Assert (make sure constructor does not call anything)
         workflowMock.VerifyNever();
         builderMock.VerifyNever(b => b.Build(It.IsAny<Type>()));

         // Act
         IDispatchResult<object> result = await sut.HandleAsync(Mock.Of<IDispatchRequest>());

         // Assert (Result)
         Assert.IsTrue(result.Successful);
         Assert.AreSame(handlerResult, result.Output);

         // Assert (Builder)
         builderMock.VerifyOnce(b => b.Build(handlerType));

         // Assert (Workflow)
         workflowMock.VerifyOnce(handler);
      }
      #endregion
   }
}
