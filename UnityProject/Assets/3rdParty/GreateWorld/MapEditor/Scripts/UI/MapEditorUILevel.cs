// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-11-10 18:30 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System;
using TMPro;
using UnityEngine;

#if UNITY_EDITOR
namespace GEngine.MapEditor
{
	public class MapEditorUILevel : MonoBehaviour
	{
		[SerializeField] private int _uiScaleMinHeight = 60;
		[SerializeField] private int _uiScaleMaxHeight = 200;
		private Transform _cameraTrans;
		private RectTransform _trans;

		public TMP_Text text;

		public void SetText(string txt)
		{
			if (text != null)
				text.text = txt;
		}

		private void Start()
		{
			_cameraTrans = Camera.main.transform;
			_trans = GetComponent<RectTransform>();
		}

		private void Update()
		{
			float dis = _cameraTrans.position.y;
			dis = Math.Clamp(dis, _uiScaleMinHeight, _uiScaleMaxHeight);
			var scale = dis / _uiScaleMinHeight;
			_trans.localScale = new Vector3(scale, scale, scale);
		}
	}
}
#endif