// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2022-10-17 17:05 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************
#if ODIN_INSPECTOR
using System;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace RenderTools.AnimInstance
{
	[CustomEditor(typeof(AnimInstance))]
	[CanEditMultipleObjects]
	public class AnimInstanceEditor : OdinEditor
	{
		private AnimInstance _current;
		private int _progress;
		
		public override void OnInspectorGUI()
		{
			_current = target as AnimInstance;
			
			SirenixEditorGUI.Title($"当前动画:	{_current.CurAnimName}", String.Empty, TextAlignment.Left, true, true);
			SirenixEditorGUI.Title($"当前速度:", String.Empty, TextAlignment.Left, true, true);
			{
				EditorGUI.indentLevel += 5;
				_current.AnimSpeed = EditorGUILayout.Slider(_current.AnimSpeed, 0, 10);
				EditorGUI.indentLevel -= 5;
			}
			
			SirenixEditorGUI.Title($"逐帧播放:", String.Empty, TextAlignment.Left, true, true);
			{
				EditorGUI.indentLevel += 5;
				_progress = _current.CurFrame;
				EditorGUI.BeginChangeCheck();
				{
					_progress = EditorGUILayout.IntSlider(_progress, 0,
						_current.GetAnimFrameCount(_current.CurAnimName) - 1);
				}
				if (EditorGUI.EndChangeCheck())
				{
					_current.PlayPose(_current.CurAnimIndex, _progress);
				}

				EditorGUI.indentLevel -= 5;
			}

			EditorGUILayout.Separator();
			
			base.OnInspectorGUI();
		}
		
		public override bool RequiresConstantRepaint()
		{
			return true;
		}
	}
}
#endif