namespace TNO.Dispatch.Abstractions.Results
{
   /// <summary>
   /// Denotes a common dispatch result with a possible output.
   /// </summary>
   /// <typeparam name="TOutput">The type of the output.</typeparam>
   public interface IDispatchResult<out TOutput> : IDispatchResult
      where TOutput : notnull
   {
      #region Properties
      /// <summary>The output of this result.</summary>
      /// <remarks>
      /// This will be <see langword="null"/> if this result
      /// is not <see cref="IDispatchResult.Successful"/>.
      /// </remarks>
      TOutput? Output { get; }
      #endregion
   }
}
