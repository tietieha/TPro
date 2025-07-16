#if UNITY_EDITOR
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace RenderTools.AnimInstance
{
	[ExecuteInEditMode]
	public class AnimatorPose : MonoBehaviour
	{
		public Animator animator;
#if ODIN_INSPECTOR
		[ListDrawerSettings(OnBeginListElementGUI = "BeginDrawAnimationsListElement", OnEndListElementGUI = "EndDrawAnimationsListElement")]
#endif
		public AnimationClip[] clips;
#if ODIN_INSPECTOR
		[PropertyRange("Min", "Max"), OnValueChanged("OnProgressChanged")]
#endif
		public int progress = 0;
		
#if ODIN_INSPECTOR
		[PropertyRange("Min", "MaxHumaniod"), OnValueChanged("OnHumaniodProgressChanged")]
#endif
		public float humaniodprogress = 0;

		private int Min = 0;
		private int Max = 0;
		private float MaxHumaniod = 0;
		private AnimationClip _currentClip;
		private void Awake()
		{
			animator = GetComponentInChildren<Animator>();
			AnimatorController controller = (AnimatorController)animator.runtimeAnimatorController;
			clips = controller.animationClips;
		}
		
		private void BeginDrawAnimationsListElement(int index)
		{
			EditorGUILayout.BeginHorizontal();
		}
		private void EndDrawAnimationsListElement(int index)
		{
			if (GUILayout.Button("Copy"))
			{
				GUIUtility.systemCopyBuffer = clips[index].name;
			}
			if (GUILayout.Button("Play"))
			{
				_currentClip = clips[index];
				progress = 0;
				humaniodprogress = 0.001f;
				Max = (int) (_currentClip.length * 30);
				MaxHumaniod = _currentClip.length;
				// OnProgressChanged(progress);
				OnHumaniodProgressChanged(humaniodprogress);
			}
			EditorGUILayout.EndHorizontal();
		}
		
		private void OnProgressChanged(int value)
		{
			// float time = _currentClip.length * value;
			animator.enabled = true;
			_currentClip.SampleAnimation(animator.gameObject, value / 30f);
			animator.enabled = false;
			// _currentClip.SampleAnimation(gameObject, time);
			// animator.Play(0, 0, time);
			// animator.Stop();
		}
		
		private void OnHumaniodProgressChanged(float value)
		{
			// float time = _currentClip.length * value;
			// animator.enabled = true;
			// _currentClip.SampleAnimation(animator.gameObject, value / 30f);
			// animator.enabled = false;
			// _currentClip.SampleAnimation(gameObject, time);
			animator.speed = 0;
			animator.Play(_currentClip.name, 0, value / _currentClip.length);
			// animator.Stop();
		}
	}
}
#endif
