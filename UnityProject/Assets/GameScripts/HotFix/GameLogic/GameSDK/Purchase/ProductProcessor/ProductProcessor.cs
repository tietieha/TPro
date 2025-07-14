namespace SDK.Purchase.ProductProcessor
{
    public abstract class ProductProcessor
    {
        public bool Buy(string orderId, PurchaseItemInfo itemInfo)
        {
            return DoBuy(orderId, itemInfo);
        }

        protected abstract bool DoBuy(string orderId, PurchaseItemInfo itemInfo);
    }
}