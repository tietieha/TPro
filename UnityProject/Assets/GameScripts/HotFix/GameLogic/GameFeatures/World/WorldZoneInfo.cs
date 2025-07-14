using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class WorldZoneInfo
{
    public int index;

    public RectInt rect;

    // 城点区域数据
    public WorldZoneData data;

    // 区域描边显示对象
    public SpriteRenderer body;

    // 区域阴影对象
    public SpriteRenderer shadow;



    // 城点区域边界对象
    public WorldZoneEdge edge;

    public bool zone_visible;
    public bool zone_loaded;

    public bool icon_visible;
    public bool icon_loaded;

    public bool edge_visible;
    public bool edge_loaded;

    public bool battleNews_visible;
    public bool marchNews_visible;
    public bool cityChangeNews_visible;

    public bool finded;
    public int findState = 0;

    public bool IsDirty = false;

    public Vector3 pos_body;
    public Vector3 pos_cityIcon;
    public Vector3 pos_edge;

    public CanvasGroup canvasGroup;

    private WorldZoneInfo()
    {
    }
    
    public WorldZoneInfo(WorldZoneData info)
    {
        SetData(info);
    }

    public void SetData(WorldZoneData info)
    {
        index = info.ZoneId;
        rect = new RectInt(info.X, info.Y, info.W, info.H);
        data = info;
        findState = 0;
        Reset();
    }

    public void Reset()
    {
        zone_visible = false;
        zone_loaded = false;
        icon_visible = false;
        icon_loaded = false;
        edge_visible = false;
        edge_loaded = false;
        cityChangeNews_visible = false;
        finded = false;
        body = null;
        edge = null;
    }

    public void SetBodyPosition(Vector3 pos)
    {
        pos_body = pos;
    }


    public void SetEdgePosition(Vector3 pos)
    {
        pos_edge = pos;
    }

    public void ToggleBody(bool visible)
    {
        if (body != null)
            body.transform.localPosition = visible ? pos_body : new Vector3(9999,9999,99999);
    }




    public void ToggleEdge(bool visible)
    {
        if (edge != null)
            edge.transform.localPosition = visible ? pos_edge : new Vector3(9999,9999,99999);
    }

    
  
}

// 简单的四叉树存储
public class WorldZoneTreeNode
{
    public RectInt rect;
    public List<WorldZoneTreeNode> nodes = new List<WorldZoneTreeNode>();
    public List<WorldZoneInfo> zones = new List<WorldZoneInfo>();

    public WorldZoneTreeNode()
    {
    }

    public WorldZoneTreeNode(RectInt rc)
    {
        rect = rc;
    }

    public void SetRect(RectInt rc)
    {
        rect = rc;
    }

    public void CrateSubNodes(int depth)
    {
        int w = Mathf.CeilToInt(rect.width * 0.5f);
        int h = Mathf.CeilToInt(rect.height * 0.5f);
        nodes.Clear();
        nodes.Add(new WorldZoneTreeNode(new RectInt(rect.x, rect.y, w, h)));
        nodes.Add(new WorldZoneTreeNode(new RectInt(rect.x + w, rect.y, w, h)));
        nodes.Add(new WorldZoneTreeNode(new RectInt(rect.x, rect.y + h, w, h)));
        nodes.Add(new WorldZoneTreeNode(new RectInt(rect.x + w, rect.y + h, w, h)));
        if (depth > 0)
        {
            foreach (var node in nodes)
            {
                node.CrateSubNodes(depth - 1);
            }
        }
    }

    bool IsRectIntersect(RectInt rc1, RectInt rc2)
    {
        if (Mathf.Max(rc1.xMin, rc2.xMin) > Mathf.Min(rc1.xMax, rc2.xMax) ||
            Mathf.Max(rc1.yMin, rc2.yMin) > Mathf.Min(rc1.yMax, rc2.yMax))
            return false;
        return true;
    }

    public void AddZone(WorldZoneInfo zone)
    {
        if (nodes.Count > 0)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (IsRectIntersect(nodes[i].rect, zone.rect))
                {
                    nodes[i].AddZone(zone);
                }
            }
        }
        else
        {
            zones.Add(zone);
        }
    }

    public void FindZone(RectInt rect, ref List<WorldZoneInfo> findZones, ref int count)
    {
        if (nodes.Count > 0)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                count++;
                if (IsRectIntersect(nodes[i].rect, rect))
                {
                    nodes[i].FindZone(rect, ref findZones, ref count);
                }
            }
        }
        else if (zones.Count > 0)
        {
            for (int i = 0; i < zones.Count; i++)
            {
                if (zones[i].finded)
                    continue;

                count++;
                if (IsRectIntersect(zones[i].rect, rect))
                {
                    zones[i].finded = true;
                    zones[i].findState++;
                    findZones.Add(zones[i]);
                }
            }
        }
    }

    public void Reset()
    {
        // for (int i = 0; i < nodes.Count; i++)
        // {
        //     WorldZoneTreeNode.Return(nodes[i]);
        // }

        nodes.Clear();
        zones.Clear();
    }

    
}