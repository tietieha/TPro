// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-11-11 10:17 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

#if UNITY_EDITOR
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace GEngine.MapEditor
{
	public class MapEditorUIManager : MonoBehaviour
	{
		public Canvas canvas;
		private Dictionary<int, MapEditorUILevel> _zoneLevelLabel = new Dictionary<int, MapEditorUILevel>();

		private void Awake()
		{
			LevelLabelInit();
		}

		private void OnEnable()
		{
			MapEditorEventCenter.AddListener(MapEditorEvent.MapEditorToolChanged, EditorToolChanged);
			
			MapEditorEventCenter.AddListener(MapEditorEvent.LevelLabelUpdateEvent, UpdateLevelLabel);
			MapEditorEventCenter.AddListener(MapEditorEvent.LevelLabelUpdateAllEvent, UpdateAllLevelLabel);
			
			MapEditorEventCenter.AddListener(MapEditorEvent.BusinessNameUpdateEvent, UpdateBusinessName);
			MapEditorEventCenter.AddListener(MapEditorEvent.BusinessNameUpdateAllEvent, UpdateAllBusinessName);
		}

		private void OnDisable()
		{
			MapEditorEventCenter.RemoveListener(MapEditorEvent.LevelLabelUpdateEvent, UpdateLevelLabel);
			MapEditorEventCenter.RemoveListener(MapEditorEvent.LevelLabelUpdateAllEvent, UpdateAllLevelLabel);
			
			MapEditorEventCenter.RemoveListener(MapEditorEvent.BusinessNameUpdateEvent, UpdateBusinessName);
			MapEditorEventCenter.RemoveListener(MapEditorEvent.BusinessNameUpdateAllEvent, UpdateAllBusinessName);
		}

		public void Reset()
		{
			LevelLabelReset();
			BusinessNameReset();
		}

		#region tool
		private void EditorToolChanged(MapToolOP tool, MapToolOP lastTool)
		{
			// switch (lastTool)
			// {
			// 	case MapToolOP.EditBusinessZone:
			// 		businessLayer.gameObject.SetActiveEx(false);
			// 		break;
			// }
			//
			// switch (tool)
			// {
			// 	case MapToolOP.EditBusinessZone:
			// 		businessLayer.gameObject.SetActiveEx(true);
			// 		UpdateAllBusinessName();
			// 		break;
			// }
		}
		#endregion
		
		#region level label
		public void UpdateLevelLabel(int zoneid)
		{
			var map = MapRender.instance.map;
			if (map == null)
			{
				Debug.LogError("UpdateLevelLabel hava no map");
				return;
			}

			var zone = map.GetZone(zoneid);
			// 这个zone已经没了
			if (zone == null || zone.hexagon == null)
			{
				LevelLabelRecycle(zoneid);
			}
			else
			{
				if (!_zoneLevelLabel.TryGetValue(zoneid, out MapEditorUILevel uilevel))
				{
					var go = _levelLabelPool.Get();
					uilevel = go.GetComponent<MapEditorUILevel>();
					_zoneLevelLabel.Add(zoneid, uilevel);
				}

				var harborPos = zone.hexagon.Pos;
				uilevel.transform.position = harborPos;
				uilevel.SetText(zone.level.ToString());
			}
		}

		public void UpdateAllLevelLabel()
		{
			var map = MapRender.instance.map;
			if (map == null)
			{
				Debug.LogError("UpdateAllLevelLabel hava no map");
				return;
			}

			LevelLabelReset();

			foreach (var zone in map.zones.Values)
			{
				UpdateLevelLabel(zone.index);
			}
		}

		

		public Transform levelLayer;
		public GameObject levelPrefab;
		private ObjectPool<GameObject> _levelLabelPool;


		void LevelLabelInit()
		{
			_levelLabelPool = new ObjectPool<GameObject>(LevelLabelCreate, LevelLabelGet, LevelLabelRelease,
				LevelLabelDestroy, true, 100, 1000);
		}

		private GameObject LevelLabelCreate()
		{
			GameObject gameObject = Instantiate(levelPrefab, levelLayer);
			return gameObject;
		}

		void LevelLabelGet(GameObject gameObject)
		{
			gameObject.SetActive(true); //显示敌人
		}

		void LevelLabelRelease(GameObject gameObject)
		{
			gameObject.SetActive(false);
		}

		void LevelLabelDestroy(GameObject gameObject)
		{
			Destroy(gameObject);
		}

		void LevelLabelReset()
		{
			foreach (var val in _zoneLevelLabel.Values)
			{
				_levelLabelPool.Release(val.gameObject);
			}

			_zoneLevelLabel.Clear();
		}

		void LevelLabelRecycle(int zoneid)
		{
			// 如果有label，回收
			if (_zoneLevelLabel.ContainsKey(zoneid))
			{
				_levelLabelPool.Release(_zoneLevelLabel[zoneid].gameObject);
				_zoneLevelLabel.Remove(zoneid);
			}
		}
		#endregion

		#region 商圈
		public Transform  businessLayer;
		public GameObject businessNamePrefab;
		private Dictionary<int, GameObject> _businessNames = new Dictionary<int, GameObject>();

		private void UpdateAllBusinessName()
		{
			var map = MapRender.instance.map;
			if (map == null)
			{
				Debug.LogError("no map");
				return;
			}

			foreach (var businessid in map.businesses.Keys)
			{
				UpdateBusinessName(businessid);
			}
		}

		public void UpdateBusinessName(int businessid)
		{
			var map = MapRender.instance.map;
			if (map == null)
			{
				Debug.LogError("no map");
				return;
			}

			var businesses = map.businesses;
			if (!businesses.ContainsKey(businessid))
			{
				Debug.LogError($"没有商圈信息{businessid}");
				return;
			}

			var business = businesses[businessid];
			if (!_businessNames.TryGetValue(businessid, out GameObject prefab))
			{
				prefab = Instantiate(businessNamePrefab, businessLayer);
				_businessNames.Add(businessid, prefab);
			}

			var tmp = prefab.GetComponent<TextMeshProUGUI>();
			tmp.SetText(business.Name);
			tmp.SetTextStyle(business.Vertical ? "Vertical" : "Normal");
			var rectTrans = prefab.GetComponent<RectTransform>();
			rectTrans.localPosition = new Vector3(business.Pos.x, business.Pos.y, 0);
			rectTrans.localRotation = Quaternion.Euler(0, 0, business.Rotation);
			// rectTrans.localScale = Vector3.one;
			rectTrans.sizeDelta = business.Rect;
		}

		public void BusinessNameReset()
		{
			foreach (var go in _businessNames.Values)
			{
				Destroy(go);
			}
			_businessNames.Clear();
		}

		#endregion

	}
}

#endif