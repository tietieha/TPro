// /*
//  * 风沙特效管理类
//  */
//
// using BehaviourEngine;
// using Crayon.Framework.Base;
// using Crayon.Model;
// using LargeMap.LargeMapCore;
// using Logic.Modules.LargeMap.Model.MapObject;
// using Model.Map;
// using UnityEngine;
// using UnityEngine.Scripting;
//
// namespace Logic.Modules.LargeMap.Weather
// {
//     [Preserve]
//     [ModelTag(ModelInitTimeGroupEnum.Free, "Heroes.LargeMapSceneState")]
//     public class WindMgr: BaseModel<WindMgr>, IMapLogicComponent
//     {
//         public const string WIND_DESSERT_RES_NAME = "BW_FX_Wind_Desert03_01";
//         public const string WIND_GRASS_RES_NAME = "BW_FX_Wind_Gress03_01";
//         public const string WIND_SNOW_RES_NAME = "BW_FX_Wind_Snow03_01";
//
//         private NBDict<string, GameObject> m_effectDict = new NBDict<string, GameObject>();
//         private GameObject m_effectGo;
//         private string m_curResName;
//
//         private Vector3 HIDE_POS = new Vector3(-1000f, -1000f, -1000f);
//
//         protected override void OnInit()
//         {
//             base.OnInit();
//             MapManager.Instance.RegLogicComponent(this);
//         }
//
//         protected override void OnReset()
//         {
//             base.OnReset();
//             ResetData();
//         }
//
//         public void Logic(bool moved, bool scaled)
//         {
//             if (scaled)
//             {
//                 if (!MapModeUtils.Is3DLayer())
//                 {
//                     foreach (var pair in m_effectDict)
//                     {
//                         pair.Value.SetActiveEfficiently(false);
//                     }
//                     return;
//                 }
//             }
//
//             if (moved || scaled)
//             {
//                 var centerPos = MapManager.Instance.CenterPos;
//                 var ptWorld = new Vector3(centerPos.x, 1.0f, centerPos.y);
//                 UpdateWindEffect(ptWorld);
//             }
//         }
//
//
//         /// <summary>
//         /// 判断当前屏幕里面的地貌
//         /// </summary>
//         /// <param name="ptWorld"></param>
//         private async void UpdateWindEffect(Vector3 ptWorld)
//         {
//             var landformType = StaticObjMgr.Instance.GetLandFormType(ptWorld);
//             string resName;
//             switch (landformType)
//             {
//                 case LandFormsType.LandFormDesert:
//                     resName = WIND_DESSERT_RES_NAME;
//                     break;
//                 case LandFormsType.LandFormSnowField:
//                     resName = WIND_SNOW_RES_NAME;
//                     break;
//                 default:
//                     resName = WIND_GRASS_RES_NAME;
//                     break;
//             }
//             m_curResName = resName;
//             if (m_effectDict.TryGetValue(resName, out var effectGo))
//             {
//                 if(MapModeUtils.Is3DLayer())
//                 {
//                     effectGo.SetActiveEfficiently(true);
//                     effectGo.transform.position = ptWorld;
//                 }
//                 else
//                 {
//                     effectGo.SetActiveEfficiently(false);
//                 }
//             }
//             else
//             {
//                 var go = await GameLoaderNode.Current.LoadRes<GameObject>(resName);
//                 if (m_effectDict.ContainsKey(resName))
//                 {
//                     GameLoaderNode.Current.RecoveryResInstance(resName, go);
//                 }
//                 else
//                 {
//                     go.transform.SetParent(null);
//                     go.SetActiveEfficiently(true);
//                     m_effectDict.Add(resName, go);
//                     if (m_curResName == resName)
//                     {
//                         if(MapModeUtils.Is3DLayer())
//                         {
//                             go.SetActiveEfficiently(true);
//                             go.transform.position = ptWorld;
//                         }
//                         else
//                         {
//                             go.SetActiveEfficiently(false);
//                         }
//                     }
//                     else
//                     {
//                         go.transform.position = HIDE_POS;
//                     }
//                 }
//             }
//         }
//
//         protected override void OnDispose()
//         {
//             base.OnDispose();
//             ResetData();
//         }
//
//         private void ResetData()
//         {
//             foreach (var pair in m_effectDict)
//             {
//                 GameLoaderNode.Current.RecoveryResInstance(pair.Key, pair.Value);
//             }
//             m_effectDict.Clear();
//             m_effectGo = null;
//             m_curResName = "";
//         }
//     }
// }