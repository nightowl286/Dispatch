using TNO.DependencyInjection.Abstractions.Components;
using TNO.Dispatch.Abstractions.Workflows;

namespace TNO.Dispatch.Workflows
{
   public class WorkflowBuilder : IWorkflowBuilder
   {
      #region Fields
      private readonly List<Type> _decoratorTypes = new List<Type>();
      private readonly IServiceBuilder _builder;
      #endregion
      public WorkflowBuilder(IServiceBuilder builder) => _builder = builder;
      public WorkflowBuilder(IServiceBuilder builder, IEnumerable<Type> decoratorTypes) : this(builder)
         => _decoratorTypes.AddRange(decoratorTypes);

      #region Methods
      public IWorkflowBuilder With(Type decoratorType)
      {
         _decoratorTypes.Add(decoratorType);
         return this;
      }
      public IDispatchWorkflow Build() => new DispatchWorkflow(_builder, _decoratorTypes);
      #endregion
   }
}
