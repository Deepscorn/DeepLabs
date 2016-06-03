// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System;

namespace Assets.Sources.Logic.Data
{
    public class PurchaseInfo
    {
        public string UserId { get; private set; }

        public string OrderId { get; private set; }

        public string PurchaseId { get; private set; }
      
        public string ReceiptData { get; private set; }
      
        public string Signature { get; private set; }
      
        public PurchaseInfo(string userId, string orderId, string purchaseId, string receiptData, string signature)
        {
            UserId = userId;
            OrderId = orderId;
            ReceiptData = receiptData;
            Signature = signature;
            PurchaseId = purchaseId;
        }

        public override string ToString()
        {
            return string.Format("UserId: {0}, OrderId: {1}, PurchaseId: {2}, ReceiptData: {3}, Signature: {4}",
                UserId, OrderId, PurchaseId, ReceiptData, Signature);
        }
    }
}

