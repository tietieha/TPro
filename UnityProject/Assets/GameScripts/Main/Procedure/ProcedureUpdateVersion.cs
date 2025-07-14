using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TEngine;
using YooAsset;
using ProcedureOwner = TEngine.IFsm<TEngine.IProcedureManager>;

namespace GameMain
{
    /// <summary>
    /// 流程 => 用户尝试更新静态版本
    /// </summary>
    public class ProcedureUpdateVersion : ProcedureBase
    {
        public override bool UseNativeDialog => true;

        private ProcedureOwner _procedureOwner;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            _procedureOwner = procedureOwner;

            base.OnEnter(procedureOwner);

            UILoadMgr.Show(UIDefine.UILoadUpdate, $"更新资源版本文件...");
            LoadUpdateLogic.Instance.DownProgressAction?.Invoke(30f);
            //检查设备是否能够访问互联网
            // if (Application.internetReachability == NetworkReachability.NotReachable)
            // {
            //     Log.Warning("The device is not connected to the network");
            //     UILoadMgr.Show(UIDefine.UILoadUpdate, LoadText.Instance.Label_Net_UnReachable);
            //     UILoadTip.ShowMessageBox(LoadText.Instance.Label_Net_UnReachable, MessageShowType.TwoButton,
            //         LoadStyle.StyleEnum.Style_Retry,
            //         GetStaticVersion().Forget,
            //         () => { ChangeState<ProcedureInitResources>(procedureOwner); });
            // }

            UILoadMgr.Show(UIDefine.UILoadUpdate, LoadText.Instance.Label_RequestVersionIng);
            GameEvent.Send(ProcedureEventDefines.Events.PROCEDURE_RES_START_UPDATE);
            // 用户尝试更新静态版本。
            GetStaticVersion().Forget();
        }

        /// <summary>
        /// 向用户尝试更新静态版本。
        /// </summary>
        private async UniTaskVoid GetStaticVersion()
        {
            await UniTask.Yield();
            if (!SettingsUtils.EnableUpdateRes())
            {
                ChangeState<ProcedureInitResources>(_procedureOwner);
                return;
            }

            var packageVersion = GameModule.Resource.GetPackageVersion();

            if (SettingsUtils.IsOverrideUpdateInfo() && !string.IsNullOrEmpty(GameModule.Resource.PackageVersion))
            {
                Log.Debug($"Updated package Version : from {packageVersion} to {GameModule.Resource.PackageVersion}");
                GameEvent.Send(ProcedureEventDefines.Events.PROCEDURE_RES_CHECK_VERSION, packageVersion, GameModule.Resource.PackageVersion);
                ChangeState<ProcedureUpdateManifest>(_procedureOwner);
                return;
            }


            var operation = GameModule.Resource.UpdatePackageVersionAsync();

            try
            {
                await operation.ToUniTask();

                if (operation.Status == EOperationStatus.Succeed)
                {
                    GameEvent.Send(ProcedureEventDefines.Events.PROCEDURE_RES_CHECK_VERSION, packageVersion, operation.PackageVersion);
                    //线上最新版本operation.PackageVersion
                    GameModule.Resource.PackageVersion = operation.PackageVersion;
                    Log.Debug($"Updated package Version : from {packageVersion} to {operation.PackageVersion}");
                    ChangeState<ProcedureUpdateManifest>(_procedureOwner);
                }
                else
                {
                    OnGetStaticVersionError(operation.Error);
                }
            }
            catch (Exception e)
            {
                OnGetStaticVersionError(e.Message);
            }
        }

        private void OnGetStaticVersionError(string error)
        {
            GameEvent.Send(ProcedureEventDefines.Events.PROCEDURE_RES_CHECK_VERSION_FAIL, error);
            Log.Error(error);
            UILoadTip.ShowMessageBox($"获取远程版本失败！点击确认重试", MessageShowType.TwoButton,
                LoadStyle.StyleEnum.Style_Retry
                , () => { ChangeState<ProcedureUpdateVersion>(_procedureOwner); }, UnityEngine.Application.Quit);
        }
    }
}