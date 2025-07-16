// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2024-01-30       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    |_ | _|			                *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************


using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace UWParticleSystemProfiler
{
	[InitializeOnLoad]
	public class UWParticleSystemGameObjectEditor
	{
		const string id = "UWParticleEffectProfiler/Switch";

		static bool ParticleProfilerEnable
		{
			get => EditorPrefs.GetBool(id, false);
			set => EditorPrefs.SetBool(id, value);
		}

		static UWParticleSystemGameObjectEditor()
		{
			OnSwitchChanged(ParticleProfilerEnable);
		}

		[MenuItem("TATools/检查/特效Profiler", true)]
		public static bool CheckPlatform()
		{
			Menu.SetChecked("TATools/检查/特效Profiler", ParticleProfilerEnable);
			return true;
		}

		[MenuItem("TATools/检查/特效Profiler")]
		static void ProfilerToggle()
		{
			ParticleProfilerEnable = !ParticleProfilerEnable;
			OnSwitchChanged(ParticleProfilerEnable);
		}

		public static void OnSwitchChanged(bool enable)
		{
			if (enable)
			{
				SceneView.duringSceneGui -= OnSceneGUIFunc;
				SceneView.duringSceneGui += OnSceneGUIFunc;
			}
			else
			{
				SceneView.duringSceneGui -= OnSceneGUIFunc;
			}
		}

		static bool m_IsOverDrawOn = false;

		// ========================= profiler ======================================
		static int  mTexCount; //贴图数量
		static long mTexMemorySize; //贴图内存
		static int  mMeshCount; //Mesh数量
		static long mMeshMemorySize; //Mesh内存
		static int  mParticleSystemsCount; //粒子系统数量
		static int  mParticleSystemsDC; //DrawCall
		static int  mMaxParticleSystemsDC; //maxDrawCall
		static int  mParticleCount; //粒子数量
		static int  mMaxParticleCount; //最大粒子数量
		static int  mHideNodeCount; //隐藏节点数量数量
		static int  mPixDrawAverage; //特效原填充像素点
		static int  mPixActualDrawAverage; //特效实际填充像素点
		static int  mPixOverDrawRate; //每像素平均重复率
		static int  mSkinMeshRenderCount;
		static int  mMaxRuntimeTris;
		static int  mMaxRuntimeShadowCaster;

		static GameObject lastObj;
		static bool       flag    = false;
		static float      timetmp = 0;

		static void OnSceneGUIFunc(SceneView sceneview)
		{
			var tarObject = Selection.activeGameObject;
			if (tarObject == null)
				return;

			if (EditorUtility.IsPersistent(tarObject))
				return;

			var ps = tarObject.GetComponentsInChildren<ParticleSystem>(true);
			if (ps.Length == 0)
				return;

			if (lastObj != null && lastObj != tarObject)
			{
				UWParticleEffectProfiler last = lastObj.GetComponent<UWParticleEffectProfiler>();
				if (last != null)
					last.StopCheck();
				timetmp = 0;
				flag = false;
				lastObj = null;
			}

			//        var particleSystemRenderer = tarObject.GetComponentsInChildren<ParticleSystemRenderer>(true);
			//        if (particleSystemRenderer.Length == 0)
			//            return;

			//开始绘制GUI
			Handles.BeginGUI();
			GUILayout.BeginArea(new Rect(10, 10, 280, 300), "检查面板 " + tarObject.name, GUI.skin.window);

			UWParticleEffectProfiler uwParticleEffectProfiler = tarObject.GetComponent<UWParticleEffectProfiler>();

			Color guiBackColor = GUI.backgroundColor;
			GUI.backgroundColor = Color.green;
			if (GUILayout.Button(uwParticleEffectProfiler ? "重新检查" : "检 查", GUILayout.Height(30)))
			{
				if (EditorApplication.isPlaying)
				{
					lastObj = tarObject;
					tarObject.SetActive(false);
					if (uwParticleEffectProfiler == null)
						uwParticleEffectProfiler = tarObject.AddComponent<UWParticleEffectProfiler>();

					ShowGameViewUtility();
					
					EditorApplication.delayCall += () => { uwParticleEffectProfiler.InitData(); };

					flag = true;
				}
				else
				{
					EditorUtility.DisplayDialog("提示", "请先运行", "OK");
					return;
				}
			}

			GUI.backgroundColor = guiBackColor;

			if (flag)
			{
				timetmp += Time.deltaTime;
				if (timetmp > 1)
				{
					tarObject.SetActive(true);

					timetmp = 0;
					flag = false;
				}
			}

			if (uwParticleEffectProfiler)
			{
				mTexCount = uwParticleEffectProfiler.TexCount;
				mTexMemorySize = uwParticleEffectProfiler.TexMemorySize;
				mMeshCount = uwParticleEffectProfiler.MeshCount;
				mMeshMemorySize = uwParticleEffectProfiler.MeshMemorySize;
				mParticleSystemsCount = uwParticleEffectProfiler.ParticleSystemsCount;

				mParticleSystemsDC = uwParticleEffectProfiler.ParticleSystemsDC;
				mMaxParticleSystemsDC = uwParticleEffectProfiler.MaxParticleSystemsDC;
				mParticleCount = uwParticleEffectProfiler.ParticleCount;
				mMaxParticleCount = uwParticleEffectProfiler.MaxParticleCount;

				mSkinMeshRenderCount = uwParticleEffectProfiler.SkinMeshRenderCount;
				mMaxRuntimeTris = uwParticleEffectProfiler.MaxRuntimeTris;
				mMaxRuntimeShadowCaster = uwParticleEffectProfiler.MaxRuntimeShadowCaster;
			}

			ShowItem("检查属性", "最大值", "实际值", EditorStyles.boldLabel);
			ShowItem("粒子系统数量", UWParticleEffectProfiler.Standard_ParticleSystemsCount, mParticleSystemsCount);
			ShowItem("粒子数量", UWParticleEffectProfiler.Standard_ParticleCount, mMaxParticleCount);
			ShowItem("DrawCall", UWParticleEffectProfiler.Standard_ParticleSystemsDC, mMaxParticleSystemsDC);
			ShowItem("贴图数量", UWParticleEffectProfiler.Standard_TexCount, mTexCount);
			ShowItem("贴图内存", UWParticleEffectProfiler.Standard_TexMemorySize, mTexMemorySize);
			ShowItem("Mesh数量", UWParticleEffectProfiler.Standard_MeshCount, mMeshCount);
			ShowItem("Mesh内存", UWParticleEffectProfiler.Standard_MeshMemorySize, mMeshMemorySize);
			ShowItem("同屏最大面数", UWParticleEffectProfiler.Standard_RuntimeTris, mMaxRuntimeTris);
			ShowItem("SkinedMeshRender", UWParticleEffectProfiler.Standard_SkinMeshRenderCount, mSkinMeshRenderCount);
			ShowItem("阴影投射", UWParticleEffectProfiler.Standard_RuntimeShadowCaster, mMaxRuntimeShadowCaster);

			// EditorGUILayout.LabelField("粒子数量", mParticleCount + " - " + mMaxParticleCount);
			// EditorGUILayout.LabelField("DrawCall", mParticleSystemsDC + " - " + mMaxParticleSystemsDC);
			// EditorGUILayout.LabelField("贴图数量", mTexCount.ToString());
			// EditorGUILayout.LabelField("贴图内存", EditorUtility.FormatBytes(mTexMemorySize));
			// EditorGUILayout.LabelField("Mesh数量", mMeshCount.ToString());
			// EditorGUILayout.LabelField("Mesh内存", EditorUtility.FormatBytes(mMeshMemorySize));
			// EditorGUILayout.LabelField("SkinedMeshRender", mSkinMeshRenderCount.ToString());
			// EditorGUILayout.LabelField("同屏最大面数", mMaxRuntimeTris.ToString());
			// EditorGUILayout.LabelField("阴影投射", mMaxRuntimeShadowCaster.ToString());

			var allTransforms = tarObject.GetComponentsInChildren<Transform>(true);
			Object[] objects = allTransforms.Cast<Transform>()
				.Select(x => x.gameObject)
				.Where(x => x != null && !x.activeInHierarchy)
				.Cast<UnityEngine.Object>().ToArray();
			GUILayout.BeginHorizontal();

			mHideNodeCount = objects.Length;
			EditorGUILayout.LabelField("隐藏节点", mHideNodeCount.ToString());
			if (objects.Length > 0)
			{
				if (GUILayout.Button("选中"))
				{
					Selection.objects = objects;
				}
			}

			GUILayout.EndHorizontal();

			// m_IsOverDrawOn = EditorGUILayout.Toggle("OverDraw Check", m_IsOverDrawOn);

			// if (particleEffectProfiler && m_IsOverDrawOn)
			// {
			// 	// TODO-待验证的数据
			// 	mPixDrawAverage = particleEffectProfiler.PixDrawAverage;
			// 	mPixActualDrawAverage = particleEffectProfiler.PixActualDrawAverage;
			// 	mPixOverDrawRate = particleEffectProfiler.PixOverDrawRate;
			// 	EditorGUILayout.LabelField("特效原填充像素点", mPixDrawAverage.ToString());
			// 	EditorGUILayout.LabelField("特效实际填充像素点", mPixActualDrawAverage.ToString());
			// 	EditorGUILayout.LabelField("每像素平均重复率", mPixOverDrawRate.ToString());
			// }

			GUILayout.EndArea();
			Handles.EndGUI();
		}

		static void ShowItem(string name, int standardValue, int value)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(name, NumberFormat(standardValue));
			var style = value > standardValue ? Styles.RedLabelStyle : EditorStyles.label;
			EditorGUILayout.LabelField(NumberFormat(value), style);
			EditorGUILayout.EndHorizontal();
		}

		static void ShowItem(string name, long standardValue, long value)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(name, EditorUtility.FormatBytes(standardValue));
			var style = value > standardValue ? Styles.RedLabelStyle : EditorStyles.label;
			EditorGUILayout.LabelField(EditorUtility.FormatBytes(value), style);
			EditorGUILayout.EndHorizontal();
		}

		static void ShowItem(string name, string standardValue, string value, GUIStyle guiStyle = null)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(name, standardValue, guiStyle);
			EditorGUILayout.LabelField(value, guiStyle);
			EditorGUILayout.EndHorizontal();
		}

		static string NumberFormat(int number)
		{
			if (number >= 100000)
			{
				return (number / 1000D).ToString("0.#K");
			}

			if (number >= 1000)
			{
				return (number / 1000D).ToString("0.##K");
			}

			return number.ToString("#,0");
		}
		
		// ========================= gameview ======================================
		static Type _gameViewType = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView");
		
		// [MenuItem ("Window/GameViewWindow Utility")]
		static void ShowGameViewUtility()
		{
			var window = EditorWindow.GetWindow(_gameViewType);
			if(window.docked)
			{
				window.Close();
				EditorWindow.GetWindow(_gameViewType, true);
			}
		}
		
		// [MenuItem ("Window/GameViewWindow")]
		static void ShowGameView()
		{
			var window = EditorWindow.GetWindow(_gameViewType);
			// window.Close();
			// EditorWindow.GetWindow(_gameViewType);
		}
		// ========================= Styles ======================================


		private class Styles
		{
			private static GUIStyle _redLabelStyle;

			public static GUIStyle RedLabelStyle
			{
				get
				{
					if (_redLabelStyle == null)
					{
						_redLabelStyle = new GUIStyle(EditorStyles.boldLabel);
						_redLabelStyle.normal.textColor = Color.red;
					}

					return _redLabelStyle;
				}
			}
		}
	}
}