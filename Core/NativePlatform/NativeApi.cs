// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System;
using System.Collections.Generic;
using Assets.Sources.Logic;
using Assets.Sources.Logic.Data;
using Assets.Sources.NativePlatform.Platforms;

namespace Assets.Sources.NativePlatform
{
    // Each public method here actually calls native method
    // todo: Just make it as singleton returning class based on preprocessor definition
    public static class NativeApi
    {
        public static string AccessToken { get; private set; }

        public static string AppId { get; private set; }

        public static string AppProductId { get; private set; }

        public static string AppPromoId { get; private set; }

        public static string UserId { get; private set; }

        // todo: try Screen.dpi
        public static float GuiScale { get; private set; }

        public static string LangCode { get; private set; }

        public static string PlatformType { get; private set; }

        public static string ApiUrl { get { return Platform.GetApiUrl(); } }

        public static bool IsPhone
        {
            get
            {
                return PlatformType.EndsWith("phone");
            }
        }

        public static bool IsUserLoggedIn
        {
            get
            {
                return Platform.GetUserId() != null && Platform.GetAccessToken() != null;
            }
        }

        public static void UpdateAllData()
        {
            AppId = Platform.GetAppId();
            AppProductId = Platform.GetAppProductId();
            AppPromoId = Platform.GetAppPromoId();
            GuiScale = Platform.GetGuiScale();
            LangCode = Platform.GetLangCode();
            PlatformType = Platform.GetPlatformType();

            UpdateAuthorizationData();
        }

        public static void UpdateAuthorizationData()
        {
            AccessToken = Platform.GetAccessToken();
            UserId = Platform.GetUserId();
        }

        public static IList<PurchaseInfo> GetInAppPurchases(string userId)
        {
            return Platform.GetInAppPurchases();
        }

        public static void RemovePurchase(PurchaseInfo purchaseInfo)
        {
            Platform.RemoveInnAppPurchase(purchaseInfo);
        }

        // We want Action, not method name because each method in project must be referenced from somewhere
        // So we can easily remove unused methods. And each remove wount force us to search for textual occurences
        public static void BeginBuyInApp(string productId, string orderId, string callBackObjName, Action callbackAction)
        {
            var callbackMethod = callbackAction.Method.Name;
            Platform.BuyInAppPurchase(productId, UserId, orderId, callBackObjName, callbackMethod);
        }

        public static void BackButton()
        {
            Platform.BackButton();
        }

        public static void ExitClick()
        {
            Platform.ExitClick();
        }

        public static void BeginUserLogin()
        {
            //var name = UserDataManager.Instance.name;
            //Action action = UserDataManager.Instance.UpdateDataFireUserChanged;
            //Platform.BeginUserLogin(name, action.Method.Name);
        }

        public static void BeginUserRegister()
        {
            //var name = UserDataManager.Instance.name;
            //Action action = UserDataManager.Instance.UpdateDataFireUserChanged;
            //Platform.BeginUserRegister(name, action.Method.Name);
        }

        public static void OpenUrl(string url)
        {
            Platform.OpenUrl(url);
        }

        public static void OnUnityApiAvailable(string gameObjectName)
        {
            Platform.OnUnityApiAvailable(gameObjectName);
        }

        private static INativePlatform platform;

        private static INativePlatform Platform
        {
            get
            {
                if (platform == null)
                {
                    platform = CreateNativePlatform();
                }

                return platform;
            }
        }

        private static INativePlatform CreateNativePlatform()
        {
            INativePlatform result;

#if UNITY_EDITOR
            result = new NativeEditor();
#elif UNITY_IOS
          result = new NativeIos();
#elif UNITY_ANDROID
          result = new NativeAndroid();
#else
            throw NotSupportedException();
#endif

            return result;
        }
    }
}
