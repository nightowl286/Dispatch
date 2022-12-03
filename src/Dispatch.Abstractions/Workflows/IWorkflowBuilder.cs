using System;

namespace TNO.Dispatch.Abstractions.Workflows
{
   public interface IWorkflowBuilder
   {
      #region Methods
      IWorkflowBuilder With(Type decoratorType);
      IDispatchWorkflow Build();
      #endregion
   }
}
