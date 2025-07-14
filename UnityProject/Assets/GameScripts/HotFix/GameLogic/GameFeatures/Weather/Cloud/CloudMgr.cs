// using Crayon.Model;
// using LargeMap.LargeMapCore;
// using Model.Map;
// using UnityEngine;
// using UnityEngine.Scripting;
//
// namespace Logic.Modules.LargeMap.Weather
// {
//     // 云的管理器
//     [Preserve]
//     [ModelTag(ModelInitTimeGroupEnum.Free, "Heroes.LargeMapSceneState")]
//     // [ModelTag(ModelInitTimeGroupEnum.Free)]
//     public class CloudMgr : BaseModel<CloudMgr>, IMapLogicComponent
//     {
//         private readonly NBList<Cloud> m_listCloud = new NBList<Cloud>();
//
//         protected override void OnInit()
//         {
//             base.OnInit();
//             MapManager.Instance.RegLogicComponent(this);
//         }
//
//         protected override void OnReset()
//         {
//             // base.OnReset();
//             Release();
//         }
//
//         protected override void OnDispose()
//         {
//             base.OnDispose();
//             Release();
//         }
//
//         private void Release()
//         {
//             if (m_listCloud != null)
//             {
//                 foreach (var cloud in m_listCloud)
//                 {
//                     cloud.Dispose();
//                 }
//             }
//
//             m_listCloud.Clear();
//         }
//
//         public void Logic(bool moved, bool scaled)
//         {
//             var posCamera = SceneMainCamera.Instance.CameraTrans.position;
//             foreach (var cloud in m_listCloud)
//             {
//                 cloud.Update(scaled, posCamera);
//             }
//         }
//
//         public void AddCloud(string strRes, float posY, float startShowHeight, float endShowHeight, float startHideHeight, float endHideHeight, Vector3 startScale, Vector3 endScale)
//         {
//             var cloud = new Cloud();
//             cloud.InitCloud(strRes, posY, startShowHeight, endShowHeight, startHideHeight, endHideHeight, startScale, endScale);
//             m_listCloud.Add(cloud);
//         }
//     }
// }
