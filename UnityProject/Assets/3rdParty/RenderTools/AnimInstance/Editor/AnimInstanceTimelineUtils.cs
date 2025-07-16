// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2022-10-13 11:06 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
#endif
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace RenderTools.AnimInstance
{
	public class AnimInstanceTimelineUtils
	{
		[MenuItem("Assets/AnimInatance/转变为AnimInstance Timeline")]
		private static void AnimInstanceTimeline()
		{
			// 拷贝timeline
			// 遍历所有track，
			//		找到animation track，创建一个animInstance track
			//			遍历clip，创建animInstance Clip
			// 最后删除所有animation track

			if (!EditorUtility.DisplayDialog("提示", $"是否将所选Timeline中的Animation转为AnimInstance", "OK"))
				return;

			var guids = Selection.assetGUIDs;

			int index = 0;

			try
			{
				foreach (var guid in guids)
				{
					var assetPath = AssetDatabase.GUIDToAssetPath(guid);
					if (string.IsNullOrEmpty(assetPath))
						continue;

					if (EditorUtility.DisplayCancelableProgressBar("正在转换",
						    string.Format("已完成：{0}/{1}", index + 1, guids.Length),
						    1.0f * (index + 1) / guids.Length))
						break;
					index++;

					var asset = AssetDatabase.LoadAssetAtPath<TimelineAsset>(assetPath);
					if (asset == null)
						continue;

					ConvertTimelineAnimIns(asset, out Dictionary<string, BindingPair> pairsDic);
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
			finally
			{
				EditorUtility.ClearProgressBar();
			}

			EditorUtility.DisplayDialog("提示", "处理完毕", "OK");
		}


		[MenuItem("CONTEXT/PlayableDirector/Convert To AnimInstance")]
		private static void _ConvertToAnimInstance(MenuCommand command)
		{
			var director = command.context as PlayableDirector;
			if (director != null)
			{
				ConvertPlayableDirectorAnimIns(director);
			}
		}

		private static void ConvertPlayableDirectorAnimIns(PlayableDirector director)
		{
			if (director.playableAsset == null)
			{
				EditorUtility.DisplayDialog("提示", "没有playableAsset", "OK");
				return;
			}
			
			// 1 获取bindings
			// 2 使用新的timeline asset
			// 3 设置bind
			
			// 1
			SerializedObject serializedObject = new SerializedObject(director);
			SerializedProperty sceneBindings = serializedObject.FindProperty("m_SceneBindings");
			var serializedProperties = new SerializedProperty[sceneBindings.arraySize];
			for (int i = 0; i < sceneBindings.arraySize; ++i)
			{
				serializedProperties[i] = sceneBindings.GetArrayElementAtIndex(i);
			}

			// track ins id,			轨道名
			Dictionary<string, Dictionary<string, BindingPropertyPair>> playerBindDict =
				new Dictionary<string, Dictionary<string, BindingPropertyPair>>();
			Dictionary<Type, List<Component>> allComponents = new Dictionary<Type, List<Component>>();
			Dictionary<string, BindingPropertyPair> bindingDict = new Dictionary<string, BindingPropertyPair>();
			
			foreach (var bind in director.playableAsset.outputs)
			{
				Type t = bind.outputTargetType;
				if (t == null)
					continue;
				
				TrackAsset ta = bind.sourceObject as TrackAsset;
				string groupname = ta.GetGroup() ? ta.GetGroup().name : "null";

				if (!playerBindDict.TryGetValue(bind.streamName, out Dictionary<string, BindingPropertyPair> binds))
				{
					binds = new Dictionary<string, BindingPropertyPair>();
					playerBindDict.Add(bind.streamName, binds);
				}

				if (!binds.ContainsKey(groupname))
				{
					foreach (var prop in serializedProperties)
					{
						if (prop.FindPropertyRelative("key").objectReferenceValue == bind.sourceObject)
						{
							Object obj = prop.FindPropertyRelative("value").objectReferenceValue;
							if (t == typeof(GameObject))
							{
								binds.Add(groupname, new BindingPropertyPair{binding = bind, obj = obj});
							}
							else
							{
								List<Component> components;
								if (!allComponents.TryGetValue(bind.outputTargetType, out components))
								{
									components = director.gameObject.GetComponentsInChildren(bind.outputTargetType, true).ToList();
									allComponents.Add(bind.outputTargetType, components);
								}

								
								GameObject go = components
									.Where(item => item == obj)
									.Select(item => item.gameObject).FirstOrDefault();

								binds.Add(groupname, new BindingPropertyPair{binding = bind, go = go, obj = obj});
							}
							break;
						}
					}
				}
			}

			// 2
			TimelineAsset timeline = ConvertTimelineAnimIns(director.playableAsset as TimelineAsset,  out Dictionary<string, BindingPair> pairsDic);
			
			// 3
			director.playableAsset = timeline;

			// TODO 如果之前的轨道存在一样的名字怎么办
			foreach (var bind in director.playableAsset.outputs)
			{
				int trackid = bind.sourceObject.GetInstanceID();
				
				if (bind.streamName.StartsWith("AnimIns-"))
				{
					// string oldname = bind.streamName.Replace("AnimIns-", "");
					
					if (pairsDic.ContainsKey(bind.streamName))
					{
						BindingPair pair = pairsDic[bind.streamName];
						if (playerBindDict.TryGetValue(pair.name, out Dictionary<string, BindingPropertyPair> pb))
						{
							if (pb.ContainsKey(pair.groupname))
							{
								BindingPropertyPair prop = pb[pair.groupname];
								director.SetGenericBinding(bind.sourceObject, prop.go);
							}
						}
					}
				}
				else
				{
					if (playerBindDict.TryGetValue(bind.streamName, out Dictionary<string, BindingPropertyPair> pb))
					{
						TrackAsset ta = bind.sourceObject as TrackAsset;
						string groupname = ta.GetGroup() ? ta.GetGroup().name : "null";
						if (pb.ContainsKey(groupname))
						{
							BindingPropertyPair prop = pb[groupname];
							director.SetGenericBinding(bind.sourceObject, prop.obj);
						}
					}
				}
			}

			serializedObject = null;
			sceneBindings = null;
			serializedProperties = null;
			allComponents.Clear();
			bindingDict.Clear();
			playerBindDict.Clear();
			TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved | RefreshReason.ContentsModified);
		}

		private static TimelineAsset ConvertTimelineAnimIns(TimelineAsset asset, out Dictionary<string, BindingPair> pairsDic)
		{
			pairsDic = new Dictionary<string, BindingPair>();
			
			string assetPath = AssetDatabase.GetAssetPath(asset);
			string newPath = Path.Combine(Path.GetDirectoryName(assetPath),
				Path.GetFileNameWithoutExtension(assetPath) + "_instance" + Path.GetExtension(assetPath));

			AssetDatabase.CopyAsset(assetPath, newPath);
			// AssetDatabase.Refresh();
			
			TimelineAsset s_TimelineAsset = AssetDatabase.LoadAssetAtPath<TimelineAsset>(newPath);
			List<int> dealedTrack = new List<int>();
			List<TrackAsset> toDeleteTrack = new List<TrackAsset>();
			
			// s_TimelineAsset.
			for (int i = 0; i < s_TimelineAsset.outputTrackCount; i++)
			{
				TrackAsset track = s_TimelineAsset.GetOutputTrack(i);
				if (dealedTrack.Contains(track.GetInstanceID()))
					continue;
				dealedTrack.Add(track.GetInstanceID());

				if (track.GetType() == typeof(AnimationTrack))
				{
					if (track.GetClips().ToArray().Length <= 0)
						continue;
					
					toDeleteTrack.Add(track);
				}
			}

			if (toDeleteTrack.Count <= 0)
			{
				return asset;
			}
			
			GroupTrack AnimInsGroup = s_TimelineAsset.CreateTrack<GroupTrack>(null, "AnimIns Track");
			Dictionary<string, GroupTrack> dic = new Dictionary<string, GroupTrack>();

			for (int i = 0; i < toDeleteTrack.Count; i++)
			{
				TrackAsset track = toDeleteTrack[i];
				GroupTrack g = track.GetGroup();
				GroupTrack parent = AnimInsGroup;
				// if (g != null)
				// {
				// 	if (!dic.TryGetValue(g.name, out parent))
				// 	{
				// 		parent = s_TimelineAsset.CreateTrack<GroupTrack>(AnimInsGroup, g.name);
				// 		dic.Add(g.name, parent);
				// 	}
				// }

				AnimInstanceTrack animInsTrack =
					s_TimelineAsset.CreateTrack<AnimInstanceTrack>(parent, $"AnimIns-{track.name}");
				foreach (var clip in track.GetClips())
				{
					TimelineClip animInsClip = animInsTrack.CreateClip<AnimInstanceClip>();
					animInsClip.start = clip.start;
					animInsClip.duration = clip.duration;
					animInsClip.displayName = clip.displayName;
					var c = animInsClip.asset as AnimInstanceClip;
					if (c != null)
						c.playName = clip.animationClip.name;
					else
						Debug.LogError("clip is null");
				}
				
				// TODO 代码删除有问题，后面再改
				// s_TimelineAsset.DeleteTrack(toDeleteTrack[i]);
				
				foreach (var clip in track.GetClips())
				{
					s_TimelineAsset.DeleteClip(clip);
				}

				// 对应关系
				string groupname = track.GetGroup() ? track.GetGroup().name : "null";
				pairsDic.Add(animInsTrack.name, new BindingPair(){name = track.name, groupname = groupname});
			}
			EditorUtility.SetDirty(s_TimelineAsset);
			toDeleteTrack.Clear();

			return s_TimelineAsset;
		}

		struct BindingPropertyPair
		{
			public PlayableBinding binding;
			public GameObject go;
			public Object obj;
		}

		struct BindingPair
		{
			public string name;
			public string groupname;
		}
	}
}