using System.Collections.Generic;
using RenderFeature;
using UnityEngine;

namespace HUDUI
{
    public enum HUDHpType
    {
        None = 0,
        SmallBlueHp,
        SmallRedHp,
    }

    public class HUDBehaviour : MonoBehaviour
    {
        private HpContext m_hpContext;

        public HpContext HPContext
        {
            get => m_hpContext;
        }
        //public HUDRenderMesh HpMesh { get => hpMesh; set => hpMesh = value; }
        //public HUDRenderMesh JumpWorldMesh { get => jumpWorldMesh; set => jumpWorldMesh = value; }

        private JumpWorldContext m_jpContext;
        private AtlasContext m_atlasContext;
        public static Camera s_HUDCamera;
        public static Camera s_HUDUICamera;

        public bool isDebug = false;

        public Transform target = null;

        //private HUDRenderMesh hpMesh = null;
        //private HUDRenderMesh jumpWorldMesh = null;
        //CommandBuffer cmd;
        private Dictionary<int, HUDJumpWorld> m_jumpWorldDic =
            new Dictionary<int, HUDJumpWorld>();

        private List<int> m_jumpWorldKeys = new List<int>();
        private bool m_IsInit = false;


        public void Init(Camera sceneCam = null)
        {
            if (m_IsInit)
            {
                return;
            }

            if (sceneCam != null)
            {
                s_HUDCamera = sceneCam;
            }

            var atlasMng = gameObject.GetComponent<AtlasManager>();
            var hudSetting = gameObject.GetComponent<HUDAnimation>();

            m_atlasContext = new AtlasContext();
            m_atlasContext.Init(atlasMng, hudSetting);

            m_hpContext = new HpContext();
            m_hpContext.Init(m_atlasContext);

            m_jpContext = new JumpWorldContext();
            m_jpContext.Init(m_atlasContext);

            m_IsInit = true;
        }

        void Update()
        {
            if (!m_IsInit)
            {
                return;
            }

            if (GetHUDMainCamera() == null)
                return;
            m_hpContext.UpdateMesh();
            m_jpContext.UpdateMesh();
            UpdateJumpWorld();
            if (isDebug)
                DebugTest();
        }

        void UpdateJumpWorld()
        {
            for (int i = 0; i < m_jumpWorldKeys.Count; i++)
            {
                HUDJumpWorld hUDJumpWorld;
                if (m_jumpWorldDic.TryGetValue(m_jumpWorldKeys[i], out hUDJumpWorld))
                {
                    hUDJumpWorld.Update();
                }
            }
        }

        private void DebugTest()
        {
#if UNITY_EDITOR
            if (target == null)
                return;

            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                ShowJumpWorld(1, (int)JumpWorldType.Damage, target.transform, -1000, 0, 20);
            }

            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                ShowJumpWorld(1, (int)JumpWorldType.Cure, target.transform, -999, 0, 10);
            }

            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                ShowJumpWorld(1, (int)JumpWorldType.MagicDamage, target.transform, -888, 0, 3);
            }

            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                ShowJumpWorld(1, (int)JumpWorldType.MagicDeny, target.transform, -888);
            }

            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                ShowJumpWorld(1, (int)JumpWorldType.MagicImmune, target.transform, -888);
            }

            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                ShowJumpWorld(1, (int)JumpWorldType.MagicResist, target.transform, -888);
            }

            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                ShowJumpWorld(1, (int)JumpWorldType.MagicUnEffect, target.transform, -888);
            }

#endif
        }

        //private void LateUpdate()
        //{
        //    ExecuteMesh();
        //}
        public void ExecuteMesh()
        {
            //if (HpMesh == null && JumpWorldMesh == null)
            //    return;
            //if (cmd == null)
            //{
            //    cmd = new CommandBuffer();
            //}
            //Camera camera = GetHUDUICamera();
            //if (camera != null)
            //{
            //    camera.RemoveCommandBuffer(CameraEvent.AfterImageEffects, cmd);
            //}
            //cmd.Clear();
            //if (HpMesh != null)
            //{
            //    cmd.DrawMesh(HpMesh.RealMesh, Matrix4x4.identity, HpMesh.RealMaterial);
            //}
            //if (JumpWorldMesh != null)
            //{

            //    cmd.DrawMesh(JumpWorldMesh.RealMesh, Matrix4x4.identity, JumpWorldMesh.RealMaterial);
            //}
            //if (camera != null)
            //{
            //    camera.AddCommandBuffer(CameraEvent.AfterImageEffects, cmd);
            //}
        }

        public void Release()
        {
            if (m_hpContext != null)
            {
                m_hpContext.Release();
                m_hpContext = null;
            }

            if (m_jpContext != null)
            {
                m_jpContext.Release();
                m_jpContext = null;
            }

            if (m_atlasContext != null)
            {
                m_atlasContext.Release();
                m_atlasContext = null;
            }

            s_HUDCamera = null;
            s_HUDUICamera = null;
            //hpMesh = null;
            //jumpWorldMesh = null;
            UIBattleRenderPassFeature.UIBattleHudPass.ClearMesh();
            m_IsInit = false;

            for (int i = 0; i < m_jumpWorldKeys.Count; i++)
            {
                HUDJumpWorld hUDJumpWorld;
                if (m_jumpWorldDic.TryGetValue(m_jumpWorldKeys[i], out hUDJumpWorld))
                {
                    hUDJumpWorld.Release();
                }
            }

            m_jumpWorldKeys.Clear();
            m_jumpWorldDic.Clear();
        }

        public static Camera GetHUDMainCamera()
        {
            if (s_HUDCamera == null)
            {
                // var mainCam = GameObject.Find("SceneCamera");
                var mainCam = GameObject.FindGameObjectWithTag("BattleCamera");
                if (mainCam != null)
                {
                    s_HUDCamera = mainCam.GetComponent<Camera>();
                }
                else
                {
                    Debug.LogError("not find main_cam in scene");
                }
            }

            return s_HUDCamera;
        }

        public static Camera GetHUDUICamera()
        {
            if (s_HUDUICamera == null)
            {
                var camUI = GameObject.Find("GUICamera");
                if (camUI != null)
                {
                    s_HUDUICamera = camUI.GetComponent<Camera>();
                }
                else
                {
                    Debug.LogError("not find cameraUI in scene");
                }
            }

            return s_HUDUICamera;
        }

        public Dictionary<int, HUDTitle> GetHpTitles()
        {
            return m_hpContext.GetTitles();
        }

        public int CreateHpTitle(int hpType, int entityId, int width = 0, int height = 0, float offsetY = 6,
            Transform target = null)
        {
            m_hpContext.CreateHpTitle((HUDHpType)hpType, entityId, width, height, offsetY, target);
            return entityId;
        }

        public bool HasHpTitle(int entityId)
        {
            return m_hpContext.HasTitle(entityId);
        }

        public void ChangeFollowTarget(int entityId, Transform target, float offsetY = 6)
        {
            m_hpContext.ChangeFollowTarget(entityId, target, offsetY);
        }

        public void ShowHpTitle(int titleId, bool bShow, float showTime = 1.5f)
        {
            m_hpContext.ShowHpTitle(titleId, bShow, showTime);
        }

        public void ClearHpFollowTarget(int entityId)
        {
            m_hpContext.ClearHpFollowTarget(entityId);
        }

        public void SetHpRate(int titleId, float hpRate, float preHpRate = 0, float time = 0)
        {
            m_hpContext.SetHpRate(titleId, hpRate, preHpRate, time);
        }

        public void ShowJumpWorld(int eneityId, int nType, Transform target = null,
            int number = 0, float offsetY = 2, float scale = 1.0f)
        {
            HUDJumpWorld jumpWorld;
            if (!m_jumpWorldDic.TryGetValue(eneityId, out jumpWorld))
            {
                jumpWorld = new HUDJumpWorld(m_jpContext, eneityId);
                m_jumpWorldDic.Add(eneityId, jumpWorld);
                m_jumpWorldKeys.Add(eneityId);
            }

            jumpWorld.TryShowJumpWorld(nType, target, number, offsetY, scale);
        }
    }
}