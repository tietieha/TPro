using UnityEngine;

namespace HUDUI
{
    public class HpContext : BaseContext
    {
        //private HUDBehaviour m_hUDBehaviour;
        private const float ShowTitleTime = 1.5f;
        //public HpContext(HUDBehaviour hUDBehaviour)
        //{
        //    m_hUDBehaviour = hUDBehaviour;

        //}
        public override void Init(AtlasContext atlasContext)
        {
            base.Init(atlasContext);
        }

        private void RefreshAllTitles()
        {
            foreach (var title in m_titles)
            {
                if (title.Value.Show)
                {
                    title.Value.ShowTime -= Time.deltaTime;
                    if (title.Value.ShowTime < 0)
                    {
                        title.Value.Show = false;
                        for (int i = 0; i < title.Value.VertexList.size; ++i)
                        {
                            m_mesh.EraseHUDVertex(title.Value.VertexList[i]);
                        }
                    }

                    if (title.Value.Show && title.Value.tweenTime > 0 && title.Value.WhiteBloodVertex != null)
                    {
                        title.Value.PreHpRate -= title.Value.deltaValue * Time.deltaTime;
                        int white = (int)(title.Value.Width * title.Value.PreHpRate + 0.5);
                        RefreshVertex(ref title.Value.WhiteBloodVertex, title.Value.WhiteBloodVertex.SpriteId, white,
                            title.Value.Height);
                        title.Value.WhiteBloodVertex.Offset.Set(-title.Value.Width / 2, 0);
                        if (title.Value.PreHpRate <= title.Value.HpRate)
                        {
                            title.Value.tweenTime = 0;
                        }
                    }

                    title.Value.tweenTime -= Time.deltaTime;
                }
            }
        }

        public override void UpdateMesh()
        {
            RefreshAllTitles();
            base.UpdateMesh();
            if (m_mesh.MeshDirty)
                UIBattleRenderPassFeature.UIBattleHudPass.HpMesh = m_mesh;
        }

        public override void Release()
        {
            base.Release();
        }

        private void PushSprite(HUDTitle title, HUDHpType hpType, int width = 0, int height = 0)
        {
            int nBkWidth = m_atlasContext.AtlasManager.BloodBk.Width;
            int nBkHeight = m_atlasContext.AtlasManager.BloodBk.Height;
            int nBkSpriteId = m_atlasContext.AtlasManager.BloodBk.SpriteId;
            int nWhiteSpriteId = m_atlasContext.AtlasManager.BloodWhite.SpriteId;

            int spriteId = 0;
            int nBloodWidth = 0;
            int nBloodHight = 0;
            if (hpType == HUDHpType.SmallBlueHp)
            {
                spriteId = m_atlasContext.AtlasManager.BloodBlue.SpriteId;
                nBloodWidth = m_atlasContext.AtlasManager.BloodBlue.Width;
                nBloodHight = m_atlasContext.AtlasManager.BloodBlue.Height;
            }
            else if (hpType == HUDHpType.SmallRedHp)
            {
                spriteId = m_atlasContext.AtlasManager.BloodRed.SpriteId;
                nBloodWidth = m_atlasContext.AtlasManager.BloodRed.Width;
                nBloodHight = m_atlasContext.AtlasManager.BloodRed.Height;
            }

            if (width != 0 && height != 0)
            {
                nBloodWidth = width;
                nBloodHight = height;
            }

            HUDVertexInfo bkVertex = new HUDVertexInfo();
            HUDVertexInfo whiteVertex = new HUDVertexInfo();
            HUDVertexInfo bloodVertex = new HUDVertexInfo();

            RefreshVertex(ref bkVertex, nBkSpriteId, nBloodWidth, nBloodHight);
            RefreshVertex(ref whiteVertex, nWhiteSpriteId, nBloodWidth, nBloodHight);
            RefreshVertex(ref bloodVertex, spriteId, nBloodWidth, nBloodHight);
            // 血条背景
            if (bkVertex != null)
            {
                title.VertexList.Add(bkVertex);
                bkVertex.Scale *= 360;
                bkVertex.Scale *= Screen.width / 1440.0f;
                bkVertex.SpriteId = nBkSpriteId;
            }

            if (whiteVertex != null)
            {
                title.VertexList.Add(whiteVertex);
                title.WhiteBloodVertex = whiteVertex;
                title.Width = nBloodWidth;
                title.Height = nBloodHight;
                whiteVertex.Scale *= 360;
                whiteVertex.Scale *= Screen.width / 1440.0f;
                whiteVertex.SpriteId = nWhiteSpriteId;
            }

            // 滑动条
            if (bloodVertex != null)
            {
                title.VertexList.Add(bloodVertex);
                title.BloodVertex = bloodVertex;
                title.Width = nBloodWidth;
                title.Height = nBloodHight;
                bloodVertex.Scale *= 360;
                bloodVertex.Scale *= Screen.width / 1440.0f;
                bloodVertex.SpriteId = spriteId;
            }

            title.HpRate = 0;
            title.PreHpRate = 0;
        }

        public void CreateHpTitle(HUDHpType hpType, int entityId, int width, int height, float offsetY,
            Transform target = null)
        {
            HUDTitle title = new HUDTitle();
            title.WorldPos = Vector3.zero;
            title.ScreenPos = Vector3.zero;
            title.TitleId = entityId;
            title.OffsetY = offsetY;
            title.OffsetX = 0;
            title.HpRate = 1;
            title.FollowTarget = target;
            title.Show = false;
            PushSprite(title, hpType, width, height);
            m_titles.Add(entityId, title);
        }

        public void ShowHpTitle(int titleId, bool bShow, float showTime = ShowTitleTime)
        {
            HUDTitle title;
            m_titles.TryGetValue(titleId, out title);
            if (title != null)
            {
                title.Show = bShow;
                if (!bShow)
                {
                    for (int i = 0; i < title.VertexList.size; ++i)
                    {
                        m_mesh.EraseHUDVertex(title.VertexList[i]);
                    }
                }
                else
                {
                    title.ShowTime = showTime;
                }
            }
        }

        public void ClearHpFollowTarget(int titleId)
        {
            HUDTitle title;
            m_titles.TryGetValue(titleId, out title);
            if (title != null)
            {
                title.Show = false;
                title.FollowTarget = null;
            }
        }

        public void ChangeFollowTarget(int titleId, Transform target, float offsetY = 6)
        {
            HUDTitle title;
            m_titles.TryGetValue(titleId, out title);
            if (title != null)
            {
                title.FollowTarget = target;
                title.OffsetY = offsetY;
            }
        }

        public bool HasTitle(int titleId)
        {
            HUDTitle title;
            m_titles.TryGetValue(titleId, out title);
            return title != null;
        }

        public void SetHpRate(int titleId, float hpRate, float preHpRate = 0, float time = 0)
        {
            HUDTitle title;
            m_titles.TryGetValue(titleId, out title);
            if (title != null && title.BloodVertex != null)
            {
                title.tweenTime = time;
                if (title.WhiteBloodVertex != null && time > 0)
                {
                    title.PreHpRate = Mathf.Max(preHpRate, title.PreHpRate);
                    var deltaValue = (title.PreHpRate - hpRate) / time;
                    title.deltaValue = deltaValue;
                    int white = (int)(title.Width * preHpRate + 0.5);
                    RefreshVertex(ref title.WhiteBloodVertex, title.WhiteBloodVertex.SpriteId, white, title.Height);
                    title.WhiteBloodVertex.Offset.Set(-title.Width / 2, 0);
                }

                title.HpRate = hpRate;
                int width = (int)(title.Width * hpRate + 0.5);
                RefreshVertex(ref title.BloodVertex, title.BloodVertex.SpriteId, width, title.Height);
                title.BloodVertex.Offset.Set(-title.Width / 2, 0);
            }
        }
    }
}