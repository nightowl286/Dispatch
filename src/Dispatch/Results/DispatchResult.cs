using TNO.Dispatch.Abstractions;

namespace TNO.Dispatch.Results;

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

   #region Functions
   /// <summary>Creates a new <see cref="DispatchResult{TOutput}"/> with the given <paramref name="result"/>.</summary>
   /// <typeparam name="T">The type of the result.</typeparam>
   /// <param name="result">The result.</param>
   /// <returns>The created <see cref="DispatchResult{TOutput}"/>.</returns>
   public static DispatchResult<T> New<T>(T result) where T : notnull => new DispatchResult<T>(result);
   #endregion
}
