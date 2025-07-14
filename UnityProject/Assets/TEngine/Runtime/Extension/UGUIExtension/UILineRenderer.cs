using System;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(CanvasRenderer))]
[RequireComponent(typeof(RectTransform))]
[UnityEngine.Scripting.Preserve]

public class UILineRenderer : MaskableGraphic
{
    public Texture texture;
    // public float multipliedPixelsPerUnit = 1;
    public bool loop = false;
    public float normalOffset = 0f;
    
    public bool debug = false;
    
    public float width = 1;
    public Vector2 uvZoom = Vector2.one;
    public float cornerOffset = 1.0f;
    public int numIter = 2;
    // 张力系数，用来控制拐角切割距离之间的比例
    public float CornerDistance = 10.0f;

    public float catmullRate = 5f;
    public int bezierRate = 10;
    
    //贝塞尔曲线绘制 点的 index 区间
    public int[] bezierIndexList;
    public float edgeDist = 2f;
    public const float CrossProductError = 0.01f; // 判断叉乘为0的误差量
    
    /// <summary>
    /// 缓存当前的点数
    /// </summary>
    public Vector2[] pts;

    protected int m_pointNum = 0;
    protected Vector2 m_startPoint = Vector2.zero;
    protected Vector2 m_endPoint = Vector2.zero;
    
    private List<UIVertex> m_cacheVertices = new List<UIVertex>();
    private List<int> m_triangles = new List<int>();
    
    private static float TOLERANCE = 0.0000001f;
    private bool m_dirty = true;
    private List<Vector2> _pts = new List<Vector2>();
    private bool _useBezier = true;
#if UNITY_EDITOR && ODIN_INSPECTOR
    [Button("测试")]
    private void Test()
    {
        SetPoints(pts);
    }
#endif

    protected override void OnEnable()
    {
        base.OnEnable();
        var newMaterial = new Material(material);
        material = newMaterial;
    }

    /// <summary>
    /// 设定是否使用 Bezier 曲线
    /// </summary>
    public void SetUseBezier( bool useBezier)
    {
        _useBezier = useBezier;
    }

    public override Texture mainTexture => texture;

    public void SetPoints(Vector2[] points)
    {
        _pts.Clear();
        int curIndex = 0;
        while (curIndex < points.Length)
        {
            if (_useBezier)
            {
                int bezierStartIndex = GetBezierIndex(curIndex);
                if (bezierStartIndex >= 0)
                {
                    //贝塞尔开始索引
                    if (bezierStartIndex >= 1 && bezierStartIndex <= points.Length - 1)
                    {
                        Vector2 point = points[bezierStartIndex];
                        Vector2 beforePoint = points[bezierStartIndex-1];
                        Vector2 afterPoint = points[bezierStartIndex+1];
                        Vector2 startPoint = GetTwoPointsBetween(point,beforePoint,CornerDistance);
                        Vector2 endPoint = GetTwoPointsBetween(point,afterPoint,CornerDistance);
                        List<Vector2> SamplePoints = SamplePointsByCount(startPoint,point, endPoint,bezierRate);
                        foreach (var p in SamplePoints)
                        {
                            _pts.Add(p);
                        }
                    }
                }
                else
                {
                    _pts.Add(points[curIndex]);
                }
            }
            else
            {
                _pts.Add(points[curIndex]);
            }
            curIndex++;
        }

        //直线
        if (_pts.Count == 2)
        {
            float len = Mathf.Abs(_pts[0].x - _pts[1].x);
            material.SetVector("_SpriteUV", new Vector4(0,0,len,1));
        }
        
        SetAllDirty();
    }

    private float _animDuration;
    private Action _onComplete;
    private bool _isAnimating = false;
    private float _curRunningTime = 0;
    public void PlayAnim(float duration, Action onComplete)
    {
        _animDuration = duration;
        _onComplete = onComplete;
        _isAnimating = true;
        _curRunningTime = 0;
    }

    private void Update()
    {
        if (_isAnimating && material != null)
        {
            _curRunningTime += Time.deltaTime;
            float progress = Mathf.Lerp(0,1,_curRunningTime/_animDuration);
            material.SetFloat("_Progress", progress);
            if (progress > 0.99f)
            {
                if (_onComplete != null)
                {
                    _onComplete();
                }
                _isAnimating = false;
            }
        }
    }

    /// <summary>
    /// 获取2个点之间的 t 距离的点
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    private Vector2 GetTwoPointsBetween(Vector2 p1, Vector2 p2, float t)
    {
        float distance = (p1 - p2).magnitude;
        float perc = t / distance;
        return p1 + perc * (p2 - p1);
    }


    private int GetBezierIndex(int index)
    {
        for (int i = 0; i < bezierIndexList.Length; i++)
        {
            if (index == bezierIndexList[i])
            {
                return index;
            }
        }
        return -1;
    }
    
    //通过给定数量采样曲线上的点
    public List<Vector2> SamplePointsByCount(Vector2 P0, Vector2 P1, Vector2 P2, int count)
    {
        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i <= count; i++)
        {
            float t = (float)i / count;
            points.Add(GetBezierPoint(t, P0, P1, P2));
        }
        return points;
    }
    
    //贝塞尔公式
    public Vector2 GetBezierPoint(float t, Vector2 P0, Vector2 P1, Vector2 P2)
    {
        float x = (float)(Mathf.Pow(1 - t, 2) * P0.x + 2 * (1 - t) * t * P1.x + Mathf.Pow(t, 2) * P2.x);
        float y = (float)(Mathf.Pow(1 - t, 2) * P0.y + 2 * (1 - t) * t * P1.y + Mathf.Pow(t, 2) * P2.y);
        return new Vector2(x, y);
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        if (_pts.Count == 0 && pts.Length > 0)
        {
            SetPoints(pts);
        }
        SetPointsSimple(vh);
    }

    /// <summary>
    /// 简单生成边缘
    /// </summary>
    private void SetPointsSimple(VertexHelper vh)
    {
        vh.Clear();
        m_cacheVertices.Clear();
        m_triangles.Clear();

        if (texture == null)
        {
            return;
        }
        
        if (texture.wrapMode != TextureWrapMode.Repeat)
        {
            texture.wrapMode = TextureWrapMode.Repeat;
        }
        
        if (_pts.Count > 1)
        {
            m_pointNum = _pts.Count;
            m_startPoint = _pts[0];
            m_endPoint = _pts[m_pointNum - 1];
            
            //UV
            var pointLen = loop ? _pts.Count : _pts.Count - 1;
            //得到距离
            var distance = 0.0f;

            var tileWidth = texture.width;
            var tileHeight = texture.height;
            var uvMin = Vector2.zero;
            var uvMax = Vector2.one;
            
            //pos
            for (int i = 0; i <= pointLen; i++)
            {
                Vector2 curP;
                Vector2 extraP;
                Vector2 nextP;
                Vector2 normal;
                if (loop)
                {
                    curP = _pts[i % _pts.Count];
                    extraP = _pts[(i - 1 + pointLen) % pointLen];
                    nextP = _pts[(i + 1) % _pts.Count];
                    var before = (extraP - curP).normalized;
                    var after = (nextP - curP).normalized;
                    // 需要判断before和after的顺逆时针方向，所以用cross
                    var cross = Vector3.Cross(before, after);
                    if (cross.z == 0)
                    {
                        normal = new Vector2(after.y, -after.x).normalized;
                    }
                    else
                    {
                        normal = (before + after).normalized;
                        var dot = Vector3.Dot(before,normal);
                        var sin = Mathf.Sqrt(1 - dot * dot);
                        normal = cross.z < 0 ? -normal / sin : normal / sin;
                    }
                }
                else
                {
                    curP = _pts[i];
                    nextP = _pts[(i + 1) % _pts.Count];
                    int index = Mathf.Clamp(i + 1, 0, pointLen);
                    if (i == 0 || i == pointLen)
                    {
                        var vector = _pts[index] - _pts[index - 1];
                        normal = new Vector2(vector.y, -vector.x).normalized;
                    }
                    else
                    {
                        var before = (_pts[i - 1] - curP).normalized;
                        var after = (nextP - curP).normalized;
                        // 需要判断before和after的顺逆时针方向，所以用cross
                        var cross = Vector3.Cross(before, after);
                        if (cross.z == 0)
                        {
                            var vector = _pts[index] - _pts[index - 1];
                            normal = new Vector2(vector.y, -vector.x).normalized;
                        }
                        else
                        {
                            normal = (before + after).normalized;
                            var dot = Vector3.Dot(before,normal);
                            var sin = Mathf.Sqrt(1 - dot * dot);
                            normal = cross.z < 0 ? -normal / sin : normal / sin;
                        }
                    }
                }
                
                var uvScale = new Vector2(distance / tileWidth, width * 2 / tileHeight);
                uvScale = Vector2.Scale(uvScale, uvZoom);
                uvMin.x = uvMax.x;
                m_cacheVertices.Add(new UIVertex()
                {
                    position = curP + normal * (width + normalOffset),
                    uv0 = Vector2.Scale(uvMin, uvScale),
                    color = color,
                });
                m_cacheVertices.Add(new UIVertex()
                {
                    position = curP - normal * (width - normalOffset),
                    uv0 = Vector2.Scale(uvMax, uvScale),
                    color = color,
                });
                
                if (i != pointLen)
                {
                    float dis = Vector3.Distance(curP, nextP);
                    distance += dis;
                }
            }

            foreach (var uiVertex in m_cacheVertices)
            {
                vh.AddVert(uiVertex);
            }

            //Triangles
            int faceNum = (m_cacheVertices.Count - 2);
            for (int i = 0; i < faceNum; i = i + 2)
            {
                m_triangles.Add(i);
                m_triangles.Add(i + 2);
                m_triangles.Add(i + 3);

                m_triangles.Add(i);
                m_triangles.Add(i + 3);
                m_triangles.Add(i + 1);

                vh.AddTriangle(i, i + 2, i + 3);
                vh.AddTriangle(i, i + 3, i + 1);
            }

            // vh.AddUIVertexStream(m_cacheVertices, m_triangles);
        }
        else
        {
            m_pointNum = 0;
        }

        // m_dirty = false;
    }
}

