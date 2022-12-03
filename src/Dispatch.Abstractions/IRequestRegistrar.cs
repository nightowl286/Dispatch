using System;

namespace TNO.Dispatch.Abstractions
{
   /// <summary>
   /// Denotes a common dispatch request registrar.
   /// </summary>
   /// <typeparam name="TCollection">The type of the dispatch collection this registrar belongs to.</typeparam>
   public interface IRequestRegistrar<out TCollection>
   {
      #region Methods
      void Register(Type outputType, Type requestType, Type handlerType);
      void Register(Type handlerType);
      void Register(Type outputType, Type requestType, Type handlerType, IDispatchWorkflow workflow);
      void Register(Type handlerType, IDispatchWorkflow workflow);
      TCollection CreateScope();
      #endregion
   }
}
