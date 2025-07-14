using System.Collections.Generic;
using System.Linq;
using GameLogic;
using UnityEngine;
using XLua;


[LuaCallCSharp]
public class CampaignLineRenderer : MonoBehaviour
{
    public List<Material> materials;
    public LineRenderer lr;

    private List<Vector3> _lineList;
    public LayerMask layerMask = new LayerMask();

    private int currentIndex = 0;

    public LineRendererVertical lineRendererVertical;

    public void ClearPos()
    {
        for (int i = 0; i < _lineList.Count; i++)
        {
            _lineList[i] = Vector3.zero;
        }
        lineRendererVertical?.SetDirty();
    }

    public void SetLineList(Vector3[] lineList, int type = -1)
    {
        SetMaterial(type);
        _lineList = lineList.ToList();
        currentIndex = lineList.Length;
        lr.positionCount = currentIndex;
        lr.SetPositions(lineList);
        lineRendererVertical?.SetDirty();
    }

    public void UpdateLine(int index)
    {
        if (index > 0)
        {
            currentIndex = index;
            lr.positionCount = currentIndex;
            lr.SetPositions(_lineList.ToArray());
        }
    }

    public void Run()
    {
        UpdateLine(currentIndex);
    }

    public void SetMaterial(int index)
    {
        if (index >= 0 && index < materials.Count)
        {
            lr.material = materials[index];
        }
    }
}
