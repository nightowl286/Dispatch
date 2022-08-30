using System.Diagnostics;
using TNO.Dispatch.Abstractions;
using TNO.DependencyInjection.Abstractions.Components;

namespace TNO.Dispatch
{
   internal sealed class DispatchWorkflow : IDispatchWorkflow
   {
      #region Fields
      private readonly List<Type> _decoratorTypes = new List<Type>();
      private readonly IServiceBuilder _builder;
      #endregion
      public DispatchWorkflow(IServiceBuilder builder) => _builder = builder;

      #region Methods
      public IDispatchWorkflow With(Type decoratorType)
      {
         _decoratorTypes.Add(decoratorType);
         return this;
      }
      public IRequestHandler<TOutput, TRequest> Build<TOutput, TRequest>(IRequestHandler<TOutput, TRequest> innerHandler)
         where TOutput : notnull
         where TRequest : notnull, IDispatchRequest
      {
         if (_decoratorTypes.Count == 0)
            return innerHandler;

         IDispatchDecorator<TOutput, TRequest>? firstDecorator = null;
         IDispatchDecorator<TOutput, TRequest>? lastDecorator = null;

         foreach (Type genericDecoratorType in _decoratorTypes)
         {
            Type decoratorType = genericDecoratorType.MakeGenericType(typeof(TOutput), typeof(TRequest));

            object decorator = _builder.Build(decoratorType);
            IDispatchDecorator<TOutput, TRequest> typedDecorator = (IDispatchDecorator<TOutput, TRequest>)decorator;

            firstDecorator ??= typedDecorator;
            if (lastDecorator is not null)
               lastDecorator.InnerHandler = typedDecorator;
            lastDecorator = typedDecorator;
         }

         Debug.Assert(lastDecorator is not null);
         lastDecorator.InnerHandler = innerHandler;

         Debug.Assert(firstDecorator is not null);
         return firstDecorator;
      }
      #endregion
   }
}
