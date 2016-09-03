// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Plugins.DeepLabs.Core.Utils.Log;
using RestSharp;

namespace Assets.Sources.Util.Network.Requests
{
    public class RequestBase<TResponseType> : IRequest<TResponseType>
    {
        #region IRequest implementation

        public event RequestFinishHandler<TResponseType> OnFinishEvent;

        public int ExecuteCount { get; private set; }

        public RequestBase()
        {
            ExecuteCount = 0;
        }

        event RequestFinishHandler IRequest.OnFinishEvent
        {
            add
            {
                InternalOnFinishHandler += value;
            }

            remove
            {
                InternalOnFinishHandler -= value;
            }
        }

        public bool IsExecuting { get; private set; }

        public TResponseType Data { get; private set; }

        object IRequest.Data
        {
            get
            {
                return Data;
            }
        }

        public RequestError Error { get; private set; }

        public void Execute()
        {
            if (IsExecuting)
            {
                System.Console.WriteLine("Error: request always executing.");
                return;
            }
            ++ExecuteCount;

            OnRequestStarted();

            var request = BuildRequest();
            PrepareRequest(request);

            RequestHandle = RestClient.ExecuteAsync<TResponseType>(request, (response, asyncHandle) =>
            {
                OnRequestStopped();
                OnResponse(response);
                OnFinish();
            });
        }

        public void ExecuteSync<TResponseTypeWithEmptyPublicConstructor>() where TResponseTypeWithEmptyPublicConstructor : TResponseType, new()
        {
            var request = BuildRequest();
            PrepareRequest(request);

            var response = RestClient.Execute<TResponseTypeWithEmptyPublicConstructor>(request);

            OnResponse((IRestResponse<TResponseType>)response);
        }

        public void Cancel()
        {
            if (!IsExecuting)
            {
                return;
            }

            RequestHandle.Abort();
            OnRequestStopped();
        }

        #endregion

        public IRestClient RestClient { private get; set; }

        protected virtual string BuildRequestPath()
        {
            throw new NotImplementedException();
        }

        protected virtual void PrepareRequest(IRestRequest request)
        {
        }

        protected virtual object BuildRequestBody()
        {
            return null;
        }

        protected virtual IList<System.Net.HttpStatusCode> BuildValidStatusCodes()
        {
            return new List<System.Net.HttpStatusCode>();
        }

        protected virtual Method RequestMethodType()
        {
            return Method.POST;
        }

        protected virtual void ValidateParams()
        {
        }

        private IRestRequest BuildRequest()
        {
            ValidateParams();

            var requestPath = BuildRequestPath();
            var requestMethodType = RequestMethodType();

            Lc.D(requestMethodType + ": " + requestPath);

            var request = new RestRequest(requestPath, requestMethodType);
            request.RequestFormat = DataFormat.Json;

            var requestBody = BuildRequestBody();

            if (requestBody != null)
            {
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(requestBody);

                var body = request.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody);
                if (body != null)
                {
                    Lc.D("Request body: " + body.Value);
                }
            }

            return request;
        }

        private void OnResponse(IRestResponse<TResponseType> response)
        {
            Error = null;
            Data = response.Data;

            var statusCode = response.StatusCode;
            if (statusCode != System.Net.HttpStatusCode.OK && !BuildValidStatusCodes().Contains(statusCode))
            {
                var errorMessage = response.ErrorMessage;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = response.Content;
                }
                Error = new RequestError(errorMessage, statusCode, response.ResponseStatus);
                Lc.D(Error.ToString());
            }
            else
            {
                Lc.D("Response data: " + response.Content);
            }
        }

        private void OnFinish()
        {
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                OnFinishEvent(this);

                if (InternalOnFinishHandler != null)
                {
                    InternalOnFinishHandler(this);
                }
            });
        }

        private void OnRequestStarted()
        {
            IsExecuting = true;

            Data = default(TResponseType);
            Error = null;
        }

        private void OnRequestStopped()
        {
            IsExecuting = false;

            RequestHandle = null;
        }

        private RestRequestAsyncHandle RequestHandle { get; set; }

        private RequestFinishHandler InternalOnFinishHandler { get; set; }
    }
}

