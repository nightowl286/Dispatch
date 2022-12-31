using System;

namespace TNO.Dispatch.Abstractions.Workflows;

/// <summary>
/// Denotes a workflow builder.
/// </summary>
public interface IWorkflowBuilder
{
    #region Methods
    /// <summary>Adds a new outer-most decorator.</summary>
    /// <param name="decoratorType">The type of the decorator to add.</param>
    /// <returns>The current instance of the <see cref="IWorkflowBuilder"/>.</returns>
    IWorkflowBuilder With(Type decoratorType);

    /// <summary>Builds a dispatch workflow with the given decorators..</summary>
    /// <returns>A newly created dispatch workflow.</returns>
    IDispatchWorkflow Build();
    #endregion
}
