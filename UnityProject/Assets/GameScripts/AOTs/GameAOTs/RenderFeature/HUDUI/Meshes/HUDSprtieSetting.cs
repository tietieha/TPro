using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HUDUI
{
    public class HUDSprtieSetting
    {
        public int m_nHeadID; // ��ͷ��ͼƬID
        public int m_nAddID;  // + ��
        public int m_nSubID;  // - ��
        public int[] m_NumberID = new int[10]; // ����ID
        public void InitNumber(AtlasContext atlasContext, string szHeadName, string szPrefix)
        {
            m_nHeadID = atlasContext.SpriteNameToID(szHeadName);
            m_nAddID = atlasContext.SpriteNameToID(szPrefix + '+');
            m_nSubID = atlasContext.SpriteNameToID(szPrefix + '-');
            for (int i = 0; i < 10; ++i)
            {
                m_NumberID[i] = atlasContext.SpriteNameToID(szPrefix + i.ToString());
            }
        }
    }

}
