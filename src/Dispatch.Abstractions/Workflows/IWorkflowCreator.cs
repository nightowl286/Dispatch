namespace TNO.Dispatch.Abstractions.Workflows;

/// <summary>
/// Denotes a workflow creator.
/// </summary>
public interface IWorkflowCreator
{
    #region Methods
    /// <summary>Creates a new <see cref="IWorkflowBuilder"/>.</summary>
    /// <returns>A newly created <see cref="IWorkflowBuilder"/>.</returns>
    IWorkflowBuilder NewWorkflow();
    #endregion
}
