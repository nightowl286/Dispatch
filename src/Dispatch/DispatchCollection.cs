using System.Diagnostics;
using TNO.Dispatch.Abstractions;
using TNO.Dispatch.Abstractions.Results;
using TNO.DependencyInjection.Abstractions.Components;
using TNO.DependencyInjection.Components;
using TNO.Dispatch;

namespace TNO.Dispatch
{
   public abstract class DispatchCollection<TRequestConstraint, TCollection> : IRequestRegistrar<TCollection>, IRequestDispatcher<TRequestConstraint>, IWorkflowCreator
      where TRequestConstraint : notnull, IDispatchRequest
   {
      #region Fields
      protected readonly SimpleCollection _collection;
      protected readonly ReplaceMode _mode;
      protected readonly DispatchCollection<TRequestConstraint, TCollection>? _outerScope;
      protected readonly Type _genericHandlerInterfaceType = typeof(IRequestHandler<,>);
      #endregion

      public DispatchCollection(IServiceBuilder builder, ReplaceMode mode = ReplaceMode.Throw)
      {
         _collection = new SimpleCollection(builder);
         _mode = mode;
      }
      protected DispatchCollection(SimpleCollection collection, ReplaceMode mode, DispatchCollection<TRequestConstraint, TCollection> outerScope)
      {
         _collection = collection;
         _mode = mode;
         _outerScope = outerScope;
      }

      #region Dispatcher
      public bool CanDispatch<TOutput, TRequest>()
         where TOutput : notnull
         where TRequest : TRequestConstraint
      {
         Type outputType = typeof(TOutput);
         Type requestType = typeof(TRequest);

         Type handlerInterfaceType = CreateHandlerInterfaceType(outputType, requestType);

         if (_collection.IsRegistered(handlerInterfaceType))
            return true;

         return _outerScope?.CanDispatch<TOutput, TRequest>() == true;
      }
      public async ValueTask<IDispatchResult<TOutput>> DispatchAsync<TOutput, TRequest>(TRequest request, CancellationToken cancellationToken = default)
         where TOutput : notnull
         where TRequest : TRequestConstraint
      {
         Type outputType = typeof(TOutput);
         Type requestType = typeof(TRequest);

         Type handlerInterfaceType = CreateHandlerInterfaceType(outputType, requestType);
         object? handler = _collection.GetOptional(handlerInterfaceType);

         if (handler is IRequestHandler<TOutput, TRequest> typedHandler)
            return await typedHandler.HandleAsync(request, cancellationToken);

         if (_outerScope is not null)
            return await _outerScope.DispatchAsync<TOutput, TRequest>(request, cancellationToken);

         throw new Exception($"A handler has not be registered for the combination ({outputType}), ({requestType}).");
      }
      #endregion

      #region Registrar
      public void Register(Type outputType, Type requestType, Type handlerType)
      {
         Type handlerInterfaceType = CreateHandlerInterfaceType(outputType, requestType);

         _collection.Singleton(handlerInterfaceType, handlerType, _mode);
      }
      public void Register(Type handlerType)
      {
         foreach (Type interfaceType in FindHandlerInterfaceImplementations(handlerType))
         {
            Type[] genericArguments = interfaceType.GetGenericArguments();
            Debug.Assert(genericArguments.Length == 2);

            Type outputType = genericArguments[0];
            Type requestType = genericArguments[1];
            Register(outputType, requestType, handlerType);
         }
      }
      public void Register(Type outputType, Type requestType, Type handlerType, IDispatchWorkflow workflow)
      {
         Type lazyType = typeof(LazyDecorator<,>).MakeGenericType(outputType, requestType);
         object lazyDecorator = Activator.CreateInstance(lazyType, (IServiceBuilder)_collection, workflow, handlerType)!;

         Type handlerInterfaceType = CreateHandlerInterfaceType(outputType, requestType);

         _collection.Instance(handlerInterfaceType, lazyDecorator, _mode);
      }
      public void Register(Type handlerType, IDispatchWorkflow workflow)
      {
         foreach (Type interfaceType in FindHandlerInterfaceImplementations(handlerType))
         {
            Type[] genericArguments = interfaceType.GetGenericArguments();
            Debug.Assert(genericArguments.Length == 2);

            Type outputType = genericArguments[0];
            Type requestType = genericArguments[1];
            Register(outputType, requestType, handlerType, workflow);
         }
      }
      public abstract TCollection CreateScope();
      #endregion

      #region Helpers
      protected IEnumerable<Type> FindHandlerInterfaceImplementations(Type handlerType)
      {
         foreach (Type interfaceType in handlerType.GetInterfaces())
         {
            Type genericDefinition = interfaceType.GetGenericTypeDefinition();
            if (genericDefinition == _genericHandlerInterfaceType)
               yield return interfaceType;
         }
      }
      protected Type CreateHandlerInterfaceType(Type outputType, Type requestType) => _genericHandlerInterfaceType.MakeGenericType(outputType, requestType);
      public IDispatchWorkflow CreateWorkflow() => new DispatchWorkflow(_collection);
      #endregion
   }
}
