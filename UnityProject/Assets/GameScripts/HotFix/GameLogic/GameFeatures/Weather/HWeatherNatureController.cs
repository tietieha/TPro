using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityEngine.HWeather
{
    [ExecuteInEditMode]
    [AddComponentMenu("HWeather/HWeather Nature Controller")]
    [RequireComponent(typeof(HWeatherNatureController))]
    public class HWeatherNatureController : MonoBehaviour
    {
        // References
        private HWeatherNatureController m_NatureController;

        public float g_CrossFadingNear = 105.0f;
        public float g_CrossFadingFar =  110.0f;

        private void Reset()
        {
        }

        private void Start()
        {
            m_NatureController = GetComponent<HWeatherNatureController>();
            UpdateStaticData();
        }

        private void OnEnable()
        {
            UpdateStaticData();
        }

        private void Update()
        {
            // Editor only
            #if UNITY_EDITOR
                UpdateStaticData();
            #endif
            // #if ART_PROJECT
                // UpdateStaticData();
            // #endif
        }
        void OnDestroy()
        {

        }
        //设置静态数据
        public void UpdateStaticData()
        {
            Shader.SetGlobalFloat("g_CrossFadingNear", g_CrossFadingNear);
            Shader.SetGlobalFloat("g_CrossFadingFar", g_CrossFadingFar);
        }


    }
}