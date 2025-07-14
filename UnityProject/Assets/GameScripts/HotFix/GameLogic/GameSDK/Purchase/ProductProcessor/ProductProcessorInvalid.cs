using TEngine;

namespace SDK.Purchase.ProductProcessor
{
    public class ProductProcessorInvalid: ProductProcessor
    {
        protected override bool DoBuy(string orderId, PurchaseItemInfo itemInfo)
        {
            Log.Error("PlatformInvalid can't buy");
            return false;
        }
    }
}