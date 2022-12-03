using System.Runtime.Serialization;

namespace TNO.Dispatch.Errors
{
   /// <summary>
   /// Represents errors that occur in the dispatch framework.
   /// </summary>
   [Serializable]
   public class DispatchException : Exception
   {
      #region Constructors
      /// <summary>Initialises a new instance of the <see cref="DispatchException"/> class.</summary>
      public DispatchException() { }

      /// <summary>
      /// Initialises a new instance of the <see cref="DispatchException"/>
      /// class with the given <paramref name="message"/>.
      /// </summary>
      /// <param name="message">The message that explains this exception.</param>
      public DispatchException(string message) : base(message) { }

      /// <summary>
      /// Initialises a new instance of the <see cref="DispatchException"/> class with the
      /// given <paramref name="message"/> and <paramref name="inner"/> <see cref="Exception"/>.
      /// </summary>
      /// <param name="message">The message that explains this exception.</param>
      /// <param name="inner">The inner exception that caused this one.</param>
      public DispatchException(string message, Exception inner) : base(message, inner) { }

      /// <summary>
      /// Initialises a new instance of the <see cref="DispatchException"/> class with serialised data.
      /// </summary>
      protected DispatchException(SerializationInfo info, StreamingContext context) : base(info, context) { }
      #endregion
   }
}
