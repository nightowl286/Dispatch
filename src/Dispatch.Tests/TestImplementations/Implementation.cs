namespace TNO.Dispatch.Tests.TestImplementations;

public class UnitCollection : DispatchCollection<IUnitRequest, IUnitCollection>, IUnitCollection
{
   public UnitCollection(IServiceScope scope) : base(scope) { }
   protected UnitCollection(IServiceScope scope, DispatchCollection<IUnitRequest, IUnitCollection> outerScope) : base(scope, outerScope) { }

   public override IUnitCollection CreateScope() => new UnitCollection(_serviceScope.CreateScope(), this);
}
