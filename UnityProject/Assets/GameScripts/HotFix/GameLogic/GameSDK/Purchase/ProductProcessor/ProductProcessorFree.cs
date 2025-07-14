namespace SDK.Purchase.ProductProcessor
{
    public class ProductProcessorFree: ProductProcessor
    {
        protected override bool DoBuy(string orderId, PurchaseItemInfo itemInfo)
        {
            // TODO:没有实现，有需求再说
            return false;
        }
    }
}