using System;

namespace TNO.Dispatch.Abstractions
{
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
