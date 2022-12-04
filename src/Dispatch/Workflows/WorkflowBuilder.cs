using TNO.DependencyInjection.Abstractions.Components;
using TNO.Dispatch.Abstractions.Workflows;

namespace TNO.Dispatch.Workflows
{
   /// <inheritdoc/>
   public class WorkflowBuilder : IWorkflowBuilder
   {
      #region Fields
      private readonly List<Type> _decoratorTypes = new List<Type>();
      private readonly IServiceBuilder _builder;
      #endregion

      #region Constructors
      /// <summary>Creates a new workflow builder.</summary>
      /// <param name="builder">The builder that the workflow should use.</param>
      public WorkflowBuilder(IServiceBuilder builder) => _builder = builder;

      /// <summary>Creates a new workflow builder.</summary>
      /// <param name="builder">The builder that the workflow should use.</param>
      /// <param name="decoratorTypes">The decorator types the builder should start with.</param>
      public WorkflowBuilder(IServiceBuilder builder, IEnumerable<Type> decoratorTypes) : this(builder)
         => _decoratorTypes.AddRange(decoratorTypes);
      #endregion

      #region Methods
      /// <inheritdoc/>
      public IWorkflowBuilder With(Type decoratorType)
      {
         _decoratorTypes.Add(decoratorType);
         return this;
      }

      /// <inheritdoc/>
      public IDispatchWorkflow Build() => new DispatchWorkflow(_builder, _decoratorTypes);
      #endregion
   }
}
