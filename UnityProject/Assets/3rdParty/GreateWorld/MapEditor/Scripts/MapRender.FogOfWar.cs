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
    public class CampaignFogInfo
    {
        public int Id;
        public int MapId;
        public int FogId;
        public Vector3 FogPos;
        public List<Vector3> FogGrid = new List<Vector3>();
        public string Locked;
        public string BubbleCon;
        public string BubbleCoor;
        public string UnlockCon;
        public string TipInfo;
        public string UnlockAnimationTime;
    }
    public partial class MapRender : MonoBehaviour
    {
        private Dictionary<int, List<CampaignFogInfo>> _campaignFogInfos = new Dictionary<int, List<CampaignFogInfo>>();

        public Dictionary<int, List<CampaignFogInfo>> CampaignFogInfos
        {
            get => _campaignFogInfos;
        }
        public void AddCampaignFogInfo(CampaignFogInfo info)
        {
            if (_campaignFogInfos.ContainsKey(info.MapId))
            {
                if (info.Id == -1)
                {
                    // 如果Id为-1，表示是新添加的FogInfo 遍历数据获取最大Id
                    int maxId = 0;
                    foreach (var fogInfo in _campaignFogInfos[info.MapId])
                    {
                        if (fogInfo.Id > maxId)
                        {
                            maxId = fogInfo.Id;
                        }
                    }
                    info.Id = maxId + 1; // 设置新的Id为最大Id加1

                }
                if (_campaignFogInfos[info.MapId].Exists(f => f.Id == info.Id))
                {
                    // Debug.LogWarning($"Fog info with Id {info.Id} already exists in MapId {info.MapId}. Updating existing entry.");
                    _campaignFogInfos[info.MapId].RemoveAll(f => f.Id == info.Id);
                }
                _campaignFogInfos[info.MapId].Add(info);
            }
            else
            {
                _campaignFogInfos.Add(info.MapId, new List<CampaignFogInfo> { info });
            }
        }

        //根据地图id和FogId删除一个CampaignFogInfo
        public void RemoveCampaignFogInfo(int mapId, int fogId)
        {
            if (_campaignFogInfos.ContainsKey(mapId))
            {
                _campaignFogInfos[mapId].RemoveAll(f => f.FogId == fogId);
            }
        }

    }
}
#endif