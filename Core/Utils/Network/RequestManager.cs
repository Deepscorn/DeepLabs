// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System;
using System.Net;
using Assets.Sources.NativePlatform;
using Assets.Sources.Util.Log;
using Assets.Sources.Util.Network.Requests;
using RestSharp;

namespace Assets.Sources.Util.Network
{
    public class RequestManager
    {
        // number of retries when any network-related problem occurs
        // implies both sync and async request execution
        public int RequestRetryCount { get; set; }

        public IRequestErrorHandler ErrorHandler { get; set; }

        public int RequestTimeout
        {
            get
            {
                return restClient.Timeout;
            }
            set
            {
                restClient.Timeout = value;
            }
        }

        private IRestClient restClient { get; set; }

        public RequestManager(string baseUrl)
        {
#if (UNITY_ANDROID || UNITY_EDITOR)
            // workaround for "Unable to find /System/Library/Frameworks/Security.framework/Security" and "Error getting response stream (Write: The authentication or decryption has failed.): SendFailure"
            // http://answers.unity3d.com/questions/521069/android-unable-to-find-systemlibraryframeworkssecu.html
            // platforms: Android, Windows 10
            ServicePointManager.ServerCertificateValidationCallback = (p1, p2, p3, p4) => true;
#endif

            // todo: remove that from all the other places where it is needed only for RequestManager
            UnityThreadHelper.EnsureHelper();

            this.restClient = new RestClient(baseUrl);

            RequestRetryCount = 3;
        }

        public void AddRequest<TResponseType>(RequestBase<TResponseType> request, Action<IRequest<TResponseType>> onFinish)
        {
            UpdateAuthInfoForRequestIfNeeded(request);

            SetRequestFinishHandler(request, response =>
            {
                if (response.Error != null)
                {
                    if (ErrorHandler != null)
                    {
                        ErrorHandler.HandleRequestError(response.Error);
                    }
                }
                else
                {
                    onFinish(response);
                }
            });
            RunRequest(request);
        }

        public void AddRequest<TResponseType>(RequestBase<TResponseType> request, Action<IRequest<TResponseType>, RequestError> onFinish)
        {
            UpdateAuthInfoForRequestIfNeeded(request);

            SetRequestFinishHandler(request, response =>
            {
                if (request.Error != null)
                {
                    LogProxy.Log("Request error (custom error-handling logic is set): " + request.Error);
                }
                onFinish(response, response.Error);
            });
            RunRequest(request);
        }

        private void RunRequest<TResponseType>(RequestBase<TResponseType> request)
        {
            request.RestClient = restClient;
            request.Execute();
        }

        // return executed request (same as passed in method)
        public IRequest<TResponseType> RunRequestSync<TResponseType>(RequestBase<TResponseType> request) where TResponseType : new()
        {
            UpdateAuthInfoForRequestIfNeeded(request);

            request.RestClient = restClient;
            request.ExecuteSync<TResponseType>();

            var retryCount = RequestRetryCount;

            while (request.Error != null && retryCount > 0 && IsNetworkError(request.Error))
            {
                request.ExecuteSync<TResponseType>();
                retryCount--;
            }

            return request;
        }

        private void UpdateAuthInfoForRequestIfNeeded<TResponseType>(RequestBase<TResponseType> request)
        {
            var authorizedRequest = request as AuthorizedRequest<TResponseType>;
            if (authorizedRequest != null)
            {
                authorizedRequest.UserId = NativeApi.UserId;
                authorizedRequest.AccessToken = NativeApi.AccessToken;
            }
        }

        private void SetRequestFinishHandler<ResponseType>(IRequest<ResponseType> workingRequest, RequestFinishHandler<ResponseType> onFinish)
        {
            workingRequest.OnFinishEvent += (IRequest<ResponseType> request) =>
            {
                if (request.Error == null)
                {
                    onFinish(request);
                }
                else
                {
                    if (IsNetworkError(request.Error) && request.ExecuteCount - 1 < RequestRetryCount)
                    {
                        request.Execute();
                    }
                    else
                    {
                        onFinish(request);
                    }
                }
            };
        }

        private bool IsNetworkError(RequestError error)
        {
            return error.StatusCode == HttpStatusCode.Gone ||
                error.StatusCode == HttpStatusCode.RequestTimeout ||
                error.ResponseStatus == RestSharp.ResponseStatus.TimedOut;
        }
    }
}

