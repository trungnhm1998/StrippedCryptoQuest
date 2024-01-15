#if UNITY_EDITOR
using UnityEngine;
using System.IO;
using UnityEditor;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class MagicStones_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/Scripts/EditorTool/Terasurware/MagicStones.xlsx";
    private static readonly string exportPath = "Assets/ScriptableObjects/Terasurware/MagicStoneDatabase.asset";
    private static readonly string[] sheetNames = { "MagicStones", };

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets)
        {
            if (!filePath.Equals(asset))
                continue;

            MagicStoneDatabase data =
                (MagicStoneDatabase)AssetDatabase.LoadAssetAtPath(exportPath, typeof(MagicStoneDatabase));
            if (data == null)
            {
                data = ScriptableObject.CreateInstance<MagicStoneDatabase>();
                AssetDatabase.CreateAsset((ScriptableObject)data, exportPath);
            }

            data.sheets.Clear();
            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                IWorkbook book = null;
                if (Path.GetExtension(filePath) == ".xls")
                {
                    book = new HSSFWorkbook(stream);
                }
                else
                {
                    book = new XSSFWorkbook(stream);
                }

                foreach (string sheetName in sheetNames)
                {
                    ISheet sheet = book.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        Debug.LogError("[QuestData] sheet not found:" + sheetName);
                        continue;
                    }

                    MagicStoneDatabase.Sheet s = new MagicStoneDatabase.Sheet();
                    s.name = sheetName;

                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        ICell cell = null;

                        MagicStoneDatabase.Param p = new MagicStoneDatabase.Param();

                        cell = row.GetCell(0);
                        p.stone_id = (cell == null ? 0.0 : cell.NumericCellValue);
                        cell = row.GetCell(1);
                        p.name_key = (cell == null ? "" : cell.ToString());
                        s.list.Add(p);
                    }

                    data.sheets.Add(s);
                }
            }

            ScriptableObject obj =
                AssetDatabase.LoadAssetAtPath(exportPath, typeof(ScriptableObject)) as ScriptableObject;
            EditorUtility.SetDirty(obj);
        }
    }
}
#endif