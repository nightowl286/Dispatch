using TNO.Dispatch.Abstractions.Workflows;

namespace TNO.Dispatch.Tests.TestImplementations
{
   public interface IUnitRequest : IDispatchRequest { }
   public interface IUnitDispatcher : IRequestDispatcher<IUnitRequest> { }
   public interface IUnitHandler<TOutput, in TUnit> : IRequestHandler<TOutput, TUnit>
      where TOutput : notnull
      where TUnit : notnull, IUnitRequest
   { }
   public interface IUnitCollection : IUnitRegistrar, IUnitDispatcher, IWorkflowCreator { }
   public interface IUnitRegistrar : IRequestRegistrar<IUnitCollection> { }
}
