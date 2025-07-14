using System.Collections.Generic;
using GameBase;
using Newtonsoft.Json.Linq;
using SDK.BaseCore;
using SDK.Purchase.NativeBuyResult;
using SDK.Purchase.ProductProcessor;
using SDK.Purchase.Verify;
using UnityEngine;
using Utils.JsonUtils;
using TEngine;

namespace SDK.Purchase
{
    public class IAPManager : Singleton<IAPManager>
    {
        // 整个支付流程的阶段划分
        private enum PurchasePhase
        {
            BeforeReq, // GameClient to GameServer for getting transactionOrderId
            Native, // GameClient to SDK (Such as Google Play Billing or AppStore Purchase)
            AfterReq, // GameClient Get Result from SDK and if success send it to GameServer for getting reward
        }
        
        // 原生层支付成功后通知游戏服务器进行发货的最终支付状态
        private enum NativeVerifyStatus
        {
            Success = 0, // 成功
            CheckFail = 1, // 校验失败
            TryAgainLater = 2, // 稍后再试，可能是调用平台api失败等原因造成的异常
        }
        
        public RequestBillProcessor CurBillProcessor { get; private set; }

        private readonly Dictionary<string, PurchaseVerify> _verifies = new();

        public void SetVerify(string orderId, PurchaseVerify verify)
        {
            _verifies[orderId] = verify;
        }

        public PurchaseVerify GetVerify(string orderId)
        {
            return _verifies.ContainsKey(orderId) ? _verifies[orderId] : null;
        }

        #region 1.预购买

        // 开始购买流程
        public void StartPreBuy(PurchaseItemInfo itemInfo)
        {
            var processor = new RequestBillProcessor(itemInfo, OnGetBillEnd);
            CurBillProcessor = processor;
        }

        // 购买回调
        private void OnGetBillEnd(RequestBillResult result)
        {
            var success = result.Success;
            var orderID = result.OrderID;
            var itemInfo = result.ItemInfo;
            if (success)
            {
                if (!string.IsNullOrEmpty(orderID))
                {
                    StartBuy(orderID, itemInfo);
                }
                else
                {
                    PurchaseFail(PurchasePhase.BeforeReq, "GetBill Status OK But With Empty SelfOrderId");
                }
            }
            else
            {
                // 原则上走不到这里来，Lua层对status作了判断拦了一层
                PurchaseFail(PurchasePhase.BeforeReq, result.Message);
            }
        }

        #endregion

        #region 2.正式购买

        // 准备正式购买
        private void StartBuy(string orderID, PurchaseItemInfo itemInfo)
        {
            var processor = GetProductProcessor(itemInfo, false);
            var success = processor.Buy(orderID, itemInfo);
            if (!success)
            {
                // 调支付直接失败通知业务层取消订单
                CancelBill(orderID);
            }
        }

        private void CancelBill(string orderID)
        {
            // TODO: 服务器需要加取消预订单的接口
        }

        private ProductProcessor.ProductProcessor GetProductProcessor(PurchaseItemInfo itemInfo, bool byItem = false)
        {
            if (byItem)
            {
                return new ProductProcessorItem();
            }

            if (itemInfo.IsFree)
            {
                return new ProductProcessorFree();
            }

            return GetProductProcessorNative();
        }

        private ProductProcessor.ProductProcessor GetProductProcessorNative()
        {
            var platformId = SDKManager.Instance.GetPlatform();
            return platformId switch
            {
                PlatformID.Google => new ProductProcessorGooglePlay(),
                PlatformID.AppStore => new ProductProcessorAppStore(),
                PlatformID.UnKnown => new ProductProcessorInvalid(),
                _ => new ProductProcessorInvalid()
            };
        }

        #endregion

        private void PurchaseFail(PurchasePhase phase, string reason)
        {
            Debug.LogError($"Purchase Fail Phase: {phase} reason: {reason}");
            if (phase is PurchasePhase.Native or PurchasePhase.BeforeReq)
            {
                SDKManager.Instance.LogEvent("purchase_failed");
            }
        }


        #region 3.原生层支付结果相关

        // 原生层支付结果回调
        public void OnSDKBuyEnd(string result)
        {
            var parser = GetNativeBuyResultParser();
            var t = parser.Parse(result);
            DealWithNativeBuyResult(t);
        }

        private void DealWithNativeBuyResult(NativeBuyResult.NativeBuyResult result)
        {
            if (result == null)
            {
                NativeBuyResultFail(string.Empty, "DealWithNativeBuyResult is null");
                return;
            }

            if (result.IsSuccess)
            {
                var verifyParam = GetNativeVerifyParam(result);
                NativeBuyResultSuccess(verifyParam);
            }
            else
            {
                NativeBuyResultFail(result.GetSelfOrderID(), result.Message);
            }
        }

        private void NativeBuyResultSuccess(PurchaseVerifyParam param)
        {
            var verify = GetPurchaseVerify(param);
            verify.Verify(param);
        }
        
        private void NativeBuyResultFail(string selfOrderID, string message)
        {
            if (!selfOrderID.IsNullOrEmpty())
            {
                CancelBill(selfOrderID);
            }

            PurchaseFail(PurchasePhase.Native, message);
        }

        private NativeBuyResultParser GetNativeBuyResultParser()
        {
            var platformId = SDKManager.Instance.GetPlatform();
            switch (platformId)
            {
                case PlatformID.Google:
                    return new NativeBuyResultParserGooglePlay();
                case PlatformID.AppStore:
                    return new NativeBuyResultParserAppStore();
                case PlatformID.UnKnown:
                    return new NativeBuyResultParserInvalid();
                default:
                    return new NativeBuyResultParserInvalid();
            }
        }

        #endregion

        #region 4.如果3的结果是成功，则通知服务器发货

        private PurchaseVerify GetPurchaseVerify(PurchaseVerifyParam param)
        {
            return param switch
            {
                {IsNative: true} => GetNativeVerify(),
                {IsByItem: true} => new PurchaseVerifyItem(PurchaseVerifyEnd),
                _ => GetPurchaseVerifyForFree(param)
            };
        }

        private PurchaseVerify GetPurchaseVerifyForFree(PurchaseVerifyParam param)
        {
            if (param is PurchaseVerifyParamFree para && para.ItemInfo.IsFree)
            {
                return new PurchaseVerifyFree(PurchaseVerifyEnd);
            }

            return new PurchaseVerifyTest(PurchaseVerifyEnd);
        }

        private PurchaseVerify GetNativeVerify()
        {
            var platformId = SDKManager.Instance.GetPlatform();

            switch (platformId)
            {
                case PlatformID.Google:
                    return new PurchaseVerifyGooglePlay(PurchaseVerifyEnd);
                case PlatformID.AppStore:
                    return new PurchaseVerifyAppStore(PurchaseVerifyEnd);
                case PlatformID.UnKnown:
                    return new PurchaseVerifyInvalid(PurchaseVerifyEnd);
                default:
                    return new PurchaseVerifyInvalid(PurchaseVerifyEnd);
            }
        }

        private bool PurchaseVerifyEnd(VerifyResult result)
        {
            if (result.IsSuccess)
            {
                return PurchaseParseData(result.Data, result.IsFree);
            }

            return false;
        }

        private bool PurchaseParseData(JObject message, bool resultIsFree)
        {
            if (!JsonHelper.ContainsKey(message, "status"))
            {
                return true;
            }
            
            var status = (int) message["data"]["payStatus"];
            var orderId = (string) message["data"]["orderId"];
            var payStatus = (NativeVerifyStatus) status;

            switch (payStatus)
            {
                case NativeVerifyStatus.Success:
                {
                }
                    break;
                case NativeVerifyStatus.CheckFail:
                {
                    PurchaseFail(PurchasePhase.AfterReq, "Verified failed maybe your orderId is illegal");
                }
                    break;
                // 服务器不发奖励，支付平台不消耗，重新走补单逻辑
                case NativeVerifyStatus.TryAgainLater:
                {
                    PurchaseFail(PurchasePhase.AfterReq, "Verified failed please try again later");
                    return false;
                }
                default:
                    return false;
            }

            return true;
        }

        private PurchaseVerifyParam GetNativeVerifyParam(NativeBuyResult.NativeBuyResult result)
        {
            var platformId = SDKManager.Instance.GetPlatform();
            switch (platformId)
            {
                case PlatformID.Google:
                    return new PurchaseVerifyParamGooglePlay {Result = result};
                case PlatformID.AppStore:
                    return new PurchaseVerifyParamAppStore {Result = result};
                default:
                    return null;
            }
        }

        #endregion

        public void Init()
        {
            SDKManager.Instance.InitPurchase();
        }
    }
}