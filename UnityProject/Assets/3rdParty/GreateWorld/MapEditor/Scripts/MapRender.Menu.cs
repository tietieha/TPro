using UnityEngine;

#if UNITY_EDITOR
namespace GEngine.MapEditor
{
    public partial class MapRender : MonoBehaviour
    {
        public void OnMenu_LoadMap()
        {
            GetOpenFileDialog("打开地图", LoadMap, new[] { "UW Map", "" });
            //需要重新设置需要调整的哨塔列表
            MapToolBar.isSetupNeedAdjustmentZones = true;
        }

        public void OnMenu_SaveMap()
        {
            GetSaveFileDialog("保存地图", SaveAsMap);
        }

        public void OnMenu_SavePictures()
        {
            GetSaveFileDialog("保存线图", SavePictures);
        }

        public void OnMenu_SavePictures1()
        {
            GetSaveFileDialog("导像素图", ExportZonePictures);
        }

        public void OnMenu_SavePictures2()
        {
            GetSaveFileDialog("导像素全图", SavePictures2, "", "png");
        }

        public void OnMenu_ExportBigImage()
        {
            GetSaveFileDialog("导整体线图", ExportBigImage, "", "png");
        }

        public void OnMenu_SaveLandformPicture()
        {
            GetSaveFileDialog("导地貌散图", SaveLandformPicture, "", "");
        }

        public void OnMenu_SaveFullLandformPicture()
        {
            GetSaveFileDialog("导地貌全图", SaveFullLandformPicture, "", "");
        }

        public void OnMenu_SaveLandAreaToPicture()
        {
            GetSaveFileDialog("导出陆地区域", SaveLandAreaToPicture, "", "");
        }

        public void OnMenu_SaveIsLandAreaToPicture()
        {
            GetSaveFileDialog("导出海岛区域", SaveIsLandAreaToPicture, "", "");
        }

        public void OnMenu_SaveSeaControlToPicture()
        {
            GetSaveFileDialog("导出海水深浅控制图", SaveSeaControlToPicture, "", "");
        }

        public void OnMenu_SaveBlockPicture()
        {
            GetSaveFileDialog("导阻挡图", SaveBlockPicture, "", "");
        }

        public void OnMenu_SaveZoneSubtypePicture()
        {
            GetSaveFileDialog("导末日实验室图", SaveZoneSubtypePicture, "", "");
        }

        public void OnMenu_ExportToServer()
        {
            GetSaveFileDialog("导出Server", ExportToServer);
        }

        public void OnMenu_ExportToClient()
        {
            GetSaveFileDialog("导出Client", ExportToClient);
        }

        public void OnMenu_ExportToCSV()
        {
            GetSaveFileDialog("导出世界CSV", ExportToCSV, "", "csv");
        }

        public void OnMenu_ExportToFisheryCSV()
        {
            GetSaveFileDialog("导出渔场CSV", ExportToFisheryCSV, "", "csv");
        }

        public void OnMenu_ExportCityToCSV()
        {
            GetSaveFileDialog("导出城点信息", ExportCityToCSV, "", "csv");
        }

        public void OnMenu_ExportTreasures()
        {
            GetSaveFileDialog("导出宝藏点", ExportTreasures, "", "csv");
        }

        public void OnMenu_ExportToJson()
        {
            GetSaveFileDialog("导出Json", ExportToJson, "", "json");
        }

        public void OnMenu_LoadFromJson()
        {
            GetOpenFileDialog("加载Json", LoadFromJson, new[] { "", "json" });
        }

        public void OnMenu_ExportAll()
        {
            GetSaveFileDialog("导出全部所需文件", ExportAll, "", "");
        }

        public void OnMenu_LoadOldPattern()
        {
            GetOpenFileDialog("加载旧版模具", (fileName) => { PatternManager.LoadOld(fileName); },
                new[] { "", "ptn" });
        }

        public void OnMenu_LoadPattern()
        {
            GetOpenFileDialog("加载模具", (fileName) => { PatternManager.Load(fileName); },
                new[] { "", "patt" });
        }

        public void OnMenu_SavePattern()
        {
            GetSaveFileDialog("保存模具", (fileName) => { PatternManager.Save(fileName); }, "", "patt");
        }

        public void OnMenu_SaveZoneLineToPicture()
        {
            GetSaveFileDialog("导出区域线图", SaveZoneLineToPicture, "", "png");
        }
    }
}

#endif