using TEngine;
using UnityEngine;
using ProcedureOwner = TEngine.IFsm<TEngine.IProcedureManager>;

namespace GameMain
{
    /// <summary>
    /// 流程 => 闪屏。
    /// </summary>
    public class ProcedureSplash : ProcedureBase
    {
        public override bool UseNativeDialog => true;

        private GameObject _splashGo;
        private UISplashPage _splash;

#if !UNITY_EDITOR
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            var asset = Resources.Load<GameObject>("AssetLoad/UISplashPage");
            _splashGo = Object.Instantiate(asset, GameModule.UI.DefaultUI);
            if (_splashGo)
            {
                _splash = _splashGo.GetComponent<UISplashPage>();
                _splash.StartSplash(() =>
                {
                    OnSplashComplete(procedureOwner);
                });
            }
            else
            {
                OnSplashComplete(procedureOwner);
            }
        }

        private void OnSplashComplete(ProcedureOwner procedureOwner)
        {
            if (_splashGo)
            {
                _splashGo.SetActive(false);
                Object.Destroy(_splashGo);
                _splashGo = null;
            }

            ChangeState<ProcedureInitModule>(procedureOwner);
        }
#endif
    }
}
