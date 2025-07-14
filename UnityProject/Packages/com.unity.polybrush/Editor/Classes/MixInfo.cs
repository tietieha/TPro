using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Polybrush
{
    internal class MixInfo
    {
        public Texture2D mixMap;
        public Color[] realMixColor = null;
        public int realMixX = 0;
        public int realMixY = 0;
        public int realMixWidth = 0;
        public int realMixHeight = 0;

        public void SetMixMap(Texture2D tex)
        {
            var change = mixMap != tex;
            mixMap = tex;
            EditorUtility.SetDirty(mixMap);
            if (change)
                checkMixTexture();
        }

        public bool ResetMixMap()
        {
            if (mixMap != null && realMixColor != null && realMixColor.Length > 0)
            {
                mixMap.SetPixels(realMixX, realMixY, realMixWidth, realMixHeight, realMixColor, 0);
                mixMap.Apply();
                return true;
            }

            // string p = AssetDatabase.GetAssetPath(mixMap);
            // TextureImporter ti = TextureImporter.GetAtPath(p) as TextureImporter;
            // TextureImporterPlatformSettings settingAndroid = ti.GetPlatformTextureSettings("Android");
            // TextureImporterPlatformSettings settingIOS = ti.GetPlatformTextureSettings("iPhone");
            //
            // ti.isReadable = false;
            // settingAndroid.overridden = true;
            // settingAndroid.format = TextureImporterFormat.ASTC_6x6;
            // ti.SetPlatformTextureSettings(settingAndroid);
            //
            //
            // settingIOS.overridden = true;
            // settingIOS.format = TextureImporterFormat.ASTC_6x6;
            // ti.SetPlatformTextureSettings(settingIOS);
            //
            // ti.SaveAndReimport();

            return false;
        }

        public void SaveMixMap()
        {
            ResetAll();
            if (mixMap != null)
            {
                EditorUtility.SetDirty(mixMap);
                AssetDatabase.SaveAssets();

                var mixMapPath = AssetDatabase.GetAssetPath(mixMap);
                var ext = Path.GetExtension(mixMapPath);
                if(ext.Equals(".png"))
                {
                    File.WriteAllBytes(mixMapPath, mixMap.EncodeToPNG());
                }
                else if (ext.Equals(".tga"))
                {
                    File.WriteAllBytes(mixMapPath, mixMap.EncodeToTGA());
                }
                // 如果是.texture2D 如何保存
                else if (ext.Equals(".texture2D"))
                {

                }
                else
                {
                    Debug.LogError("Unsupported texture format: " + ext);
                }
            }
        }

        private void checkMixTexture()
        {
            if (mixMap != null)
            {
                bool changed = false;
                string p = AssetDatabase.GetAssetPath(mixMap);
                TextureImporter ti = TextureImporter.GetAtPath(p) as TextureImporter;
                if (ti == null)
                    return;
                TextureImporterPlatformSettings settingDefault = ti.GetDefaultPlatformTextureSettings();
                TextureImporterPlatformSettings settingAndroid = ti.GetPlatformTextureSettings("Android");
                TextureImporterPlatformSettings settingIOS = ti.GetPlatformTextureSettings("iPhone");
                if (!ti.isReadable)
                {
                    changed = true;
                    ti.isReadable = true;
                }

                if(settingDefault.format != TextureImporterFormat.RGBA32)
                {
                    changed = true;
                    settingDefault.format = TextureImporterFormat.RGBA32;
                    ti.SetPlatformTextureSettings(settingDefault);
                }

                if(settingAndroid.format != TextureImporterFormat.RGBA32)
                {
                    changed = true;
                    settingAndroid.overridden = true;
                    settingAndroid.format = TextureImporterFormat.RGBA32;
                    ti.SetPlatformTextureSettings(settingAndroid);
                }

                if(settingIOS.format != TextureImporterFormat.RGBA32)
                {
                    changed = true;
                    settingIOS.overridden = true;
                    settingIOS.format = TextureImporterFormat.RGBA32;
                    ti.SetPlatformTextureSettings(settingIOS);
                }

                if (changed)
                    ti.SaveAndReimport();

            }
        }

        public void ResetAll()
        {
            ResetMixMap();
            realMixX = 0;
            realMixY = 0;
            realMixWidth = 0;
            realMixHeight = 0;
            realMixColor = null;
        }

        private Texture2D CreateTextureByRawData(Texture2D sourceTexture)
        {
            if (sourceTexture != null)
            {
                Texture2D newTextureByRawData = new Texture2D(sourceTexture.width, sourceTexture.height, TextureFormat.RGBA32, false);
                // 对于运行时纹理生成，也可以通过GetRawTextureData直接写入纹理数据，返回一个Unity.Collections.NativeArray
                // 这可以更快，因为它避免了 LoadRawTextureData 会执行的内存复制。
                newTextureByRawData.LoadRawTextureData(sourceTexture.GetRawTextureData());
                newTextureByRawData.Apply();
                return newTextureByRawData;
            }
            else
            {
                Debug.LogWarning("Texture is null");
                return null;
            }
        }
    }
}
