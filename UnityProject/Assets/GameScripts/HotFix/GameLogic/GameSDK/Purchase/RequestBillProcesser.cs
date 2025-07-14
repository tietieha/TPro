using System;
using Utils.JsonUtils;

namespace SDK.Purchase
{
    public class RequestBillResult
    {
        public bool Success;
        public PurchaseItemInfo ItemInfo;
        public string OrderID;
        public string Message;
    }

    public class RequestBillProcessor
    {
        private readonly PurchaseItemInfo _itemInfo;
        private Action<RequestBillResult> _callBack;

        public RequestBillProcessor(PurchaseItemInfo itemInfo, Action<RequestBillResult> callBack)
        {
            _itemInfo = itemInfo;
            _callBack = callBack;
        }
        
        /// <summary>
        /// 账单请求的回调
        /// </summary>
        /// <param name="jsonData"></param>
        public void OnRequestBillCalBack(string jsonData)
        {
            var data = JsonHelper.FromJson(jsonData);
            var selfOrderId = (string) data["selfOrderId"];
            var tResult = new RequestBillResult {Success = true, ItemInfo = _itemInfo, OrderID = selfOrderId};
            _callBack?.Invoke(tResult);
        }
    }
}