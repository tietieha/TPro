using Newtonsoft.Json.Linq;
using Utils.JsonUtils;

namespace SDK.Purchase.NativeBuyResult
{
    public class NativeBuyResultParserAppStore : NativeBuyResultParser
    {
        public override NativeBuyResult Parse(string result)
        {
            if (string.IsNullOrEmpty(result))
            {
                return ParseFailResult("SDK Pay fail due to result null", null);
            }

            var data = JsonHelper.FromJson(result);
            if (data == null)
            {
                return ParseFailResult("SDK Pay fail due to ParseJson null", null);
            }

            return ParseResult(data);
        }

        private NativeBuyResult ParseResult(JObject data)
        {
            var result = new NativeBuyResultAppStore(data);
            var payState = result.PayState;

            switch (payState)
            {
                case NativeBuyResultState.Success:
                    result.IsSuccess = true;
                    result.Message = "Native pay success";
                    return result;
                case NativeBuyResultState.Cancel:
                    result.IsSuccess = false;
                    result.Message = "Native pay cancel";
                    return result;
                case NativeBuyResultState.Fail:
                    result.IsSuccess = false;
                    result.Message = "Native pay fail by: " + result.MessageFromNative;
                    return result;
                case NativeBuyResultState.UnKnown:
                    result.IsSuccess = false;
                    result.Message = "Native pay result unknown";
                    return result;
                case NativeBuyResultState.NoSku:
                    result.IsSuccess = false;
                    result.Message = "Native pay fail by no sku";
                    return result;
                default:
                    result.IsSuccess = false;
                    result.Message = "Native pay result unhandled";
                    return result;
            }
        }
    }
}