using System;

namespace TNO.Dispatch.Abstractions
{
   // Todo(Anyone): This might require a rework to follow the builder pattern better;

   /// <summary>
   /// Denotes a common dispatch workflow.
   /// </summary>
   public interface IDispatchWorkflow
   {
      #region Methods
      IDispatchWorkflow With(Type decoratorType);

      IRequestHandler<TOutput, TRequest> Build<TOutput, TRequest>(IRequestHandler<TOutput, TRequest> innerHandler)
         where TOutput : notnull
         where TRequest : notnull, IDispatchRequest;
      #endregion
   }
}
