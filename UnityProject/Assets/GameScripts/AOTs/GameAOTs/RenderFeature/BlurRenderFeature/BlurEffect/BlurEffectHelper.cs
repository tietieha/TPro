// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-12-06 15:57 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using TEngine;
using UnityEngine;

namespace BlurEffect
{
	public class BlurEffectHelper
	{
		const string BLUR_SHADER_NAME = "Unlit/BlurEffect";

		private static Shader _blurShader;
		private static Material _blurMat;

		public static RenderTexture BlurTexture(Texture2D src, BlurEffectParam param)
		{
			if (_blurShader == null)
			{
#if UNITY_EDITOR
				_blurShader = Shader.Find(BLUR_SHADER_NAME);
#else
				_blurShader = GameModule.Resource.LoadAsset<Shader>("Unlit-BlurEffect");
#endif
			}

			if (_blurShader == null)
				return null;

			if (_blurMat == null)
				_blurMat = new Material(_blurShader);
			
			int rtW = src.width / param.blur_down_sample;
			int rtH = src.height / param.blur_down_sample;
			var rt = RenderTexture.GetTemporary(rtW, rtH, 0);
			rt.name = "RT_UIBlur";
			rt.filterMode = FilterMode.Bilinear;
			Graphics.Blit(src, rt);
			
			_blurMat.SetFloat("_Dark", param.color_to_dark);
			_blurMat.SetFloat("_Saturate", param.color_saturate);
			for (int i = 0; i < param.blur_iteration; i++)
			{
				_blurMat.SetFloat("_BlurSize", (1.0f + i * param.blur_spread) * param.blur_size); // 设置模糊扩散uv偏移
				
				var temp_rt = RenderTexture.GetTemporary(rtW, rtH, 0);
				Graphics.Blit(rt, temp_rt, _blurMat, 0);
				Graphics.Blit(temp_rt, rt, _blurMat, 1);
				RenderTexture.ReleaseTemporary(temp_rt); // 释放临时RT
			}
			return rt;
		}
	}
}