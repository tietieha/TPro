﻿using System;
using Cysharp.Threading.Tasks;
using TEngine;
using YooAsset;
using ProcedureOwner = TEngine.IFsm<TEngine.IProcedureManager>;

namespace GameMain
{
    /// <summary>
    /// 流程 => 用户尝试更新清单
    /// </summary>
    public class ProcedureUpdateManifest : ProcedureBase
    {
        public override bool UseNativeDialog { get; }

        private ProcedureOwner _procedureOwner;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            Log.Info("更新资源清单！！！");
            _procedureOwner = procedureOwner;
            UILoadMgr.Show(UIDefine.UILoadUpdate, $"更新清单文件...");
            LoadUpdateLogic.Instance.DownProgressAction?.Invoke(40f);
            UpdateManifest(procedureOwner).Forget();
        }

        private async UniTaskVoid UpdateManifest(ProcedureOwner procedureOwner)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            var operation = GameModule.Resource.UpdatePackageManifestAsync(GameModule.Resource.PackageVersion);
            try
            {
                await operation.ToUniTask();

                if (operation.Status == EOperationStatus.Succeed)
                {
                    //更新成功
                    //注意：保存资源版本号作为下次默认启动的版本!
                    operation.SavePackageVersion();

                    if (GameModule.Resource.PlayMode == EPlayMode.WebPlayMode ||
                        GameModule.Resource.UpdatableWhilePlaying)
                    {
                        // 边玩边下载还可以拓展首包支持。
                        ChangeState<ProcedurePreload>(procedureOwner);
                        return;
                    }

                    ChangeState<ProcedureCreateDownloader>(procedureOwner);
                }
                else
                {
                    OnUpdateManifestError(operation.Error);
                }
            }
            catch (Exception e)
            {
                OnUpdateManifestError(e.Message);
            }
        }

        private void OnUpdateManifestError(string error)
        {
            Log.Error(error);
            UILoadTip.ShowMessageBox($"更新清单失败！点击确认重试", MessageShowType.TwoButton,
                LoadStyle.StyleEnum.Style_Retry
                , () => { ChangeState<ProcedureUpdateManifest>(_procedureOwner); },
                UnityEngine.Application.Quit);
        }
    }
}