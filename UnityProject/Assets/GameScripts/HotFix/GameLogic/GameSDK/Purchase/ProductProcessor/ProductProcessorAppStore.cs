#if !UNITY_EDITOR && UNITY_IOS
using System;
using Newtonsoft.Json.Linq;
using SDK.BaseCore;
using Utils.JsonUtils;
#endif

namespace SDK.Purchase.ProductProcessor
{
    public class ProductProcessorAppStore : ProductProcessor
    {
        protected override bool DoBuy(string orderId, PurchaseItemInfo itemInfo)
        {
#if !UNITY_EDITOR && UNITY_IOS
            PurchaseUtils.SetCacheItemId(itemInfo.ID);

            try
            {
                var data = new JObject
                {
                    ["skuId"] = itemInfo.IAP,
                    ["selfOrderId"] = orderId,
                };

                var jsonData = JsonHelper.ToJson(data);
                SDKManager.Instance.SDKPay(jsonData);
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return false;
            }

#endif
            return false;
        }
    }
}