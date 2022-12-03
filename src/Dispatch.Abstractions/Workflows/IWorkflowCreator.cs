namespace TNO.Dispatch.Abstractions.Workflows
{
   public interface IWorkflowCreator
   {
      #region Methods
      IWorkflowBuilder NewWorkflow();
      #endregion
   }
}
