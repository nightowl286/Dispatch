using TNO.Dispatch.Abstractions.Results;
using TNO.Dispatch.Abstractions.Workflows;
using TNO.Dispatch.Decorators;
using TNO.Dispatch.Results;

namespace TNO.Dispatch.Tests.Decorators
{
   [TestClass]
   public class LazyDecoratorTests
   {
      #region Test Methods
      [TestMethod]
      public async Task HandleAsync_NoInstance_BuildsAndDecoratesInstance()
      {
         // Arrange (Handler)
         object handlerResult = new object();
         Mock<IUnitHandler<object, IUnitRequest>> handlerMock = new Mock<IUnitHandler<object, IUnitRequest>>(MockBehavior.Strict);
         handlerMock.Setup(h => h.HandleAsync(It.IsAny<IUnitRequest>(), default)).ReturnsAsync(new DispatchResult<object>(handlerResult));
         IUnitHandler<object, IUnitRequest> handler = handlerMock.Object;

         Type handlerType = typeof(IUnitHandler<object, IUnitRequest>);

         // Arrange (Workflow)
         Mock<IDispatchWorkflow> workflowMock = new Mock<IDispatchWorkflow>(MockBehavior.Strict);
         workflowMock.Setup(w => w.Build(handler)).Returns(handler);

         // Arrange (Builder)
         Mock<IServiceBuilder> builderMock = new Mock<IServiceBuilder>(MockBehavior.Strict);
         builderMock.Setup(b => b.Build(handlerType)).Returns(handler);

         // Arrange (Sut)
         LazyDecorator<object, IUnitRequest> sut =
            new LazyDecorator<object, IUnitRequest>(
               builderMock.Object,
               workflowMock.Object,
               handlerType);

         // Pre-Act Assert (make sure constructor does not call anything)
         workflowMock.VerifyNever(d => d.Build(It.IsAny<IRequestHandler<object, IUnitRequest>>()));
         builderMock.VerifyNever(b => b.Build(It.IsAny<Type>()));

         // Act
         IDispatchResult<object> result = await sut.HandleAsync(Mock.Of<IUnitRequest>());

         // Assert (Result)
         Assert.IsTrue(result.Successful);
         Assert.AreSame(handlerResult, result.Output);

         // Assert (Builder)
         builderMock.VerifyOnce(b => b.Build(handlerType));

         // Assert (Workflow)
         workflowMock.VerifyOnce(w => w.Build(handler));
      }
      #endregion
   }
}
