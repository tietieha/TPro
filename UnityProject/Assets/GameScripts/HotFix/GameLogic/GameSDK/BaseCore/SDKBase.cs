using System;
using TEngine;

namespace SDK.BaseCore
{
    public class SDKBase
    {
        // 平台ID：1 Google 2 AppStore
        private int _platformId;
        
        public void SetPlatformId(string platformIdStr)
        {
            _platformId = StringUtils.TryParseInt(platformIdStr);
        }

        public int GetPlatformId()
        {
            return _platformId;
        }
        
        protected bool CheckIsInitSDK()
        {
            return _platformId != 0;
        }
        
        public virtual void AddListener()
        {
        }

        public virtual void Init()
        {
        }

        public virtual void SetGameUid(string gameUid)
        {
        }

        public virtual void SendDataToNative(string funcName, string data)
        {
        }

        public virtual string GetDataFromNative(string funcName, string data)
        {
            return string.Empty;
        }

        #region 支付

        // 初始化
        public virtual void InitPurchase()
        {
        }

        // 支付
        public virtual void Pay(string data)
        {
        }

        // 订单查询
        public virtual void QueryPurchaseOrder()
        {
        }

        // 消耗回调
        public virtual void ConsumeCallback(string orderId, int state)
        {
        }

        // 获取货币单位
        public virtual string GetPurchaseCurrencyCode(string productId)
        {
            return "USD";
        }

        // 获取SKU价格
        public virtual string GetPurchaseLocalPrice(string productId)
        {
            return "";
        }

        #endregion

        #region 推送
        public virtual void CancelAllNotifications() 
        {
        }

        public virtual void SendNotification(int id, string title, string content, long time)
        {
        }

        public virtual void GetFCMToken(Action<string> action)
        {
            
        }

        #endregion
    }
}