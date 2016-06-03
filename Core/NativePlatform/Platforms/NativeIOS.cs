// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
#if UNITY_EDITOR || UNITY_IOS

using System.Collections.Generic;
using System.Runtime.InteropServices;
using Assets.Sources.Logic.Data;
using MiniJSON;
using UnityEngine;

namespace Assets.Sources.NativePlatform.Platforms
{
    public class NativeIos : INativePlatform
    {
        [DllImport("__Internal")]
        private static extern void _NativeBeginBuyInApp(string productId, string userId, string orderId, string callBackObj, string successCallBackMethod, string failureCallbackMethod);

        [DllImport("__Internal")]
        private static extern string _NativeGetPurchasedInApps();

        [DllImport("__Internal")]
        private static extern void _NativeRemoveInAppPurchase(string purchaseId);

        [DllImport("__Internal")]
        private static extern string _NativeGetAppId();

        [DllImport("__Internal")]
        private static extern string _NativeGetAppProductId();

        [DllImport("__Internal")]
        private static extern string _NativeGetAppPromoId();

        [DllImport("__Internal")]
        private static extern string _NativeGetAccessToken();

        [DllImport("__Internal")]
        private static extern string _NativeGetUserId();

        [DllImport("__Internal")]
        private static extern float _NativeGetGuiScale();

        [DllImport("__Internal")]
        private static extern string _NativeGetLangCode();

        [DllImport("__Internal")]
        private static extern string _NativeGetPlatformType();

        [DllImport("__Internal")]
        private static extern void _NativeBackButton();

        [DllImport("__Internal")]
        private static extern void _NativeExitClick();

        [DllImport("__Internal")]
        private static extern void _NativeBeginUserRegister(string callbackObj, string callbackMethod);

        [DllImport("__Internal")]
        private static extern void _NativeBeginUserLogin(string callbackObj, string callbackMethod);
        
        [DllImport("__Internal")]
        private static extern void _NativeOpenUrl(string url);

        public void BuyInAppPurchase(string productId, string userId, string orderId, string callBackObj, string callbackMethod)
        {
            // todo: serialize like in android. Method signature will look like:
            // _NativeBeginBuyInApp(productId, developerPayload, callBackObj, callbackMethod);
            _NativeBeginBuyInApp(productId, userId, orderId, callBackObj, callbackMethod, callbackMethod);
        }

        public IList<PurchaseInfo> GetInAppPurchases()
        {
            var purchasesListJson = _NativeGetPurchasedInApps();

            var dictPurchases = (List<object>)Json.Deserialize(purchasesListJson);

            var purchases = new List<PurchaseInfo>(dictPurchases.Count);

            foreach (Dictionary<string, object> purchaseObj in dictPurchases)
            {
                var userId = (string)purchaseObj["user_id"];
                var orderId = (string)purchaseObj["order_id"];
                var purchaseId = (string)purchaseObj["purchase_id"];
                var receiptData = (string)purchaseObj["receipt_data"];

                var purchaseInfo = new PurchaseInfo(userId, orderId, purchaseId, receiptData, null);

                purchases.Add(purchaseInfo);
            }

            return purchases;
        }

        public void RemoveInnAppPurchase(PurchaseInfo purchaseInfo)
        {
            _NativeRemoveInAppPurchase(purchaseInfo.PurchaseId);
        }

        public string GetAppId()
        {
            return _NativeGetAppId();
        }

        public string GetAppProductId()
        {
            return _NativeGetAppProductId();
        }

        public string GetAppPromoId()
        {
            return _NativeGetAppPromoId();
        }

        public string GetAccessToken()
        {
            return _NativeGetAccessToken();
        }

        public string GetUserId()
        {
            return _NativeGetUserId();
        }

        public float GetGuiScale()
        {
            return _NativeGetGuiScale();
        }

        public string GetLangCode()
        {
            return _NativeGetLangCode();
        }

        public string GetPlatformType()
        {
            return _NativeGetPlatformType();
        }

        public void BackButton()
        {
            _NativeBackButton();
        }

        public void ExitClick()
        {
            _NativeExitClick();
        }

        public void BeginUserRegister(string callbackObj, string callbackMethod)
        {
            _NativeBeginUserRegister(callbackObj, callbackMethod);
        }

        public void BeginUserLogin(string callbackObj, string callbackMethod)
        {
            _NativeBeginUserLogin(callbackObj, callbackMethod);
        }

        public void OpenUrl(string url)
        {
            _NativeOpenUrl(url);
        }

        public string GetApiUrl()
        {
            return "https://api2.4game.com";
        }

        public void OnUnityApiAvailable(string gameObjectName)
        {
            // todo: control unity app start / stop from IOs
            GameObject.Find(gameObjectName).SendMessage("StartGame");
        }
    }
}

#endif