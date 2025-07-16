#if UNITY_EDITOR
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using FileMode = System.IO.FileMode;
using Random = UnityEngine.Random;
namespace GEngine.MapEditor
{
    public class InteractionBackupInfos
    {
        public int Id;
        public List<Vector2Int> BlockPoints = new List<Vector2Int>();
        public string Comments = string.Empty;
    }
    public partial class MapRender : MonoBehaviour
    {
        /// <summary>
        /// 地图信息
        /// </summary>
        public List<CampaignMapInfo> CampaignMapList = new List<CampaignMapInfo>();

        private HashSet<int> _usedInteractionIds = new HashSet<int>();
        private static int InteractionStarId = 10000;
        public int SelectInteractBlockId { get; set; }

        public int InteractionIndex
        {
            get
            {
                InteractionStarId += 1;
                // 确保生成的ID不在已使用的ID中
                while (_usedInteractionIds.Contains(InteractionStarId))
                {
                    InteractionStarId += 1;
                }
                return InteractionStarId;
            }
        }

        /// <summary>
        /// 所有地图的交互点数据
        /// </summary>
        public Dictionary<int, List<InteractionPoint>> AllInteractionPoints = new Dictionary<int, List<InteractionPoint>>();

        private int _currentMapId = 0;

        public int CurrentMapId
        {
            get => _currentMapId;
            set
            {
                if (_currentMapId != value)
                {
                    _currentMapId = value;
                }
            }
        }

        public Dictionary<int, InteractionBackupInfos> ClearCurrentMapInteractionPoints()
        {
            var backupInfos = new Dictionary<int, InteractionBackupInfos>();
            // var blockPoints = new Dictionary<int, List<Vector2Int>>();
            if (AllInteractionPoints.TryGetValue(_currentMapId, out var points))
            {
                foreach (var point in points)
                {
                    _usedInteractionIds.Remove(point.id);
                    if (point.BlockPoints != null)
                    {
                        backupInfos[point.id] = new InteractionBackupInfos
                        {
                            Id = point.id,
                            BlockPoints = new List<Vector2Int>(point.BlockPoints),
                            Comments = point.comments
                        };
                    }
                }
                points.Clear();
            }
            // _usedInteractionIds.Clear();
            InteractionStarId = 10000;
            return backupInfos;
        }

        public void ResetInteractionIndexId()
        {
            InteractionStarId = 10000;
        }

        public List<InteractionPoint> CurrentMapInteractionPoints
        {
            get
            {
                if (AllInteractionPoints.TryGetValue(CurrentMapId, out var points))
                {
                    return points;
                }
                return null;
            }
        }

        public void AddInteractionPoint(int mapId, InteractionPoint point)
        {
            if (!AllInteractionPoints.ContainsKey(mapId))
            {
                AllInteractionPoints[mapId] = new List<InteractionPoint>();
            }
            AllInteractionPoints[mapId].Add(point);
            _usedInteractionIds.Add(point.id);
            InteractionStarId = Mathf.Max(InteractionStarId, point.id);
        }

        public List<Vector2Int> GetBlockPoints(int eventId)
        {
            var ponits = CurrentMapInteractionPoints;
            if (ponits != null)
            {
                foreach (var point in ponits)
                {
                    if (point.id == eventId)
                    {
                        return point.BlockPoints;
                    }
                }
            }

            return null;
        }

        public void ClearInteractionBlocks(int eventId)
        {
            var ponits = CurrentMapInteractionPoints;
            if (ponits != null)
            {
                foreach (var point in ponits)
                {
                    if (point.id == eventId)
                    {
                        point.BlockPoints.Clear();
                        break;
                    }
                }
            }
        }

        //向地图数据中添加Excel中的地图配置
        public void AddCampaignMap(CampaignMapInfo mapInfo)
        {
            if (CampaignMapList == null)
            {
                CampaignMapList = new List<CampaignMapInfo>();
            }
            var oldInfo = CampaignMapList.Find(m => m.Id == mapInfo.Id);
            if (oldInfo != null)
            {
                // 如果已经存在，则更新
                oldInfo.Name = mapInfo.Name;
            }
            else
            {
                // 如果不存在，则添加
                CampaignMapList.Add(mapInfo);
            }
        }
        private const string WORLD_MAP_Prefab_PATH =
            "Assets/GameAssets/Scenes/Campaign/{0}/prefabs/{0}_map.prefab";
        private const string WORLD_RES_RENDER_DATA_PATH =
            "Assets/GameAssets/Scenes/Campaign/{0}/prefabs/WorldResRenderData.prefab";
        public void CreateCampaignMap(CampaignMapInfo mapInfo)
        {
            if (CampaignMapList == null)
            {
                CampaignMapList = new List<CampaignMapInfo>();
            }
            var oldInfo = CampaignMapList.Find(m => m.Id == mapInfo.Id);
            if (oldInfo != null)
            {
                // 如果已经存在，则更新
                oldInfo.Name = mapInfo.Name;
                if (!string.IsNullOrEmpty(oldInfo.MapUWFilePath))
                {
                    oldInfo.MapUWFilePath = null;
                }
            }
            else
            {
                mapInfo.PrefabFilePath = string.Format(WORLD_MAP_Prefab_PATH, mapInfo.Name);
                mapInfo.RenderDataFilPath = string.Format(WORLD_RES_RENDER_DATA_PATH, mapInfo.Name);
                // 如果不存在，则添加
                CampaignMapList.Add(mapInfo);
            }
        }
        public int GenerateNewMapId()
        {
            var maxId = 0;
            foreach (var map in CampaignMapList)
            {
                if (map.Id > maxId)
                {
                    maxId = map.Id;
                }
            }
            return maxId + 1;
        }
    }
}
#endif