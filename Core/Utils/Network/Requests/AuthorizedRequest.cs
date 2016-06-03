// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System.Diagnostics;
using RestSharp;

namespace Assets.Sources.Util.Network.Requests
{
    public class AuthorizedRequest<TResponseType> : RequestBase<TResponseType>
    {
        private const string AnonymousUserId = "anonymous";
        private const string AnonymousAccessToken = "Anonymous";
        private string accessToken;

        public string AccessToken
        {
            get
            {
                return accessToken ?? AnonymousAccessToken;
            }
            set
            {
                accessToken = value;
            }
        }
      
        private string userId;

        public string UserId
        {
            get
            {
                return userId ?? AnonymousUserId;
            }
            set
            {
                userId = value;
            }
        }

        protected override void ValidateParams()
        {
            base.ValidateParams();

            Debug.Assert(IsValidData);
        }

        protected override void PrepareRequest(IRestRequest request)
        {
            Debug.Assert(AccessToken != null);

            if (IsAnonymousUser)
            {
                request.AddHeader("Authorization", AccessToken);
            }
            else
            {
//#if UNITY_EDITOR
//                request.AddHeader ("Authorization", "Basic " + AccessToken);
//#else
                request.AddHeader("Authorization", "bearer " + AccessToken);
//#endif
            }
        }

        private bool IsAnonymousUser
        {
            get
            {
                return userId == null;
            }
        }

        private bool IsValidData
        {
            get
            {
                return userId != null && accessToken != null
                    || userId == null && accessToken == null;
            }
        }
    }
}

