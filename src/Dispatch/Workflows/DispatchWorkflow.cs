using System.Diagnostics;
using TNO.DependencyInjection.Abstractions.Components;
using TNO.Dispatch.Abstractions;
using TNO.Dispatch.Abstractions.Workflows;
using TNO.Dispatch.Workflows;

namespace TNO.Dispatch;

internal sealed class DispatchWorkflow : IDispatchWorkflow
{
    #region Fields
    private readonly List<Type> _decoratorTypes;
    private readonly IServiceBuilder _builder;
    #endregion
    public DispatchWorkflow(IServiceBuilder builder, IEnumerable<Type> decoratorTypes)
    {
        _builder = builder;
        _decoratorTypes = new List<Type>(decoratorTypes);
    }

    #region Methods
    public IRequestHandler<TOutput, TRequest> Build<TOutput, TRequest>(IRequestHandler<TOutput, TRequest> innerHandler)
       where TOutput : notnull
       where TRequest : notnull, IDispatchRequest
    {
        if (_decoratorTypes.Count == 0)
            return innerHandler;

        IDispatchDecorator<TOutput, TRequest>? lastDecorator = null;

        foreach (Type genericDecoratorType in _decoratorTypes)
        {
            Type decoratorType = genericDecoratorType.MakeGenericType(typeof(TOutput), typeof(TRequest));

            object decorator = _builder.Build(decoratorType);
            IDispatchDecorator<TOutput, TRequest> typedDecorator = (IDispatchDecorator<TOutput, TRequest>)decorator;

            typedDecorator.InnerHandler = lastDecorator ?? innerHandler;
            lastDecorator = typedDecorator;
        }

        Debug.Assert(lastDecorator is not null);
        return lastDecorator;
    }

    public IWorkflowBuilder Clone() => new WorkflowBuilder(_builder, _decoratorTypes);
    #endregion
}
