using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;

public class ResizeTilesetEditor
{
    [MenuItem("Assets/Resize Tileset by 3")]
    private static void ResizePNGImages()
    {
        var folderPath = GetCurrentFolderPath();
        if (folderPath == null) return;

        var resizeFactor = 3f;

        var pngFilePaths = Directory.GetFiles(folderPath, "*.png", SearchOption.AllDirectories);

        foreach (var pngFilePath in pngFilePaths)
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

            Debug.Log("PNG image resized and saved at: " + pngFilePath);
        }

        AssetDatabase.Refresh();
        Debug.Log("PNG images resized.");

        ModifyTSX(resizeFactor);
    }

    private static void ModifyTSX(float resizeFactor)
    {
        var folderPath = GetCurrentFolderPath();
        if (folderPath == null) return;
        var tsxFilePaths = Directory.GetFiles(folderPath, "*.tsx", SearchOption.AllDirectories);

        foreach (var tsxFilePath in tsxFilePaths)
        {
            var doc = new XmlDocument();
            doc.Load(tsxFilePath);

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

            Debug.Log("Tileset modified (tilewidth and tileheight reduced by a factor of 3) and saved.");
        }

        AssetDatabase.Refresh();
        Debug.Log("TSX resized.");
    }

    private static void ModifyAttribute(XmlElement element, string attr, float resizeFactor)
    {
        Debug.Log($"{attr}: {element.GetAttribute(attr)}");
        var value = int.Parse(element.GetAttribute(attr));
        var newValue = Mathf.FloorToInt(value / resizeFactor);
        element.SetAttribute(attr, newValue.ToString());
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
}