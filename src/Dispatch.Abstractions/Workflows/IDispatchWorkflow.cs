namespace TNO.Dispatch.Abstractions.Workflows
{
   /// <summary>
   /// Denotes a common dispatch workflow.
   /// </summary>
   public interface IDispatchWorkflow
   {
      #region Methods
      /// <summary>Decorates the given <paramref name="handler"/> with this workflow.</summary>
      /// <typeparam name="TOutput">The output type of the given <paramref name="handler"/>.</typeparam>
      /// <typeparam name="TRequest">The request type that the given <paramref name="handler"/> can handle.</typeparam>
      /// <param name="handler">The handler to decorate.</param>
      /// <returns></returns>
      IRequestHandler<TOutput, TRequest> Build<TOutput, TRequest>(IRequestHandler<TOutput, TRequest> handler)
         where TOutput : notnull
         where TRequest : notnull, IDispatchRequest;

      /// <summary>
      /// Creates a new <see cref="IWorkflowBuilder"/> with the current decorators already added.
      /// </summary>
      /// <returns>A new <see cref="IWorkflowBuilder"/>.</returns>
      IWorkflowBuilder Clone();
      #endregion
   }
}
