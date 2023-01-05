using System.Diagnostics;
using TNO.DependencyInjection.Abstractions;
using TNO.DependencyInjection.Abstractions.Components;
using TNO.Dispatch.Abstractions;
using TNO.Dispatch.Abstractions.Workflows;
using TNO.Dispatch.Decorators;
using TNO.Dispatch.Results;
using TNO.Dispatch.Workflows;

namespace TNO.Dispatch;

/// <summary>
/// A base class for a dispatch collection.
/// </summary>
/// <typeparam name="TRequestConstraint">The type of the allowed requests.</typeparam>
/// <typeparam name="TCollection">The type of the collection itself.</typeparam>
public abstract class DispatchCollection<TRequestConstraint, TCollection> : IRequestRegistrar<TCollection>, IRequestDispatcher<TRequestConstraint>, IWorkflowCreator
where TRequestConstraint : notnull, IDispatchRequest
{
   #region Fields
   /// <summary>The service facade scope that will contain the registered handlers.</summary>
   protected readonly IServiceFacade _serviceFacade;

   /// <summary>The outer collection scope that can be used as a fall back.</summary>
   protected readonly DispatchCollection<TRequestConstraint, TCollection>? _outerScope;

   /// <summary>The type of the open request handler interface.</summary>
   protected static readonly Type OpenHandlerInterfaceType = typeof(IRequestHandler<,>);
   #endregion

   #region Constructors
   /// <summary>Creates a new dispatch collection using the given <paramref name="outerScope"/>.</summary>
   /// <param name="outerScope">The service scope that will be used to create the main scope for this collection.</param>
   public DispatchCollection(IServiceScope outerScope)
   {
      _serviceFacade = outerScope.CreateScope(AppendValueMode.ReplaceAll);
   }

   /// <summary>Creates a new dispatch collection with the given <paramref name="scope"/> and <paramref name="outerScope"/>.</summary>
   /// <param name="scope">The main scope of this collection.</param>
   /// <param name="outerScope">The outer scope of this collection.</param>
   /// <exception cref="ArgumentException">
   /// Throw if the given <paramref name="scope"/> does not have
   /// a <see cref="IServiceRegistrar.DefaultRegistrationMode"/>
   /// of <see cref="AppendValueMode.ReplaceAll"/>.
   /// </exception>
   protected DispatchCollection(IServiceFacade scope, DispatchCollection<TRequestConstraint, TCollection> outerScope)
   {
      if (scope.DefaultRegistrationMode != AppendValueMode.ReplaceAll)
         throw new ArgumentException($"The given scope must have the {nameof(scope.DefaultRegistrationMode)} as {AppendValueMode.ReplaceAll}.", nameof(scope));
      _serviceFacade = scope;
      _outerScope = outerScope;
   }
   #endregion

   #region Dispatcher
   /// <inheritdoc/>
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

   /// <inheritdoc/>
   public async ValueTask<DispatchResult<TOutput>> DispatchAsync<TOutput, TRequest>(TRequest request, CancellationToken cancellationToken = default)
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
   /// <inheritdoc/>
   public IRequestRegistrar<TCollection> Register(Type outputType, Type requestType, Type handlerType)
   {
      Type handlerInterfaceType = CreateHandlerInterfaceType(outputType, requestType);

      _serviceFacade.Singleton(handlerInterfaceType, handlerType);

      return this;
   }

   /// <inheritdoc/>
   public IRequestRegistrar<TCollection> Register(Type outputType, Type requestType, object handler)
   {
      Type handlerInterfaceType = CreateHandlerInterfaceType(outputType, requestType);

      _serviceFacade.Instance(handlerInterfaceType, handler);

      return this;
   }

   /// <inheritdoc/>
   public IRequestRegistrar<TCollection> Register(Type handlerType)
   {
      foreach (Type interfaceType in FindHandlerInterfaceImplementations(handlerType))
      {
         Type[] genericArguments = interfaceType.GetGenericArguments();
         Debug.Assert(genericArguments.Length == 2);

         Type outputType = genericArguments[0];
         Type requestType = genericArguments[1];
         Register(outputType, requestType, handlerType);
      }

      return this;
   }

   /// <inheritdoc/>
   public IRequestRegistrar<TCollection> Register(object handler)
   {
      Type handlerType = handler.GetType();
      foreach (Type interfaceType in FindHandlerInterfaceImplementations(handlerType))
      {
         Type[] genericArguments = interfaceType.GetGenericArguments();
         Debug.Assert(genericArguments.Length == 2);

         Type outputType = genericArguments[0];
         Type requestType = genericArguments[1];
         Register(outputType, requestType, handler);
      }

      return this;
   }

   /// <inheritdoc/>
   public IRequestRegistrar<TCollection> Register(Type outputType, Type requestType, Type handlerType, IDispatchWorkflow workflow)
   {
      // Todo(Nightowl): This might need a different approach? This works but doesn't seem that neat;
      Type lazyType = typeof(LazyDecorator<,>).MakeGenericType(outputType, requestType);
      object lazyDecorator =
         Activator.CreateInstance(lazyType, _serviceFacade, workflow, handlerType)
         ?? throw new NullReferenceException($"Failed to create a lazy decorator in the {nameof(DispatchCollection<TRequestConstraint, TCollection>)}.");

      Type handlerInterfaceType = CreateHandlerInterfaceType(outputType, requestType);

      _serviceFacade.Instance(handlerInterfaceType, lazyDecorator);

      return this;
   }

   /// <inheritdoc/>
   public IRequestRegistrar<TCollection> Register(Type handlerType, IDispatchWorkflow workflow)
   {
      foreach (Type interfaceType in FindHandlerInterfaceImplementations(handlerType))
      {
         Type[] genericArguments = interfaceType.GetGenericArguments();
         Debug.Assert(genericArguments.Length == 2);

         Type outputType = genericArguments[0];
         Type requestType = genericArguments[1];
         Register(outputType, requestType, handlerType, workflow);
      }

      return this;
   }

   /// <inheritdoc/>
   public abstract TCollection CreateScope();

   #endregion

   #region Methods
   /// <inheritdoc/>
   public IWorkflowBuilder NewWorkflow() => new WorkflowBuilder(_serviceFacade);
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
         if (interfaceType.IsGenericType == false)
            continue;

         Type genericDefinition = interfaceType.GetGenericTypeDefinition();
         if (genericDefinition == OpenHandlerInterfaceType)
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
   protected Type CreateHandlerInterfaceType(Type outputType, Type requestType) => OpenHandlerInterfaceType.MakeGenericType(outputType, requestType);
   #endregion
}
