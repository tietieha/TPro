using System;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using DG.Tweening;
using XLua;

[LuaCallCSharp]
public class EmbattleHexagonGrid : MonoBehaviour
{
    private static int S_ColorID = Shader.PropertyToID("_Color");

    [SerializeField] private Material MeshMaterial;
    [Tooltip("排数")] public int rowNum = 20;
    [Tooltip("列数")] public int colNum = 20;
    [Tooltip("格子大小 大于1的均为1")] public float gridSize = 1f;
    [Tooltip("尖顶 默认平顶的")] public bool pointyTop = true;
    public bool showTest = false;
    private EmbattleHexagonCell[] _mCells;
    private EmbattleHexagonMesh _mHexagonMesh;
    private Material _mMaterial;

    private Tweener _tweener;

    private float _mOffsetX;
    private float _mOffsetY;
    EmbattleCellColorType _defaultColorType = EmbattleCellColorType.Blue;
    public float OffsetX => _mOffsetX;
    public float OffsetY => _mOffsetY;
    private float _hexagonXDis = 0;
    private float _hexagonYDis = 0;

    private void Start()
    {
        if (!showTest) return;
        InitializeHexagon(pointyTop, rowNum, colNum, gridSize, 5, EmbattleCellColorType.Transparent);
        ShowGridRender(true);
    }

    public void InitializeHexagon(bool pointyTop, int width, int height, float cellSize, int hexagonTexGridNum,
        EmbattleCellColorType defaultColorType, Dictionary<int, int> hidDic = null)
    {
        rowNum = width;
        colNum = height;
        gridSize = cellSize;
        this._defaultColorType = defaultColorType;
        this.pointyTop = pointyTop;
        transform.localPosition = new Vector3(0, 0, 0); //Vector3.zero;
        if (!_mMaterial)
        {
            _mMaterial = Instantiate(MeshMaterial);
        }

        InitializeGrid(hexagonTexGridNum, hidDic);
    }


    public void ShowGridRender(bool show)
    {
        _mHexagonMesh.ShowRender(show);
        KillTween();
        Color currentColor = _mMaterial.GetColor(S_ColorID);
        currentColor.a = 1.0f;
        _mMaterial.SetColor(S_ColorID, currentColor);
    }

    public void ChangeGridColor(int x, int y, EmbattleCellColorType cellColorType)
    {
        int index = y * rowNum + x;
        EmbattleHexagonCell cell = _mCells[index];
        if (cell == null) return;
        cell.ColorType = cellColorType;
        _mHexagonMesh.Triangulate(_mCells);
    }

    public void ChangeGridColorOnly(int x, int y, EmbattleCellColorType cellColorType)
    {
        int index = y * rowNum + x;
        if (index < 0 || index >= _mCells.Length) return;
        EmbattleHexagonCell cell = _mCells[index];
        if (cell == null) return;
        cell.ColorType = cellColorType;
    }

    public void HexagonMeshTriangulate()
    {
        _mHexagonMesh.Triangulate(_mCells);
    }

    private void InitializeGrid(int hexagonTexGridNum, Dictionary<int, int> hidDic = null)
    {
        _mCells = new EmbattleHexagonCell[rowNum * colNum];
        EmbattleHexagonMetrics.Initialize(gridSize, hexagonTexGridNum);
        _hexagonXDis = HexagonGridUtil.HexagonXDistance(pointyTop, gridSize);
        _hexagonYDis = HexagonGridUtil.HexagonYDistance(pointyTop, gridSize);
        _mHexagonMesh = GetComponentInChildren<EmbattleHexagonMesh>();

        _mHexagonMesh.InitUVMap(hexagonTexGridNum);


        // 创建mesh，创建地图
        for (int z = 0, i = 0; z < colNum; ++z)
        {
            for (int x = 0; x < rowNum; ++x)
            {
                var tmpI = i++;
                if (hidDic != null && hidDic.ContainsKey(tmpI)) continue;
                CreateCell(x, z, tmpI);
            }
        }

        ConstructMesh(hexagonTexGridNum);
        AdjustPosition();

        _mHexagonMesh.ShowRender(true);
    }

    private void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = HexagonGridUtil.GetGridMiddlePositionX(pointyTop, x, z, _hexagonXDis);
        position.y = 0f;
        position.z = HexagonGridUtil.GetGridMiddlePositionY(pointyTop, x, z, _hexagonYDis);
        EmbattleHexagonCell cell = _mCells[i] = new EmbattleHexagonCell();
        cell.CenterPosition = position;
        cell.Coordinates = EmbattleHexagonCoordinates.FromOffsetCoordinates(x, z);
        cell.Color = Color.blue;
        cell.ColorType = _defaultColorType;
    }

    private void ConstructMesh(int hexagonTexGridNum)
    {
        // 构建mesh
        _mHexagonMesh.Triangulate(_mCells);
        _mHexagonMesh.SetMaterial(_mMaterial);
    }

    private void AdjustPosition()
    {
        float positionX = HexagonGridUtil.GetHexagonMeshMiddlePositionX(pointyTop, rowNum, colNum, gridSize);
        float positionY = HexagonGridUtil.GetHexagonMeshMiddlePositionY(pointyTop, rowNum, colNum, gridSize);

        Vector3 oldPosition = transform.localPosition;
        transform.localPosition = new Vector3(oldPosition.x - positionX, oldPosition.y, oldPosition.z - positionY);
    }

    public void Fade(float time, float endValue = 0)
    {
        KillTween();
        _tweener = DOTween.To(() => _mMaterial.GetColor(S_ColorID).a,
                (value) =>
                {
                    // 获取当前的颜色并更新 alpha 值
                    Color currentColor = _mMaterial.GetColor(S_ColorID);
                    currentColor.a = value;
                    _mMaterial.SetColor(S_ColorID, currentColor);
                },
                endValue, // 目标 alpha 值
                time)
            .OnComplete(() => { ShowGridRender(false); });
    }

    private void KillTween()
    {
        if (_tweener == null) return;
        _tweener.Kill();
        _tweener = null;
    }

    private void OnDestroy()
    {
        KillTween();
        _mHexagonMesh = null;
        _mCells = null;
        MeshMaterial = null;
        Destroy(_mMaterial);
        _mMaterial = null;
    }
}