// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System;
using System.Net;
using RestSharp;

namespace Assets.Sources.Util.Network
{
    public class RequestError
    {
        public RequestError(string description, HttpStatusCode statusCode, ResponseStatus responseStatus)
        {
            Description = description;
            StatusCode = statusCode;
            ResponseStatus = responseStatus;
        }

        public string Description { get; private set; }

        public HttpStatusCode StatusCode { get; private set; }

        public ResponseStatus ResponseStatus { get; private set; }

        public override string ToString()
        {
            return string.Format("HttpStatusCode: {0}\nResponseStatus : {1}\n{2}", StatusCode, ResponseStatus, Description);
        }
    }
}

