using Utils.JsonUtils;

namespace SDK.Purchase
{
    public class PurchaseItemInfo
    {
        public readonly string ID = "";
        public readonly string ProductID = "";
        public readonly string IAP = "";

        public PurchaseItemInfo(string jsonData)
        {
            var data = JsonHelper.FromJson(jsonData);
            if (data == null)
            {
                return;
            }

            if (data.ContainsKey("id"))
            {
                ID = (string) data["id"];
            }

            if (data.ContainsKey("product_id"))
            {
                ProductID = (string) data["product_id"];
            }

            if (data.ContainsKey("IAPForPay"))
            {
                IAP = (string) data["IAPForPay"];
            }

            if (data.ContainsKey("isFree"))
            {
                IsFree = (bool) data["isFree"];
            }
        }

        public bool IsFree { get; }
    }
}