using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TNO.Dispatch.Abstractions;

namespace TNO.Dispatch.Results;

/// <inheritdoc/>
public class DispatchResult<TOutput> : IDispatchResult<TOutput>
   where TOutput : notnull
{
   #region Properties
   /// <inheritdoc/>
   public TOutput? Output { get; }

   /// <inheritdoc/>
   public IReadOnlyCollection<IDispatchError> Errors { get; }

   /// <inheritdoc/>
#if NET5_0_OR_GREATER
   [MemberNotNullWhen(true, nameof(Output))]
#endif
   public bool Successful { get; }
   #endregion

   #region Constructors
   /// <summary>Creates a new successful dispatch result with the given <paramref name="output"/>.</summary>
   /// <param name="output">The output of this result.</param>
   public DispatchResult(TOutput output)
   {
      Output = output;
      Successful = true;
      Errors = Array.Empty<IDispatchError>();
   }

   /// <summary>Creates a new unsuccessful dispatch result with the given <paramref name="errors"/>.</summary>
   /// <param name="errors">The errors associated with this result.</param>
   public DispatchResult(params IDispatchError[] errors)
   {
      Output = default;
      Successful = false;
      Errors = errors;
   }
   #endregion

   #region Implicit Conversions
   /// <summary>Converts the given <paramref name="result"/> to a successful <see cref="DispatchResult{TOutput}"/>.</summary>
   /// <param name="result">The result to convert.</param>
   public static implicit operator DispatchResult<TOutput>(TOutput result) => new DispatchResult<TOutput>(result);
   #endregion
}
