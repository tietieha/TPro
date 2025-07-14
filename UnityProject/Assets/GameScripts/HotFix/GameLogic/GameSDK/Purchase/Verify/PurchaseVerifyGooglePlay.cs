using SDK.Purchase.NativeBuyResult;
using TEngine;
using UnityEngine.Assertions;

namespace SDK.Purchase.Verify
{
    public class PurchaseVerifyGooglePlay: PurchaseVerify
    {
        public PurchaseVerifyGooglePlay(VerifyCallBack callBack) : base(callBack)
        {
        }

        public override void Verify(PurchaseVerifyParam param)
        {
            var para = param as PurchaseVerifyParamGooglePlay;
            Assert.IsNotNull(para);
            
            var result = para.Result as NativeBuyResultGooglePlay;
            Assert.IsNotNull(result);
            
            IAPManager.Instance.SetVerify(result.OrderID, this);
            
            var productId = result.ProductId;
            var signData = result.SignData;
            var payCurrencyCode = PurchaseUtils.GetCurrencyCode(productId);
            var localPrice = PurchaseUtils.GetLocalPrice(productId);
            var signature = result.Signature;

            var luaTable = GameApp.Lua.Env.NewTable();
            luaTable.Set("signData", signData);
            luaTable.Set("signature", signature);
            luaTable.Set("pay_closingCurrency", payCurrencyCode);
            luaTable.Set("pay_PriceOfClosingCurrency", localPrice);
            
            LuaModule.CallLuaFunc("CSCallLuaUtils", "PayGoogleReq", luaTable);
        }
    }
}