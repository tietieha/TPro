using DG.Tweening;
using TEngine;
using YooAsset;
using ProcedureOwner = TEngine.IFsm<TEngine.IProcedureManager>;
using Language = TEngine.Language;

namespace GameMain
{
    /// <summary>
    /// 流程 => 启动器。
    /// </summary>
    public class ProcedureLaunch : ProcedureBase
    {
        public override bool UseNativeDialog => true;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

#if UNITY_EDITOR
            ChangeState<ProcedureInitModule>(procedureOwner);
#else
            // 运行一帧即切换到 Splash 展示流程
            ChangeState<ProcedureSplash>(procedureOwner);
#endif
        }
    }
}
