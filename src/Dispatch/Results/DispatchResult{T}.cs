using TNO.Dispatch.Abstractions;
using TNO.Dispatch.Abstractions.Results;

namespace TNO.Dispatch.Results
{
   public class DispatchResult<TOutput> : IDispatchResult<TOutput>
      where TOutput : notnull
   {
      #region Properties
      public TOutput? Output { get; }
      public IReadOnlyCollection<IDispatchError> Errors { get; }
      public bool Successful { get; }
      #endregion
      public DispatchResult(TOutput output)
      {
         Output = output;
         Successful = true;
         Errors = Array.Empty<IDispatchError>();
      }
      public DispatchResult(params IDispatchError[] errors)
      {
         Output = default;
         Successful = false;
         Errors = errors;
      }
   }
}
