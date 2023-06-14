using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Threading;
using System;
using System.IO;
using ScriptableObjectBrowser;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest;

public class CharacterSOEditor : ScriptableObjectBrowserEditor<CharacterSO>
{
    public CharacterSOEditor()
    {
        this.createDataFolder = false;
        this.defaultStoragePath = "Assets/_Project/Data/CharacterSO";
    }
    public override void ImportBatchData(string directory, System.Action<ScriptableObject> callback)
    {

        string[] allLines = File.ReadAllLines(directory);

        var startRow = 1;
        foreach (string s in allLines)
        {
            if (startRow-- > 0) continue;
            string[] splitedData = s.Split('\t');
            var id = splitedData[0];
            var name = splitedData[2];
            var path = "Assets/_Project/Data/CharacterSO/" + name + ".asset";


            CharacterSO instance;
            instance = ScriptableObject.CreateInstance<CharacterSO>();
            // }

            instance.id = int.Parse(splitedData[0]);
            instance.displayName = splitedData[2];
            instance.isMaleGender = splitedData[3] == "male" ? true : false;
            instance.age = int.Parse(splitedData[4]);
            instance.birthplace = splitedData[6];
            instance.height = splitedData[7];
            instance.weight = splitedData[8];
            instance.description = splitedData[10];
            AssetDatabase.CreateAsset(instance, path);

            callback(instance);
            AssetDatabase.SaveAssets();
        }
    }
}
