﻿using System.Collections.Generic;
using RenderFeature;
using UnityEngine;

namespace HUDUI
{
    [System.Serializable]
    public enum JumpWorldType
    {
        [HideInInspector]
        None = 0,
        Damage,                 //  普通伤害
        DamageRight,            //  暴击伤害
        MagicDamage,             //  敌方给我的伤害
        Cure,                   //  治疗
        MagicDeny,             //魔法否定
        MagicImmune,           //魔法免疫
        MagicResist,           //魔法抵抗
        MagicUnEffect,         //不受影响
        BlackBar,  //进度条 底板
        RedBar, //进度条 红色
        GreenBar, //进度条 绿色
        WhiteBar, //进度条 白色
        Max,
    }
    [System.Serializable]
    public enum JumpWorldSpriteType
    {
        [HideInInspector]
        None = 0,
        Number,
        Word,
    }

    public class JumpWorldContext:BaseContext
    {
        Queue<HUDJumpWorldTitle> m_titlePool = new Queue<HUDJumpWorldTitle>();
        Queue<HUDVertexInfo> m_vertexPool = new Queue<HUDVertexInfo>();
        BetterList<int> m_tempNumb = new BetterList<int>();
        List<int> m_invalidTitleId = new List<int>();
        public float ShowTime;
        const float JumpOffsetY = 2.0f;
        int TitleId = 0;
        //private HUDBehaviour m_hUDBehaviour;
        //public JumpWorldContext(HUDBehaviour hUDBehaviour)
        //{
        //    m_hUDBehaviour = hUDBehaviour;


        //}
        public override void Init(AtlasContext atlasContext)
        {
            base.Init(atlasContext);
            ShowTime = m_atlasContext.HUDSetting.AnimationTime > 0.01 ? m_atlasContext.HUDSetting.AnimationTime : 1;
            for (int i=0;i<10;++i)
            {
                m_titlePool.Enqueue(new HUDJumpWorldTitle());
            }
            for(int i=0;i<20;++i)
            {
                m_vertexPool.Enqueue(new HUDVertexInfo());
            }
        }
        public override void Release()
        {
            base.Release();
            m_titlePool.Clear();
            m_vertexPool.Clear();
            m_invalidTitleId.Clear();
        }
        private void PlayAnimation(HUDJumpWorldTitle title)
        {
            var spriteConfig = m_atlasContext.GetSpriteConfig(title.JPType, out bool bExist);
            if (!bExist)
                return;
            var animAttribute = m_atlasContext.GetAnimation(spriteConfig.AnimationName);
            float curDuration = Time.time - title.BeginTime;
            int tempAlpha = (int)(animAttribute.AlphaCurve.Evaluate(curDuration) * 255+0.5);
            float scale = title.Scale;
            scale *= animAttribute.ScaleCurve.Evaluate(curDuration);
            //// 临时 todo
            //scale = 1;
            scale *= 360;
            scale *= Screen.width / 1440.0f;
            float movePosX = animAttribute.MoveXCurve.Evaluate(curDuration);
            float movePosY = animAttribute.MoveYCurve.Evaluate(curDuration);
            movePosX *= scale;
            movePosY *= scale;
            tempAlpha = Mathf.Min(255, tempAlpha);
            tempAlpha = Mathf.Max(0, tempAlpha);
            //// 临时 todo
            //tempAlpha = 255;

            byte alpha = (byte)tempAlpha;

            for (int i=0; i<title.VertexList.size; ++i)
            {
                var vertex = title.VertexList[i];
                vertex.Move.Set(movePosX, movePosY);
                vertex.Scale = scale;
                vertex.clrLU.a = alpha;
                vertex.clrLD.a = alpha;
                vertex.clrRU.a = alpha;
                vertex.clrRD.a = alpha;
                m_mesh.MeshDirty = true;
            }
        }
        private void RefreshAllTitles()
        {
            HUDJumpWorldTitle jpTitle = null;
            foreach(var title in m_titles)
            {
                jpTitle = (HUDJumpWorldTitle)title.Value;
                if(!jpTitle.Show)
                {
                    m_invalidTitleId.Add(title.Key);
                }
                else
                {
                    if(Time.time-jpTitle.BeginTime>jpTitle.ShowTime)
                    {
                        jpTitle.Show = false;
                        m_invalidTitleId.Add(title.Key);
                    }
                    else
                    {
                        PlayAnimation(jpTitle);
                    }
                }
            }
        }
        private void ClearInvalidTitle()
        {
            foreach(var key in m_invalidTitleId)
            {
                var bHave = m_titles.TryGetValue(key, out HUDTitle title);
                if(bHave)
                {
                    for(int i=0; i<title.VertexList.size; ++i)
                    {
                        m_mesh.EraseHUDVertex(title.VertexList[i]);
                        BackHUDVertex(title.VertexList[i]);
                    }
                    BackHUDTitle((HUDJumpWorldTitle)title);
                    m_titles.Remove(key);
                }
            }
            m_invalidTitleId.Clear();
        }
        public override void UpdateMesh()
        {
            RefreshAllTitles();
            base.UpdateMesh();
            if (m_mesh.MeshDirty)
                UIBattleRenderPassFeature.UIBattleHudPass.JumpWorldMesh = m_mesh;
            ClearInvalidTitle();
        }
        private HUDJumpWorldTitle GetHUDTitle()
        {
            HUDJumpWorldTitle title = null;
            if (m_titlePool.Count > 0)
                title = m_titlePool.Dequeue();
            else
                title = new HUDJumpWorldTitle();
            TitleId++;
            return title;
        }
        private void BackHUDTitle(HUDJumpWorldTitle title)
        {
            title.Reset();
            m_titlePool.Enqueue(title);
        }
        private void BackHUDVertex(HUDVertexInfo vertex)
        {
            vertex.Reset();
            m_vertexPool.Enqueue(vertex);
        }
        private HUDVertexInfo GetVertex()
        {
            HUDVertexInfo vertex = null;
            if (m_vertexPool.Count > 0)
                vertex = m_vertexPool.Dequeue();
            else
                vertex = new HUDVertexInfo();
            return vertex;
        }
        private void PushNumberSprite(HUDJumpWorldTitle title, JumpWorldType type, int number)
        {
            number = Mathf.Abs(number);
            // if (number == 0)
            //     return;
            var spriteConfig = m_atlasContext.GetSpriteConfig(type, out bool bExist);
            if (!bExist)
                return;

            int tempWidth = 0;

            if ( spriteConfig.HeadIDs != null && spriteConfig.HeadIDs.Length > 0)
            {
                for (int index = 0; index < spriteConfig.HeadIDs.Length; index++)
                {
                    var headId = spriteConfig.HeadIDs[index];
                    HUDVertexInfo vertex = GetVertex();
                    RefreshVertex(ref vertex, headId);
                    tempWidth += vertex.Width;
                    title.VertexList.Add(vertex);
                }
            }
            var numberSpriteId = spriteConfig.NumberSpriteId;
            m_tempNumb.Clear();
            int nI = 0;
            do
            {
                nI = number % 10;
                number /= 10;
                m_tempNumb.Add(nI);
            } while (number > 0);
            // 反转数组
            m_tempNumb.Reverse();
            int tempNum = 0;

            for (int i=0; i<m_tempNumb.size;++i)
            {
                tempNum = m_tempNumb[i];
                if(numberSpriteId[tempNum]!=0)
                {
                    HUDVertexInfo vertex = GetVertex();
                    RefreshVertex(ref vertex, numberSpriteId[tempNum]);
                    tempWidth += vertex.Width;
                    title.VertexList.Add(vertex);
                }
            }
            HUDVertexInfo v = null;
            float numOffset = -tempWidth / 2;
            for (int i = 0; i < title.VertexList.size; ++i)
            {
                v = title.VertexList[i];
                v.Offset.Set(numOffset, JumpOffsetY);
                numOffset += v.Width;
            }
        }
        private void PushWorldSprite(HUDJumpWorldTitle title, JumpWorldType type)
        {
            var spriteConfig = m_atlasContext.GetSpriteConfig(type, out bool bExist);
            if (!bExist)
                return;
            int tempWidth = 0;

            var spriteId = spriteConfig.SpriteId;
            HUDVertexInfo vertex = GetVertex();
            RefreshVertex(ref vertex, spriteId);
            tempWidth += vertex.Width;
            title.VertexList.Add(vertex);

            if (spriteConfig.HeadIDs != null && spriteConfig.HeadIDs.Length > 0)
            {
                for (int index = 0; index < spriteConfig.HeadIDs.Length; index++)
                {
                    var headId = spriteConfig.HeadIDs[index];
                    HUDVertexInfo vertexTmp = GetVertex();
                    RefreshVertex(ref vertexTmp, headId);
                    tempWidth += vertexTmp.Width;
                    title.VertexList.Add(vertexTmp);
                }
            }


            HUDVertexInfo v = null;
            float numOffset = -tempWidth / 2;
            for (int i = 0; i < title.VertexList.size; ++i)
            {
                v = title.VertexList[i];
                v.Offset.Set(numOffset, JumpOffsetY);
                numOffset += v.Width;
            }
        }


        private void PushSprite(HUDJumpWorldTitle title, JumpWorldType type, int number)
        {
            if (type == JumpWorldType.Damage
                || type == JumpWorldType.MagicDamage
                || type == JumpWorldType.DamageRight
                || type == JumpWorldType.Cure
                )
            {
                PushNumberSprite(title, type, number);
            }
            else
            {
                PushWorldSprite(title, type);
            }
        }
        public void ShowJumpWorld(JumpWorldType type, Transform target=null, int number=0, float offsetY = 6, float scale = 1f)
        {
            var spriteConfig = m_atlasContext.GetSpriteConfig(type, out bool bExist);
            if (!bExist)
                return;
            var title = GetHUDTitle();
            title.TitleId = TitleId;
            title.FollowTarget = target;
            title.BeginTime = Time.time;
            title.ShowTime = ShowTime;
            title.OffsetY = offsetY;
            //title.OffsetX = Random.Range(0.5f, 1.5f);
            title.OffsetX = 0;
            title.Show = true;
            title.JPType = type;
            title.Scale = scale;
            PushSprite(title, type, number);
            m_titles.Add(TitleId, title);
        }
    }
}
