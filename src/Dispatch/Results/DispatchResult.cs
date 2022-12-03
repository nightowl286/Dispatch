using TNO.Dispatch.Abstractions;

namespace TNO.Dispatch.Results
{
   /// <inheritdoc/>
   public class DispatchResult : DispatchResult<Void>
   {
      #region Properties
      /// <summary>A cached successful dispatch result.</summary>
      public static DispatchResult Success { get; } = new DispatchResult();
      #endregion

      #region Constructors
      /// <summary>Creates a new successful dispatch result.</summary>
      /// <remarks><see cref="Success"/> should be used instead.</remarks>
      private DispatchResult() : base(Void.Value) { }

      /// <inheritdoc/>
      public DispatchResult(params IDispatchError[] errors) : base(errors) { }
      #endregion
   }
}
