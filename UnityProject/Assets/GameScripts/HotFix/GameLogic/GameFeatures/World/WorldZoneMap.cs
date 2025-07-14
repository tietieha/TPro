using BitBenderGames;
using DG.Tweening;

using System.Collections.Generic;
using UnityEngine;

using World;
using XLua;

[LuaCallCSharp]
public class WorldZoneMap : MonoBehaviour
{
    // public static WorldZoneMap Instance;

    public MobileTouchCamera touchCamera;

    public Transform zoneRoot;
    public Transform edgeRoot;

    // public SpriteRenderer zoneTemplate;
    // public SpriteRenderer zoneFill;

    // public WorldZoneEdge edgeTemplate;

    public Vector3 shadowOffset;

    public Texture2D[] splashTextures;

    public float cityUIScaleUp = 2f;

    [HideInInspector] public Vector3 lastCamPosition = Vector3.zero;
    [HideInInspector] public Rect lastRect = Rect.zero;
    [HideInInspector] public Rect camVertsRect = Rect.zero;
    [HideInInspector] public bool inited = false;


    // [HideInInspector] public EPOOutline.Outliner _outLiner;

    [HideInInspector] public List<WorldZoneInfo> _lastClippedZones = new List<WorldZoneInfo>();
    [HideInInspector] public List<WorldZoneInfo> _clippedZones = new List<WorldZoneInfo>();
    [HideInInspector] public List<WorldZoneInfo> _lastClippedEdges = new List<WorldZoneInfo>();
    [HideInInspector] public List<WorldZoneInfo> _clippedEdges = new List<WorldZoneInfo>();

    private static readonly int SolidOutline = Shader.PropertyToID("_SolidOutline");
    private static readonly int BaseAlpha = Shader.PropertyToID("_BaseAlpha");
    private static readonly int SplashTex = Shader.PropertyToID("_SplashTex");
    private static readonly int SplashAlpha = Shader.PropertyToID("_SplashAlpha");
    private static readonly int InnerColor = Shader.PropertyToID("_InnerColor");
    private static readonly int InnerColorIntensity = Shader.PropertyToID("_InnerColorIntensity");
    private static readonly int SplashAngle = Shader.PropertyToID("_Angle");
    private static readonly int SplashTex_ST = Shader.PropertyToID("_SplashTex_ST");
    private static readonly int Outline_Clip = Shader.PropertyToID("_Clip");

    private void Awake()
    {
        // _outLiner =  touchCamera.gameObject.GetComponent<EPOOutline.Outliner>();;
        // _outLiner.GlobalTransparent = 0f;

        // zoneFill.sortingOrder = WorldUtils.World_Sort_SmallMap;
        // zoneFill.CreatePool();
        // zoneFill.gameObject.SetActiveEx(false);
       
        zoneRoot.gameObject.SetActiveEx(false);

        Init();
    }

    public void Init()
    {
        lastRect = Rect.zero;
        lastCamPosition = Vector3.zero;
        inited = true;
    }

    public void ClearZoneMap()
    {
        // GameEntry.UI.CloseUIByKey("LFWorldSmallMap");

        WorldZoneEdge.ClearCacheMat();

        _lastClippedZones.Clear();
        _clippedZones.Clear();
        _lastClippedEdges.Clear();
        _clippedEdges.Clear();

        inited = false;
    }

    public void UnInit()
    {
        ClearZoneMap();
        foreach (var mat in zoneMaterialDict.Values)
        {
            Destroy(mat);
        }
        zoneMaterialDict.Clear();
    }

 

    private static int _switchState = -1;





    /// <summary>
    /// 刷新地区区块颜色，依据的是占领联盟信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnUpdateWorldCityOwnerInfo(object sender, GameEventArgs e)
    {
        // var changes = GameEntry.World.Zone.UpdateAllZoneOwner();
        //
        // WorldZoneInfo zoneInfo;
        // for (int i = 0; i < changes.Count; i++)
        // {
        //     zoneInfo = GameEntry.World.Zone.GetWorldZoneInfoById(changes[i]);
        //     if (zoneInfo == null)
        //         continue;
        //
        //     cfg = GameEntry.World.Zone.GetWorldCityColor(zoneInfo.data.color);
        //     if (zoneInfo.body != null)
        //     {
        //         var material = GetMaterialInZone(zoneInfo.body.gameObject);
        //         SetOutlineParam(zoneInfo.body.transform, cfg, zoneInfo);
        //         SetRenderParam(material, cfg);
        //     }
        //
        //     if (zoneInfo.edge != null)
        //     {
        //         if (cfg != null)
        //         {
        //             var groundColor = cfg.groundColor;
        //             // 同军团蓝色
        //             if (zoneInfo.data.outlineColor > 0)
        //             {
        //                 groundColor = Color.red;//LFDefines.Color_Alliance;
        //             }
        //             zoneInfo.edge.SetRenderParam(1, groundColor);
        //         }
        //         else
        //             zoneInfo.edge.SetRenderParam(0, Color.white);
        //     }
        // }
    }

    public bool IsInClip(Vector3 worldPos)
    {
        if (worldPos.x >= lastRect.xMin && worldPos.x <= lastRect.xMax &&
            worldPos.z >= lastRect.yMax && worldPos.z <= lastRect.yMin)
            return true;

        return false;
    }

    //
    // [HideInInspector]public Vector3 p0, p1, p2, p3, curPos;
    // [HideInInspector]public Vector2Int[] camVerts = new Vector2Int[4];

  
    public void SetOutlineParam(Transform trans, WorldCityColor cfg, WorldZoneInfo zoneInfo)
    {
        // EPOOutline.Outlinable outline = trans.gameObject.GetComponent<EPOOutline.Outlinable>();
        // if (outline == null)
        //     return;
        //
        // if (cfg == null)
        // {
        //     outline.OutlineParameters.DilateShift = 1.0f;
        //     ColorUtility.TryParseHtmlString("#220F02",out var c );
        //     outline.OutlineParameters.Color = c;
        //     // outline.OutlineParameters.Color2 = Color.cyan;
        //     trans.transform.GetChild(0).gameObject.SetActiveEx(false);
        //     trans.transform.GetChild(1).gameObject.SetActiveEx(false);
        //     return;
        // }
        //
        // // 同军团
        // Color outlineColor = cfg.outlineColor;
        // if (zoneInfo.data.outlineColor > 0)
        // {
        //     outlineColor = Color.blue;//LFDefines.Color_Alliance;
        // }
        //
        // outline.OutlineParameters.Color = outlineColor;
        // outline.OutlineParameters.Color2 = outlineColor;
        // outline.OutlineParameters.DilateShift = 1.0f;
        // trans.transform.GetChild(0).gameObject.SetActiveEx(true);
        // trans.transform.GetChild(1).gameObject.SetActiveEx(true);
    }

    public  void SetRenderParam(Material material, WorldCityColor cfg)
    {
        if (material == null)
            return;

        if (cfg == null)
        {
            material.SetTexture(SplashTex, null);
            material.SetFloat(SplashAlpha, 0.1f);
            material.SetColor(SolidOutline, Color.white);
            material.SetFloat(BaseAlpha, 0.1f);
            material.SetColor(InnerColor, Color.white);
            material.SetFloat(InnerColorIntensity, 0.1f);
            return;
        }


        if (splashTextures.Length > 0)
        {
            var idx = Mathf.Clamp(cfg.splashIndex, 0, splashTextures.Length);
            material.SetTexture(SplashTex, splashTextures[idx]);
        }

        material.SetColor(SolidOutline, cfg.baseColor);
        material.SetFloat(BaseAlpha, cfg.baseAlpha);
        material.SetColor(InnerColor, cfg.innerColor);
        material.SetFloat(InnerColorIntensity, cfg.innerIntensity);
        material.SetFloat(SplashAlpha, cfg.splashAlpha);
        material.SetFloat(SplashAngle, cfg.splashRot);
        material.SetVector(SplashTex_ST, new Vector4(cfg.splashX, cfg.splashY, 0, 0));
    }

    [HideInInspector]public float DstAlpha = 1f;
    [HideInInspector]public float SrcAlpha = 0f;
    [HideInInspector]public int FadeDir = 1;

    private float FadeSpeed = 3f;
    //private bool IsLockUpdate = false;

    private void SetOutlinerTransparent(float a)
    {
        // if (_outLiner != null)
        // {
        //     SrcAlpha = a;
        //     _outLiner.GlobalTransparent = a;
        // }
    }

    // 选中的区域播放呼吸效果
    public void BreathZone(WorldZoneInfo zoneInfo, float time, int loop)
    {
        if (zoneInfo == null || zoneInfo.body == null)
            return;

        //todo 设置区域颜色
        var cfg = new WorldCityColor();//GameEntry.World.Zone.GetWorldCityColor(zoneInfo.data.color);
        var material = GetMaterialInZone(zoneInfo.body.gameObject);
        var originColor = cfg?.baseColor ?? Color.white;

        ColorUtility.TryParseHtmlString("#f2f2f2", out var c1);
        ColorUtility.TryParseHtmlString("#000000", out var c2);
        var aOrigin = cfg?.baseAlpha ?? 0.1f;
        var a1 = 0.5f;
        var a2 = 0.5f;
        BreathAnimation(material, c1, c2, originColor, a1, a2, aOrigin, time, loop);
    }

    void BreathAnimation(Material mat, Color c1, Color c2, Color endColor, float a1, float a2, float endAlpha,
        float time, int loop)
    {
        var progress = 0f;
        DOTween.To(() => progress, x => progress = x, 1f, time).OnUpdate(() =>
        {
            var color = Color.Lerp(c1, c2, progress);
            var alpha = Mathf.Lerp(a1, a2, progress);
            SetMatColorAndAlpha(mat, color, alpha);
        }).OnComplete(() =>
        {
            DOTween.To(() => progress, x => progress = x, 0f, time).OnUpdate(() =>
            {
                var color = Color.Lerp(c1, c2, progress);
                var alpha = Mathf.Lerp(a1, a2, progress);
                SetMatColorAndAlpha(mat, color, alpha);
            }).OnComplete(() =>
            {
                loop--;
                if (loop > 0)
                {
                    BreathAnimation(mat, c1, c2, endColor, a1, a2, endAlpha, time, loop);
                }
                else
                {
                    DOTween.To(() => progress, x => progress = x, 1f, time).OnUpdate(() =>
                    {
                        var color = Color.Lerp(c1, endColor, progress);
                        var alpha = Mathf.Lerp(a1, endAlpha, progress);
                        SetMatColorAndAlpha(mat, color, alpha);
                    });
                }
            });
        });
    }

    private static void SetMatColorAndAlpha(Material mat, Color color, float alpha)
    {
        mat.SetColor(SolidOutline, color);
        mat.SetFloat(BaseAlpha, alpha);
    }

    // 缓存材质对象。
    private Dictionary<GameObject, Material> zoneMaterialDict = new Dictionary<GameObject, Material>();

    public Material GetMaterialInZone(GameObject body)
    {
        if (body == null)
            return null;

        if (zoneMaterialDict.TryGetValue(body, out var mat))
        {
            return mat;
        }

        var rd = body.gameObject.GetComponent<Renderer>();
        if (rd == null)
            return null;

        mat = rd.material;
        zoneMaterialDict.Add(body, mat);
        return mat;
    }

    // // 填空城点区域效果表现
    // public void FillZone(WorldZoneInfo zoneInfo, System.Action onComplete = null)
    // {
    //     if (this == null || zoneInfo == null || zoneFill == null)
    //         return;
    //
    //     if (zoneInfo.body == null || zoneInfo.data == null)
    //         return;
    //
    //     var rd = zoneInfo.body.gameObject.GetComponent<Renderer>();
    //     if (rd != null)
    //         rd.enabled = false;
    //
    //     var fillBody = zoneFill.Spawn(zoneRoot);
    //     if (fillBody == null)
    //         return;
    //
    //     var fillTran = fillBody.transform;
    //     if (fillTran != null)
    //     {
    //         // fillTran.localScale = GameEntry.World.Zone.InitScale;
    //         fillTran.localPosition = zoneInfo.pos_body;
    //     }
    //
    //     var material = GetMaterialInZone(fillBody.gameObject);
    //
    //     if (material == null || material.shader == null)
    //     {
    //         Log.Error("zone material == " + material + " material shader " + material.shader);
    //         return;
    //     }
    //
    //     //todo 设置区域颜色
    //     var cfg = new WorldCityColor();//GameEntry.World.Zone.GetWorldCityColor(zoneInfo.data.color);
    //     if (cfg != null)
    //     {
    //         if (zoneInfo.body != null)
    //         {
    //             SetOutlineParam(zoneInfo.body.transform, cfg, zoneInfo);
    //             SetRenderParam(material, cfg);
    //         }
    //     }
    //
    //     //todo 加载区块图
    //     // string zonePath = GameEntry.World.State.GetWorldZoneSpritePath(zoneInfo.index);
    //     // fillBody.LoadAsync(zonePath);
    //
    //     var cur = 0f;
    //     material.SetFloat(Outline_Clip, cur);
    //
    //     var tx = (float) zoneInfo.data.Cx / zoneInfo.data.W;
    //     var ty = 1f - (float) zoneInfo.data.Cy / zoneInfo.data.H;
    //     material.SetVector("_DissolveCenterUV", new Vector4(tx, ty));
    //
    //     DOTween.To(() => cur, x => cur = x, 10f, 2f).OnUpdate(() =>
    //     {
    //         if (material != null)
    //             material.SetFloat(Outline_Clip, cur);
    //     }).OnComplete(() =>
    //     {
    //         Destroy(fillBody.gameObject);
    //         if (zoneInfo.body != null)
    //         {
    //             var renderer = zoneInfo.body.gameObject.GetComponent<Renderer>();
    //             if (renderer != null)
    //                 renderer.enabled = true;
    //         }
    //
    //         if (onComplete != null)
    //         {
    //             onComplete.Invoke();
    //         }
    //     });
    // }



    // 小地图调用的同步位置接口
    public void MoveToPoint(float sx, float sy)
    {
        // var worldScene = GameEntry.SceneContainer.WorldScene;
        // if (worldScene == null)
        //     return;
        // var camera = worldScene.Camera;
        // if (camera == null)
        //     return;
        // var mapData = GameEntry.World.State.curWorld.zone.MapData;
        // if (mapData == null)
        //     return;
        //
        // int tx = (int) (sx * mapData.MapWidth);
        // int ty = (int) (sy * mapData.MapHeight);
        //
        // if (mapData.mapType == MapType.ESmallWorld)
        // {
        //     // + 左上角偏移
        //     tx = tx + (int) mapData.Offset.x;
        //     ty = ty + (int) mapData.Offset.y;
        // }
        //
        // var worldPos = WorldUtils.TileToWorld(new Vector2Int(tx, ty));
        // camera.LookAt(worldPos);
        //
        // OnClip();
    }

    public void HideZoneRoot()
    {
        for (int i = 0; i < _clippedZones.Count; i++)
        {
            _clippedZones[i].finded = false;
            // ToggleZone(_clippedZones[i], false);

        }

        _clippedZones.Clear();

        SetOutlinerTransparent(0f);
        zoneRoot.gameObject.SetActiveEx(false);
    }

    // private List<int> _allAssetUid = new List<int>();
    // public WorldZoneEdge CreateEdgeNode()
    // {
    //     WorldZoneEdge temp = null; 
    //     var id = GameEntry.AssetManager.LoadAsync<GameObject>("Assets/GameAsset/Scene/WorldScene/map/Common/Prefab/Edge.prefab",
    //         (Object obj,int gUid) =>
    //         {
    //             temp = (obj as GameObject).GetComponent<WorldZoneEdge>();
    //         });
    //     temp.gameObject.SetActiveEx(false);
    //     _allAssetUid.Add(id);
    //     return temp;
    // }
    //
    // public SpriteRenderer CreateZoneNode()
    // { 
    //     SpriteRenderer temp=null; 
    //     var id = GameEntry.AssetManager.LoadAsync<GameObject>("Assets/GameAsset/Scene/WorldScene/map/Common/ZoneCell.prefab",
    //         (Object obj,int gUid) =>
    //         {
    //             temp = (obj as GameObject).GetComponent<SpriteRenderer>();
    //         });
    //     temp.sortingOrder = WorldUtils.World_Sort_SmallMap;
    //     temp.gameObject.SetActiveEx(false);
    //     _allAssetUid.Add(id);
    //     return temp;
    // }
    public void OnUpdate()
    {
        // if (_outLiner != null && SrcAlpha != DstAlpha)
        // {
        //     SrcAlpha += FadeSpeed * Time.deltaTime * FadeDir;
        //     if (FadeDir > 0 && SrcAlpha >= DstAlpha)
        //     {
        //         SrcAlpha = DstAlpha;
        //     }
        //     else if (FadeDir < 0 && SrcAlpha < DstAlpha)
        //     {
        //         HideZoneRoot();
        //         SrcAlpha = DstAlpha;
        //     }
        //
        //     //Debug.LogFormat("GlobalTransparent {0}  {1} <= {2}", FadeDir, DstAlpha, SrcAlpha);
        //     _outLiner.GlobalTransparent = SrcAlpha;
        // }

    }
  
}