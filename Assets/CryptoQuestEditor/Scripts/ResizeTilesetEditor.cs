using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using SuperTiled2Unity;
using SuperTiled2Unity.Editor;
using UnityEditor.SceneManagement;
using System;

public class ResizeTilesetEditor
{
    private const float RESIZE_FACTOR = 3f;

    [MenuItem("Assets/Tileset Resize/Resize Tileset by 3")]
    private static void ResizeTilesetSize()
    {
        ResizePNG(RESIZE_FACTOR);
        ModifyTSX(RESIZE_FACTOR);
        ModifyTSXPolygon(RESIZE_FACTOR);
        ModifyTMX(RESIZE_FACTOR);
    }

    private static void ResizePNG(float resizeFactor)
    {
        foreach (var pngFilePath in TraverseFolder("*.png"))
        {
            // Load the original PNG texture
            var pngBytes = File.ReadAllBytes(pngFilePath);
            var originalTexture = new Texture2D(2, 2); 
            originalTexture.LoadImage(pngBytes);

            // Clone the original texture to a new texture
            var resizedTexture = new Texture2D((int)(originalTexture.width / resizeFactor), (int)(originalTexture.height / resizeFactor));

            // Loop through pixels and set them on the resized texture
            for (var y = 0; y < resizedTexture.height; y++)
            {
                for (var x = 0; x < resizedTexture.width; x++)
                {
                    var originalPixel = originalTexture.GetPixelBilinear((float)x / resizedTexture.width, (float)y / resizedTexture.height);
                    resizedTexture.SetPixel(x, y, originalPixel);
                }
            }

            resizedTexture.Apply();

            var resizedBytes = resizedTexture.EncodeToPNG();
            File.WriteAllBytes(pngFilePath, resizedBytes);

            Debug.Log($"{pngFilePath} resized and saved.");
        }

        AssetDatabase.Refresh();
        Debug.Log("All PNG images resized.");
    }

    private static void ModifyTSXPolygon(float resizeFactor)
    {
        foreach (var tsxFilePath in TraverseFolder("*.tsx"))
        {
            var doc = new XmlDocument();
            doc.Load(tsxFilePath);
            var root = doc.DocumentElement;
            var counter = 0;

            foreach (XmlElement element in root.SelectNodes("//object"))
            {
                var polygonNode = element.SelectSingleNode("polygon");

                ModifyAttributeFloat(element, "x", resizeFactor);
                ModifyAttributeFloat(element, "y", resizeFactor);
                ModifyAttributeFloat(element, "width", resizeFactor);
                ModifyAttributeFloat(element, "height", resizeFactor);
                if (polygonNode == null) continue;
                var polyElement = polygonNode as XmlElement;
                counter++;
                var attr = "points";
                Debug.Log($"{counter} points: {polyElement.GetAttribute(attr)}");
                var value = polyElement.GetAttribute(attr);
                var vectors = value.Split(" ");
                var result = "";
                foreach (var item in vectors)
                {
                    var points = item.Split(",");
                    Debug.Log($"parse x {float.Parse(points[0])}");
                    Debug.Log($"parse y {float.Parse(points[1])}");
                    var x = float.Parse(points[0])/resizeFactor;
                    var y = float.Parse(points[1])/resizeFactor;
                    Debug.Log($"result {x.ToString()},{y.ToString()}");
                    result += $"{x.ToString()},{y.ToString()} ";
                }
                polyElement.SetAttribute(attr, result[0..^1]);
            }

            doc.Save(tsxFilePath);

            Debug.Log($"{tsxFilePath} modified and saved.");
        }

        AssetDatabase.Refresh();
        Debug.Log("All TSX polygon resized.");
    }

    private static void ModifyTSX(float resizeFactor)
    {
        foreach (var tsxFilePath in TraverseFolder("*.tsx"))
        {
            var doc = new XmlDocument();
            doc.Load(tsxFilePath);
            var importer = AssetImporter.GetAtPath(tsxFilePath);

            if (importer is TiledAssetImporter tiledImporter)
            {
                var so = new SerializedObject(tiledImporter);
                // you can Shift+Right Click on property names in the Inspector to see their paths
                so.FindProperty("m_PixelsPerUnit").floatValue /= resizeFactor;
                so.ApplyModifiedProperties();
            }

            var root = doc.DocumentElement;

            foreach (XmlElement tileset in root.SelectNodes("//tileset"))
            {
                ModifyAttribute(tileset, "tilewidth", resizeFactor);
                ModifyAttribute(tileset, "tileheight", resizeFactor);
                foreach (XmlElement node in root.SelectNodes("//image"))
                {
                    ModifyAttribute(node, "width", resizeFactor);
                    ModifyAttribute(node, "height", resizeFactor);
                }
            }

            doc.Save(tsxFilePath);

            Debug.Log($"{tsxFilePath} modified and saved.");
        }

        AssetDatabase.Refresh();
        Debug.Log("All TSX resized.");
    }

    private static void ModifyTMX(float resizeFactor)
    {
        foreach (var tmxFilePath in TraverseFolder("*.tmx"))
        {
            var doc = new XmlDocument();
            doc.Load(tmxFilePath);

            var root = doc.DocumentElement;
            var importer = AssetImporter.GetAtPath(tmxFilePath);

            if (importer is TiledAssetImporter tiledImporter)
            {
                var so = new SerializedObject(tiledImporter);
                // you can Shift+Right Click on property names in the Inspector to see their paths
                so.FindProperty("m_PixelsPerUnit").floatValue /= resizeFactor;
                so.ApplyModifiedProperties();
            }


            foreach (XmlElement tileset in root.SelectNodes("//map"))
            {
                ModifyAttribute(tileset, "tilewidth", resizeFactor);
                ModifyAttribute(tileset, "tileheight", resizeFactor);
            }

            foreach (XmlElement element in root.SelectNodes("//object"))
            {
                ModifyAttributeFloat(element, "x", resizeFactor);
                ModifyAttributeFloat(element, "y", resizeFactor);
                ModifyAttributeFloat(element, "width", resizeFactor);
                ModifyAttributeFloat(element, "height", resizeFactor);
            }

            doc.Save(tmxFilePath);

            Debug.Log($"{tmxFilePath} modified and saved.");
        }

        AssetDatabase.Refresh();
        Debug.Log("All TMX resized.");
    }

    private static IEnumerable<string> TraverseFolder(string fileExtension, SearchOption searchOption = SearchOption.AllDirectories)
    {
        var folderPath = GetCurrentFolderPath();
        if (folderPath == null) yield break;
        var filePaths = Directory.GetFiles(folderPath, fileExtension, searchOption);

        foreach (var filePath in filePaths)
        {
            yield return filePath;
        }
    }

    private static string GetCurrentFolderPath()
    {
        var folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!Directory.Exists(folderPath))
        {
            Debug.LogWarning("Please select a folder in the Project view.");
            return null;
        }
        return folderPath;
    }

    private static void ModifyAttribute(XmlElement element, string attr, float resizeFactor)
    {
        var value = int.Parse(element.GetAttribute(attr));
        var newValue = Mathf.FloorToInt(value / resizeFactor);
        element.SetAttribute(attr, newValue.ToString());
    }

    private static void ModifyAttributeFloat(XmlElement element, string attr, float resizeFactor)
    {
        var attrValue = element.GetAttribute(attr);
        if (attrValue == "") return;
        var value = float.Parse(attrValue);
        var newValue = value / resizeFactor;
        element.SetAttribute(attr, newValue.ToString());
    }
}