namespace UnityEngine.HWeather
{
    public enum ELandType {
        /// <summary>
        /// @name 无效的
        /// </summary>
        ENull = -2,
        /// <summary>
        /// @name 全局
        /// </summary>
        EAll = -1,
        /// <summary>
        /// @name 草地
        /// </summary>
        EGrassland = 0,
        /// <summary>
        /// @name 雪地
        /// </summary>
        ESnowland = 1,
        /// <summary>
        /// @name 荒地
        /// </summary>
        EWasteland = 2,
        /// <summary>
        /// @name 主城
        /// </summary>
        EMainCity = 3,
        
        EMax = 4,
    }
    
    public enum EWeatherStrength {
        ENull = -1,
        /// <summary>
        /// @name 天气强度小
        /// </summary>
        ESmall = 0,
        /// <summary>
        /// @name 天气强度中
        /// </summary>
        EMiddle = 1,
        /// <summary>
        /// @name 天气强度大
        /// </summary>
        EBig = 2,
  
    }
    public enum EWeatherType {

        ENull = -1,
        /// <summary>
        /// @name 晴天
        /// </summary>
        ESunny = 0,
        /// <summary>
        /// @name 下雪
        /// </summary>
        ESnow = 1,
        /// <summary>
        /// @name 沙尘
        /// </summary>
        EDust = 2,
        /// <summary>
        /// @name 下雨
        /// </summary>
        ERain = 3
  
    }
    public enum EDaytimeType {

        ENull = -1,
        /// <summary>
        /// @name 深夜
        /// </summary>
        ENightLate = 0,
        /// <summary>
        /// @name 清晨
        /// </summary>
        EMorning = 1,
        /// <summary>
        /// @name 白天
        /// </summary>
        EDaytime = 2,
        /// <summary>
        /// @name 黄昏
        /// </summary>
        EDusk = 3,
        /// <summary>
        /// @name 夜晚
        /// </summary>
        ENight = 4
  
    }
    
    public static class HWeatherCommon
    {
        public  static bool s_reDrawShadow = false;   //重绘阴影的标志
        public  static bool s_openDayTime = true;     //昼夜开关
        public  static bool s_openWeather = true;     //天气开关
        public static ELandType  GetLandType(string name)
        {
            if (name.Contains("Snowland"))
            {
                return ELandType.ESnowland;
            }

            //因为Base是全局的，所以返回EAll即可。
            if (name.Contains("Base"))
            {
                return ELandType.EAll;
            }

            if (name.Contains("Grassland"))
            {
                return ELandType.EGrassland;
            }

            if (name.Contains("Wasteland"))
            {
                return ELandType.EWasteland;
            }

            return ELandType.EMainCity;

        }

        public static EWeatherType GetWeatherType(string name)
        {
            if (name.Contains("Snow"))
            {
                return EWeatherType.ESnow;
            }

            //因为Base是全局的，所以返回Sunny即可。
            if (name.Contains("Base"))
            {
                return EWeatherType.ESunny;
            }

            if (name.Contains("Rain"))
            {
                return EWeatherType.ERain;
            }

            if (name.Contains("Sand"))
            {
                return EWeatherType.EDust;
            }
            
            return EWeatherType.ESunny;
        }
    }
    
}
