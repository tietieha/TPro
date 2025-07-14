using SDK.BaseCore;
using SDK.Purchase.Verify;
using Utils.JsonUtils;

namespace SDK.Purchase
{
    public static class PurchaseLuaHelper
    {
        // 是否开启沙盒支付(用于内网环境也测真实支付)
        public static bool IsSandBoxPurchase
        {
            get
            {
#if SAND_BOX_PURCHASE
        return true;
#endif
                return false;
            }
        }

        // 历史订单查询
        public static void QueryPurchaseOrder()
        {
            PurchaseUtils.QueryPurchaseOrder();
        }

        // 支付前信息设置
        public static void StartPreBuy(string jsonData)
        {
            var info = new PurchaseItemInfo(jsonData);
            IAPManager.Instance.StartPreBuy(info);
        }

        // 支付前回调
        public static void OnRequestBillCalBack(string jsonData)
        {
            IAPManager.Instance.CurBillProcessor.OnRequestBillCalBack(jsonData);
        }

        public static string GetPriceText(string dollar, string productId)
        {
            return PurchaseUtils.GetPriceText(dollar, productId);
        }

        public static string GetPriceTextWithOutCurrencyCode(string dollar, string productID)
        {
            return PurchaseUtils.GetPriceText(dollar, productID, false);
        }
        
        //只获取纯数字价格
        public static string GetPriceValueText(string dollar, string productId)
        {
            return PurchaseUtils.GetPriceValueText(dollar, productId);
        }
        
        public static string GetCurrencyCode(string productID)
        {
            return PurchaseUtils.GetCurrencyCode(productID);
        }
        
        public static string GetPriceSymbol()
        {
            return Utils.StorageUtils.GetString(GameDefines.SettingKeys.UwPriceSymbol, "US $");
        }
        
        //PriceSymbol是否在价格数字后面出现 
        public static bool GetPriceSymbolBack()
        {
            return Utils.StorageUtils.GetBool(GameDefines.SettingKeys.UwPriceSymbolBack);
        }

        // 真正支付后的回调
        public static void OnVerifyRequestEnd(string jsonData)
        {
            var data = JsonHelper.FromJson(jsonData);
            if (data.ContainsKey("status"))
            {
                var status = (int) data["status"];
                var success = (status == 0);
                var payStatus = (int) data["data"]["payStatus"];
                var orderId = (string) data["data"]["orderId"];
                
                var verify = IAPManager.Instance.GetVerify(orderId);
                if (verify is null)
                {
                    return;
                }
                
                if (!success)
                {
                    verify.OnVerifyEnd(
                        VerifyResult.Fail($"Verify Failed PayStatus: {payStatus}"));
                    return;
                }

                verify.OnVerifyEnd(VerifyResult.Success(data));
                
                // 支付状态 0 成功 1 校验失败 都消耗掉商品
                if (payStatus is 0 or 1)
                {
                    var platform = (string) data["data"]["pf"];
                    // 先写死判断
                    if (platform is "AppStore" or "market_global")
                    {
                        SDKManager.Instance.ConsumeCallback(orderId, status);
                    }
                }
            }
        }
    }
}