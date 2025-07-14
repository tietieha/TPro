// using BehaviourEngine;
// using BigWorld;
// using CEngine;
// using Crayon.Framework.Base;
// using Crayon.Model;
// using GameLog;
// using Heroes.Base;
// using HGame;
// using LargeMap.LargeMapCore;
// using Logic.Modules.LargeMap.Model.MapObject;
// using Logic.Modules.LargeMap.Model.MapSceneData;
// using Logic.Modules.LargeMap.Model.Shadow;
//
// using Model.Map;
// using Model.Map.Config;
// using UnityEngine;
// using UnityEngine.HWeather;
// using UnityEngine.Scripting;
//
// namespace Logic.Modules.LargeMap.Weather
// {
//     [Preserve]
//     [ModelTag(ModelInitTimeGroupEnum.Free, "Heroes.LargeMapSceneState", "Heroes.CityMapSceneState", "Heroes.PreludeMapSceneState", "Heroes.ArenaMapSceneState")]
//     public class WeatherMgr : BaseModel<WeatherMgr>
//     {
//         private GameObject m_weatherRoot;
//         private GameObject m_weather;
//         private BigWorldWeatherInfo m_weatherInfo;
//         private HWeatherEffectsController m_effectCtrl;
//         private HWeatherTimeController m_timeCtrl;
//         private HWeatherController m_weatherCtrl;
//         private LandFormsType m_landTypeLargerMap = LandFormsType.LandFormNull; //大地图的多地貌
//         private ELandType m_landTypeCity = ELandType.EAll; //城郊的多地貌
//         private WeatherConfig.Types.E_WEATHER_TYPE m_curWeatherType = WeatherConfig.Types.E_WEATHER_TYPE.ESunny;
//         private WeatherConfig.Types.E_WEATHER_STRENGTH m_curWeatherStrength = WeatherConfig.Types.E_WEATHER_STRENGTH.EBig;
//         private uint m_specialWeatherDuration;
//         private bool m_isOpen = true;
//         private bool m_isOpenServer = true;
//         private bool m_changeLandType = false;
//         private float m_daylyTimeline = 12f;
//         private readonly float MinuteSec = 60f;
//         private readonly float DayLong = 24f;
//         private HConfig_DayAndNightConfig Config;
//         private DayAndNightConstantConfig m_config => Config?.value;
//         public EWeatherType[] weatherTypes = new EWeatherType[(int) ELandType.EMax];
//         /// 刷新小地图主城数据的定时器
//         private readonly uint TimerID = GameTimer.Instance.CreateTimerID();
//
//         /// <summary>
//         /// 记录上次加载场景天气资源名称
//         /// 用来判断下次是否存在差异
//         /// </summary>
//         private string m_lastSceneManager;
//         // 场景灯光等参数设置
//         public GameObject sceneManagerRoot;
//         // private HWeatherController m_weatherController;
//         private PVEFogOfWar m_pveFogOfWar;
//
//         public PVEFogOfWar PVEFogOfWar => m_pveFogOfWar;
//
//         protected override void OnInit()
//         {
//             base.OnInit();
//             var cfg = DayAndNightConfigLoader.Instance.Config;
//             if (Config == null)
//             {
//                 Config = cfg;
//             }
//         }
//
//
//         protected override void InitListener()
//         {
//             base.InitListener();
//             RegisterEvent((int) GameEventDefine.LandFormsChange, OnLandFormsChange);
//             RegisterEvent((int) GameEventDefine.UpdateGuideStatus, OnChangeChapterEvent);
//             RegisterEvent((int) GameEventDefine.GetPlayerInfoEvent, OnChangeChapterEvent);
//             RegisterEvent((int) GameEventDefine.ChangeMyCityLevel, OnChangeMyCityLevel);
//             RegisterEvent((int)GameEventDefine.HotFixConfigRefreshNotify, OnHotFixConfigRefreshNotify);
//         }
//
//         protected override void OnReset()
//         {
//             ResetData();
//         }
//
//         public override void Dispose()
//         {
//             base.Dispose();
//             Config = null;
//         }
//
//         // /// <summary>
//         // /// 目前和OnInit没有区别
//         // /// </summary>
//         // protected override void OnPostSceneChangeInit()
//         // {
//         //     InitData();
//         // }
//
//         /// <summary>
//         /// 目前和OnReset没有区别
//         /// </summary>
//         protected override void OnSceneChangeReset()
//         {
//             ResetData();
//         }
//
//         public bool IsOpen
//         {
//             get => m_isOpen;
//             set
//             {
//                 if (value != m_isOpen)
//                 {
//                     m_isOpen = value;
//                     HWeatherCommon.s_openDayTime = value;
//                     HWeatherCommon.s_openWeather = value;
//                     if (!value)
//                     {
//                         if (m_effectCtrl != null)
//                         {
//                             m_effectCtrl.StopAllEffect();
//                         }
//
//                         if (m_timeCtrl != null)
//                         {
//                             m_timeCtrl.timeline = m_daylyTimeline;
//                         }
//                     }
//                     else
//                     {
//                         InitCfg();
//                         SetLandFormsWeather();
//                     }
//                 }
//             }
//         }
//
//         public bool IsOpenServer
//         {
//             get => m_isOpenServer;
//             set => m_isOpenServer = value;
//         }
//
//         public WeatherConfig.Types.E_WEATHER_TYPE CurWeatherType
//         {
//             get => m_curWeatherType;
//             set => m_curWeatherType = value;
//         }
//
//         public WeatherConfig.Types.E_WEATHER_STRENGTH CurWeatherStrength
//         {
//             get => m_curWeatherStrength;
//             set => m_curWeatherStrength = value;
//         }
//
//         public async void RefreshSceneManager()
//         {
//             var mapSceneConfig = MapManager.Instance.mapSceneConfig;
//             var curSceneManagerName = GetSceneManagerName(mapSceneConfig);
//             if (curSceneManagerName.IsNullOrEmpty() || m_lastSceneManager == curSceneManagerName)
//             {
//                 return;
//             }
//
//             m_lastSceneManager = curSceneManagerName;
//             var SceneManagerRootName = "SceneManagerRoot";
//             var root = GameObject.Find(SceneManagerRootName);
//             if (root == null)
//             {
//                 root = new GameObject(SceneManagerRootName);
//             }
//
//             for (int i = root.transform.childCount - 1; i >= 0; i--)
//             {
//                 var child = root.transform.GetChild(i);
//                 if (child != null)
//                 {
//                     Object.Destroy(child.gameObject);
//                 }
//             }
//             sceneManagerRoot = await GameLoaderNode.Current.LoadRes<GameObject>(curSceneManagerName);
//             if (sceneManagerRoot == null)
//             {
//                 Log.Error(LogModules.MAPMGR, "MapManager InitSceneManager load res {0} failed", curSceneManagerName);
//
//                 return;
//             }
//             sceneManagerRoot.AddToParent(root);
//
//             // 城郊和PVE迷雾
//             m_pveFogOfWar = sceneManagerRoot.transform.GetComponentInChildren<PVEFogOfWar>();
//             GameEventPool.Instance.Dispatch((int)GameEventDefine.InitCityMist, new GameEventArgs()
//             {
//                 ArgObj = m_pveFogOfWar
//             });
//
//             if (HMap.MapType == MapSceneType.CityMap || HMap.MapType == MapSceneType.LargeMap || HMap.MapType == MapSceneType.ArenaMap)
//             {
//                 Instance.InitCom(sceneManagerRoot);
//             }
//             else
//             {
//                 HWeatherEffectsController.IsAutoPlay = true;
//             }
//
//             // 云 风沙
//             if (HMap.MapType == MapSceneType.LargeMap)
//             {
//                 CloudMgr.Instance.AddCloud("BW_FX_Sky_Cloud01_high100", 0,
//                     140, 170, 170, 200,
//                     new Vector3(200, 200, 50), new Vector3(300, 300, 50));
//                 CloudMgr.Instance.AddCloud("BW_FX_Sky_Cloud01_high100_500", 0,
//                     140,  260, 440, 560,
//                     new Vector3(500, 500, 400), new Vector3(800, 800, 400));
//
//                 WindMgr.Instance.Init();
//             }
//
//             /*// 距离雾，城郊主城和非主城区域过渡特殊处理
//             if (HMap.MapType == MapSceneType.CityMap && weatherController != null)
//             {
//                 m_weatherController = weatherController;
//                 m_fogLineStartPve = m_weatherController.settings.g_FogLineStart;
//                 m_fogLineEndPve = m_weatherController.settings.g_FogLineEnd;
//             }*/
//
//             // TODO 后处理
//             var volumeProfileName = mapSceneConfig.volumeProfileName;
//             if (!volumeProfileName.IsNullOrEmpty())
//             {
//                 var volumeProfile = await GameLoaderNode.Current.LoadRes<GameObject>(volumeProfileName);
//                 if (volumeProfile != null)
//                 {
//                     volumeProfile.AddToParent(root);
//                     PostProcessManager.Instance.SetActiveVolumeProfile(volumeProfile);
//                 }
//             }
//         }
//
//         /// <summary>
//         /// 获取当前的天气资源名称
//         /// </summary>
//         /// <returns></returns>
//         private string GetSceneManagerName(MapSceneConfig mapSceneConfig)
//         {
//             var raceIndex = (int) HMap.raceType - 1;
//             // var mapSceneConfig = MapManager.Instance.mapSceneConfig;
//             var len = mapSceneConfig.sceneManagerName.Length;
//
//             if (mapSceneConfig.bSceneManagerNameByRace)
//             {
//                 if (raceIndex >= 0 && raceIndex < len)
//                 {
//                     // 城郊天气系统和种族绑定
//                     return mapSceneConfig.sceneManagerName[raceIndex];
//                 }
//             }
//
//             if (len > 0)
//             {
//                 // 大地图天气系统唯一
//                 return mapSceneConfig.sceneManagerName[0];
//             }
//
//             return "";
//         }
//
//         public void InitCom(GameObject obj)
//         {
//             HWeatherEffectsController.IsAutoPlay = false;
//             m_changeLandType = false;
//             m_weatherRoot = obj;
//             /*if (HMap.MapType == MapSceneType.CityMap)
//             {
//                 InitRaceWeather();
//             }*/
//
//             m_weather = m_weatherRoot.FindFirstChildWithPartName("Hweather");
//             if (m_weather != null)
//             {
//                 m_timeCtrl = m_weather.GetComponent<HWeatherTimeController>();
//                 m_weatherCtrl = m_weather.GetComponent<HWeatherController>();
//                 m_effectCtrl = m_weather.GetComponent<HWeatherEffectsController>();
//                 m_weatherCtrl.ChangeLandType = ChangeLandType;
//                 m_landTypeCity = m_weatherCtrl.landType;
//                 if (!HasSpecialWeather())
//                 {
//                     m_effectCtrl.StopAllEffect();
//                 }
//
//                 m_timeCtrl.DayTimeFinish = DayTimeFinish;
//                 m_timeCtrl.DayTypeChange = DayTypeChange;
//                 if (!m_changeLandType && m_landTypeCity != ELandType.EAll && m_weatherInfo != null)
//                 {
//                     ChangeLandType(m_landTypeCity);
//                 }
//
//                 var sceneCamera = CameraManager.Instance.SceneMainCamera;
//                 if (sceneCamera != null)
//                 {
//                     var transform = sceneCamera.transform;
//                     m_weatherCtrl.weatherZoneTrigger = transform;
//                     m_effectCtrl.followTarget = transform;
//                 }
//             }
//             else
//             {
//                 Log.Warn(LogModules.WEATHER, "InitData m_weather is Null, please recheck the code");
//             }
//
//
//             SwitchWeather();
//
//             InitCfg();
//             DayTypeChange(true);
//
//             if (HMap.MapType == MapSceneType.LargeMap)
//             {
//                 PlayWeatherLargeMap();
//             }
//         }
//
//         public GameObject DirectionalLightGO()
//         {
//             if (m_weatherCtrl != null)
//             {
//                 return m_weatherCtrl.directionalLight.gameObject;
//             }
//
//             return null;
//         }
//
//         private void SwitchWeather()
//         {
//             if (HMap.MapType == MapSceneType.InstanceMap || HMap.MapType == MapSceneType.PreludeMap || HMap.MapType == MapSceneType.ArenaMap)
//             {
//                 IsOpen = false;
//             }
//             else
//             {
//                 if (HMap.MapType == MapSceneType.CityMap)
//                 {
//                     var mineCity = MapSceneDataMgr.Instance.MineCity;
//                     if (mineCity != null)
//                     {
//                         IsOpen = mineCity.Payload?.PlayerCity?.Level >= ConstConfigLoader.Instance.GlobalConfig.value.OpenWeatherLevel;
//                     }
//                     else
//                     {
//                         IsOpen = false;
//                     }
//                 }
//                 else
//                 {
//                     IsOpen = true;
//                 }
//             }
//         }
//
//         /// <summary>
//         /// 计算过渡时间
//         /// </summary>
//         /// <returns></returns>
//         private float CalculateTransitionTime()
//         {
//             var MaxTime = 0.25f;
//             var MinTime = 0.017f;
//             var time = 10f; //过渡时间 10秒
//             var transitionTime = DayLong * time / m_config.PeriodSecond / 2;
//             if (transitionTime > MaxTime)
//             {
//                 transitionTime = MaxTime;
//             }
//             else if (transitionTime < MinTime)
//             {
//                 transitionTime = MinTime;
//             }
//
//             return transitionTime;
//         }
//
//         /// <summary>
//         /// 初始化时间配置
//         /// </summary>
//         private void InitCfg()
//         {
//             float timeline;
//             if (m_timeCtrl != null && m_config != null)
//             {
//                 var transition = CalculateTransitionTime();
//                 m_timeCtrl.dayLength = m_config.PeriodSecond / MinuteSec;
//                 var keys = m_timeCtrl.keys;
//                 //如果配置了清晨和黄昏
//                 if (m_config.MorningSecond != 0 && m_config.DuskSecond != 0)
//                 {
//                     //关键字数量必须是10，否则会出问题。
//                     if (keys.Length == 10)
//                     {
//                         keys[0].time = 0;
//                         var halfNight = DayLong * m_config.NightSecond / m_config.PeriodSecond / 2;
//                         keys[1].time = halfNight - transition;
//                         keys[2].time = halfNight + transition;
//                         var morning = DayLong * m_config.MorningSecond / m_config.PeriodSecond;
//                         keys[3].time = keys[2].time + morning - 2 * transition;
//                         keys[4].time = keys[2].time + morning;
//                         var daytime = DayLong * m_config.DailySecond / m_config.PeriodSecond;
//                         m_daylyTimeline = keys[4].time + transition; //使用白天时间为默认timeline
//                         keys[5].time = keys[4].time + daytime - 2 * transition;
//                         keys[6].time = keys[4].time + daytime;
//                         var dusk = DayLong * m_config.DuskSecond / m_config.PeriodSecond;
//                         keys[7].time = keys[6].time + dusk - 2 * transition;
//                         keys[8].time = keys[6].time + dusk;
//                         keys[9].time = DayLong;
//                         m_timeCtrl.keys = keys;
//                     }
//                     else
//                     {
//                         Log.Error(LogModules.WEATHER, "HWeatherTimeController AnimationCurve length not equal 10, please recheck the curve");
//                     }
//                 }
//                 else
//                 {
//                     //如果保留10个节点,则删掉4个 清晨和白天相关的节点节点
//                     if (keys.Length == 10)
//                     {
//                         var newKeys = new Keyframe[keys.Length - 4];
//                         newKeys[0].time = 0;
//                         newKeys[0].value = keys[0].value;
//                         var halfNight = DayLong * m_config.NightSecond / m_config.PeriodSecond / 2;
//                         newKeys[1].time = halfNight - transition;
//                         newKeys[1].value = keys[1].value;
//
//                         newKeys[2].time = halfNight + transition;
//                         newKeys[2].value = keys[4].value;
//                         var daytime = DayLong * m_config.DailySecond / m_config.PeriodSecond;
//                         m_daylyTimeline = newKeys[2].time + transition; //使用白天时间为默认timeline
//                         newKeys[3].time = newKeys[2].time + daytime - 2 * transition;
//                         newKeys[3].value = keys[5].value;
//                         newKeys[4].time = newKeys[2].time + daytime;
//                         newKeys[4].value = keys[8].value;
//                         newKeys[5].time = DayLong;
//                         newKeys[5].value = keys[9].value;
//                         m_timeCtrl.keys = newKeys;
//                     }
//                     // 如果美术配置已经删掉了4个无效节点
//                     else if (keys.Length == 6)
//                     {
//                         keys[0].time = 0;
//                         var halfNight = DayLong * m_config.NightSecond / m_config.PeriodSecond / 2;
//                         keys[1].time = halfNight - transition;
//                         keys[2].time = halfNight + transition;
//                         var daytime = DayLong * m_config.DailySecond / m_config.PeriodSecond;
//                         m_daylyTimeline = keys[2].time + transition; //使用白天时间为默认timeline
//                         keys[3].time = keys[2].time + daytime - 2 * transition;
//                         keys[4].time = keys[2].time + daytime;
//                         keys[5].time = DayLong;
//                         m_timeCtrl.keys = keys;
//                     }
//                 }
//
//
//                 if (HWeatherCommon.s_openDayTime)
//                 {
//                     var curtime = TimeHelper.ServerTimestamp();
//                     timeline = DayLong * (curtime % m_config.PeriodSecond) / m_config.PeriodSecond;
//                 }
//                 else
//                 {
//                     timeline = m_daylyTimeline;
//                 }
//
//                 Log.Debug(LogModules.WEATHER, "WeatherMgr InitCfg {0} s_openDayTime {1}", timeline, HWeatherCommon.s_openDayTime);
//                 m_timeCtrl.timeline = timeline;
//
//                 // 关闭天气系统，并把时间定在14点
//                 IsOpen = false;
//                 m_timeCtrl.timeline = 14;
//
//             }
//         }
//
//
//         private void ResetData()
//         {
//             m_effectCtrl = null;
//             if (m_timeCtrl != null)
//             {
//                 m_timeCtrl.DayTimeFinish = null;
//                 m_timeCtrl = null;
//             }
//             if (m_weatherCtrl != null)
//             {
//                 m_weatherCtrl.ChangeLandType = null;
//                 m_weatherCtrl = null;
//             }
//             m_weatherRoot = null;
//             m_weather = null;
//
//             DestroySceneManager();
//         }
//
//         public void DestroySceneManager()
//         {
//             if (sceneManagerRoot == null)
//             {
//                 return;
//             }
//
//             Object.Destroy(sceneManagerRoot);
//             // m_weatherController = null;
//             m_pveFogOfWar = null;
//         }
//
//         public void SetDayLength(float len)
//         {
//             if (m_timeCtrl != null)
//             {
//                 m_timeCtrl.dayLength = len;
//             }
//         }
//
//         /// <summary>
//         /// 设置天气数据
//         /// </summary>
//         /// <param name="info"></param>
//         public void SetWeatherInfo(BigWorldWeatherInfo info)
//         {
//             if (!IsOpenServer && m_weatherInfo != null)
//             {
//                 return;
//             }
//
//             m_weatherInfo = info;
//             if (m_landTypeLargerMap != LandFormsType.LandFormNull)
//             {
//                 if (HMap.MapType == MapSceneType.LargeMap)
//                 {
//                     PlayWeatherLargeMap();
//                 }
//             }
//
//             if (!m_changeLandType && m_landTypeCity != ELandType.EAll)
//             {
//                 ChangeLandType(m_landTypeCity);
//             }
//         }
//
//         //一天结束后得重新更新配置，有可能配置刷新了。
//         private void DayTimeFinish()
//         {
//             var cfg = DayAndNightConfigLoader.Instance.Config;
//             if (m_config == null || !cfg.Equals(m_config))
//             {
//                 Config = cfg;
//                 InitCfg();
//             }
//         }
//
//         //一天结束后得重新更新配置，有可能配置刷新了。
//         private void DayTypeChange(bool isBeginChange)
//         {
//             // 序章昼夜由timeline控制
//             if (HMap.MapType == MapSceneType.PreludeMap)
//             {
//                 return;
//             }
//
//             DispatchEvent((int) GameEventDefine.DayTypeChange, new GameEventArgs() {ArgBool = isBeginChange});
//         }
//
//         //插件回调的地貌改变
//         private void ChangeLandType(ELandType type)
//         {
//             if (!m_isOpen)
//             {
//                 return;
//             }
//
//             if (HMap.MapType != MapSceneType.CityMap)
//             {
//                 return;
//             }
//
//             m_landTypeCity = type;
//             //全局天气时特殊处理
//             if (type == ELandType.EAll)
//             {
//                 m_specialWeatherDuration = 0;
//                 m_curWeatherType = WeatherConfig.Types.E_WEATHER_TYPE.ESunny;
//                 PlayEffect((EWeatherType) m_curWeatherType, (EWeatherStrength) m_curWeatherStrength);
//                 m_effectCtrl?.StopAllEffect();
//
//                 return;
//             }
//
//             var typeSvr = GetWeatherType((WeatherConfig.Types.E_SURFACE_TYPE) type);
//             //var typeSvr = (WeatherConfig.Types.E_WEATHER_TYPE)weatherTypes[(int) type];
//
//             if (m_weatherCtrl == null)
//             {
//                 return;
//             }
//
//             if (m_isOpenServer)
//             {
//                 if (m_weatherInfo == null)
//                 {
//                     return;
//                 }
//
//                 var typeClt = (WeatherConfig.Types.E_WEATHER_TYPE) m_weatherCtrl.weatherTypes[(int) type];
//                 m_changeLandType = true;
//                 m_curWeatherType = typeSvr;
//                 if (type != ELandType.EMainCity)
//                 {
//                     if (typeSvr != typeClt && typeSvr == WeatherConfig.Types.E_WEATHER_TYPE.ESunny)
//                     {
//                         m_curWeatherType = typeSvr;
//                         //todo 这里设置false可能有问题。
//                         m_weatherCtrl.EnableWeatherZone(type, (EWeatherType) typeClt, false);
//                     }
//                     else
//                     {
//                         m_curWeatherType = typeClt;
//                         m_weatherCtrl.EnableWeatherZone(type, (EWeatherType) typeClt, true);
//                     }
//                 }
//                 else
//                 {
//                     if (typeSvr == WeatherConfig.Types.E_WEATHER_TYPE.ESunny)
//                     {
//                         m_curWeatherType = typeSvr;
//                         m_weatherCtrl.EnableWeatherZone(type, EWeatherType.ERain, false);
//                         m_weatherCtrl.EnableWeatherZone(type, EWeatherType.ESunny, true);
//                     }
//                     else
//                     {
//                         //这里只能是rain了，不管后台传什么都只是rain。
//                         m_curWeatherType = WeatherConfig.Types.E_WEATHER_TYPE.ERain;
//                         m_weatherCtrl.EnableWeatherZone(type, EWeatherType.ERain, true);
//                         m_weatherCtrl.EnableWeatherZone(type, EWeatherType.ESunny, false);
//                     }
//                 }
//
//                 m_curWeatherStrength = GetCurStrength();
//             }
//             else
//             {
//                 m_specialWeatherDuration = 0;
//                 var typeClt = weatherTypes[(int) type];
//                 //主城需要把另一个天气关掉
//                 if (type == ELandType.EMainCity)
//                 {
//                     if (typeClt == EWeatherType.ESunny)
//                     {
//                         m_weatherCtrl.EnableWeatherZone(type, EWeatherType.ERain, false);
//                         m_weatherCtrl.EnableWeatherZone(type, EWeatherType.ESunny, true);
//                     }
//                     else
//                     {
//                         m_weatherCtrl.EnableWeatherZone(type, EWeatherType.ESunny, false);
//                         m_weatherCtrl.EnableWeatherZone(type, EWeatherType.ERain, true);
//                     }
//                 }
//                 else
//                 {
//                     m_weatherCtrl.EnableWeatherZone(type, typeClt, true);
//                 }
//
//                 m_curWeatherType = (WeatherConfig.Types.E_WEATHER_TYPE) typeClt;
//             }
//
//             PlayEffect((EWeatherType) m_curWeatherType, (EWeatherStrength) m_curWeatherStrength);
//         }
//
//
//         /// <summary>
//         /// 获得当前的天气强度
//         /// </summary>
//         /// <returns></returns>
//         private WeatherConfig.Types.E_WEATHER_STRENGTH GetCurStrength()
//         {
//             if (m_weatherInfo != null)
//             {
//                 return m_weatherInfo.WeatherStrength;
//             }
//
//             return WeatherConfig.Types.E_WEATHER_STRENGTH.ESmall;
//         }
//
//
//         /// <summary>
//         /// 获得对应地貌的天气效果
//         /// </summary>
//         /// <param name="type"></param>
//         /// <returns></returns>
//         private WeatherConfig.Types.E_WEATHER_TYPE GetWeatherType(WeatherConfig.Types.E_SURFACE_TYPE type)
//         {
//             if (m_weatherInfo != null)
//             {
//                 var index = (int) type;
//                 var arr = m_weatherInfo.WeatherCombinations;
//                 if (index < arr.Count)
//                 {
//                     var item = arr[index];
//                     var curtime = TimeHelper.ServerTimestamp();
//                     //if(true)
//                     if (curtime >= m_weatherInfo.WeatherBeginTime &&
//                         curtime < m_weatherInfo.WeatherBeginTime + m_weatherInfo.WeatherDuration)
//                     {
//                         m_specialWeatherDuration = m_weatherInfo.WeatherBeginTime + m_weatherInfo.WeatherDuration - curtime;
//
//                         return item.WeatherType;
//                     }
//                     else
//                     {
//                         m_specialWeatherDuration = 0;
//
//                         return item.DailyWeather;
//                     }
//                 }
//                 else
//                 {
//                     Log.Error(LogModules.WEATHER, $"GetWeatherType index {index} >= arr.Count {arr.Count}");
//                 }
//             }
//
//             return WeatherConfig.Types.E_WEATHER_TYPE.ESunny;
//         }
//
//
//         /// <summary>
//         ///  当前场景是否需要控制天气系统
//         /// </summary>
//         /// <returns></returns>
//         private bool NeedCtrlWeather()
//         {
//             return HMap.MapType == MapSceneType.LargeMap || HMap.MapType == MapSceneType.CityMap;
//         }
//
//
//         /// <summary>
//         /// 当前有没特殊天气
//         /// </summary>
//         /// <returns></returns>
//         private bool HasSpecialWeather()
//         {
//             if (!m_isOpen)
//             {
//                 return false;
//             }
//
//             if (m_isOpenServer)
//             {
//                 if (m_weatherInfo != null)
//                 {
//                     return m_weatherInfo.WeatherDuration > 0;
//                 }
//             }
//             else
//             {
//                 for (int j = 0; j < weatherTypes.Length; j++)
//                 {
//                     var weather = weatherTypes[j];
//                     if (weather != EWeatherType.ENull && weather != EWeatherType.ESunny)
//                     {
//                         return true;
//                     }
//                 }
//             }
//
//             return false;
//         }
//
//
//         /// <summary>
//         /// 不同枚举的地貌装换
//         /// </summary>
//         /// <param name="type"></param>
//         /// <returns></returns>
//         private ELandType TranslateLandType(LandFormsType type)
//         {
//             switch (type)
//             {
//                 case LandFormsType.LandFormDesert:
//                     return ELandType.EWasteland;
//                 case LandFormsType.LandFormField:
//                     return ELandType.EGrassland;
//                 case LandFormsType.LandFormSnowField:
//                     return ELandType.ESnowland;
//                 default:
//                     return ELandType.EMainCity;
//             }
//         }
//
//
//         /// <summary>
//         /// 播放大地图的天气效果
//         /// </summary>
//         private void PlayWeatherLargeMap()
//         {
//             if (!m_isOpen)
//             {
//                 return;
//             }
//
//             if (m_isOpenServer)
//             {
//                 var landType = WeatherConfig.Types.E_SURFACE_TYPE.EGrassland;
//                 switch (m_landTypeLargerMap)
//                 {
//                     case LandFormsType.LandFormDesert:
//                         m_curWeatherType = GetWeatherType(WeatherConfig.Types.E_SURFACE_TYPE.EWasteland);
//                         break;
//                     case LandFormsType.LandFormField:
//                         m_curWeatherType = GetWeatherType(WeatherConfig.Types.E_SURFACE_TYPE.EGrassland);
//                         break;
//                     case LandFormsType.LandFormSnowField:
//                         m_curWeatherType = GetWeatherType(WeatherConfig.Types.E_SURFACE_TYPE.ESnowland);
//                         break;
//                     default:
//                         m_curWeatherType = WeatherConfig.Types.E_WEATHER_TYPE.ESunny;
//                         break;
//                 }
//
//                 m_curWeatherStrength = GetCurStrength();
//
//                 PlayEffect((EWeatherType) m_curWeatherType, (EWeatherStrength) m_curWeatherStrength);
//             }
//             else
//             {
//                 m_specialWeatherDuration = 0;
//                 var type = TranslateLandType(m_landTypeLargerMap);
//                 m_curWeatherType = (WeatherConfig.Types.E_WEATHER_TYPE) weatherTypes[(int) type];
//                 PlayEffect((EWeatherType) m_curWeatherType, (EWeatherStrength) m_curWeatherStrength);
//             }
//         }
//
//         /// <summary>
//         /// 设置地貌对应的天气
//         /// </summary>
//         public void SetLandFormsWeather()
//         {
//             //只处理大地图
//             if (HMap.MapType == MapSceneType.LargeMap)
//             {
//                 PlayWeatherLargeMap();
//             }
//             else if (HMap.MapType == MapSceneType.CityMap)
//             {
//                 ChangeLandType(m_landTypeCity);
//             }
//         }
//
//
//         public DayType GetDayType()
//         {
//             var ret = DayType.Daily;
//             if (m_timeCtrl == null)
//             {
//                 return ret;
//             }
//
//             var timeCtrlKeys = m_timeCtrl.keys;
//             var timeline = m_timeCtrl.timeline;
//             var keyIndex = 0;
//             for (var i = timeCtrlKeys.Length - 1; i >= 0; i--)
//             {
//                 if (timeline >= timeCtrlKeys[i].time)
//                 {
//                     keyIndex = i;
//                     break;
//                 }
//             }
//
//             if (timeCtrlKeys.Length == 10)
//             {
//                 switch (keyIndex)
//                 {
//                     case 1:
//                     case 2:
//                         ret = DayType.Morning;
//                         break;
//                     case 3:
//                     case 4:
//                         ret = DayType.Daily;
//                         break;
//                     case 5:
//                     case 6:
//                         ret = DayType.Dusk;
//                         break;
//                     case 7:
//                     case 8:
//                     case 9:
//                     case 0:
//                         ret = DayType.Night;
//                         break;
//                 }
//             }
//             else if (timeCtrlKeys.Length == 6)
//             {
//                 switch (keyIndex)
//                 {
//                     case 1:
//                     case 2:
//                         ret = DayType.Daily;
//                         break;
//                     case 3:
//                     case 4:
//                     case 5:
//                     case 0:
//                         ret = DayType.Night;
//                         break;
//                 }
//             }
//
//             return ret;
//         }
//
//         /// <summary>
//         /// 播放天气特效
//         /// </summary>
//         /// <param name="type"></param>
//         /// <param name="strength"></param>
//         private void PlayEffect(EWeatherType type, EWeatherStrength strength)
//         {
//             if (!m_isOpen)
//             {
//                 return;
//             }
//
//             if (m_effectCtrl != null && m_weatherCtrl != null && NeedCtrlWeather())
//             {
//                 if (m_effectCtrl.PlayEffect(type, strength))
//                 {
//                     GameTimer.Instance.UnRegister(this, TimerID);
//                     if (m_specialWeatherDuration > 0)
//                     {
//                         GameTimer.Instance.Register(this, TimerID, m_specialWeatherDuration * 1000, 1,
//                             (count, millisecond) => { SetLandFormsWeather(); });
//                         //大地图才需要控制全局天气
//                         if (HMap.MapType == MapSceneType.LargeMap)
//                         {
//                             var index = m_weatherCtrl.GetNewWeatherProfile(type);
//                             if (index < 0)
//                             {
//                                 index = (int) type;
//                             }
//
//                             m_weatherCtrl.SetNewWeatherProfile(index);
//                         }
//                     }
//                 }
//             }
//         }
//
//
//         private void OnLandFormsChange(Event<int, GameEventArgs> e)
//         {
//             if (HMap.MapType != MapSceneType.LargeMap)
//             {
//                 return;
//             }
//
//             m_landTypeLargerMap = (LandFormsType) e.body.ArgInt;
//             SetLandFormsWeather();
//         }
//
//         /// <summary>
//         /// 根据新手序章来决定是否开启天气系统
//         /// </summary>
//         /// <param name="e"></param>
//         private void OnChangeChapterEvent(Event<int, GameEventArgs> e)
//         {
//             SwitchWeather();
//         }
//
//         private void OnChangeMyCityLevel(Event<int, GameEventArgs> e)
//         {
//             SwitchWeather();
//         }
//
//         private void OnHotFixConfigRefreshNotify(Event<int, GameEventArgs> evt)
//         {
//             if (evt?.body != null &&
//                 evt.body.ArgObj is string configName &&
//                 configName == DayAndNightConfigLoader.Instance.ConfigName)
//             {
//                 var cfg = DayAndNightConfigLoader.Instance.Config;
//                 if (m_config == null || !cfg.Equals(m_config))
//                 {
//                     Config = cfg;
//                     InitCfg();
//                 }
//             }
//         }
//
//
//         /// <summary>
//         /// 展示场景灯光
//         /// </summary>
//         public void SetSceneManagerRootActive(bool active)
//         {
//             if (sceneManagerRoot != null)
//             {
//                 sceneManagerRoot.SetActive(active);
//                 //切换到其他场景时，地图中灯光被关闭了，此时阴影清除是不会生效的，回来会出现残留
//                 //故而当灯光重新激活时，将阴影重新绘制一遍
//                 ShadowModel.Instance.SetDrawOnceShadow(true);
//             }
//         }
//     }
//
//     public enum DayType
//     {
//         Morning,
//         Daily,
//         Dusk,
//         Night
//     }
// }
