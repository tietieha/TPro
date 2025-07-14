// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-09-12 17:09 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using TEngine;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

namespace BlurEffect
{
	[RequireComponent(typeof(RawImage))]
	public class UIBlur : MonoBehaviour
	{
#if UNITY_EDITOR
		[ShowInInspector, ReadOnly]
		[InfoBox("$UIBLUR_COUNT_INFO", InfoMessageType = InfoMessageType.Info, VisibleIf = "$INFOBOX_VISIBLE")]
		[InfoBox("$UIBLUR_COUNT_INFO", InfoMessageType = InfoMessageType.Error, VisibleIf = "$ERRORBOX_VISIBLE")]
#endif
		private static int S_UIBLUR_COUNT = 0;

		private static int S_UIBLUR_COUNT_MAX = 3;

		private string UIBLUR_COUNT_INFO => $"当前背景模糊层数: {S_UIBLUR_COUNT}";
		private bool INFOBOX_VISIBLE => S_UIBLUR_COUNT <= S_UIBLUR_COUNT_MAX;
		private bool ERRORBOX_VISIBLE => S_UIBLUR_COUNT > S_UIBLUR_COUNT_MAX;

		[SerializeField] private BlurEffectParam blurEffectParam;

		private RawImage _rawImage;
		private RenderTexture _renderTexture;

		private Texture2D _cacheScreenTexture;

		private void OnEnable()
		{
			_rawImage = GetComponent<RawImage>();
			if (_rawImage == null)
				return;
			_rawImage.enabled = false;

			if (S_UIBLUR_COUNT >= S_UIBLUR_COUNT_MAX)
			{
				Log.Error($"背景模糊叠加超{S_UIBLUR_COUNT_MAX}层了, 不会生效了");
				return;
			}

			S_UIBLUR_COUNT++;

			SceneCameraManager.Instance.GetScreenCapture(GetScreenCaprureCallback);
		}

		private void GetScreenCaprureCallback(Texture2D texture2D)
		{
			if (texture2D == null)
			{
				_rawImage.enabled = false;
			}
			else
			{
				_cacheScreenTexture = texture2D;
				BlurTexture();
#if UNITY_EDITOR
#else
				Destroy(_cacheScreenTexture);
#endif
			}
		}

		private void BlurTexture()
		{
			if (_rawImage == null)
				return;

			if (_renderTexture != null)
				RenderTexture.ReleaseTemporary(_renderTexture);
			_renderTexture = BlurEffectHelper.BlurTexture(_cacheScreenTexture, blurEffectParam);

			if (_renderTexture != null)
			{
				_rawImage.texture = _renderTexture;
				_rawImage.enabled = true;
			}
			else
			{
				_rawImage.enabled = false;
			}
		}

		private void OnDisable()
		{
			if (_rawImage != null)
				S_UIBLUR_COUNT--;
			Destroy(_cacheScreenTexture);
			if (_renderTexture != null)
			{
				RenderTexture.ReleaseTemporary(_renderTexture);
			}
		}

		private void OnValidate()
		{
			BlurTexture();
		}
	}
}