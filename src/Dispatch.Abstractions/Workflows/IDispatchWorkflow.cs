namespace TNO.Dispatch.Abstractions.Workflows
{
   /// <summary>
   /// Denotes a common dispatch workflow.
   /// </summary>
   public interface IDispatchWorkflow
   {
      #region Methods
      IRequestHandler<TOutput, TRequest> Build<TOutput, TRequest>(IRequestHandler<TOutput, TRequest> innerHandler)
         where TOutput : notnull
         where TRequest : notnull, IDispatchRequest;

      IWorkflowBuilder Clone();
      #endregion
   }
}
