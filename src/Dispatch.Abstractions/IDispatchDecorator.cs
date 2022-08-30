namespace TNO.Dispatch.Abstractions
{
   public interface IDispatchDecorator<TOutput, TRequest> : IRequestHandler<TOutput, TRequest>
      where TOutput : notnull
      where TRequest : notnull, IDispatchRequest
   {
      #region Properties
      IRequestHandler<TOutput, TRequest> InnerHandler { get; set; }
      #endregion
   }
}
