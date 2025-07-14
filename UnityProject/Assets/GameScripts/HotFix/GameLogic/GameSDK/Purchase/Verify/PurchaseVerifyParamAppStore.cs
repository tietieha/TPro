namespace SDK.Purchase.Verify
{
    public class PurchaseVerifyParamAppStore: PurchaseVerifyParam
    {
        public NativeBuyResult.NativeBuyResult Result;
        public PurchaseVerifyParamAppStore() : base(true)
        {
        }
    }
}