using Newtonsoft.Json.Linq;

namespace SDK.Purchase.NativeBuyResult
{
    // 原生层支付的几个结果枚举
    public enum NativeBuyResultState
    {
        UnKnown = -1,
        Success = 0,
        Cancel = 1,
        Fail = 2,
        NoSku = 3,
    }
    
    public class NativeBuyResult
    {
        public bool IsSuccess;
        public JObject Data;
        public string Message;

        public static NativeBuyResult Success(JObject data)
        {
            return new() {IsSuccess = true, Data = data};
        }

        public static NativeBuyResult Fail(string message, JObject data = null)
        {
            return new() {IsSuccess = false, Message = message, Data = data};
        }
        
        public virtual string GetSelfOrderID()
        {
            return string.Empty;
        }
    }
}