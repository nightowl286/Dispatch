using TNO.Dispatch.Abstractions;

namespace TNO.Dispatch.Results
{
   public class DispatchResult : DispatchResult<Void>
   {
      #region Properties
      public static DispatchResult Success { get; } = new DispatchResult();
      #endregion
      private DispatchResult() : base(Void.Value) { }
      public DispatchResult(params IDispatchError[] errors) : base(errors) { }
   }
}
