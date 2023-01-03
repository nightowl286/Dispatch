using System;
using System.Collections.Generic;
using TNO.Dispatch.Abstractions;

namespace TNO.Dispatch.Results;

/// <summary>
/// Denotes a common dispatch result.
/// </summary>
public interface IDispatchResult
{
   #region Properties
   /// <summary>A collection of errors related to this result.</summary>
   IReadOnlyCollection<IDispatchError> Errors { get; }

   /// <summary>
   /// <see langword="true"/> if this result is a successful result and 
   /// has no <see cref="Errors"/>, <see langword="false"/> otherwise.
   /// </summary>
   bool Successful { get; }
   #endregion

   #region Methods
   /// <summary>Combines all the <see cref="Errors"/> into an <see cref="AggregateException"/>.</summary>
   /// <returns>An <see cref="AggregateException"/> that combines all of the <see cref="Errors"/>.</returns>
   AggregateException GetAggregateException()
   {
      List<Exception> aggregates = new List<Exception>(Errors.Count);
      foreach (IDispatchError error in Errors)
      {
         Exception exception = error.GetException();
         aggregates.Add(exception);
      }

      AggregateException aggregate = new AggregateException(aggregates);
      return aggregate;
   }
   #endregion
}
