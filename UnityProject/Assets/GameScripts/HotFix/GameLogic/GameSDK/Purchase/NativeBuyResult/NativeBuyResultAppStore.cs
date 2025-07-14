using System;
using Newtonsoft.Json.Linq;
using TEngine;
using Utils.JsonUtils;

namespace SDK.Purchase.NativeBuyResult
{
    public class NativeBuyResultAppStore: NativeBuyResult
    {
        public NativeBuyResultAppStore(JObject data)
        {
            Data = data;
            PayState = (NativeBuyResultState) data["code"].ToString().ToInt();

            try
            {
                if (JsonHelper.ContainsKey(data, "serverOrderId"))
                {
                    _selfOrderID = (string) data["serverOrderId"];
                }

                if (JsonHelper.ContainsKey(data, "encodedReceipt"))
                {
                    SignData = (string) data["encodedReceipt"];
                }

                if (JsonHelper.ContainsKey(data, "transactionIdentifier"))
                {
                    OrderID = (string) data["transactionIdentifier"];
                }

                if (JsonHelper.ContainsKey(data, "productIdentifier"))
                {
                    ProductId = (string) data["productIdentifier"];
                }

                if (JsonHelper.ContainsKey(data, "payCountryCode"))
                {
                    CountryCode = (string) data["payCountryCode"];
                }
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public override string GetSelfOrderID() => _selfOrderID;
        public NativeBuyResultState PayState { get; set; }
        public string SelfOrderID => _selfOrderID;
        public string SignData { get; }
        public string OrderID { get; } // appstore的收据ID
        public string ProductId { get; } //IAP
        public string MessageFromNative { get; }
        public string CountryCode { get; } = string.Empty;

        private readonly string _selfOrderID; //我们游戏内部自己的订单ID
    }
}