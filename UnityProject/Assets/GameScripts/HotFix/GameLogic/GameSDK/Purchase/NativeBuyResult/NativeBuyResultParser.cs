using Newtonsoft.Json.Linq;

namespace SDK.Purchase.NativeBuyResult
{
    public abstract class NativeBuyResultParser
    {
        public abstract NativeBuyResult Parse(string result);

        protected NativeBuyResult ParseSuccessResult(JObject result)
        {
            return NativeBuyResult.Success(result);
        }

        protected NativeBuyResult ParseFailResult(string message, JObject data)
        {
            return NativeBuyResult.Fail(message, data);
        }

        protected string GetCurItemID()
        {
            return string.Empty;
        }
    }
}