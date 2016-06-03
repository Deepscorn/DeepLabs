// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
namespace Assets.Sources.Util.Network.Requests
{
    public delegate void RequestFinishHandler(IRequest request);

    public delegate void RequestFinishHandler<ResponseType>(IRequest<ResponseType> request);
   
    public interface IRequest
    {
        void Execute();
      
        void Cancel();
      
        bool IsExecuting { get; }
      
        event RequestFinishHandler OnFinishEvent;
      
        object Data { get; }
      
        RequestError Error { get; }

        // normally, after calling Execute, Execute count equals 1. But when network error
        // occurs, then request is retried for some count. So, Execute() is called
        // more than once and ExecuteCount > 1
        // field used by RequestManager to manage retry policy
        int ExecuteCount { get; }
    }
   
    public interface IRequest<TResponseType> : IRequest
    {
        new event RequestFinishHandler<TResponseType> OnFinishEvent;
      
        new TResponseType Data { get; }
      
    }
}