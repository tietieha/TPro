namespace SDK.Purchase.Verify
{
    public class PurchaseVerifyParamFree: PurchaseVerifyParam
    {
        public string OrderID;
        public PurchaseItemInfo ItemInfo;
        protected PurchaseVerifyParamFree() : base(false)
        {
        }
    }
}