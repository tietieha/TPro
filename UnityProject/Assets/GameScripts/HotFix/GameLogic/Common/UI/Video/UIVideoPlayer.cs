using System;
using TEngine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using XLua;

namespace GameLogic
{
    [LuaCallCSharp]
    [RequireComponent(typeof(VideoPlayer))]
    [RequireComponent(typeof(RawImage))]
    public class UIVideoPlayer : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private RawImage rawImage;
        [SerializeField] public RectTransform rawImageRect;

        public Action OnVideoPlayEnd; // Lua 回调

        private void Awake()
        {
            videoPlayer.loopPointReached += (vp) => { OnVideoFinished(vp); };
            videoPlayer.errorReceived += (vp, message) => { Log.Error($"[UIVideoPlayer] {message}"); };
        }

        private void OnVideoFinished(VideoPlayer vp)
        {
            OnVideoPlayEnd?.Invoke();
        }

        private void AdjustVideoSize(VideoPlayer vp)
        {
            float videoWidth = vp.width;
            float videoHeight = vp.height;

            if (videoWidth == 0 || videoHeight == 0) return;

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            float screenRatio = screenWidth / screenHeight;
            float videoRatio = videoWidth / videoHeight;

            Vector2 newSize = rawImageRect.sizeDelta;

            if (videoRatio > screenRatio)
            {
                // 视频更宽，以屏幕宽度为基准
                newSize.x = screenWidth;
                newSize.y = screenWidth / videoRatio;
            }
            else
            {
                // 视频更高，以屏幕高度为基准
                newSize.y = screenHeight;
                newSize.x = screenHeight * videoRatio;
            }

            rawImageRect.sizeDelta = newSize;
        }


        public void PlayVideo(string videoPath, Action onEnd)
        {
            OnVideoPlayEnd = onEnd;

            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = $"{Application.streamingAssetsPath}/{videoPath}";
            videoPlayer.Prepare();
            videoPlayer.prepareCompleted += (vp) =>
            {
                Debug.Log($"[UIVideoPlayer] {videoPath} prepareCompleted");
                rawImage.texture = vp.texture;
                AdjustVideoSize(vp);
                vp.Play();
            };
        }

        public void StopVideo()
        {
            videoPlayer.Stop();
        }

        public void SkipVideo()
        {
            videoPlayer.Stop();
            OnVideoFinished(videoPlayer);
        }
    }
}