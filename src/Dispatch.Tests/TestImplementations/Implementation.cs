using TNO.DependencyInjection.Components;

namespace TNO.Dispatch.Tests.TestImplementations
{
   public class UnitCollection : DispatchCollection<IUnitRequest, IUnitCollection>, IUnitCollection
   {
      public UnitCollection(IServiceBuilder builder, ReplaceMode mode = ReplaceMode.Throw) : base(builder, mode) { }
      protected UnitCollection(SimpleCollection collection, ReplaceMode mode, DispatchCollection<IUnitRequest, IUnitCollection> outerScope) : base(collection, mode, outerScope) { }

      public override IUnitCollection CreateScope() => new UnitCollection(_collection.CreateScope(), _mode, this);
   }
}
