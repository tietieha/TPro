using System;
using Cysharp.Threading.Tasks;
using TEngine;
using UnityEngine;
using YooAsset;
using ProcedureOwner = TEngine.IFsm<TEngine.IProcedureManager>;
using Utility = TEngine.Utility;

namespace GameMain
{
    public class ProcedureDownloadFile : ProcedureBase
    {
        public override bool UseNativeDialog { get; }

        private ProcedureOwner _procedureOwner;

        private float _lastUpdateDownloadedSize;
        private float _totalSpeed;
        private int _speedSampleCount;
        private bool _waitingConfirmCarrierDataNetwork;

        private float CurrentSpeed
        {
            get
            {
                float interval = Math.Max(GameTime.deltaTime, 0.01f); // 防止deltaTime过小
                var sizeDiff = GameModule.Resource.Downloader.CurrentDownloadBytes - _lastUpdateDownloadedSize;
                _lastUpdateDownloadedSize = GameModule.Resource.Downloader.CurrentDownloadBytes;
                var speed = sizeDiff / interval;

                // 使用滑动窗口计算平均速度
                _totalSpeed += speed;
                _speedSampleCount++;
                return _totalSpeed / _speedSampleCount;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            _procedureOwner = procedureOwner;

            Log.Info("开始下载更新文件！");

            UILoadMgr.Show(UIDefine.UILoadUpdate, "开始下载文件...");

            BeginDownload().Forget();
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (GameModule.Resource.ConfirmedCarrierDataNetwork)
                return;

            if (Application.internetReachability != NetworkReachability.ReachableViaCarrierDataNetwork)
                return;

            if (_waitingConfirmCarrierDataNetwork)
                return;
            // 如果网络从wifi切成了移动数据网络，提示用户
            if (!GameModule.Resource.ConfirmedCarrierDataNetwork
                && GameModule.Resource.Downloader.TotalDownloadBytes > GameModule.Resource.MaxCarrierDataNetworkSizeMb * 1024 * 1024)
            {
                _waitingConfirmCarrierDataNetwork = true;
                GameModule.Resource.Downloader.PauseDownload();
                UILoadTip.ShowMessageBox(LoadText.Instance.Label_Net_ReachableViaCarrierDataNetwork,
                    MessageShowType.TwoButton,
                    LoadStyle.StyleEnum.Style_Default,
                    () =>
                    {
                        GameModule.Resource.ConfirmedCarrierDataNetwork = true;
                        GameModule.Resource.Downloader.ResumeDownload();
                    },
                    UnityEngine.Application.Quit);
            }
        }

        private async UniTaskVoid BeginDownload()
        {
            GameEvent.Send(ProcedureEventDefines.Events.COMMON, ProcedureEventDefines.Command.PROCEDURE_RES_DOWNLOAD_START);
            var downloader = GameModule.Resource.Downloader;

            // 注册下载回调
            downloader.OnDownloadErrorCallback = OnDownloadErrorCallback;
            downloader.OnDownloadProgressCallback = OnDownloadProgressCallback;
            downloader.BeginDownload();
            await downloader;

            // 检测下载结果
            if (downloader.Status != EOperationStatus.Succeed)
                return;

            ChangeState<ProcedureDownloadOver>(_procedureOwner);
        }

        private void OnDownloadErrorCallback(string fileName, string error)
        {
            // GameEvent.Send(ProcedureEventDefines.Events.COMMON, ProcedureEventDefines.Command.PROCEDURE_RES_DOWNLOAD_ERROR);
            Log.Error($"下载文件失败: {fileName}, 错误信息: {error}");
            UILoadTip.ShowMessageBox(LoadText.Instance.Label_DownLoadFailed,
                MessageShowType.TwoButton,
                LoadStyle.StyleEnum.Style_Default,
                () => { ChangeState<ProcedureCreateDownloader>(_procedureOwner); }, UnityEngine.Application.Quit);
        }

        private void OnDownloadProgressCallback(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
        {
            string currentSizeMb = (currentDownloadBytes / 1048576f).ToString("f1");
            string totalSizeMb = (totalDownloadBytes / 1048576f).ToString("f1");
            float progressPercentage = GameModule.Resource.Downloader.Progress * 100;
            string speed = Utility.File.GetLengthString((int)CurrentSpeed);

            string line1 = Utility.Text.Format("[{0}MB/{1}MB] {2}/s (预计用时:{3})", currentSizeMb, totalSizeMb, speed, GetRemainingTime(totalDownloadBytes, currentDownloadBytes, CurrentSpeed));
            // string line2 = Utility.Text.Format("已更新大小 {0}MB/{1}MB", currentSizeMb, totalSizeMb);
            // string line3 = Utility.Text.Format("当前网速 {0}/s，剩余时间 {1}", speed, GetRemainingTime(totalDownloadBytes, currentDownloadBytes, CurrentSpeed));

            LoadUpdateLogic.Instance.DownProgressAction?.Invoke(GameModule.Resource.Downloader.Progress);
            UILoadMgr.Show(UIDefine.UILoadUpdate, line1);

            int progressInt = (int)progressPercentage;
            if (progressInt % 10 == 0)
            {
                Log.Debug($"下载进度: {progressInt}% - {currentSizeMb}MB/{totalSizeMb}MB");
                GameEvent.Send(ProcedureEventDefines.Events.COMMON, $"{ProcedureEventDefines.Command.PROCEDURE_RES_DOWNLOAD_PROGRESS}|{progressInt}");
            }
        }

        private string GetRemainingTime(long totalBytes, long currentBytes, float speed)
        {
            int needTime = 0;
            if (speed > 0)
            {
                needTime = (int)((totalBytes - currentBytes) / speed);
            }
            TimeSpan ts = new TimeSpan(0, 0, needTime);
            return ts.ToString(@"mm\:ss");
        }



    }
}
