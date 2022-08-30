namespace TNO.Dispatch.Abstractions
{
   public interface IWorkflowCreator
   {
      #region Methods
      IDispatchWorkflow CreateWorkflow();
      #endregion
   }
}
