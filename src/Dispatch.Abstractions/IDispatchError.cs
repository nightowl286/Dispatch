using System;

namespace TNO.Dispatch.Abstractions
{
   public interface IDispatchError
   {
      #region Properties
      string Description { get; }
      IDispatchError? InnerError { get; }
      #endregion

      #region Methods
      Exception GetException()
      {
         if (InnerError is not null)
            return new Exception(Description, InnerError.GetException());

         return new Exception(Description);
      }
      #endregion
   }
}
