using System.Diagnostics;
using TNO.DependencyInjection.Abstractions;
using TNO.DependencyInjection.Abstractions.Components;
using TNO.Dispatch.Abstractions;
using TNO.Dispatch.Abstractions.Results;

namespace TNO.Dispatch
{
   public abstract class DispatchCollection<TRequestConstraint, TCollection> : IRequestRegistrar<TCollection>, IRequestDispatcher<TRequestConstraint>, IWorkflowCreator
      where TRequestConstraint : notnull, IDispatchRequest
   {
      #region Fields
      protected readonly IServiceFacade _serviceFacade;
      protected readonly DispatchCollection<TRequestConstraint, TCollection>? _outerScope;
      protected readonly Type _genericHandlerInterfaceType = typeof(IRequestHandler<,>);
      #endregion

      public DispatchCollection(IServiceScope scope)
      {
         _serviceFacade = scope.CreateScope(AppendValueMode.ReplaceAll);
      }
      protected DispatchCollection(IServiceFacade scope, DispatchCollection<TRequestConstraint, TCollection> outerScope)
      {
         if (scope.DefaultRegistrationMode != AppendValueMode.ReplaceAll)
            throw new ArgumentException($"The given scope must have the {nameof(scope.DefaultRegistrationMode)} as {AppendValueMode.ReplaceAll}.", nameof(scope));
         _serviceFacade = scope;
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

         if (_serviceFacade.IsRegistered(handlerInterfaceType))
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
         object? handler = _serviceFacade.GetOptional(handlerInterfaceType);

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

         _serviceFacade.Singleton(handlerInterfaceType, handlerType);
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
         object lazyDecorator = 
            Activator.CreateInstance(lazyType, _serviceFacade, workflow, handlerType)
            ?? throw new NullReferenceException($"Failed to create a lazy decorator in the {nameof(DispatchCollection<TRequestConstraint, TCollection>)}.");

         Type handlerInterfaceType = CreateHandlerInterfaceType(outputType, requestType);

         _serviceFacade.Instance(handlerInterfaceType, lazyDecorator);
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

      /// <summary>Creates a new scope for this dispatch collection.</summary>
      /// <returns>A <typeparamref name="TCollection"/> that represents the new scope.</returns>
      public abstract TCollection CreateScope();
      #endregion

      #region Helpers
      /// <summary>
      /// Finds all the types of <see cref="IRequestHandler{TOutput, TRequest}"/>
      /// that the given <paramref name="handlerType"/> implements.
      /// </summary>
      /// <param name="handlerType">The type of the handler to check.</param>
      /// <returns>
      /// An enumerable of all the variants of <see cref="IRequestHandler{TOutput, TRequest}"/>
      /// that the given <paramref name="handlerType"/> implements.
      /// </returns>
      protected IEnumerable<Type> FindHandlerInterfaceImplementations(Type handlerType)
      {
         foreach (Type interfaceType in handlerType.GetInterfaces())
         {
            Type genericDefinition = interfaceType.GetGenericTypeDefinition();
            if (genericDefinition == _genericHandlerInterfaceType)
               yield return interfaceType;
         }
      }

      /// <summary>
      /// Constructs a generic interface type (using <see cref="IRequestHandler{TOutput, TRequest}"/>)
      /// for the given <paramref name="outputType"/> and <paramref name="requestType"/>.
      /// </summary>
      /// <param name="outputType">The output type to use.</param>
      /// <param name="requestType">The request type to use.</param>
      /// <returns></returns>
      protected Type CreateHandlerInterfaceType(Type outputType, Type requestType) => _genericHandlerInterfaceType.MakeGenericType(outputType, requestType);
      public IDispatchWorkflow CreateWorkflow() => new DispatchWorkflow(_serviceFacade);
      #endregion
   }
}
