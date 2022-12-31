using System;
using TNO.Dispatch.Abstractions.Workflows;

namespace TNO.Dispatch.Abstractions;

/// <summary>
/// Denotes a common dispatch request registrar.
/// </summary>
/// <typeparam name="TCollection">The type of the dispatch collection this registrar belongs to.</typeparam>
public interface IRequestRegistrar<out TCollection>
{
   #region Methods
   /// <summary>
   /// Registers the given <paramref name="handlerType"/> with the given
   /// <paramref name="outputType"/>/<paramref name="requestType"/> combination.
   /// </summary>
   /// <param name="outputType">The type of the output the handler will return.</param>
   /// <param name="requestType">The type of the request the handler can handle.</param>
   /// <param name="handlerType">
   /// The type of the handler to register with the given
   /// <paramref name="outputType"/>/<paramref name="requestType"/> combination.
   /// </param>
   void Register(Type outputType, Type requestType, Type handlerType);

   /// <summary>
   /// Registers the given <paramref name="handlerType"/> for
   /// all <see cref="IRequestHandler{TOutput, TRequest}"/> 
   /// that the <paramref name="handlerType"/> implements.
   /// </summary>
   /// <param name="handlerType">The type of the handler to register.</param>
   void Register(Type handlerType);

   /// <summary>
   /// Registers the given <paramref name="handlerType"/> with the given workflow and
   /// <paramref name="outputType"/>/<paramref name="requestType"/> combination.
   /// </summary>
   /// <param name="outputType">The type of the output the handler will return.</param>
   /// <param name="requestType">The type of the request the handler can handle.</param>
   /// <param name="handlerType">
   /// The type of the handler to register with the given workflow and
   /// <paramref name="outputType"/>/<paramref name="requestType"/> combination.
   /// </param>
   /// <param name="workflow">The workflow to use when registering the <paramref name="handlerType"/>.</param>
   void Register(Type outputType, Type requestType, Type handlerType, IDispatchWorkflow workflow);

   /// <summary>
   /// Registers the given <paramref name="handlerType"/> (with the given <paramref name="workflow"/>) 
   /// for all <see cref="IRequestHandler{TOutput, TRequest}"/> that the <paramref name="handlerType"/> implements.
   /// </summary>
   /// <param name="handlerType">The type of the handler to register.</param>
   /// <param name="workflow">The workflow to use when registering the <paramref name="handlerType"/>.</param>
   void Register(Type handlerType, IDispatchWorkflow workflow);

   /// <summary>Creates a new scope for the current collection.</summary>
   /// <returns>The newly created scope.</returns>
   TCollection CreateScope();
   #endregion
}
