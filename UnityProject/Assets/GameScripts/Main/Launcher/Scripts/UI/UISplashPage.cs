using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TEngine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UISplashPage : MonoBehaviour
{
    [Serializable]
    public class SplashPageData
    {
        public GameObject Go;
        public float Duration;
    }
    public List<SplashPageData> SplashPageDatas;
    public string MovieName;

    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private RawImage _rawImage;
    
    private Action _onSplashEnd;
    private int _currentSplashIndex = 0;

    public void StartSplash(Action callback)
    {
        _currentSplashIndex = 0;
        _onSplashEnd = callback;
        StartCoroutine(FadeNextLogo());
    }

    IEnumerator FadeNextLogo()
    {
        while (_currentSplashIndex < SplashPageDatas.Count)
        {
            var splashPage = SplashPageDatas[_currentSplashIndex];
            var curGo = splashPage.Go;
            var duration = splashPage.Duration;

            curGo.SetActive(true);
            if (_currentSplashIndex == 1)
            {
                PlaySplashMovie();
            }
            yield return new WaitForSeconds(duration);
            curGo.SetActive(false);
            _currentSplashIndex++;
        }

        if (_onSplashEnd != null)
        {
            _onSplashEnd();
        }
        else
        {
            Log.Error("SplashPage callback is null");
        }
    }

    private void PlaySplashMovie()
    {
        _videoPlayer.source = VideoSource.Url;
        _videoPlayer.url = PlatformUtils.GetStreamingAssetsUrl(MovieName);
        _videoPlayer.Prepare();
        _videoPlayer.prepareCompleted += OnPrepareCompleted;
    }

    private void OnPrepareCompleted(VideoPlayer vp)
    {
        _rawImage.texture = vp.texture;
        vp.Play();
    }
}