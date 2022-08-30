namespace TNO.Dispatch.Tests
{
   [TestClass]
   [TestCategory(Category.Dispatch)]
   public class DispatchWorkflowTests
   {
      #region Tests
      [TestMethod]
      public void Build_WithNoDecorators_ReturnsHandler()
      {
         // Arrange
         DispatchWorkflow sut = CreateSut();
         IUnitHandler<object, IUnitRequest> expectedHandler = Mock.Of<IUnitHandler<object, IUnitRequest>>();

         // Act
         IRequestHandler<object, IUnitRequest> handler = sut.Build(expectedHandler);

         // Assert
         Assert.AreSame(expectedHandler, handler);
      }

      [TestMethod]
      public void Build_WithOneDecorator_ReturnsDecoratorWithInnerHandler()
      {
         // Arrange
         IUnitHandler<object, IUnitRequest> expectedHandler = Mock.Of<IUnitHandler<object, IUnitRequest>>();
         IUnitDecorator<object, IUnitRequest> expectedDecorator = Mock.Of<IUnitDecorator<object, IUnitRequest>>();
         Type decoratorType = typeof(IUnitDecorator<object, IUnitRequest>);
         Type genericDecoratorType = typeof(IUnitDecorator<,>);

         Mock<IServiceBuilder> builder = new Mock<IServiceBuilder>();
         builder.Setup(b => b.Build(decoratorType)).Returns(expectedDecorator);

         DispatchWorkflow sut = new DispatchWorkflow(builder.Object);
         sut.With(genericDecoratorType);

         // Act
         IRequestHandler<object, IUnitRequest> decoratedHandler = sut.Build(expectedHandler);

         // Assert
         Assert.AreSame(expectedDecorator, decoratedHandler);

         IUnitDecorator<object, IUnitRequest> decorator = (IUnitDecorator<object, IUnitRequest>)decoratedHandler;
         Assert.AreSame(expectedHandler, decorator.InnerHandler);

         builder.VerifyOnce(b => b.Build(decoratorType));
      }

      [TestMethod]
      public void Build_WithTwoDecorators_ReturnsCorrectStructure()
      {
         // Arrange
         IUnitHandler<object, IUnitRequest> expectedHandler = Mock.Of<IUnitHandler<object, IUnitRequest>>();
         IUnitDecorator<object, IUnitRequest> expectedDecorator1 = Mock.Of<IUnitDecorator<object, IUnitRequest>>();
         IUnitDecorator2<object, IUnitRequest> expectedDecorator2 = Mock.Of<IUnitDecorator2<object, IUnitRequest>>();

         Type decorator1Type = typeof(IUnitDecorator<object, IUnitRequest>);
         Type decorator2Type = typeof(IUnitDecorator2<object, IUnitRequest>);

         Type genericDecorator1Type = typeof(IUnitDecorator<,>);
         Type genericDecorator2Type = typeof(IUnitDecorator2<,>);

         Mock<IServiceBuilder> builder = new Mock<IServiceBuilder>();
         builder.Setup(b => b.Build(decorator1Type)).Returns(expectedDecorator1);
         builder.Setup(b => b.Build(decorator2Type)).Returns(expectedDecorator2);

         DispatchWorkflow sut = new DispatchWorkflow(builder.Object);
         sut.With(genericDecorator1Type);
         sut.With(genericDecorator2Type);

         // Act
         IRequestHandler<object, IUnitRequest> decoratedHandler = sut.Build(expectedHandler);

         // Assert
         Assert.AreSame(expectedDecorator1, decoratedHandler);

         IUnitDecorator<object, IUnitRequest> decorator1 = (IUnitDecorator<object, IUnitRequest>)decoratedHandler;
         Assert.AreSame(expectedDecorator2, decorator1.InnerHandler);

         IUnitDecorator2<object, IUnitRequest> decorator2 = (IUnitDecorator2<object, IUnitRequest>)decorator1.InnerHandler;
         Assert.AreSame(expectedHandler, decorator2.InnerHandler);

         builder.VerifyOnce(b => b.Build(decorator1Type));
         builder.VerifyOnce(b => b.Build(decorator2Type));
      }
      #endregion

      #region Helpers
      private static DispatchWorkflow CreateSut() => new DispatchWorkflow(Mock.Of<IServiceBuilder>());
      #endregion
   }
}
