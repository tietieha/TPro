using UnityEngine;

namespace HUDUI
{
    public class HUDSpriteInfo
    {
        public string name = "Unity Bug";   // ���������
        public Rect outer = new Rect(0f, 0f, 1f, 1f);     // ��򣬾����ʵ�ʴ�С�����������������)
        public Rect inner = new Rect(0f, 0f, 1f, 1f);     // �ڿ����������ģʽʱ���������꣬��������������֮�ڵ�
        public bool rotated = false;

        // Padding is needed for trimmed sprites and is relative to sprite width and height
        public float paddingLeft = 0f;   // ����������ͼ��ѡ��ʱ��չѡ���Χ�Ķ�����û��ʵ������
        public float paddingRight = 0f;
        public float paddingTop = 0f;
        public float paddingBottom = 0f;

        // ��������չ����
        public int m_nNameID;   // ����ID
        public int m_nAtlasID;  // ����ID
        public string m_szAtlasName;  // ��Ӧ�Ĳ�������

        public bool hasPadding { get { return paddingLeft != 0f || paddingRight != 0f || paddingTop != 0f || paddingBottom != 0f; } }

        public HUDSpriteInfo Clone()
        {
            HUDSpriteInfo p = new HUDSpriteInfo();
            p.Copy(this);
            return p;
        }

        // ���ܣ��������� 
        public void Copy(HUDSpriteInfo src)
        {
            name = src.name.Clone() as string;
            outer = new Rect(src.outer.xMin, src.outer.yMin, src.outer.width, src.outer.height);
            inner = new Rect(src.inner.xMin, src.inner.yMin, src.inner.width, src.inner.height);
            rotated = src.rotated;
            paddingLeft = src.paddingLeft;
            paddingRight = src.paddingRight;
            paddingTop = src.paddingTop;
            paddingBottom = src.paddingBottom;
            m_nNameID = src.m_nNameID;
            m_nAtlasID = src.m_nAtlasID;
            m_szAtlasName = src.m_szAtlasName.Clone() as string;
        }
        public void Serailize(ref HUDCSerialize ar)
        {
            ar.ReadWriteValue(ref name);
            ar.ReadWriteValue(ref outer);
            ar.ReadWriteValue(ref inner);
            ar.ReadWriteValue(ref rotated);
            ar.ReadWriteValue(ref paddingLeft);
            ar.ReadWriteValue(ref paddingRight);
            ar.ReadWriteValue(ref paddingTop);
            ar.ReadWriteValue(ref paddingBottom);
            ar.ReadWriteValue(ref m_szAtlasName);
            if (ar.GetVersion() >= 1)
            {
                ar.ReadWriteValue(ref m_nNameID);
                ar.ReadWriteValue(ref m_nAtlasID);
            }
        }
    }
}

   
