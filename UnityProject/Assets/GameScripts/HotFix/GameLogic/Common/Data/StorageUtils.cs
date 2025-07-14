using UnityEngine;

namespace Utils
{
    public static class StorageUtils
    {
        public static bool HasKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            return PlayerPrefs.HasKey(key);
        }

        public static void SetString(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            PlayerPrefs.SetString(key, value);
        }

        public static string GetString(string key, string defaultValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }

            return PlayerPrefs.GetString(key, defaultValue);
        }

        public static void SetFloat(string key, float value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            PlayerPrefs.SetFloat(key, value);
        }

        public static float GetFloat(string key, float defaultValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }

            return PlayerPrefs.GetFloat(key, defaultValue);
        }


        public static void SetBool(string key, bool value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public static bool GetBool(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            return PlayerPrefs.GetInt(key) != 0;
        }

        public static bool GetBool(string key, bool defaultValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }

            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) != 0;
        }
    }
}