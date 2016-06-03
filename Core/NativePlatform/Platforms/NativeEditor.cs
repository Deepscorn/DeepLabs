// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Assets.Sources.Logic.Data;
using Assets.Sources.Util.Data;
using Assets.Sources.Util.Log;
using Assets.Sources.Util.UI;
using UnityEngine;

namespace Assets.Sources.NativePlatform.Platforms
{
    public class NativeEditor: INativePlatform
    {
        private readonly IList<PurchaseInfo> purchases = new List<PurchaseInfo>();

        public NativeEditor()
        {
            DataModel.SaveGameData("shooting.IsUserLogined", true);
        }

        public void BuyInAppPurchase(string productId, string userId, string orderId, string callBackObj, string callbackMethod)
        {
            var purchase = new PurchaseInfo(userId, orderId, "0", string.Format("productId : {0}", productId), "this is a test from Unity player");
            purchases.Add(purchase);
            LogProxy.Log("Bought " + purchase.ToString());

            GameObject.Find(callBackObj).SendMessage(callbackMethod, null);
        }

        public IList<PurchaseInfo> GetInAppPurchases()
        {
            return purchases;
        }

        public void RemoveInnAppPurchase(PurchaseInfo purchaseInfo)
        {
            purchases.Remove(purchaseInfo);
        }

        public string GetAppId()
        {
            return "4game";
        }

        public string GetAppProductId()
        {
            return "mobile-duck-hunting-v1";
        }

        public string GetAppPromoId()
        {
            return "duck-hunting";
        }

        public string GetAccessToken()
        {
            return !IsUserLoggedIn() ? null : "bvFEOavW2wPVbONEgJ2M_ru-sTLBAs5L53Od88ugsXenXr_LmfCq0qq0_3tam6mCjVi8rsV7NRdhv1fEMwGzODs9p0aqMEkYK_CtwluimBW8H8Op0TtPLhGDPqp7oBr2cxCGRJY2l0p1lSntRuvH6PcYoUfPGJTsOWHOvKvSAdFQmHkRGdh5xTNnrji3swmCPxzvc2wAzMlNKGmTAu4bfS0";
        }

        public string GetUserId()
        {
            return !IsUserLoggedIn() ? null : "94713808";
        }

        public float GetGuiScale()
        {
            // Note: that override's editor's setting with DensityBasedScaling script
            // GuiScale is useful to change when making UI, according to task design.
            return 1f;
        }

        public string GetLangCode()
        {
            return "ru";
        }
        
        // Note: phone scenes wount work if NativeApi.IsPhone() returns false
        public string GetPlatformType()
        {
            return "unity-editor"; // -phone
        }

        public void BackButton()
        {
            throw new InvalidOperationException("BackButton is for handling hardware back button on device and must not be called from Unity Editor");
        }

        public void ExitClick()
        {
            Toast.Show("exit clicked", Toast.DurationLong);
        }

        public void BeginUserRegister(string callbackObj, string callbackMethod)
        {
            DataModel.SaveGameData("shooting.IsUserLogined", true);

            GameObject.Find(callbackObj).SendMessage(callbackMethod);
        }

        public void BeginUserLogin(string callbackObj, string callbackMethod)
        {
            DataModel.SaveGameData("shooting.IsUserLogined", true);

            GameObject.Find(callbackObj).SendMessage(callbackMethod);
        }

        public void OpenUrl(string url)
        {
            Toast.Show("open url requested: " + url);
        }

        public string GetApiUrl()
        {
            return "https://api2.4game.com";
        }

        public void OnUnityApiAvailable(string gameObjectName)
        {
            GameObject.Find(gameObjectName).SendMessage("StartGame");
        }

        private bool IsUserLoggedIn()
        {
            return DataModel.LoadGameData("shooting.IsUserLogined", false);
        }
    }
}

#endif