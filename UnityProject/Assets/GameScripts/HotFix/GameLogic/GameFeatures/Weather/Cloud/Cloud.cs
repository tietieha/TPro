//
// using CEngine;
// using Crayon.Framework.Base;
// using LODControl;
// using UnityEngine;
// using UnityEngine.Scripting;
//
// namespace Logic.Modules.LargeMap.Weather
// {
//     // 云
//     [Preserve]
//     public class Cloud : Node
//     {
//         private static readonly int Opacity = Shader.PropertyToID("_Opacity");
//         private readonly float MaxOpacity = 0.5f;
//         private float m_posY;
//         private float m_startShowHeight;
//         private float m_endShowHeight;
//         private float m_startHideHeight;
//         private float m_endHideHeight;
//         private Vector3 m_startScale;
//         private Vector3 m_endScale;
//         private Transform m_trans;
//         private NBList<Material> m_listMat;
//         private ParticleSystem[] m_listParticle;
//
//         public async void InitCloud(string strRes, float posY, float startShowHeight, float endShowHeight, float startHideHeight, float endHideHeight, Vector3 startScale, Vector3 endScale)
//         {
//             m_posY = posY;
//             m_startShowHeight = startShowHeight;
//             m_endShowHeight = endShowHeight;
//             m_startHideHeight = startHideHeight;
//             m_endHideHeight = endHideHeight;
//             m_startScale = startScale;
//             m_endScale = endScale;
//
//             var go = await LoadRes<GameObject>(strRes);
//             if (go != null)
//             {
//                 m_trans = go.transform;
//                 m_trans.AddToParent(RootManager.Instance.GetRoot(RootType.LayerStatic));
//                 m_listParticle = m_trans.GetComponentsInChildren<ParticleSystem>();
//
//                 m_listMat = new NBList<Material>();
//                 if (m_listParticle != null)
//                 {
//                     foreach (var particle in m_listParticle)
//                     {
//                         var renderer = particle.GetComponent<Renderer>();
//                         if (renderer != null)
//                         {
//                             var mat = renderer.material;
//                             if (mat != null)
//                             {
//                                 m_listMat.Add(mat);
//                             }
//                         }
//                     }
//                 }
//             }
//         }
//
//         public void Update(bool isScale, Vector3 posCamera)
//         {
//             if (m_trans == null)
//             {
//                 return;
//             }
//
//             var curHeight = posCamera.y;
//             m_trans.position = new Vector3(posCamera.x, m_posY, posCamera.z);
//
//             if (isScale)
//             {
//                 if (curHeight < m_startShowHeight || curHeight > m_endHideHeight)
//                 {
//                     m_trans.SetActiveEfficiently(false);
//
//                     return;
//                 }
//
//                 m_trans.SetActiveEfficiently(true);
//
//                 // 设置透明度
//                 var rateOpacity = 1.0f;
//                 if (curHeight > m_startHideHeight)
//                 {
//                     rateOpacity = (m_endHideHeight - curHeight) / (m_endHideHeight - m_startHideHeight);
//                 }
//                 else if (curHeight < m_endShowHeight)
//                 {
//                     rateOpacity = (curHeight - m_startShowHeight) / (m_endShowHeight - m_startShowHeight);
//                 }
//
//                 rateOpacity = Mathf.Abs(rateOpacity * MaxOpacity);
//                 if (m_listMat != null)
//                 {
//                     foreach (var mat in m_listMat)
//                     {
//                         mat.SetFloat(Opacity, rateOpacity);
//                     }
//                 }
//
//                 // 设置缩放
//                 var rate = (curHeight - m_startShowHeight) / (m_endHideHeight - m_startShowHeight);
//                 rate = Mathf.Abs(rate);
//                 var rateScale = Vector3.Lerp(m_startScale, m_endScale, rate);
//                 if (m_listParticle != null)
//                 {
//                     foreach (var particle in m_listParticle)
//                     {
//                         var shape = particle.shape;
//                         shape.scale = rateScale;
//                     }
//                 }
//             }
//         }
//     }
// }