using TNO.DependencyInjection.Abstractions.Components;
using TNO.Dispatch.Abstractions;
using TNO.Dispatch.Abstractions.Results;
using TNO.Dispatch.Abstractions.Workflows;

namespace TNO.Dispatch.Decorators;

internal sealed class LazyDecorator<TOutput, TRequest> : IRequestHandler<TOutput, TRequest>
   where TOutput : notnull
   where TRequest : notnull, IDispatchRequest
{
    #region Fields
    private IRequestHandler<TOutput, TRequest>? _instance;
    private readonly IServiceBuilder _builder;
    private readonly Type _handlerType;
    private readonly IDispatchWorkflow _workflow;
    #endregion
    public LazyDecorator(IServiceBuilder builder, IDispatchWorkflow workflow, Type handlerType)
    {
        _builder = builder;
        _handlerType = handlerType;
        _workflow = workflow;
    }

    #region Methods
    public async ValueTask<IDispatchResult<TOutput>> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        if (_instance is null)
        {
            object handler = _builder.Build(_handlerType);
            IRequestHandler<TOutput, TRequest> typedHandler = (IRequestHandler<TOutput, TRequest>)handler;

            _instance = _workflow.Build(typedHandler);
        }

        return await _instance.HandleAsync(request, cancellationToken);
    }
    #endregion
}
