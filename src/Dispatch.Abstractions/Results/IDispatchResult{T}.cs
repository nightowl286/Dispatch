namespace TNO.Dispatch.Abstractions.Results
{
   public interface IDispatchResult<out TOutput> : IDispatchResult
      where TOutput : notnull
   {
      #region Properties
      TOutput? Output { get; }
      #endregion
   }
}
