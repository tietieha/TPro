using System;
using Cysharp.Threading.Tasks;
using TEngine;

namespace GameMain
{
    public class ProcedureStartGame : ProcedureBase
    {
        public override bool UseNativeDialog { get; }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            LoadUpdateLogic.Instance.DownProgressAction?.Invoke(100f);
            StartGame().Forget();
        }

        private async UniTaskVoid StartGame()
        {
            await UniTask.Yield();
            UILoadMgr.HideAll();
        }
    }
}