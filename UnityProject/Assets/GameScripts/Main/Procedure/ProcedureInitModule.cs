using DG.Tweening;
using TEngine;
using YooAsset;
using ProcedureOwner = TEngine.IFsm<TEngine.IProcedureManager>;
using Language = TEngine.Language;

namespace GameMain
{
    /// <summary>
    /// 流程 => 初始化。
    /// </summary>
    public class ProcedureInitModule : ProcedureBase
    {
        public override bool UseNativeDialog => true;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            ModuleSystem.Init();

            DOTween.Init();

            //热更新UI初始化
            UILoadMgr.Initialize();

            // 语言配置：设置当前使用的语言，如果不设置，则默认使用操作系统语言
            InitLanguageSettings();

            // 声音配置：根据用户配置数据，设置即将使用的声音选项
            InitSoundSettings();

            UILoadMgr.Show(UIDefine.UILoadUpdate, $"初始化...");
            LoadUpdateLogic.Instance.DownProgressAction?.Invoke(10f);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!GameModule.SDK.IsInitSuccess())
            {
                return;
            }

            ChangeState<ProcedureInitPackage>(procedureOwner);
        }

        private void InitLanguageSettings()
        {
            if (GameModule.Resource.PlayMode == EPlayMode.EditorSimulateMode && GameModule.Base.EditorLanguage == TEngine.Language.Unspecified)
            {
                // 编辑器资源模式直接使用 Inspector 上设置的语言
                return;
            }
            
            TEngine.Language language = GameModule.Localization.Language;
            if (GameModule.Setting.HasSetting(Constant.Setting.Language))
            {
                try
                {
                    string languageString = GameModule.Setting.GetString(Constant.Setting.Language);
                    language = (TEngine.Language)System.Enum.Parse(typeof(TEngine.Language), languageString);
                }
                catch(System.Exception exception)
                {
                    TEngine.Log.Error("Init language error, reason {0}",exception.ToString());
                }
            }
            
            if (language != TEngine.Language.English
                && language != TEngine.Language.ChineseSimplified
                && language != TEngine.Language.ChineseTraditional)
            {
                // 若是暂不支持的语言，则使用英语
                language = TEngine.Language.English;
            
                GameModule.Setting.SetString(Constant.Setting.Language, language.ToString());
                GameModule.Setting.Save();
            }
            
            GameModule.Localization.Language = language;
            TEngine.Log.Info("Init language settings complete, current language is '{0}'.", language.ToString());
        }

        private void InitSoundSettings()
        {
            GameModule.Audio.MusicEnable = !GameModule.Setting.GetBool(Constant.Setting.MusicMuted, false);
            GameModule.Audio.MusicVolume = GameModule.Setting.GetFloat(Constant.Setting.MusicVolume, 1f);
            GameModule.Audio.SoundEnable = !GameModule.Setting.GetBool(Constant.Setting.SoundMuted, false);
            GameModule.Audio.SoundVolume = GameModule.Setting.GetFloat(Constant.Setting.SoundVolume, 1f);
            GameModule.Audio.UISoundEnable = !GameModule.Setting.GetBool(Constant.Setting.UISoundMuted, false);
            GameModule.Audio.UISoundVolume = GameModule.Setting.GetFloat(Constant.Setting.UISoundVolume, 1f);
            TEngine.Log.Info("Init sound settings complete.");
        }
    }
}
