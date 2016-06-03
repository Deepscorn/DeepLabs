// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System.Collections.Generic;
using Assets.Sources.Logic.Data;

namespace Assets.Sources.NativePlatform.Platforms
{
    public interface INativePlatform
    {
        void BuyInAppPurchase(string productId, string userId, string orderId, string callBackObj, string callBackMethod);
      
        IList<PurchaseInfo> GetInAppPurchases();
      
        void RemoveInnAppPurchase(PurchaseInfo purchaseInfo);
      
        string GetAppId();

        string GetAppProductId();

        string GetAppPromoId();
      
        string GetAccessToken();
      
        string GetUserId();
      
        float GetGuiScale();

        string GetPlatformType(); // iphone, ipad, android-phone, android-tablet, unity-editor
      
        string GetLangCode();
      
        void BackButton();
      
        void ExitClick();

        void BeginUserRegister(string callbackObj, string callbackMethod);

        void BeginUserLogin(string callbackObj, string callbackMethod);

        void OpenUrl(string url);

        string GetApiUrl();

        void OnUnityApiAvailable(string gameObjectName);
    }
}

