namespace TNO.Dispatch.Tests
{
   public interface IUnitDecorator<TOutput, TRequest> : IDispatchDecorator<TOutput, TRequest>
      where TOutput : notnull
      where TRequest : notnull, IDispatchRequest
   { }

   public interface IUnitDecorator2<TOutput, TRequest> : IDispatchDecorator<TOutput, TRequest>
      where TOutput : notnull
      where TRequest : notnull, IDispatchRequest
   { }
}
