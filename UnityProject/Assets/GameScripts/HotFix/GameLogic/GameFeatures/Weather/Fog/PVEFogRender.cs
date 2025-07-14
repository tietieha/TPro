using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using XLua;
using Hex = GEngine.MapEditor.Hex;

[LuaCallCSharp]
public class PVEFogRender : MonoBehaviour
{
    private int DefaultFogOfWarId = 0;
    private int gridWidth = 0;
    private int gridHeight = 0;
    private float hexRadius = 1f;
    private int[] gridData;

    private Camera _camera;
    private CameraVisibleRect _cameraVisibleRect;
    private Rect viewRect;


    private Dictionary<int, FogOfWarType>
        FogOfWarTypeDict = new Dictionary<int, FogOfWarType>(); // fogid -> FogOfWarType

    public LayerMask renderLayer;
    public int BlockWCount = 8;
    public int BlockHCount = 8;

    public RenderUnitRes NormalRenderUnitRes;
    public RenderUnitRes FadeRenderUnitRes;

    public BlockData[,] BlockDatas;

    #region Render
    private CommandBuffer _commandBuffer;

    private void InitializeCommandBuffer()
    {
        if (_commandBuffer == null && _camera != null)
        {
            _commandBuffer = new CommandBuffer();
            _commandBuffer.name = "PVEFogRender CommandBuffer";
            _camera.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, _commandBuffer);
        }
    }



    #endregion

    public void SetCamera(Camera camera)
    {
        if (camera == null)
        {
            Debug.LogError("Camera is null in PVEFogRender.");
            return;
        }

        _camera = camera;
    }
    public void SetCameraVisibleRect(CameraVisibleRect cameraVisibleRect)
    {
        _cameraVisibleRect = cameraVisibleRect;
    }

    private void Update()
    {
        // Leon-TODO: 不用一直画
        Draw();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="index">fog id</param>
    /// <param name="data">配置数据</param>
    public void SetData(int gridWidth, int gridHeight, float hexRadius, int[] data)
    {
        if (data == null)
        {
            Debug.LogError("Invalid data provided for PVEFogRender.");
            return;
        }

        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        this.hexRadius = hexRadius;
        gridData = data;

        BlockDatas = new BlockData[BlockWCount, BlockHCount];
        Vector2 wh = Hex.GetWH(gridWidth, gridHeight);
        Vector2 blockWH = wh / new Vector2(BlockWCount, BlockHCount);
        Debug.Log("格子宽高: " + wh + ", 每个Block的宽高: " + blockWH);
        // 获取整个地图的边界
        Vector3 minPos = Vector3.zero;
        Vector3 maxPos = new Vector3(blockWH.x, 0, -blockWH.y);
        Debug.Log("地图边界: " + minPos + " - " + maxPos);
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                int index = y * gridWidth + x;
                if (index < data.Length)
                {
                    var fogId = data[index];
                    if (fogId > DefaultFogOfWarId)
                    {
                        Vector3 position = Hex.OffsetToWorld(x, y, hexRadius);

                        // 计算相对于地图最小位置的偏移量
                        float relativeX = position.x - minPos.x;
                        float relativeZ = -position.z - (-minPos.z); // 反转z轴并计算相对位置

                        // 计算block索引
                        int blockX = (int)(relativeX / blockWH.x);
                        int blockY = (int)(relativeZ / blockWH.y);

                        blockX = Math.Clamp(blockX, 0, BlockWCount - 1);
                        blockY = Math.Clamp(blockY, 0, BlockHCount - 1);

                        // 计算blockX， blockY对应的Bounds
                        if (BlockDatas[blockX, blockY] == null)
                        {
                            BlockDatas[blockX, blockY] = new BlockData
                            {
                                blockX = blockX,
                                blockY = blockY,
                                bounds = new Bounds(
                                    new Vector3(
                                        minPos.x + blockX * blockWH.x + blockWH.x / 2,
                                        0,
                                        minPos.z - (blockY * blockWH.y + blockWH.y / 2)), // 注意z轴是负方向
                                    new Vector3(blockWH.x, 0, blockWH.y))
                            };
                        }

                        BlockDatas[blockX, blockY].perDataList.Add(new PerData
                        {
                            index = index,
                            fogId = fogId,
                            position = position
                        });
                    }
                }
            }
        }
    }

    public void SetFogOfWarType(int fogId, bool isUnlock)
    {
        var fogType = isUnlock ? FogOfWarType.Unlock : FogOfWarType.Lock;
        if (FogOfWarTypeDict.ContainsKey(fogId))
        {
            FogOfWarTypeDict[fogId] = fogType;
        }
        else
        {
            FogOfWarTypeDict.Add(fogId, fogType);
        }
    }

    public void UnlockFog(int fogId, float fadeTime)
    {
        if (FogOfWarTypeDict.ContainsKey(fogId))
        {
            FogOfWarTypeDict[fogId] = FogOfWarType.Fade;
            if (FadeRenderUnitRes != null && FadeRenderUnitRes.material != null)
            {
                FadeRenderUnitRes.material.SetColor("_Color", Color.red);
                FadeRenderUnitRes.material.DOColor(Color.black, fadeTime)
                    .OnUpdate(() =>
                    {
                        // draw
                    })
                    .onComplete = () =>
                    {
                        // draw
                        // 淡出完成后，确保类型被设置为Show
                        if (FogOfWarTypeDict.ContainsKey(fogId))
                        {
                            FogOfWarTypeDict[fogId] = FogOfWarType.Unlock;
                        }
                    };
            }
        }
    }

    public void Draw()
    {
        if (_cameraVisibleRect == null || _camera == null)
        {
            return;
        }
        InitializeCommandBuffer();
        _commandBuffer.Clear();

        var rectVisible = _cameraVisibleRect.GetVisibleRect();
        // 将输入的2D rect转换为3D bounds，因为我们的block使用的是3D bounds
        Bounds viewBounds = new Bounds(
            new Vector3(rectVisible.center.x, 0, rectVisible.center.y),
            new Vector3(rectVisible.width, 0, rectVisible.height)
        );

        // 判断renderlayer 是不是nothing
        if (renderLayer.value == 0) return;
        int layer = (int)Mathf.Log(renderLayer.value, 2);

        // _camera.Render();

        // 遍历所有block
        for (int y = 0; y < BlockHCount; y++)
        {
            for (int x = 0; x < BlockWCount; x++)
            {
                BlockData block = BlockDatas[x, y];
                if (block == null) continue;

                // 检查block的bounds是否与视图矩形相交
                if (viewBounds.Intersects(block.bounds))
                {
                    // 如果相交，则渲染这个block中的所有PerData
                    foreach (PerData data in block.perDataList)
                    {
                        // 根据fogId获取对应的战争迷雾类型
                        // 解锁了的画
                        FogOfWarType fogType;
                        if (!FogOfWarTypeDict.TryGetValue(data.fogId, out fogType))
                        {
                            fogType = FogOfWarType.Unlock; // 默认隐藏
                        }

                        // 根据不同的战争迷雾类型选择不同的渲染资源
                        RenderUnitRes renderRes = null;
                        switch (fogType)
                        {
                            case FogOfWarType.Unlock:
                                renderRes = NormalRenderUnitRes;
                                break;
                            case FogOfWarType.Fade:
                                renderRes = FadeRenderUnitRes;
                                break;
                            case FogOfWarType.Lock:
                            default:
                                continue;
                        }

                        // 实际渲染逻辑
                        if (renderRes != null && renderRes.mesh != null && renderRes.material != null)
                        {
                            // Leon-TODO: drawinstance, 现在是动态合批
                            // _commandBuffer.DrawMesh(
                            //     renderRes.mesh,
                            //     Matrix4x4.TRS(data.position, Quaternion.identity, renderRes.Scale),
                            //     renderRes.material
                            // );
                            Graphics.DrawMesh(
                                renderRes.mesh,
                                Matrix4x4.TRS(data.position, Quaternion.identity, renderRes.Scale),
                                renderRes.material,
                                layer,
                                _camera
                            );
                        }
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (_commandBuffer != null)
        {
            if (_camera != null)
            {
                _camera.RemoveCommandBuffer(CameraEvent.BeforeForwardOpaque, _commandBuffer);
            }
            _commandBuffer.Dispose();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_cameraVisibleRect == null)
        {
            return;
        }
        var rectVisible = _cameraVisibleRect.GetVisibleRect();
        // 将输入的2D rect转换为3D bounds，因为我们的block使用的是3D bounds
        Bounds viewBounds = new Bounds(
            new Vector3(rectVisible.center.x, 0, rectVisible.center.y),
            new Vector3(rectVisible.width, 0, rectVisible.height)
        );
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(viewBounds.center, viewBounds.size);

        // 遍历所有block
        for (int y = 0; y < BlockHCount; y++)
        {
            for (int x = 0; x < BlockWCount; x++)
            {
                BlockData block = BlockDatas[x, y];
                if (block == null) continue;

                // 检查block的bounds是否与视图矩形相交
                if (viewBounds.Intersects(block.bounds))
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(block.bounds.center, block.bounds.size);
                }
            }
        }
    }

    private void OnValidate()
    {
        // 编辑器模式下强制限制为单 Layer
        if (renderLayer.value != 0 && (renderLayer.value & (renderLayer.value - 1)) != 0)
        {
            Debug.LogWarning("只能选择单个 Layer！已自动修正。");
            // 取最低位的 Layer（例如 5 (101) 会变成 1 (001)）
            renderLayer.value = renderLayer.value & -renderLayer.value;
        }
    }
#endif
}

public class BlockData
{
    public int blockX;
    public int blockY;
    public Bounds bounds;
    public List<PerData> perDataList = new List<PerData>();
}

public class PerData
{
    public int index;
    public int fogId;
    public Vector3 position;
}

[Serializable]
public class RenderUnitRes
{
    public Mesh mesh;
    public Material material;
    public Vector3 Scale = Vector3.one;
}

public enum FogOfWarType
{
    Lock,
    Unlock, // 解锁了的绘制
    Fade
}