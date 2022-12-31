namespace TNO.Dispatch.Tests;

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

        DispatchWorkflow sut = new DispatchWorkflow(builder.Object, new[] { genericDecoratorType });

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
        IUnitHandler<object, IUnitRequest> handler = Mock.Of<IUnitHandler<object, IUnitRequest>>();
        IUnitDecorator<object, IUnitRequest> expectedInnerDecorator = Mock.Of<IUnitDecorator<object, IUnitRequest>>();
        IUnitDecorator2<object, IUnitRequest> expectedOuterDecorator = Mock.Of<IUnitDecorator2<object, IUnitRequest>>();

        Type innerDecoratorType = typeof(IUnitDecorator<object, IUnitRequest>);
        Type outerDecoratorType = typeof(IUnitDecorator2<object, IUnitRequest>);

        Type genericInnerDecoratorType = typeof(IUnitDecorator<,>);
        Type genericOuterDecoratorType = typeof(IUnitDecorator2<,>);

        Mock<IServiceBuilder> builder = new Mock<IServiceBuilder>();
        builder.Setup(b => b.Build(innerDecoratorType)).Returns(expectedInnerDecorator);
        builder.Setup(b => b.Build(outerDecoratorType)).Returns(expectedOuterDecorator);

        DispatchWorkflow sut = new DispatchWorkflow(builder.Object, new[] { genericInnerDecoratorType, genericOuterDecoratorType });

        // Act
        IRequestHandler<object, IUnitRequest> decoratedHandler = sut.Build(handler);

        // Assert
        Assert.AreSame(expectedOuterDecorator, decoratedHandler);

        IUnitDecorator2<object, IUnitRequest> outerDecorator = (IUnitDecorator2<object, IUnitRequest>)decoratedHandler;
        Assert.AreSame(expectedInnerDecorator, outerDecorator.InnerHandler);

        IUnitDecorator<object, IUnitRequest> innerDecorator = (IUnitDecorator<object, IUnitRequest>)outerDecorator.InnerHandler;
        Assert.AreSame(handler, innerDecorator.InnerHandler);

        builder.VerifyOnce(b => b.Build(innerDecoratorType));
        builder.VerifyOnce(b => b.Build(outerDecoratorType));
    }
    #endregion

    #region Helpers
    private static DispatchWorkflow CreateSut() => new DispatchWorkflow(Mock.Of<IServiceBuilder>(), Array.Empty<Type>());
    #endregion
}
