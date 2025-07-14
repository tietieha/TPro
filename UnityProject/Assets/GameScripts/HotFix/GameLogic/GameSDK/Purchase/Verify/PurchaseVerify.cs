namespace SDK.Purchase.Verify
{
    public abstract class PurchaseVerify
    {
        public delegate bool VerifyCallBack(VerifyResult result);

        private VerifyCallBack _callBack;

        public PurchaseVerify(VerifyCallBack callBack)
        {
            _callBack = callBack;
        }

        public abstract void Verify(PurchaseVerifyParam data);

        protected string GetCurItemId()
        {
            return string.Empty;
        }

        public bool OnVerifyEnd(VerifyResult result)
        {
            return _callBack.Invoke(result);
        }

        protected void OnVerifyFail(string message)
        {
            OnVerifyEnd(VerifyResult.Fail(message));
        }
    }
}