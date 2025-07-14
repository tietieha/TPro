using System;
using UnityEngine;
using UnityEngine.UI;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace TEngine
{
    [RequireComponent(typeof(RectTransform))]
    public class FullscreenImageAdapter : MonoBehaviour
    {
        private RectTransform rectTransform;
        private Image backgroundImage;
        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            backgroundImage = GetComponent<Image>();

            if (backgroundImage == null)
            {
                Debug.LogWarning("FullscreenImageAdapter: No Image component found on " + name);
                Destroy(this);
                return;
            }
            GameModule.UI.OnScreenOrientation += _onOrientation;
            
            AdjustBackgroundSize();
        }

        void AdjustBackgroundSize()
        {
            backgroundImage.SetNativeSize();
            rectTransform.anchoredPosition = Vector2.zero;
            
            var canvasScaler = GameModule.UI.GetCanvasScaler();
            var screenSize = canvasScaler.GetComponent<RectTransform>().sizeDelta;
            var bgSize = rectTransform.sizeDelta;
            var devRatio = bgSize.x / bgSize.y;
            var screenRatio = screenSize.x / screenSize.y;
            
            Vector2 newSize;
            // 长了，按照高等比拉伸
            if (screenRatio < devRatio)
            {
                var ratio = screenSize.y / bgSize.y;
                newSize = new Vector2(bgSize.x * ratio, screenSize.y);
            }
            // 短了，按照高、宽等比拉伸
            else
            {
                var ratio = screenSize.x / bgSize.x;
                newSize = new Vector2(screenSize.x, bgSize.y * ratio);
            }

            rectTransform.sizeDelta = newSize;
            
            rectTransform.anchorMin = Vector2.one * 0.5f;
            rectTransform.anchorMax = Vector2.one * 0.5f;
            
            // var safeArea = GameModule.UI.GetSafeArea();
            // if (GameModule.UI.Orientation == ScreenOrientation.LandscapeLeft)
            // {
            //     rectTransform.anchoredPosition = new Vector2(-safeArea.x / 2, 0);
            // }
            // else if(GameModule.UI.Orientation == ScreenOrientation.LandscapeRight)
            // {
            //     rectTransform.anchoredPosition = new Vector2(safeArea.x / 2, 0);
            // }
        }

        private void _onOrientation()
        {
            AdjustBackgroundSize();
        }
        
        private void OnDestroy()
        {
            if (GameModule.UI != null)
            {
                GameModule.UI.OnScreenOrientation -= _onOrientation;
            }
        }
        
#if ODIN_INSPECTOR
        [Button("测试")]
        public void Test()
        {
            AdjustBackgroundSize();
        }
#endif
    }
}
