// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System;
using System.Collections.Generic;
using Assets.Sources.Util.Network.Requests;

namespace Assets.Sources.Util.Network
{
    // todo: get rid of TResponseType - it must be identified implicitly
    public class RequestSequenceExecutor
    {
        public delegate void OnSuccessListener<TResponseType>(RequestSequenceExecutor executor, TResponseType response);

        public delegate void OnFailListener(RequestSequenceExecutor executor,RequestError error);

        private readonly Dictionary<Type, object> requestClassToSuccessListener = new Dictionary<Type, object>();
        private bool isExecuteRequested = false;
        private IRequestExecutor currentRequest;

        public OnFailListener FailListener { get; set; }

        private readonly RequestManager requestManager;

        // that interface & class only to not force user call Execute<SomeMeaninglessParameter>() and
        // let user simply write Execute() instead
        private interface IRequestExecutor
        {
            void ExecuteRequest();
        }

        private class RequestExecutor<T> : IRequestExecutor
        {
            private readonly RequestBase<T> request;
            private readonly RequestSequenceExecutor executor;

            public RequestExecutor(RequestSequenceExecutor executor, RequestBase<T> request)
            {
                this.executor = executor;
                this.request = request;
            }

            public void ExecuteRequest()
            {
                executor.requestManager.AddRequest(request, ((response, error) =>
                {
                    executor.currentRequest = null;
                    if (error == null)
                    {
                        var listener = (OnSuccessListener<T>)executor.requestClassToSuccessListener[request.GetType()];
                        listener(executor, response.Data);
                    }
                    else
                    {
                        if (executor.FailListener != null)
                        {
                            executor.FailListener(executor, error);
                        }
                    }
                }));
            }
        }

        public RequestSequenceExecutor(RequestManager requestManager)
        {
            this.requestManager = requestManager;
        }

        // Note: when execution is started calling setRequest() will start request
        // immediately, so ensure, that you've set all the listeners before setRequest() call
        public void SetRequest<T>(RequestBase<T> request)
        {
            if (currentRequest != null)
                throw new InvalidOperationException("Parallel execution not allowed. Only one by one!");

            currentRequest = new RequestExecutor<T>(this, request);

            if (isExecuteRequested)
            {
                currentRequest.ExecuteRequest();
            }
        }

        public void SetOnSuccessListener<TRequestType, TResponseType>(OnSuccessListener<TResponseType> onSuccessListener) where TRequestType : RequestBase<TResponseType>
        {
            requestClassToSuccessListener.Add(typeof(TRequestType), onSuccessListener);
        }

        public void Execute()
        {
            if (currentRequest == null)
                throw new InvalidOperationException("At least 1 request must be added to execute");

            isExecuteRequested = true;
            currentRequest.ExecuteRequest();
        }
    }
}
