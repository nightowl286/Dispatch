using TNO.Dispatch.Abstractions;
using TNO.Dispatch.Errors;

namespace TNO.Dispatch.Errors
{
   public class DispatchError : IDispatchError
   {
      #region Properties
      public string Description { get; }
      public IDispatchError? InnerError { get; }
      #endregion
      public DispatchError(string description) => Description = description;
      public DispatchError(string description, IDispatchError? innerError) : this(description) => InnerError = innerError;

      #region Methods
      public Exception GetException()
      {
         if (InnerError is not null)
            return new DispatchException(Description, InnerError.GetException());

         return new DispatchException(Description);
      }
      #endregion
   }
}
