using System.Runtime.Serialization;

namespace TNO.Dispatch.Errors
{
   [Serializable]
   public class DispatchException : Exception
   {
      public DispatchException() { }
      public DispatchException(string message) : base(message) { }
      public DispatchException(string message, Exception inner) : base(message, inner) { }
      protected DispatchException(SerializationInfo info, StreamingContext context) : base(info, context) { }
   }
}
