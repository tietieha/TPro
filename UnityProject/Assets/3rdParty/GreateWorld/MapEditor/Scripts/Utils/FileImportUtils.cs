using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using UnityEngine;

namespace GEngine.MapEditor
{
    public class FileImportUtils
    {
        public static Dictionary<Color32, Vector3Int> ImportLevelColor(string path)
        {
            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                if (package.Workbook.Worksheets.Count == 0)
                {
                    Debug.LogError("No worksheets found.");
                    return null;
                }
                var worksheet = package.Workbook.Worksheets[1];
                if (worksheet == null)
                {
                    Debug.LogError("Worksheet not found.");
                    return null;
                }

                var colorDict = new Dictionary<Color32, Vector3Int>();
                var rowCount = worksheet.Dimension.Rows;
                for (var row = 1; row <= rowCount; row++)
                {
                    var colorCell = worksheet.Cells[row, 3];
                    var levelCell = worksheet.Cells[row, 2];
                    var zoneIdCell = worksheet.Cells[row, 1];
                    var zoneSizeCell = worksheet.Cells[row, 4];

                    if (colorCell.Text == "" || levelCell.Text == "")
                        continue;

                    // if (ColorUtility.TryParseHtmlString(colorCell.Text, out var color))
                    // {
                    //     int level = int.Parse(levelCell.Text);
                    //     colorDict[color] = level;
                    // }
                    var colorValue = colorCell.Text.Split("|");
                    if (colorValue.Length == 3)
                    {
                        var r = byte.Parse(colorValue[0]);
                        var g = byte.Parse(colorValue[1]);
                        var b = byte.Parse(colorValue[2]);
                        var color = new Color32(r, g, b, 255);
                        int level = int.Parse(levelCell.Text);
                        int zoneId = int.Parse(zoneIdCell.Text);
                        int zoneSize = int.Parse(zoneSizeCell.Text);
                        colorDict[color] = new Vector3Int(level, zoneId, zoneSize);
                    }
                    else
                    {
                        Debug.LogError($"Invalid color format in row {row}: {colorCell.Text}");
                    }
                }

                return colorDict;
            }

            return null;
        }
    }
}