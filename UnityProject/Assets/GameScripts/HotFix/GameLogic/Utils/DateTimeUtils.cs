using System;

namespace Utils
{
    public class DateTimeUtils
    {
        public static int CurrentTimeZone()
        {
            var gmtOff = (DateTime.Now - DateTime.UtcNow).TotalSeconds;
            var gmtOffValue = System.Math.Round(gmtOff);
            gmtOffValue /= 3600;
            return (int)gmtOffValue;
        }
    }
}