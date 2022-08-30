using System;

namespace TNO.Dispatch.Abstractions
{
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
