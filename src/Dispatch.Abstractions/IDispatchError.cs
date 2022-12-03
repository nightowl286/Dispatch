using System;

namespace TNO.Dispatch.Abstractions
{
   /// <summary>
   /// Denotes a common dispatch error.
   /// </summary>
   public interface IDispatchError
   {
      #region Properties
      /// <summary>The description of this error.</summary>
      string Description { get; }

      /// <summary>
      /// An inner error that caused this error, or <see langword="null"/> if there is no inner error.
      /// </summary>
      IDispatchError? InnerError { get; }
      #endregion

      #region Methods
      /// <summary>Gets this error as an <see cref="Exception"/>.</summary>
      /// <returns>An <see cref="Exception"/> with the same information as this error.</returns>
      Exception GetException()
      {
         // Todo(Nightowl): Create a custom exception type;
         if (InnerError is not null)
            return new Exception(Description, InnerError.GetException());

         return new Exception(Description);
      }
      #endregion
   }
}
