using TEngine;
using ProcedureOwner = TEngine.IFsm<TEngine.IProcedureManager>;

namespace GameMain
{
    public class ProcedureDownloadOver : ProcedureBase
    {
        public override bool UseNativeDialog { get; }

        private bool _needClearCache;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            Log.Info("下载完成!!!");

            UILoadMgr.Show(UIDefine.UILoadUpdate, $"下载完成...");
            GameEvent.Send(ProcedureEventDefines.Events.COMMON, ProcedureEventDefines.Command.PROCEDURE_RES_DOWNLOAD_SUCCESS);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            if (_needClearCache)
            {
                ChangeState<ProcedureClearCache>(procedureOwner);
            }
            else
            {
                ChangeState<ProcedureInitResources>(procedureOwner);
            }
        }
    }
}