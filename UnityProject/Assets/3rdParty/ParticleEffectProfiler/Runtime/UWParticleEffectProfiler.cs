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


#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;

/// <summary>
/// 特效性能分析工具的管理类
/// 将此类添加到特效上即可
/// </summary>
namespace UWParticleSystemProfiler
{
	public class UWParticleEffectProfiler : MonoBehaviour
	{
		private static MethodInfo m_CalculateEffectUIDataMethod;

		public static MethodInfo CalculateEffectUIDataMethod
		{
			get
			{
				if (m_CalculateEffectUIDataMethod == null)
				{
#if UNITY_2017_1_OR_NEWER
					m_CalculateEffectUIDataMethod = typeof(ParticleSystem).GetMethod("CalculateEffectUIData", BindingFlags.Instance | BindingFlags.NonPublic);
#else
					m_CalculateEffectUIDataMethod =
																								                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                typeof(ParticleSystem).GetMethod("CountSubEmitterParticles", BindingFlags.Instance | BindingFlags.NonPublic);
#endif
				}

				return m_CalculateEffectUIDataMethod;
			}
		}


		private int mInitiateDC = 0;
		private int mInitiateTris = 0;
		private int mInitiateShdowCaster = 0;

		/// <summary>
		///									实际值													参考值
		/// </summary>
		private int  mTexCount				 = -1; public static int  Standard_TexCount				= 5;
		private int  mMeshCount              = -1; public static int  Standard_MeshCount            = 5;
		private long mTexMemorySize          = -1; public static long Standard_TexMemorySize        = 256 * 1024;
		private long mMeshMemorySize         = -1; public static long Standard_MeshMemorySize       = 256 * 1024;
		private int  mMaxParticleSystemsDC   = -1; public static int  Standard_ParticleSystemsDC    = 10;
		private int  mParticleSystemsCount   = -1; public static int  Standard_ParticleSystemsCount = 10;
		private int  mMaxParticleCount       = -1; public static int  Standard_ParticleCount        = 60;
		private int  mMaxRuntimeTris         = -1; public static int  Standard_RuntimeTris          = 2000;
		private int  mSkinMeshRenderCount    = -1; public static int  Standard_SkinMeshRenderCount  = 0;
		private int  mMaxRuntimeShadowCaster = -1; public static int  Standard_RuntimeShadowCaster  = 0;


		private int   mParticleCount     = -1;
		private int   mPixOverDrawRate   = -1;
		private int   mPixDrawAverage    = -1, mPixActualDrawAverage = -1;
		private int   mParticleSystemsDC = -1;
		private float mParticleDuration  = -1;

		public int TexCount
		{
			get
			{
				if (mTexCount < 0)
				{
					GetRuntimeMemorySize();
				}

				return mTexCount;
			}
		}

		public long TexMemorySize
		{
			get
			{
				if (mTexMemorySize < 0)
				{
					GetRuntimeMemorySize();
				}

				return mTexMemorySize;
			}
		}

		public int MeshCount
		{
			get
			{
				if (mMeshCount < 0)
				{
					GetRuntimeMemorySize();
				}

				return mMeshCount;
			}
		}

		public long MeshMemorySize
		{
			get
			{
				if (mMeshMemorySize < 0)
				{
					GetRuntimeMemorySize();
				}

				return mMeshMemorySize;
			}
		}

		public int SkinMeshRenderCount
		{
			get { return mSkinMeshRenderCount; }
		}

		public int MaxRuntimeTris
		{
			get { return mMaxRuntimeTris; }
		}

		public int MaxRuntimeShadowCaster
		{
			get { return mMaxRuntimeShadowCaster; }
		}

		public float ParticleDuration
		{
			get
			{
				if (mParticleDuration < 0)
				{
					mParticleDuration = gameObject.GetParticleDuration(true, true, true);
				}
				return mParticleDuration;
			}
		}

		public int ParticleSystemsCount
		{
			get { return mParticleSystemsCount; }
		}

		public int ParticleCount
		{
			get { return mParticleCount; }
		}

		public int MaxParticleCount
		{
			get { return mMaxParticleCount; }
		}

		public int ParticleSystemsDC
		{
			get { return mParticleSystemsDC; }
		}

		public int MaxParticleSystemsDC
		{
			get { return mMaxParticleSystemsDC; }
		}

		public List<string> AnimatorPaths
		{
			get { return mAnimatorPaths; }
		}

		public List<string> UnexceptedAnimationPaths
		{
			get { return mUnexceptedAnimationPaths; }
		}

		public List<Component> ParentExtraComponents
		{
			get { return mParentExtraComponents; }
		}

		// 特效是否循环播放
		public bool loop = false;


		[SerializeField] private AnimationCurve ParticleCountCurve = new AnimationCurve();
		[SerializeField] private AnimationCurve DrawCallCurve = new AnimationCurve();
		[SerializeField] private AnimationCurve OverdrawCurve = new AnimationCurve();
		
		private ParticleSystem[] m_ParticleSystems;

		UWParticleEffectCurve m_CurveParticleCount;
		UWParticleEffectCurve m_CurveDrawCallCount;
		UWParticleEffectCurve m_CurveOverdraw;

		private bool mStopCheck = true;

		private List<string> mAnimatorPaths = new List<string>();
		private List<Component> mParentExtraComponents = new List<Component>();
		private List<string> mUnexceptedAnimationPaths = new List<string>();

		private void Update()
		{
			if (mStopCheck)
				return;

			UpdateStats();
			UpdateParticleCount();
			UpdateParticleEffecDrawCall();
		}

		private void LateUpdate()
		{
			if (mStopCheck)
				return;

			UpdateParticleCountCurve();
			UpdateDrawCallCurve();
		}

		public void Clear()
		{
			mTexCount = mMeshCount = -1;
			mTexMemorySize = mMeshMemorySize = -1;
			mParticleSystemsCount = -1;
			mParticleSystemsDC = mMaxParticleSystemsDC = -1;
			mParticleCount = mMaxParticleCount = -1;
			mPixDrawAverage = mPixActualDrawAverage = -1;
			mPixOverDrawRate = -1;
			mSkinMeshRenderCount = -1;
			mInitiateDC = 0;
			mInitiateTris = 0;
			mInitiateShdowCaster = 0;
			mStopCheck = false;
		}

		public void InitData()
		{
			Application.targetFrameRate = UWParticleEffectCurve.FPS;
			if (m_CurveParticleCount == null)
				m_CurveParticleCount = new UWParticleEffectCurve();
			if (m_CurveDrawCallCount == null)
				m_CurveDrawCallCount = new UWParticleEffectCurve();
			if (m_CurveOverdraw == null)
				m_CurveOverdraw = new UWParticleEffectCurve();

			Clear();
			InitStats();
			GetRuntimeMemorySize();
			GetParticleSystemCount();
			DoSearchSkinMeshRender();
			DoSearchAnimator();
			DoSearchUnexceptedAnimation();
			DoSearchParentExtraComponent();
			Update();
		}

		private void DoSearchParentExtraComponent()
		{
			mParentExtraComponents.Clear();
			var components = gameObject.GetComponents<Component>();
			foreach (var component in components)
			{
				if (!(component is Transform ||
				      component is UWParticleEffectProfiler))
				{
					mParentExtraComponents.Add(component);
				}
			}
		}

		private void DoSearchUnexceptedAnimation()
		{
			mUnexceptedAnimationPaths.Clear();
			var particleSystems = m_ParticleSystems;
			foreach (var particleSystem in particleSystems)
			{
				var animations = particleSystem.gameObject.GetComponentsInChildren<Animation>();
				if (animations.Length > 0)
				{
					foreach (var animation in animations)
					{
						mUnexceptedAnimationPaths.Add(animation.gameObject.transform.GetFullHierarchyPath());
					}
				}
			}
		}

		private void DoSearchSkinMeshRender()
		{
			var renders = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			mSkinMeshRenderCount = renders.Length;
		}

		private void DoSearchAnimator()
		{
			mAnimatorPaths.Clear();

			var animators = gameObject.GetComponentsInChildren<Animator>(true);
			foreach (var animator in animators)
			{
				mAnimatorPaths.Add(animator.gameObject.transform.GetFullHierarchyPath());
			}
		}

		private void InitStats()
		{
			mInitiateDC = UnityStats.batches;
			mInitiateTris = UnityStats.triangles;
			mInitiateShdowCaster = UnityStats.shadowCasters;
		}

		public void StopCheck()
		{
			mStopCheck = true;
		}

		public void UpdateStats()
		{
			var tris = UnityStats.triangles - mInitiateTris;
			if (mMaxRuntimeTris < tris)
			{
				mMaxRuntimeTris = tris;
			}

			var shadowCasters = UnityStats.shadowCasters - mInitiateShdowCaster;
			if (mMaxRuntimeShadowCaster < shadowCasters)
			{
				mMaxRuntimeShadowCaster = shadowCasters;
			}
		}

		public void UpdateParticleCount()
		{
			if (m_ParticleSystems == null || m_ParticleSystems.Length <= 0)
				return;
			mParticleCount = 0;
			foreach (var ps in m_ParticleSystems)
			{
				mParticleCount += ps.particleCount;
// 				int count = 0;
// #if UNITY_2017_1_OR_NEWER
// 				object[] invokeArgs = {count, 0.0f, Mathf.Infinity};
// 				CalculateEffectUIDataMethod.Invoke(ps, invokeArgs);
// 				count = (int) invokeArgs[0];
// #else
//             object[] invokeArgs = { count };
//             CalculateEffectUIDataMethod.Invoke(ps, invokeArgs);
//             count = (int)invokeArgs[0];
//             count += ps.particleCount;
// #endif
				// mParticleCount += count;
			}

			if (mMaxParticleCount < mParticleCount)
			{
				mMaxParticleCount = mParticleCount;
			}
		}

		public int GetParticleCount()
		{
			return mParticleCount;
		}

		public int GetMaxParticleCount()
		{
			return mMaxParticleCount;
		}

		void UpdateParticleCountCurve()
		{
			ParticleCountCurve = m_CurveParticleCount.UpdateAnimationCurve(mParticleCount, loop, ParticleDuration);
		}

		void UpdateDrawCallCurve()
		{
			DrawCallCurve = m_CurveDrawCallCount.UpdateAnimationCurve(mParticleSystemsDC, loop, ParticleDuration);
		}

		// Data Method
		private void GetRuntimeMemorySize()
		{
			mTexCount = 0;
			mTexMemorySize = 0;
			mMeshCount = 0;
			mMeshMemorySize = 0;

			var deps = EditorUtility.CollectDependencies(new Object[] {gameObject});
			foreach (var obj in deps)
			{
				if (obj is Mesh)
				{
					mMeshCount++;
					mMeshMemorySize += Profiler.GetRuntimeMemorySizeLong(obj);
				}
				else if (obj is Texture || obj is Sprite)
				{
					mTexCount++;
					mTexMemorySize += Profiler.GetRuntimeMemorySizeLong(obj);
				}
			}
		}

		private void GetParticleSystemCount()
		{
			m_ParticleSystems = gameObject.GetComponentsInChildren<ParticleSystem>(true);
			mParticleSystemsCount = m_ParticleSystems.Length;
		}

		private void UpdateParticleEffecDrawCall()
		{
			//因为Camera 实际上渲染了两次，一次用作取样，一次用作显示。 狂飙这里给出了详细的说明：
			//https://networm.me/2019/07/28/unity-particle-effect-profiler/#drawcall-%E6%95%B0%E5%80%BC%E4%B8%BA%E4%BB%80%E4%B9%88%E6%AF%94%E5%AE%9E%E9%99%85%E5%A4%A7-2-%E5%80%8D
			// int drawCall = UnityEditor.UnityStats.batches / 2;

			mParticleSystemsDC = UnityStats.batches - mInitiateDC;

			if (mMaxParticleSystemsDC < mParticleSystemsDC)
			{
				mMaxParticleSystemsDC = mParticleSystemsDC;
			}
		}
	}
}
#endif