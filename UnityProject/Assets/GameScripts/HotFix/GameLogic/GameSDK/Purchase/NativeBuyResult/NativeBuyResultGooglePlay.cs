using System;
using Newtonsoft.Json.Linq;
using TEngine;
using Utils.JsonUtils;

namespace SDK.Purchase.NativeBuyResult
{
 public class NativeBuyResultGooglePlay : NativeBuyResult
    {
        public NativeBuyResultGooglePlay(JObject data)
        {
            Data = data;

            PayState = (NativeBuyResultState) data["code"].ToString().ToInt();
            try
            {
                if (JsonHelper.ContainsKey(data, "selfOrderID"))
                {
                    _selfOrderID = (string) data["selfOrderID"];
                }

                if (JsonHelper.ContainsKey(data, "uuid"))
                {
                    _uuid = (string) data["uuid"];
                }
                
                if (JsonHelper.ContainsKey(data, "signData"))
                {
                    SignData = (string) data["signData"];
                }

                if (JsonHelper.ContainsKey(data, "orderId"))
                {
                    OrderID = (string) data["orderId"];
                }

                if (JsonHelper.ContainsKey(data, "productId"))
                {
                    ProductId = (string) data["productId"];
                }

                if (JsonHelper.ContainsKey(data, "signature"))
                {
                    Signature = (string) data["signature"];
                }

                if (JsonHelper.ContainsKey(data, "message"))
                {
                    MessageFromNative = (string) data["message"];
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
        public string Uuid => _uuid;

        public string SignData { get; }

        public string OrderID { get; } // Google 的订单ID

        public string ProductId { get; } //IAP

        public string Signature { get; } //IAP

        public string MessageFromNative { get; }

        private readonly string _selfOrderID; //我们游戏内部自己的订单ID

        private readonly string _uuid; //用户ID
    }
}