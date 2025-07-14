using SDK.Purchase.NativeBuyResult;
using TEngine;
using UnityEngine.Assertions;

namespace SDK.Purchase.Verify
{
    public class PurchaseVerifyAppStore: PurchaseVerify
    {
        public PurchaseVerifyAppStore(VerifyCallBack callBack) : base(callBack)
        {
        }

        public override void Verify(PurchaseVerifyParam param)
        {
            var para = param as PurchaseVerifyParamAppStore;
            Assert.IsNotNull(para);
            var result = para.Result as NativeBuyResultAppStore;
            Assert.IsNotNull(result);
            
            var orderID = result.OrderID;
            IAPManager.Instance.SetVerify(orderID, this);
            
            var luaTable = GameApp.Lua.Env.NewTable();
            luaTable.Set("transactionId", orderID);
            LuaModule.CallLuaFunc("CSCallLuaUtils", "Pay_iOSReq", luaTable);
        }
    }
}