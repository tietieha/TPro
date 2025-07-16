#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GEngine.MapEditor
{
    public partial class MapRender : MonoBehaviour
    {
        public void ExportZonePictures()
        {
            if (!CheckMapValid())
                return;
            var dir = Path.Combine(GetMapFolder(), "SeaOutline");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var filename = Path.Combine(dir, $"{map.Name}");
            ExportZonePictures(filename);
        }

        void ExportZonePictures(string filename)
        {
            SaveImageConfig cfg = new SaveImageConfig();
            cfg.width = map.width;
            cfg.height = map.height;
            cfg.scale = 1;
            cfg.images.Clear();

            int count = 0;
            EditorUtility.DisplayProgressBar("导出像素图", $"total : {map.zones.Values.Count}", 0f);

            foreach (var zone in map.zones.Values)
            {
                var sfile = GetFileName(filename, "_" + zone.index + ".png");
                zone.SaveImage(sfile, out int w, out int h, out int x, out int y);

                var ofile = GetFileName(filename, "_" + zone.index + "_outline.png");
                zone.SaveOutLineImage(ofile, out w, out h, out x, out y);

                var image = new ZoneImageInfo(zone, x, y, w, h);
                cfg.images.Add(image);

                count++;
                EditorUtility.DisplayProgressBar("导出像素图", $"zone {zone.index}",
                    (float)count / map.zones.Values.Count);
            }

            // var cfgFile = GetFileName(filename, ".bytes");
            // var bw = new BinaryWriter(new FileStream(cfgFile, FileMode.Create));
            // cfg.Save(map, bw);
            // bw.Close();

            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("导出像素图", $"像素图导出完成，共{map.zones.Values.Count}张！", "确定");
        }


        public void ExportIslandTemplate()
        {
            GetOpenFileDialog("加载岛屿模板", LoadIslands, new[] { "island", "" });
        }

        private void _ExportSailLine(string fileName)
        {
            // var sFileName = GetFileName(fileName, ".bytes");
            // var bw = new BinaryWriter(new FileStream(sFileName, FileMode.Create));
            // map.ExportSailPoint2Server(bw);
            // bw.Close();

            var cFileName = GetFileName(fileName + "_connect", ".bytes");
            var bwLine = new BinaryWriter(new FileStream(cFileName, FileMode.Create));
            map.ExportSailConnect2Server(bwLine);
            bwLine.Close();

            EditorUtility.DisplayDialog("导出航线", $"导出完成!!! \n{fileName}", "确定");
        }

        public void ExportSailLine()
        {
            if (CheckSailLine())
            {
                var filename = GetExportPath();
                _ExportSailLine(filename);
                // GetSaveFileDialog("导出航线", _ExportSailLine);
            }
            else
            {
                EditorUtility.DisplayDialog("导出航线", "航线异常,请查看报错日志！！！", "确定");
            }
        }

        public void ExportTreasures(string fileName)
        {
            if (map != null)
            {
                map.ExportTreasures1(GetFileName(fileName, "_valid.csv"));
                map.ExportTreasures2(GetFileName(fileName, "_invalid.csv"));
                map.ExportTreasures3(GetFileName(fileName, "_none.csv"));
                EditorUtility.DisplayDialog("导出宝藏点CSV", $"导出宝藏点CSV完成! \n{fileName}", "确定");
            }
            else
            {
                EditorUtility.DisplayDialog("导出宝藏点CSV", $"未发现地图对象!!!", "确定");
            }
        }

        public void ExportMapObjects(Transform transform, string dataPath, string prefabPath,
            bool isUseGameObject = false)
        {
            if (CheckMapValid())
                map.ExportMapObjects(transform, dataPath, prefabPath, isUseGameObject);
        }

        void ExportBigImage(string fileName)
        {
            StartCoroutine(SaveBigLineImage(fileName));
        }

        public void ExportToServer()
        {
            if (!CheckMapValid())
            {
                return;
            }

            var filename = GetExportPath();
            ExportToServer(filename);
        }

        void ExportToServer(string fileName)
        {
            if (!CheckMapValid())
            {
                return;
            }

            // if (!map.CheckMapValid())
            // {
            // 	return;
            // }

            var str = GetFileName(fileName, "_server.bytes");
            using (BinaryWriter bw = new BinaryWriter(File.Open(str, FileMode.OpenOrCreate)))
            {
                map.ExportToServer(bw);
                bw.Close();
            }

            EditorUtility.DisplayDialog("导出Server", $"导出Server完成! \n{str}", "确定");
        }

        public void ExportToClient()
        {
            if (!CheckMapValid())
            {
                return;
            }

            var filename = GetExportPath();
            ExportToClient(filename);
        }

        void ExportToClient(string fileName)
        {
            var sfile = GetFileName(fileName, "_client.bytes");
            var bw = new BinaryWriter(new FileStream(sfile, FileMode.OpenOrCreate));
            map.ExportToClient(bw);
            bw.Close();

            EditorUtility.DisplayDialog("导出Client", "导出客户端数据完成。", "确定");
        }

        void ExportToCSV(string fileName)
        {
            if (map != null)
            {
                map.ExportToCSV(fileName);
                EditorUtility.DisplayDialog("导出世界CSV", $"导出世界CSV完成! \n{fileName}", "确定");
            }
            else
            {
                EditorUtility.DisplayDialog("导出世界CSV", $"未发现地图对象!!!", "确定");
            }
        }

        void ExportToFisheryCSV(string fileName)
        {
            if (map != null)
            {
                map.ExportToFisheryCSV(fileName);
                EditorUtility.DisplayDialog("导出渔场CSV", $"导出渔场CSV完成! \n{fileName}", "确定");
            }
            else
            {
                EditorUtility.DisplayDialog("导出渔场CSV", $"未发现地图对象!!!", "确定");
            }
        }

        public void ExportCityToCSV()
        {
            if (!CheckMapValid())
                return;
            var filename = Path.Combine(GetMapFolder(), $"{map.Name}_city.csv");
            ExportCityToCSV(filename);
        }

        void ExportCityToCSV(string fileName)
        {
            if (map != null)
            {
                map.ExportCityToCsv(fileName);
                EditorUtility.DisplayDialog("导出海港CSV", $"导出海港CSV完成! \n{fileName}", "确定");
            }
            else
            {
                EditorUtility.DisplayDialog("导出海港CSV", $"未发现地图对象!!!", "确定");
            }
        }

        void ExportToJson(string fileName)
        {
            if (map != null)
            {
                map.ExportMapByJson(fileName);
                EditorUtility.DisplayDialog("导出Json", $"导出Json完成! \n{fileName}", "确定");
            }
            else
            {
                EditorUtility.DisplayDialog("导出Json", $"未发现地图对象!!!", "确定");
            }
        }

        public void ExportAll()
        {
            if (!CheckMapValid())
            {
                return;
            }

            ExportToServer();
            ExportToClient();
            ExportZonePictures();
        }

        void ExportAll(string fileName)
        {
            if (!CheckMapValid())
                return;

            ExportToServer(fileName);
            ExportToClient(fileName);
            ExportZonePictures(fileName);

            return;
            var str1 = GetFileName(fileName, ".csv");
            ExportToCSV(str1);

            var str3 = GetFileName(fileName + "_fishery", ".csv");
            ExportToFisheryCSV(str3);

            var str2 = GetFileName(fileName, ".json");
            ExportToJson(str2);
        }
    }
}
#endif