using System;
using System.Collections.Generic;
using TNO.Dispatch.Abstractions;

namespace TNO.Dispatch.Abstractions.Results
{
   public interface IDispatchResult
   {
      #region Properties
      IReadOnlyCollection<IDispatchError> Errors { get; }
      bool Successful { get; }
      #endregion

      #region Methods
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
}
