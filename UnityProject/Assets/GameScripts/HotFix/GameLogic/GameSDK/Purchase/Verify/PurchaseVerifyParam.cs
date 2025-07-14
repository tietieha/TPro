namespace SDK.Purchase.Verify
{
    public class PurchaseVerifyParam
    {
        public readonly bool IsNative;
        public readonly bool IsByItem;

        protected PurchaseVerifyParam(bool isNative, bool isByItem = false)
        {
            IsNative = isNative;
            IsByItem = isByItem;
        }
    }
}