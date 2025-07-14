using Newtonsoft.Json.Linq;

namespace SDK.Purchase.Verify
{
    public class VerifyResult
    {
        public bool IsSuccess;
        public JObject Data;
        public string ErrorMessage;
        public bool IsFree;

        public VerifyResult(bool success, JObject data, string errorMessage)
        {
            IsSuccess = success;
            Data = data;
            ErrorMessage = errorMessage;
        }

        public static VerifyResult Fail(string errorMessage)
        {
            return new(false, null, errorMessage);
        }

        public static VerifyResult Success(JObject result)
        {
            return new(true, result, string.Empty);
        }
    }
}