// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
#if UNITY_EDITOR || UNITY_ANDROID

using System.Collections.Generic;
using Assets.Sources.Logic.Data;
using MiniJSON;
using UnityEngine;

namespace Assets.Sources.NativePlatform.Platforms
{
    public class NativeAndroid : INativePlatform
    {
        private readonly AndroidJavaObject activity;

        public NativeAndroid()
        {
            var javaClass = new AndroidJavaClass("eu.inn.forgame.activities.UnityPlayerActivity");
            activity = javaClass.CallStatic<AndroidJavaObject>("getInstance");
        }

        public void BuyInAppPurchase(string productId, string userId, string orderId, string callbackObj, string callbackMethod)
        {
            var developerPayloadDict = new Dictionary<string, string>
            {
                { "userId", userId },
                { "orderId", orderId },
                { "appId", NativeApi.AppId }
            };
            var developerPayload = Json.Serialize(developerPayloadDict);
            activity.Call("buyInAppPurchase", productId, developerPayload, callbackObj, callbackMethod);
        }

        public IList<PurchaseInfo> GetInAppPurchases()
        {
            var jsonStr = activity.Call<string>("getInAppPurchases");

            var json = Json.Deserialize(jsonStr) as Dictionary<string, object>;

            var purchases = json["purchases"] as List<object>;

            IList<PurchaseInfo> result = new List<PurchaseInfo>();
            foreach (Dictionary<string, object> purchase in purchases)
            {
                var purchaseDataStr = (string)purchase["purchaseData"];
                var purchaseData = Json.Deserialize(purchaseDataStr) as Dictionary<string, object>;
                var developerPayloadStr = (string)purchaseData["developerPayload"];
                var developerPayload = Json.Deserialize(developerPayloadStr) as Dictionary<string, object>;

                result.Add(new PurchaseInfo((string)developerPayload["userId"], (string)developerPayload["orderId"],
                    (string)purchaseData["orderId"], purchaseDataStr, (string)purchase["signature"]));
            }

            return result;
        }

        public void RemoveInnAppPurchase(PurchaseInfo purchaseInfo)
        {
            activity.Call("removeInAppPurchase", purchaseInfo.ReceiptData);
        }

        public string GetAppId()
        {
            return activity.Call<string>("getAppId");
        }

        public string GetAppProductId()
        {
            return activity.Call<string>("getAppProductId");
        }

        public string GetAppPromoId()
        {
            return activity.Call<string>("getAppPromoId");
        }

        public string GetAccessToken()
        {
            return activity.Call<string>("getAccessToken");
        }

        public string GetUserId()
        {
            return activity.Call<string>("getUserId");
        }

        public float GetGuiScale()
        {
            return activity.Call<float>("getGuiScale");
        }

        public string GetLangCode()
        {
            return activity.Call<string>("getLangCode");
        }

        public string GetPlatformType()
        {
            return activity.Call<string>("getPlatformType");
        }

        public void BackButton()
        {
            activity.Call("onBackBtnClick");
        }

        public void ExitClick()
        {
            activity.Call("onExitBtnClick");
        }

        public void BeginUserRegister(string callbackObj, string callbackMethod)
        {
            activity.Call("beginUserRegister", callbackObj, callbackMethod);
        }

        public void BeginUserLogin(string callbackObj, string callbackMethod)
        {
            activity.Call("beginUserLogin", callbackObj, callbackMethod);
        }

        public void OpenUrl(string url)
        {
            activity.Call("openUrl", url);
        }

        public string GetApiUrl()
        {
            return activity.Call<string>("getApiUrl");
        }

        public void OnUnityApiAvailable(string gameObjectName)
        {
            GameObject.Find(gameObjectName).SendMessage("StartGame");
        }
    }
}

#endif