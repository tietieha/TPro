// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2022-08-01 18:10 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RenderTools.Utils;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;
using M.Battle.Render;

namespace RenderTools.AnimInstance
{
	public class AnimatorInstanceCreator : EditorWindow
	{
		private const int BoneMatrixRowCount = 3;
		private enum ShaderTextureSize
		{
			Size_Smallest_Common = 0,
			Size_Largest_Common = 1,
			Size_Most_Common = 2,
			Size_32 = 32,
			Size_64 = 64,
			Size_128 = 128,
			Size_256 = 256,
			Size_512 = 512,
			Size_1024 = 1024,
			Size_2048 = 2048,
			Size_4096 = 4096
		}

		private enum ShaderTextureQuality
		{
			Quality_8_Bit__Low = TextureFormat.RGBA32,
			Quality_16_Bit__Medium = TextureFormat.RGBAHalf,
			Quality_32_Bit__High = TextureFormat.RGBAFloat,
		}

		private static readonly string[] shaderEnum = new[]
		{
			"UWUnlit/Opaque",
		};

		private static readonly List<ShaderTextureSize> textureSizes =
			System.Enum.GetValues(typeof(ShaderTextureSize)).Cast<ShaderTextureSize>().ToList();

		private static readonly List<ShaderTextureQuality> textureQualities =
			System.Enum.GetValues(typeof(ShaderTextureQuality)).Cast<ShaderTextureQuality>().ToList();

		private static readonly string[] textureSizeNames = System.Enum.GetNames(typeof(ShaderTextureSize))
			.Select(x => x.Remove(0, 5).Replace("__", " - ").Replace("_", " ")).ToArray();

		private static readonly string[] textureQualityNames = System.Enum.GetNames(typeof(ShaderTextureQuality))
			.Select(x => x.Remove(0, 8).Replace("__", " - ").Replace("_", " ")).ToArray();

		private static readonly string s_MessageOverwrite = "覆盖此目录下已有的";
		private static readonly string s_OutFolderName = "AnimInstance";

		// GUI
		private Vector2 _scrollPos;
		private static AnimatorInstanceCreator window;

		[MenuItem("Tools/Animator Instance Generator", false, 999)]
		static void OpenWindow()
		{
			// EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
			// EditorApplication.isPlaying = true;
			window = GetWindow<AnimatorInstanceCreator>();
		}

		// 

		
		
		protected GameObject _prefab;
		private GameObject _previousPrefab;
		protected GameObject _spawnedPrefab;

		private string _fbxDir;
		private string _fbxPath;

		protected DefaultAsset _outputFolder;
		protected string _outputName;
		protected string _outputDir;
		protected string _prefsPath;
		protected bool _overwriteExist = false;

		protected Animator _animator;
		protected Animation _animation;
		protected Avatar _animAvatar;
		protected RuntimeAnimatorController _animController;

		protected List<MeshFilter> meshFilters = new List<MeshFilter>();
		protected List<SkinnedMeshRenderer> skinnedRenderers = new List<SkinnedMeshRenderer>();
		// private Dictionary<string, BakeAnimState> bakeAnimStates = new Dictionary<string, BakeAnimState>();
		protected Dictionary<Mesh, Material[]> meshMaterialsDic = new Dictionary<Mesh, Material[]>();
		protected List<AnimationClip> clipsCache = new List<AnimationClip>();
		// private List<AnimationClip> customClips = new List<AnimationClip>();
		
		private int fps = 30;
		private ShaderTextureSize selectedTextureSize = ShaderTextureSize.Size_Smallest_Common;
		private ShaderTextureQuality selectedTextureQuality = ShaderTextureQuality.Quality_16_Bit__Medium;
		private float compressionAccuracy = 1;
		private bool customCompression = false;
		private int selectedShader = 0;
		protected BakeAnimPreferences _preferences;

		private GameObject _copyComponentGO;
		private List<Component> _components = new List<Component>();
		private bool[] _componentsIsCopy;
		

		protected List<Transform> boneTransList = new List<Transform>();

		protected void OnEnable()
		{
			titleContent = new GUIContent("Animator Instance Generator");
			if (_prefab == null)
			{
				_prefab = Selection.activeGameObject;
			}

			OnPrefabChanged();
			
			_copyComponentGO = GameObject.Find("CopyComponent");
			if (_copyComponentGO == null)
			{
				_copyComponentGO = new GameObject("CopyComponent");
			}

			OnCopyComponentChanged();
		}

		protected void OnDisable()
		{
			if (_spawnedPrefab)
			{
				DestroyImmediate(_spawnedPrefab.gameObject);
				_spawnedPrefab = null;
			}

			_preferences = null;
		}

		private void Update()
		{

		}

		private void OnProjectChange()
		{
			// OnPrefabChanged();
		}

		public void SetSelectShader(int index)
		{
			selectedShader = index;
		}

		#region GUI

		private void OnGUI()
		{
			GUI.skin.label.wordWrap = true;
			
			DrawFoldOut("Asset to Bake", () =>
			{
				using (new EditorGUILayout.HorizontalScope())
				{
					_prefab =
						EditorGUILayout.ObjectField("Asset to Bake", _prefab, typeof(GameObject), true) as GameObject;
					if (GUILayout.Button("刷新", new GUILayoutOption[] {GUILayout.Width(50)}))
					{
						OnPrefabChanged();
					}
				}
				if (_previousPrefab != _prefab || _spawnedPrefab == null)
					OnPrefabChanged();

				EditorGUI.BeginChangeCheck();
				_outputFolder = (DefaultAsset) EditorGUILayout.ObjectField("Output Folder", _outputFolder, typeof(DefaultAsset), false);
				if (EditorGUI.EndChangeCheck())
				{
					OnOutputFolderChanged();
				}

				if (_overwriteExist) DrawWarning(s_MessageOverwrite);
			});

			if (_prefab != null)
			{
				var scrollScope = new GUILayout.ScrollViewScope(_scrollPos);
				using (scrollScope)
				{
					DrawFoldOut("Anim Instance Shader", () =>
					{
						EditorGUILayout.HelpBox("自身shader不支持即生效", MessageType.Info);
						selectedShader = EditorGUILayout.Popup(selectedShader, shaderEnum);
					});
					
					DrawFoldOut("Animation Setup", () =>
					{
						EditorGUILayout.ObjectField("Animation Controller", _animController,
							typeof(RuntimeAnimatorController), true);
						EditorGUILayout.ObjectField("Avatar", _animAvatar, typeof(Avatar), true);
					});
					
					DrawFoldOut("Mesh Setup", () =>
					{
						for (int i = 0; i < meshFilters.Count; i++)
						{
							bool remove = false;
							GUILayout.BeginHorizontal();
							{
								EditorGUILayout.ObjectField("Mesh Filter " + i, meshFilters[i], typeof(MeshFilter), true);
								if (GUILayout.Button("X", GUILayout.MaxWidth(20)))
									remove = true;
							}
							GUILayout.EndHorizontal();
							if (remove)
							{
								meshFilters.RemoveAt(i);
								break;
							}
						}
						// if (GUILayout.Button("+ Add MeshFilter"))
						// 	meshFilters.Add(null);

						for (int i = 0; i < skinnedRenderers.Count; i++)
						{
							bool remove = false;
							GUILayout.BeginHorizontal();
							{
								EditorGUILayout.ObjectField("Skinned Mesh " + i, skinnedRenderers[i], typeof(SkinnedMeshRenderer), true);
								if (GUILayout.Button("X", GUILayout.MaxWidth(20)))
									remove = true;
							}
							GUILayout.EndHorizontal();
							if (remove)
							{
								skinnedRenderers.RemoveAt(i);
								break;
							}
						}
						
						// if(meshFilters.Count + skinnedRenderers.Count > 1)
						// 	DrawWarning("合并模型，或者删除不烘培的模型");
						
						// if (GUILayout.Button("+ Add SkinnedMeshRenderer"))
						// 	skinnedRenderers.Add(null);
					});

					DrawFoldOut("Bake Animations", () =>
					{
						EditorGUILayout.LabelField("Custom Clips");
						for (int i = 0; i < _preferences.customClips.Count; i++)
						{
							GUILayout.BeginHorizontal();
							{
								var previous = _preferences.customClips[i];
								_preferences.customClips[i] = (AnimationClip)EditorGUILayout.ObjectField(_preferences.customClips[i], typeof(AnimationClip), false);
								if (previous != _preferences.customClips[i])
								{
									GetClips();
								}

								if (GUILayout.Button("X", GUILayout.Width(32)))
								{
									_preferences.customClips.RemoveAt(i);
									GetClips();
									GUILayout.EndHorizontal();
									break;
								}
							}
							GUILayout.EndHorizontal();
						}
						
						if (GUILayout.Button("Add Custom Animation Clip"))
						{
							_preferences.customClips.Add(null);
							GetClips();
						}
						
						var clipNames = _preferences.bakeAnimStates.dictionary.Keys.ToArray();

						GUILayout.BeginHorizontal();
						{
							if (GUILayout.Button("Select All", GUILayout.Width(100)))
							{
								foreach (var clipName in clipNames)
									_preferences.bakeAnimStates[clipName].isBake = true;
							}

							if (GUILayout.Button("Deselect All", GUILayout.Width(100)))
							{
								foreach (var clipName in clipNames)
									_preferences.bakeAnimStates[clipName].isBake = false;
							}
						}
						GUILayout.EndHorizontal();
						
						GUILayout.BeginHorizontal();
						{
							GUILayout.Label("Bake", GUILayout.MaxWidth(50));
							GUILayout.Label("Animation", GUILayout.MaxWidth(300));
							GUILayout.Label("Frame Progress");
						}
						GUILayout.EndHorizontal();
						
						EditorGUI.BeginChangeCheck();
						// foreach (var clipName in clipNames)
						for (int i = 0; i < clipNames.Length; i++)
						{
							var clipName = clipNames[i];
							AnimationClip clip = clipsCache.Find(q => q.name == clipName);
							int framesToBake = clip ? (int) (clip.length * fps) : 0;
							bool isodd = (i & 1) > 0;

							Color bgcolor = GUI.color;
							if (_preferences.bakeAnimStates[clipName].isBake)
							{
								GUI.color = Color.green;
							}
							GUILayout.BeginHorizontal(isodd ? GUI.skin.label : GUI.skin.box);
							{
								_preferences.bakeAnimStates[clipName].isBake =
									EditorGUILayout.Toggle(_preferences.bakeAnimStates[clipName].isBake, GUILayout.Width(30));
								GUILayout.Label($"{clipName}", GUILayout.MaxWidth(200));
								GUILayout.Label($"{framesToBake} frames", GUILayout.MaxWidth(100));
								_preferences.bakeAnimStates[clipName].frame =
									EditorGUILayout.IntSlider(_preferences.bakeAnimStates[clipName].frame, -1, framesToBake);
							}
							GUILayout.EndHorizontal();
							GUI.color = bgcolor;
							
							if (framesToBake > 500)
							{
								GUI.skin.label.richText = true;
								EditorGUILayout.LabelField(
									"<color=red>Long animations degrade performance, consider using a higher frame skip value.</color>",
									GUI.skin.label);
							}
						}

						if (EditorGUI.EndChangeCheck())
						{
							
						}
					});
					DrawFoldOut("Copy Components", () =>
					{
						EditorGUILayout.BeginHorizontal();
						{
							_copyComponentGO = (GameObject) EditorGUILayout.ObjectField("Copy Component GO",
								_copyComponentGO, typeof(GameObject), true);
							if (GUILayout.Button("刷新", new GUILayoutOption[] {GUILayout.Width(50)}))
							{
								OnCopyComponentChanged();
							}
						}
						EditorGUILayout.EndHorizontal();
						for (int i = 0; i < _components.Count; i++)
						{
							var comp = _components[i];
							bool isodd = (i & 1) > 0;

							Color bgcolor = GUI.color;
							if (_componentsIsCopy[i])
							{
								GUI.color = Color.green;
							}
							GUILayout.BeginHorizontal(isodd ? GUI.skin.label : GUI.skin.box);
							{
								_componentsIsCopy[i] = EditorGUILayout.Toggle(_componentsIsCopy[i], GUILayout.Width(30));
								GUILayout.Label($"{comp.GetType()}");
							}
							GUILayout.EndHorizontal();
							GUI.color = bgcolor;
						}
					});
					//boneTransList
					DrawFoldOut("Bone Trans", () =>
					{

						if (GUILayout.Button("添加"))
						{
							boneTransList.Add(null);
						}

						if (GUILayout.Button("删除"))
						{
							if (boneTransList.Count != 0)
							{
								boneTransList.RemoveAt(boneTransList.Count-1);
							}
						}

						for (int i = 0; i < boneTransList.Count; i++)
						{
							boneTransList[i] = EditorGUILayout.ObjectField(boneTransList[i],typeof(Transform)) as Transform;
						}
					});
					/*
					DrawFoldOut("Bake Preferences", () =>
					{
						fps = EditorGUILayout.IntSlider("Bake FPS", fps, 1, 500);
						
						int selectedIndex = textureSizes.IndexOf(selectedTextureSize);
						if (selectedIndex == -1)
							selectedIndex = 0;
						selectedIndex = EditorGUILayout.Popup("Bake Texture Size", selectedIndex, textureSizeNames);
						selectedTextureSize = textureSizes[selectedIndex];

						selectedIndex = textureQualities.IndexOf(selectedTextureQuality);
						if (selectedIndex == -1)
							selectedIndex = 0;
						selectedIndex = EditorGUILayout.Popup("Bake Texture Quality", selectedIndex, textureQualityNames);
						selectedTextureQuality = textureQualities[selectedIndex];

						int[] optionsValues = new int[5] { 1, 10, 100, 1000, 10000 };
						string[] options = new string[5] { "None - Best Quality", "0.1 - Low Quality", "0.01 - Medium Quality", "0.001 - High Quality", "0.0001 - Highest Quality" };
                        int selected = 0;
                        for (int i = 0; i < optionsValues.Length; i++)
                        {
                            if (optionsValues[i] == compressionAccuracy)
                                selected = i;
                        }
                        if (customCompression == false)
                        {
                            compressionAccuracy = optionsValues[EditorGUILayout.Popup("Position Compression", selected, options)];
                            if (selected > 0)
                            {
                                string message = "Lower compression values increase the accuracy of vertex positions.";
                                DrawInformation(message);
                            }
                        }
                        else
                        {
                            compressionAccuracy = EditorGUILayout.Slider("Position Compression", compressionAccuracy, 1, 10000);
                            if (compressionAccuracy != 1)
                            {
                                string message = "Lower custom compression values reduce the accuracy of vertex positions.";
                                DrawInformation(message);
                            }
                        }
                        customCompression = EditorGUILayout.Toggle("Custom Compression Value", customCompression);
					});
					*/
					_scrollPos = scrollScope.scrollPosition;
				}
				GUILayout.Space(10);
				int bakeCount = _preferences.bakeAnimStates.dictionary.Count(q => q.Value.isBake);
				GUI.enabled = bakeCount > 0;
				var c = GUI.color;
				GUI.color = Color.green;
				if (GUILayout.Button($"Bake {bakeCount} animation", GUILayout.Height(30)))
				{
					// BakeAnimInstance();
					BakeBoneAnimInstance();
				}
				GUI.color = c;
			}
			else
			{
				DrawWarning("Select an asset to bake.");
			}
		}
		
		private void DrawInformation(string text)
		{
			int w = (int)Mathf.Lerp(300, 900, text.Length / 200f);
			using (new EditorGUILayout.HorizontalScope(GUILayout.MinHeight(30)))
			{
				var style = new GUIStyle(GUI.skin.FindStyle("CN EntryInfoIcon"));
				style.margin = new RectOffset();
				style.contentOffset = new Vector2();
				GUILayout.Box("", style, GUILayout.Width(15), GUILayout.Height(15));
				var textStyle = new GUIStyle(GUI.skin.label);
				textStyle.contentOffset = new Vector2(10, 2);
				GUILayout.Label(text, textStyle);
			}
		}

		private void DrawWarning(string text)
		{
			int w = (int) Mathf.Lerp(300, 900, text.Length / 200f);
			using (new EditorGUILayout.HorizontalScope(GUILayout.MinHeight(30)))
			{
				var style = new GUIStyle(GUI.skin.FindStyle("CN EntryWarnIcon"));
				style.margin = new RectOffset();
				style.contentOffset = new Vector2();
				GUILayout.Box("", style, GUILayout.Width(15), GUILayout.Height(15));
				var textStyle = new GUIStyle(GUI.skin.label);
				textStyle.contentOffset = new Vector2(10, 2);
				GUILayout.Label(text, textStyle);
			}
		}

		private void DrawError(string text)
		{
			int w = (int) Mathf.Lerp(300, 900, text.Length / 200f);
			using (new EditorGUILayout.HorizontalScope(GUILayout.MinHeight(30)))
			{
				var style = new GUIStyle(GUI.skin.FindStyle("CN EntryErrorIcon"));
				style.margin = new RectOffset();
				style.contentOffset = new Vector2();
				GUILayout.Box("", style, GUILayout.Width(15), GUILayout.Height(15));
				var textStyle = new GUIStyle(GUI.skin.label);
				textStyle.contentOffset = new Vector2(10, 2);
				GUILayout.Label(text, textStyle);
			}
		}
		
		static Dictionary<string, bool> s_FoldOutStates = new Dictionary<string, bool>();
		static GUIStyle s_FoldOutStyle;

		/// <summary>
		/// Draw a foldout section, and returns foldout state.
		/// </summary>
		public static void DrawFoldOut(string labelName, System.Action drawAction, bool defaultState = true)
		{
			if (s_FoldOutStyle == null)
			{
				s_FoldOutStyle = new GUIStyle(GUI.skin.GetStyle("Foldout"));
				s_FoldOutStyle.richText = true;
				s_FoldOutStyle.fontStyle = FontStyle.Bold;
			}

			if (!s_FoldOutStates.ContainsKey(labelName))
			{
				s_FoldOutStates[labelName] = defaultState;
			}

			using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
			{
				s_FoldOutStates[labelName] =
					EditorGUILayout.Foldout(s_FoldOutStates[labelName], labelName, true, s_FoldOutStyle);
				if (s_FoldOutStates[labelName])
				{
					using (new EditorGUILayout.HorizontalScope())
					{
						GUILayout.Space(15);
						using (new EditorGUILayout.VerticalScope())
						{
							drawAction();
						}
					}
				}
			}
		}
		#endregion

		#region Utility
		protected void LoadPreferencesForAsset()
		{
			try
			{
				if (_animAvatar != null)
				{
					_fbxPath = AssetDatabase.GetAssetPath(_animAvatar);
				}

				if (string.IsNullOrEmpty(_fbxPath))
					return;

				_fbxDir = Path.GetDirectoryName(_fbxPath);
				_outputName = Path.GetFileNameWithoutExtension(_fbxPath);
				_prefsPath = Path.Combine(_fbxDir, $"{_outputName}_prefs.asset");
				_preferences = AssetDatabase.LoadAssetAtPath<BakeAnimPreferences>(_prefsPath);
				if (_preferences == null)
				{
					// 兼容以前的 之前用的prefab的name存的
					string oldPath = Path.Combine(_fbxDir, s_OutFolderName, $"{_prefab.name}_prefs.asset");
					_preferences = AssetDatabase.LoadAssetAtPath<BakeAnimPreferences>(oldPath);
				}

				if(_preferences == null)
				{
					_preferences = ScriptableObject.CreateInstance<BakeAnimPreferences>();
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e.ToString());
				throw;
			}
		}

		protected void SavePreferencesForAsset()
		{
			if (AssetDatabase.Contains(_preferences))
			{
				EditorUtility.SetDirty(_preferences);
			}
			else
			{
				AssetDatabase.CreateAsset(_preferences, _prefsPath);
			}
		}

		private void ClearAssets()
		{
			if (_preferences != null)
			{
				DestroyImmediate(_preferences);
				_preferences = null;
			}

			if (_spawnedPrefab != null)
			{
				DestroyImmediate(_spawnedPrefab);
				_spawnedPrefab = null;
			}
			
			if (_prefab != null)
			{
				_prefab = null;
			}
		}

		private void OnCopyComponentChanged()
		{
			_copyComponentGO.GetComponents(_components);
			_components = _components.Where(item => item.GetType() != typeof(Transform)).ToList();
			_componentsIsCopy = new bool[_components.Count];
			
			// 默认拷贝
			int index = _components.FindIndex(item => item.GetType() == typeof(MaterialEffect));
			if (index >= 0)
			{
				_componentsIsCopy[index] = true;
			}
		}

		protected void OnPrefabChanged()
		{
			if (_spawnedPrefab != null)
			{
				DestroyImmediate(_spawnedPrefab.gameObject);
			}

			if (Application.isPlaying)
			{
				return;
			}

			_animator = null;
			_animation = null;
			_animAvatar = null;

			_preferences = null;
			_prefsPath = null;
			
			_fbxPath = String.Empty;
			_fbxDir = string.Empty;
			_outputFolder = null;

			_outputDir = string.Empty;

			if (_prefab != null)
			{
				_spawnedPrefab = Instantiate(_prefab, new Vector3(-10, 0, 0), Quaternion.identity) as GameObject;
				SetChildFlags(_spawnedPrefab.transform, HideFlags.HideAndDontSave);
				
				// 获取信息
				AutoPopulateFiltersAndRenderers();
				AutoPopulateAnimatorAndController();
				LoadPreferencesForAsset();
				AutoPopulateOutFolder();
				GetClips();
			}

			_previousPrefab = _prefab;
		}


		/// Sets flags on a transform and all it's children
		private void SetChildFlags(Transform t, HideFlags flags)
		{
			Queue<Transform> q = new Queue<Transform>();
			q.Enqueue(t);
			for (int i = 0; i < t.childCount; i++)
			{
				Transform c = t.GetChild(i);
				q.Enqueue(c);
				SetChildFlags(c, flags);
			}

			while (q.Count > 0)
			{
				q.Dequeue().gameObject.hideFlags = flags;
			}
		}

		private string GetPrefabPath()
		{
			if (_animAvatar != null)
			{
				return AssetDatabase.GetAssetPath(_animAvatar);
			}

			string assetPath = AssetDatabase.GetAssetPath(_prefab);
			if (string.IsNullOrEmpty(assetPath))
			{
				UnityEngine.Object parentObject = PrefabUtility.GetCorrespondingObjectFromSource(_prefab);
				assetPath = AssetDatabase.GetAssetPath(parentObject);
			}

			return assetPath;
		}
		
		/// Return the Avatar if available from the prefab
		protected Avatar GetAvatar()
		{
			var objs = EditorUtility.CollectDependencies(new Object[] { _prefab }).ToList();
			foreach (var obj in objs.ToArray())
				objs.AddRange(AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(obj)));
			objs.RemoveAll(q => q is Avatar == false || q == null);
			if (objs.Count > 0)
				_animAvatar = objs[0] as Avatar;
			return _animAvatar;
		}

		protected void GetClips()
		{
			// bakeAnimStates.Clear();
			clipsCache.Clear();
			if (_animController)
			{
				var controllerClips = _animController.animationClips;
				foreach (var clip in controllerClips)
				{
					clipsCache.Add(clip);
				}
			}

			if (_animation)
			{
				var animationClips =
					new List<AnimationState>(_animation.Cast<AnimationState>()).Select(item => item.clip);
				clipsCache.AddRange(animationClips);
			}

			clipsCache = clipsCache.Union(_preferences.customClips).Distinct().ToList();
			clipsCache = clipsCache.Where(item => item != null).ToList();
			clipsCache.Sort((x, y) => x.name.CompareTo(y.name));
			
			for (int i = 0; i < clipsCache.Count; i++)
			{
				string clipName = clipsCache[i].name;
				// if (!bakeAnimStates.ContainsKey(clipName))
				// 	bakeAnimStates.Add(clipName, new BakeAnimState() {isBake = false, frame = -1});
				if (!_preferences.bakeAnimStates.dictionary.ContainsKey(clipName))
					_preferences.bakeAnimStates[clipName] = new BakeAnimState() {isBake = false, frame = -1};
			}

			// bakeAnimStates = _preferences.bakeAnimStates.dictionary;
		}

		private void AutoPopulateOutFolder()
		{
			if (_prefab == null)
				return;

			if (_preferences != null)
				_outputFolder = _preferences.outputFolder;

			if (_outputFolder == null) 
				_outputFolder = AssetDatabase.LoadAssetAtPath<DefaultAsset>(_fbxDir);

			OnOutputFolderChanged();
		}

		private void OnOutputFolderChanged()
		{
			_outputDir = String.Empty;
			if (_outputFolder != null)
			{
				_outputDir = Path.Combine(AssetDatabase.GetAssetPath(_outputFolder), s_OutFolderName);
				_preferences.outputFolder = _outputFolder;
			}

			CheckOverwrite();
		}

		private void CheckOverwrite()
		{
			_overwriteExist = false;
			if (_outputFolder != null)
			{
				var files = AnimatorInstanceUtils.GetTargetsPath(_outputDir, "prefab");
				
				for (int f = 0; f < files.Length; f++)
				{
					var existingMa = AssetDatabase.LoadAssetAtPath<AnimInstance>(files[f]);
					if (existingMa != null)
					{
						_overwriteExist = true;
						break;
					}
				}
			}
		}

		/// Find all renderers in the prefab
		protected void AutoPopulateFiltersAndRenderers()
		{
			meshFilters.Clear();
			skinnedRenderers.Clear();
			meshMaterialsDic.Clear();
			MeshFilter[] filtersInPrefab = _spawnedPrefab.GetComponentsInChildren<MeshFilter>();
			for (int i = 0; i < filtersInPrefab.Length; i++)
			{
				meshFilters.Add(filtersInPrefab[i]);
				var r = filtersInPrefab[i].GetComponent<MeshRenderer>();
				if (r != null)
				{
					meshMaterialsDic.Add(filtersInPrefab[i].sharedMesh, r.sharedMaterials);
				}
			}

			SkinnedMeshRenderer[] renderers = _spawnedPrefab.GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int i = 0; i < renderers.Length; i++)
			{
				skinnedRenderers.Add(renderers[i]);
				// meshMaterialsDic.Add(renderers[i].sharedMesh, renderers[i].sharedMaterials);
			}
			// useOriginalMesh = meshFilters.Count + skinnedRenderers.Count <= 1;
		}

		/// Find the animator and controllers in the prefab
		private void AutoPopulateAnimatorAndController()
		{
			_animation = _spawnedPrefab.GetComponent<Animation>();
			_animator = _spawnedPrefab.GetComponent<Animator>();
			if (_animator == null)
				_animator = _spawnedPrefab.GetComponentInChildren<Animator>(true);
			if (_animator)
			{
				_animController = _animator.runtimeAnimatorController;
				_animAvatar = _animator.avatar;
			}
			// GetAvatar();
		}
		
		#endregion

		#region Baking Methods


		private void BakeAnimInstance()
		{
			AnimatorController bakeController = null;
			try
			{
				if (string.IsNullOrEmpty(_fbxPath))
				{
					EditorUtility.DisplayDialog("Animator Instance", "Unable to locate the asset path for prefab: " + _prefab.name, "OK");
					return;
				}

				if (string.IsNullOrEmpty(_outputDir))
				{
					EditorUtility.DisplayDialog("Animator Instance", "Unable to load Output Folder. Please ensure an output folder is populated in the bake window", "OK");
					return;
				}

				if (!Directory.Exists(_outputDir))
				{
					Directory.CreateDirectory(_outputDir);
				}
				
				HashSet<string> allAssets = new HashSet<string>();
				foreach (var clip in clipsCache)
					allAssets.Add(AssetDatabase.GetAssetPath(clip));
				
				int animCount = 0;
				
				var sampleGO = Instantiate(_prefab, Vector3.zero, Quaternion.identity) as GameObject;
				if (meshFilters.Count(q => q) == 0 && skinnedRenderers.Count(q => q) == 0)
				{
					throw new System.Exception("Bake Error! No MeshFilter's or SkinnedMeshRenderer's found to bake!");
				}
				else
				{
					Animator animator = sampleGO.GetComponentInChildren<Animator>();
					if (animator == null)
					{
						animator = sampleGO.AddComponent<Animator>();
					}

					bakeController = CreateBakeController();
					animator.runtimeAnimatorController = bakeController;
					animator.avatar = _animAvatar;
					animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;

					GameObject asset = new GameObject(_prefab.name + "_" + s_OutFolderName);
					// AnimInstance AI = asset.AddComponent<AnimInstance>();
					List<Material> mats = GatherMaterials();
					asset.AddComponent<MeshRenderer>().sharedMaterials = mats.ToArray();
					List<AnimBaker> createdAnims = new List<AnimBaker>();
					
					Mesh bakeMesh = null;

					for (int i = 0; i < clipsCache.Count; i++)
					{
						AnimationClip animClip = clipsCache[i];
						if (_preferences.bakeAnimStates[animClip.name] != null && _preferences.bakeAnimStates[animClip.name].isBake == false) continue;
						
						string meshAnimationPath = string.Format("{0}/{1}.asset", _outputDir, FormatClipName(animClip.name));
                        AnimBaker animBaker = AssetDatabase.LoadAssetAtPath(meshAnimationPath, typeof(AnimBaker)) as AnimBaker;
      
                        if (animBaker == null)
                        {
	                        animBaker = ScriptableObject.CreateInstance<AnimBaker>();
	                        AssetDatabase.CreateAsset(animBaker, meshAnimationPath);
                        }
                        animBaker.AnimName = animClip.name;
                        animBaker.Length = animClip.length;
                        animBaker.WrapMode = animClip.wrapMode;
                        
                        int totalClipFrame = Mathf.ClosestPowerOfTwo((int)(animClip.frameRate * animClip.length));
                        float perFrameTime = animClip.length / totalClipFrame;
                        float lastFrameTime = 0;

                        Dictionary<string, Color[]> meshColors = new Dictionary<string, Color[]>();

                        for (var frame = 0; frame < totalClipFrame; frame++)
                        {
	                        float bakeDelta = Mathf.Clamp01((float) frame / totalClipFrame);
	                        EditorUtility.DisplayProgressBar("Baking Animation", string.Format("Processing: {0} Frame: {1}", animClip.name, frame), bakeDelta);
	                        float animationTime = bakeDelta * animClip.length;
	                        animator.enabled = true;
	                        animClip.SampleAnimation(sampleGO, animationTime);
	                        animator.enabled = false;
	                        
	                        // float normalizedTime = animationTime / animClip.length;
	                        // animator.Play(animClip.name, 0, normalizedTime);
	                        //
	                        // if (lastFrameTime == 0)
	                        // {
		                       //  float nextBakeDelta = Mathf.Clamp01((float) (frame + 1) / totalClipFrame);
		                       //  float nextAnimationTime = nextBakeDelta * animClip.length;
		                       //  lastFrameTime = animationTime - nextAnimationTime;
	                        // }
	                        // animator.Update(animationTime - lastFrameTime);
	                        // lastFrameTime = animationTime;
	                        
	                        // animClip.SampleAnimation(sampleGO, animationTime);
	                        for (int meshIndex = 0; meshIndex < meshFilters.Count; meshIndex++)
	                        {
		                        var sampleMF = FindMatchingTransform(_prefab.transform, meshFilters[meshIndex].transform, sampleGO.transform).GetComponent<MeshFilter>();
		                        var sampleMR = sampleMF.gameObject.GetComponent<MeshRenderer>();
		                        if(sampleMR == null || !sampleMR.enabled)
			                        continue;

		                        bakeMesh = Instantiate(sampleMF.sharedMesh);
		                        Color[] colors;
		                        if (!meshColors.TryGetValue(bakeMesh.name, out colors))
		                        {
			                        colors = new Color[bakeMesh.vertexCount * totalClipFrame];
			                        meshColors.Add(bakeMesh.name, colors);
		                        }

		                        for(var vIndex = 0; vIndex < bakeMesh.vertexCount; vIndex++)
		                        {
			                        var vertex = bakeMesh.vertices[vIndex];
			                        colors[frame * bakeMesh.vertexCount + vIndex] =
				                        new Color(vertex.x, vertex.y, vertex.z);
		                        }
		                        DestroyImmediate(bakeMesh);
	                        }
	                        
	                        for (int meshIndex = 0; meshIndex < skinnedRenderers.Count; meshIndex++)
	                        {
		                        var sampleSR = FindMatchingTransform(_prefab.transform, skinnedRenderers[meshIndex].transform, sampleGO.transform).GetComponent<SkinnedMeshRenderer>();
		                        if(sampleSR == null || !sampleSR.enabled)
			                        continue;
		                        bakeMesh = new Mesh();
		                        bakeMesh.name = sampleSR.sharedMesh.name;
		                        sampleSR.BakeMesh(bakeMesh, true);
		                        Color[] colors;
		                        if (!meshColors.TryGetValue(bakeMesh.name, out colors))
		                        {
			                        colors = new Color[bakeMesh.vertexCount * totalClipFrame];
			                        meshColors.Add(bakeMesh.name, colors);
		                        }

		                        for(var vIndex = 0; vIndex < bakeMesh.vertexCount; vIndex++)
		                        {
			                        var vertex = bakeMesh.vertices[vIndex];
			                        colors[frame * bakeMesh.vertexCount + vIndex] =
				                        new Color(vertex.x, vertex.y, vertex.z);
		                        }
		                        DestroyImmediate(bakeMesh);
	                        }
                        }
                        
                        var existingTextures = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(meshAnimationPath).Where(a => a is Texture2D).ToArray();
                        foreach (var existing in existingTextures)
	                        DestroyImmediate(existing, true);
                        animBaker.AnimTex.Clear();

                        foreach (var kv in meshColors)
                        {
	                        var tex = new Texture2D(kv.Value.Length / totalClipFrame, totalClipFrame, TextureFormat.RGBAHalf, false);
	                        tex.name = $"{animClip.name}_{kv.Key}";
	                        tex.SetPixels(kv.Value);
	                        tex.Apply();
	                        AssetDatabase.AddObjectToAsset(tex, animBaker);
	                        animBaker.AnimTex[kv.Key] = tex;
                        }
                        EditorUtility.SetDirty(animBaker);
					}
					
					AssetDatabase.SaveAssets();
					
					MeshFilter mf = asset.AddComponent<MeshFilter>();
					if (skinnedRenderers.Count > 0)
					{
						mf.sharedMesh = skinnedRenderers[0].sharedMesh;
					}
					else
					{
						mf.sharedMesh = meshFilters[0].sharedMesh;
					}
					// AI.baseMesh = meshFilters[0].sharedMesh;
					// AI.meshFilter = AI.gameObject.GetOrAddComponent<MeshFilter>();
					// AI.meshFilter.sharedMesh = AI.baseMesh;
					// AI.SetAnimations(createdAnims.ToArray());
					string maPrefabPath = string.Format("{0}/{1}.prefab", _outputDir, asset.name);
					var maPrefab = AssetDatabase.LoadAssetAtPath(maPrefabPath, typeof(GameObject));
					if (maPrefab != null)
					{
						PrefabUtility.ReplacePrefab(asset, maPrefab);
					}
					else
					{
						PrefabUtility.CreatePrefab(maPrefabPath, asset);
					}
					GameObject.DestroyImmediate(asset);
				}
				GameObject.DestroyImmediate(sampleGO);
				EditorUtility.ClearProgressBar();

			}
			catch (Exception e)
			{
				EditorUtility.ClearProgressBar();
				EditorUtility.DisplayDialog("Bake Error", string.Format("There was a problem baking the animations.\n\n{0}\n\n", e), "OK");
				Debug.LogException(e);
			}
			finally
			{
				if (bakeController)
				{
					AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(bakeController));
				}
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		protected void BakeBoneAnimInstance()
		{
			try
			{
				if (string.IsNullOrEmpty(_fbxPath))
				{
					EditorUtility.DisplayDialog("Animator Instance", "Unable to locate the asset path for prefab: " + _prefab.name, "OK");
					return;
				}

				if (string.IsNullOrEmpty(_outputDir))
				{
					EditorUtility.DisplayDialog("Animator Instance", "Unable to load Output Folder. Please ensure an output folder is populated in the bake window", "OK");
					return;
				}

				if (!Directory.Exists(_outputDir))
				{
					Directory.CreateDirectory(_outputDir);
				}
				
				int animCount = 0;
				GameObject _sampleGO = Instantiate(_prefab, Vector3.zero, Quaternion.identity);
				_sampleGO.transform.localScale = Vector3.one;
				Animator _sampleAnimator = _sampleGO.GetComponentInChildren<Animator>();
				if (_sampleAnimator == null)
				{
					_sampleAnimator = _sampleGO.AddComponent<Animator>();
				}

				AnimatorController _sampleController = CreateBakeController();
				_sampleAnimator.runtimeAnimatorController = _sampleController;
				_sampleAnimator.avatar = _animAvatar;
				_sampleAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
				
				if (meshFilters.Count(q => q) == 0 && skinnedRenderers.Count(q => q) == 0)
				{
					throw new System.Exception("Bake Error! No MeshFilter's or SkinnedMeshRenderer's found to bake!");
				}
				else
				{
					GameObject asset = new GameObject(_outputName + "_" + s_OutFolderName);
					asset.layer = LayerMask.NameToLayer("Scene_Actor");
					AnimInstance AI = asset.AddComponent<AnimInstance>();

					List<AnimationClip> selectClips = new List<AnimationClip>();
					List<string> selectClipNames = new List<string>();
					for (int i = 0; i < clipsCache.Count; i++)
					{
						AnimationClip animClip = clipsCache[i];
						if (_preferences.bakeAnimStates[animClip.name] != null && _preferences.bakeAnimStates[animClip.name].isBake == false) continue;
						
						selectClips.Add(animClip);
						selectClipNames.Add(animClip.name.ToLower());
					}

					Bounds bounds = new Bounds();
					var boundsRenders = _sampleGO.GetComponentsInChildren<Renderer>();
					for (int i = 0; i < boundsRenders.Length; i++)
					{
						bounds.Encapsulate(boundsRenders[i].bounds);
					}
					
					for (int i = 0; i < skinnedRenderers.Count; i++)
					{
						SkinnedMeshRenderer smr = skinnedRenderers[i];
						var sampleSR = FindMatchingTransform(_prefab.transform, smr.transform, _sampleGO.transform).GetComponent<SkinnedMeshRenderer>();
						// 烘骨骼贴图
						var animationTexture = GenerateAnimationTexture(_sampleGO, _sampleAnimator, selectClips, sampleSR);
						AssetDatabase.CreateAsset(animationTexture, $"{_outputDir}/{sampleSR.sharedMesh.name}_AnimBoneTexture.asset");
						// 生成mesh
						var mesh = GenerateUvBoneWeightedMesh(sampleSR);
						AssetDatabase.CreateAsset(mesh, $"{_outputDir}/{sampleSR.sharedMesh.name}_AnimMesh.asset");
						// 材质球
						var material = GenerateMaterial(sampleSR, animationTexture, sampleSR.bones.Length);
						
						// AnimMeshInstance
						var go = GenerateMeshRendererObject(asset, mesh, material);
						go.transform.localPosition = smr.transform.localPosition;
						// go.transform.localRotation = smr.transform.localRotation;
						// go.transform.localScale = smr.transform.localScale;
						go.transform.localScale = Vector3.one;
					}
					
					// 挂点
					
					Dictionary<string, Dictionary<string, List<TransData>>> effectBoneData = GenerateAnimationBoneIndexData(_sampleAnimator, selectClips);
					List<Transform> effectTransList = new List<Transform>();
					//添加特效节点
					for (int index = 0; index < boneTransList.Count; index++)
					{
						var go = new GameObject();
						go.name = boneTransList[index].name;
						go.transform.localPosition = Vector3.zero;
						go.transform.localEulerAngles = Vector3.zero;
						go.transform.localScale = Vector3.one;
						go.transform.parent = asset.transform;
						go.layer = asset.layer;
						effectTransList.Add(go.transform);
					}
					
					// 数据挂载
					var renders = asset.GetComponentsInChildren<MeshRenderer>();
					var frameInformations = GenerateFrameInfos(_animController, selectClips, effectBoneData);
					string defaultState = string.Empty;
					if (_animController != null)
					{
						var animationController = _animController as AnimatorController;
						var baseLayer = animationController.layers[0];
						defaultState = baseLayer.stateMachine.defaultState.name;
					}
					AI.SetUp(selectClipNames, frameInformations, renders, bounds,effectTransList, defaultState);
					
					// 添加的MeshRender
					for (int i = 0; i < meshFilters.Count; i++)
					{
						var mesh = meshFilters[i].sharedMesh;
						if(meshMaterialsDic.TryGetValue(mesh, out Material[] mats))
						{
							var cloneMesh = Object.Instantiate(mesh);
							AssetDatabase.CreateAsset(cloneMesh, $"{_outputDir}/{mesh.name}_AnimMesh.asset");
							var go = GenerateMeshRendererObject(asset, cloneMesh, mats);
							go.transform.localPosition = meshFilters[i].transform.localPosition;
							go.transform.localRotation = meshFilters[i].transform.localRotation;
							go.transform.localScale = meshFilters[i].transform.lossyScale;
						}
					}

					CopyComponent(asset, renders);
					
					AddModelSocket(asset);

					var maPrefabPath = $"{_outputDir}/{asset.name}.prefab";
					var maPrefab = AssetDatabase.LoadAssetAtPath(maPrefabPath, typeof(GameObject));
					if (maPrefab != null)
					{
						PrefabUtility.ReplacePrefab(asset, maPrefab, ReplacePrefabOptions.ConnectToPrefab);
					}
					else
					{
						PrefabUtility.CreatePrefab(maPrefabPath, asset, ReplacePrefabOptions.ConnectToPrefab);
					}
					DestroyImmediate(asset);
					// asset.transform.position = _prefab.transform.position;
				}
				
				if (_sampleController)
				{
					AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_sampleController));
				}
				GameObject.DestroyImmediate(_sampleGO);
				EditorUtility.ClearProgressBar();

			}
			catch (Exception e)
			{
				EditorUtility.ClearProgressBar();
				EditorUtility.DisplayDialog("Bake Error", string.Format("\n\n{0}\n\n There was a problem baking the animations.\n\n{1}\n\n",_prefab.name, e), "OK");
				Debug.LogException(e);
			}
			finally
			{
				// SavePreferencesForAsset();
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
				ClearAssets();
			}
		}

		private void CopyComponent(GameObject gameObject, MeshRenderer[] meshRenderers)
		{
			if (_copyComponentGO != null)
			{
				MaterialEffect me = _copyComponentGO.GetComponent<MaterialEffect>();
				if(me != null)
				{
					UnityEditorInternal.ComponentUtility.CopyComponent(me);
					UnityEditorInternal.ComponentUtility.PasteComponentAsNew(gameObject);
					MaterialEffect e = gameObject.GetComponent<MaterialEffect>();
					// e.SetRenders(meshRenderers);
				}
			}
		}

		protected void AddModelSocket(GameObject gameObject)
		{
			ModelSocket comp = gameObject.AddComponent<ModelSocket>();
			comp.CreateAndGetSockets();
		}

		protected List<AnimInstanceFrameInfo> GenerateFrameInfos(
			RuntimeAnimatorController sampleAnimController,
			IEnumerable<AnimationClip> clips,
			Dictionary<string, Dictionary<string, List<TransData>>> effectBoneData = null)
		{
			List<AnimInstanceFrameInfo> frameInfos = new List<AnimInstanceFrameInfo>();
			Dictionary<string, int> frameSort = new Dictionary<string, int>();			
			var currentClipFrames = 0;
			var currentClip = 0;
			foreach (var clip in clips)
			{
				var state = _preferences.bakeAnimStates[clip.name];
				var frameCount = state.frame > -1 ? 1 : Math.Max(2, (int) (clip.length * fps) + 1);
				var startFrame = currentClipFrames + 1;
				var endFrame = startFrame + frameCount - 1;
				Dictionary<string, List<TransData>> boneData = null;
				//List<EffectBoneData> effectBoneDatas = new List<EffectBoneData>();
				if (effectBoneData != null)
				{
					boneData = effectBoneData[clip.name];
					//foreach (var item in boneData)
					//{
					//	EffectBoneData effectBone = new EffectBoneData();
					//	effectBone.boneName = item.Key;
					//	effectBone.AnimBoneDataList = item.Value;
					//}
				}
				frameInfos.Add(new AnimInstanceFrameInfo(clip.name.ToLower(), startFrame, endFrame, frameCount, clip.isLooping, -1, boneData)); 
				frameSort.Add(clip.name.ToLower(), currentClip);
				currentClipFrames = endFrame;
				currentClip++;
			}
			
			// translation
			if(sampleAnimController != null)
			{
				var animationController = sampleAnimController as AnimatorController;
				var baseLayer = animationController.layers[0];
				var stateMachine = baseLayer.stateMachine;

				// 所有state
				Dictionary<string, AnimatorState> states = new Dictionary<string, AnimatorState>(stateMachine.states.Length);
				for (int i = 0; i < stateMachine.states.Length; i++)
				{
					var state = stateMachine.states[i].state;
					states.Add(state.name.ToLower(), state);
				}
				
				// 自动跳转
				foreach (var frameInfo in frameInfos)
				{
					if (states.TryGetValue(frameInfo.Name, out var state))
					{
						foreach (var transition in state.transitions)
						{
							if (transition.hasExitTime)
							{
								if (frameSort.ContainsKey(transition.destinationState.name.ToLower()))
								{
									frameInfo.AutoTransition = frameSort[transition.destinationState.name.ToLower()];
								}
								else
								{
									Debug.LogError($"{_prefab.name} 动画 {frameInfo.Name} 自动转向 ${transition.destinationState.name.ToLower()} 动作丢了 检查资源");
								}
								break;
							}
						}
					}
				}
			}

			return frameInfos;
		}

		/// <summary>
		/// 读取boneindex Matrix
		/// </summary>
		/// <param name="targetObject"></param>
		/// <param name="sampleAnimator"></param>
		/// <param name="clips"></param>
		/// <param name="smr"></param>
		/// <returns></returns>
		protected Dictionary<string, Dictionary<string, List<TransData>>> GenerateAnimationBoneIndexData(
			Animator sampleAnimator,
			IEnumerable<AnimationClip> clips)
		{
			// 整理骨骼列表
			var sampleAnimatorBones = GetBones(sampleAnimator);

			/// cilp name , bone name ,bone data
			Dictionary<string, Dictionary<string, List<TransData>>> boneIndexData = new Dictionary<string, Dictionary<string, List<TransData>>>();
			foreach (var clip in clips)
			{
				boneIndexData.Add(clip.name,new Dictionary<string, List<TransData>>());

                for (int index = 0; index < boneTransList.Count; index++)
                {
					boneIndexData[clip.name].Add(boneTransList[index].name,new List<TransData>());

				}
				BakeAnimState state = _preferences.bakeAnimStates[clip.name];
				var totalFrames = (int)(clip.length * fps);
				for (int frame = 0; frame <= totalFrames; frame++)
				{
					if (state.frame > -1 && frame != state.frame)
						continue;

					float bakeDelta = Mathf.Clamp01((float)frame / totalFrames);
					sampleAnimator.enabled = true;
					clip.SampleAnimation(sampleAnimator.gameObject, bakeDelta * clip.length);
					sampleAnimator.enabled = false;

                    for (int index = 0; index < boneTransList.Count; index++)
                    {
	                    var boneName = boneTransList[index].name;
						var boneTrans = sampleAnimatorBones[boneName];
						var bonematx = boneTrans.localToWorldMatrix/** smr.sharedMesh.bindposes[boneIndex]*/;
						var pos = new Vector3(bonematx[0, 3], bonematx[1, 3], bonematx[2, 3]);
						var rot = bonematx.rotation.eulerAngles;
						TransData transData = new TransData();
						transData.pos = pos;
						transData.rot = rot;
						//tempTransData.Add(transData);

	                    boneIndexData[clip.name][boneTransList[index].name].Add(transData);
					}

					//foreach (var boneMatrix in smr.bones.Select((b, idx) =>
					//			 b.localToWorldMatrix * smr.sharedMesh.bindposes[idx]))
					//{
					//	tempTransData
					//}
				}
			}


			return boneIndexData;
		}


		// 烘培骨骼instance
		protected Texture GenerateAnimationTexture(GameObject targetObject, Animator sampleAnimator,
			IEnumerable<AnimationClip> clips, SkinnedMeshRenderer smr)
        {
            var textureBoundary = GetCalculatedTextureBoundary(clips, smr.bones.Count());

            var texture = new Texture2D((int) textureBoundary.x, (int) textureBoundary.y, TextureFormat.RGBAHalf, false, true);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            var pixels = texture.GetPixels();
            var pixelIndex = 0;

            //Setup 0 to bindPoses
            foreach (var boneMatrix in smr.bones.Select(
                         (b, idx) => b.localToWorldMatrix * smr.sharedMesh.bindposes[idx]))
            {
                pixels[pixelIndex++] = new Color(boneMatrix.m00, boneMatrix.m01, boneMatrix.m02, boneMatrix.m03);
                pixels[pixelIndex++] = new Color(boneMatrix.m10, boneMatrix.m11, boneMatrix.m12, boneMatrix.m13);
                pixels[pixelIndex++] = new Color(boneMatrix.m20, boneMatrix.m21, boneMatrix.m22, boneMatrix.m23);
            }

            foreach (var clip in clips)
            {
	            BakeAnimState state = _preferences.bakeAnimStates[clip.name];
                var totalFrames = (int) (clip.length * fps);
                for (int frame = 0; frame <= totalFrames; frame++)
                {
	                if (state.frame > -1 && frame != state.frame)
		                continue;
	                
	                float bakeDelta = Mathf.Clamp01((float) frame / totalFrames);
	                sampleAnimator.enabled = true;
                    clip.SampleAnimation(sampleAnimator.gameObject, bakeDelta * clip.length);
                    sampleAnimator.enabled = false;
                    
                    foreach (var boneMatrix in smr.bones.Select((b, idx) =>
                                 b.localToWorldMatrix * smr.sharedMesh.bindposes[idx]))
                    {
                        pixels[pixelIndex++] =
                            new Color(boneMatrix.m00, boneMatrix.m01, boneMatrix.m02, boneMatrix.m03);
                        pixels[pixelIndex++] =
                            new Color(boneMatrix.m10, boneMatrix.m11, boneMatrix.m12, boneMatrix.m13);
                        pixels[pixelIndex++] =
                            new Color(boneMatrix.m20, boneMatrix.m21, boneMatrix.m22, boneMatrix.m23);
                    }
                }
            }

            texture.SetPixels(pixels);
            texture.Apply(true, true);

            return texture;
        }
		
		protected Vector2 GetCalculatedTextureBoundary(IEnumerable<AnimationClip> clips, int boneLength)
		{
			var boneMatrixCount = BoneMatrixRowCount * boneLength;

			var totalPixels = clips.Aggregate(boneMatrixCount,
				(pixels, currentClip) =>
				{
					BakeAnimState state = _preferences.bakeAnimStates[currentClip.name];
					var totalFrames = state.frame > -1 ? 1 : (int) (currentClip.length * fps) + 1;
					return pixels + boneMatrixCount * totalFrames;
				}
			);

			var textureWidth = 1;
			var textureHeight = 1;

			while (textureWidth * textureHeight < totalPixels)
			{
				if (textureWidth <= textureHeight)
				{
					textureWidth *= 2;
				}
				else
				{
					textureHeight *= 2;
				}
			}

			return new Vector2(textureWidth, textureHeight);
		}
		protected Mesh GenerateUvBoneWeightedMesh(SkinnedMeshRenderer smr)
		{
			var mesh = Object.Instantiate(smr.sharedMesh);
			// var mesh = smr.sharedMesh;

			var boneSets = smr.sharedMesh.boneWeights;
			var boneIndexes = boneSets.Select(x => new Vector4(x.boneIndex0, x.boneIndex1, x.boneIndex2, x.boneIndex3))
				.ToList();
			var boneWeights = boneSets.Select(x => new Vector4(x.weight0, x.weight1, x.weight2, x.weight3)).ToList();
			mesh.SetUVs(3, boneIndexes);
			mesh.SetUVs(4, boneWeights);
			mesh.ClearBlendShapes();
			mesh.boneWeights = null;

			// EditorUtility.SetDirty(mesh);
			return mesh;
		}
		
		protected Material[] GenerateMaterial(SkinnedMeshRenderer smr, Texture texture, int boneLength)
		{
			Material[] mats = new Material[smr.sharedMaterials.Length];
			for (int i = 0; i < smr.sharedMaterials.Length; i++)
			{
				var material = Object.Instantiate(smr.sharedMaterials[i]);
				if (!material.HasProperty("_AnimInstanceOn"))
				{
					material.shader = Shader.Find(shaderEnum[selectedShader]);
				}

				material.SetTexture("_AnimMap", texture);
				material.SetInt("_PixelCountPerFrame", BoneMatrixRowCount * boneLength);
				material.SetInt("_AnimInstanceOn", 1);
				material.EnableKeyword("_ANIMINSTANCE_ON");
				material.enableInstancing = true;
				mats[i] = material;
				string ext = smr.sharedMaterials.Length > 1 ? ("_" + i) : "";
				AssetDatabase.CreateAsset(material, $"{_outputDir}/{smr.sharedMesh.name}_AnimMaterial{ext}.mat");
			}
			return mats;
		}

		protected GameObject GenerateMeshRendererObject(GameObject asset, Mesh mesh, Material[] material)
		{
			var go = new GameObject();
			go.name = mesh.name;
			go.transform.SetParent(asset.transform);
			go.layer = asset.layer;

			var mf = go.AddComponent<MeshFilter>();
			mf.mesh = mesh;

			var mr = go.AddComponent<MeshRenderer>();
			mr.sharedMaterials = material;
			mr.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
			mr.reflectionProbeUsage = ReflectionProbeUsage.Off;
			mr.lightProbeUsage = LightProbeUsage.Off;

			// var animMeshInstance = go.AddComponent<AnimMeshInstance>();
			// var frameInformations = new List<AnimMeshInstanceFrameInfo>();
			// var currentClipFrames = 0;
			//
			// foreach (var clip in clips)
			// {
			// 	var frameCount = (int) (clip.length * fps);
			// 	var startFrame = currentClipFrames + 1;
			// 	var endFrame = startFrame + frameCount - 1;
			//
			// 	frameInformations.Add(new AnimMeshInstanceFrameInfo(clip.name, startFrame, endFrame, frameCount));
			//
			// 	currentClipFrames = endFrame;
			// }
			//
			// animMeshInstance.Setup(frameInformations);
			return go;
		}
		
		/// Creates a temporary AnimatorController for baking
		protected AnimatorController CreateBakeController()
		{
			// Creates the controller automatically containing all animation clips
			string tempPath = "Assets/TempBakeController.controller";
			var bakeName = AssetDatabase.GenerateUniqueAssetPath(tempPath);
			var controller = AnimatorController.CreateAnimatorControllerAtPath(bakeName);
			var baseStateMachine = controller.layers[0].stateMachine;
			foreach (var clip in clipsCache)
			{
				var state = baseStateMachine.AddState(clip.name);
				state.motion = clip;
			}
			return controller;
		}
		
		/// Find all materials associated with the renderers
		private List<Material> GatherMaterials()
		{
			List<Material> mats = new List<Material>();
			MeshRenderer mr = null;
			foreach (MeshFilter mf in meshFilters)
				if (mf && ((mr = mf.GetComponent<MeshRenderer>())))
					mats.AddRange(mr.sharedMaterials);
			foreach (SkinnedMeshRenderer sm in skinnedRenderers)
				if (sm) mats.AddRange(sm.sharedMaterials);
			mats.RemoveAll(q => q == null);
			mats = mats.Distinct().ToList();
			return mats;
		}
		
		private Dictionary<Mesh, Material[]> GatherMeshMaterials()
		{
			Dictionary<Mesh, Material[]> mats = new Dictionary<Mesh, Material[]>();
			MeshRenderer mr = null;
			foreach (MeshFilter mf in meshFilters)
				if (mf && ((mr = mf.GetComponent<MeshRenderer>())))
					mats.Add(mf.sharedMesh, mr.sharedMaterials);
			foreach (SkinnedMeshRenderer sm in skinnedRenderers)
				if (sm) 
					mats.Add(sm.sharedMesh, sm.sharedMaterials);
			return mats;
		}
		
		private string FormatClipName(string name)
		{
			string badChars = "!@#$%%^&*()=+}{[]'\";:|";
			for (int i = 0; i < badChars.Length; i++)
			{
				name = name.Replace(badChars[i], '_');
			}
			return name;
		}
		
		/// Finds a matching transform in the hierarchy of another transform
		protected Transform FindMatchingTransform(Transform parent, Transform source, Transform newParent)
		{
			List<int> stepIndexing = new List<int>();
			while (source != parent && source != null)
			{
				if (source.parent == null)
					break;
				for (int i = 0; i < source.parent.childCount; i++)
				{
					if (source.parent.GetChild(i) == source)
					{
						stepIndexing.Add(i);
						source = source.parent;
						break;
					}
				}
			}
			stepIndexing.Reverse();
			for (int i = 0; i < stepIndexing.Count; i++)
			{
				newParent = newParent.GetChild(stepIndexing[i]);
			}
			return newParent;
		}

		#endregion

		#region animator extension

		public static Dictionary<string, Transform> GetBones(Animator animator)
		{
			var bones = new List<Transform>();
			if (animator.avatar && animator.avatar.isHuman)
			{
				foreach (HumanBodyBones bone in Enum.GetValues(typeof(HumanBodyBones)))
				{
					if (bone == HumanBodyBones.LastBone) continue;
					Transform t = animator.GetBoneTransform(bone);
					if (t != null)
					{
						bones.Add(t);
					}
				}
			}
			else
			{
				AddBones(animator.transform, bones);
			}

			return bones.ToDictionary(trans => trans.name, trans => trans);
		}
		
		private static void AddBones(Transform root, List<Transform> bones)
		{
			foreach (Transform child in root)
			{
				bones.Add(child);
				AddBones(child, bones);
			}
		}

		#endregion

	}
}