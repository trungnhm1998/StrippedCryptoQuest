using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CryptoQuest.Character.Enemy;
using CryptoQuest.EditorTool;
using CryptoQuest.Gameplay.Battle.ScriptableObjects;
using CryptoQuest.Gameplay.Loot;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyDefSetUpTests
{
    private const string EnemyDatabaseAssetPath = "Assets/ScriptableObjects/Character/Enemies/_Database.asset";
    private EnemyDatabase _database;
    private List<string[]> _data = new();
    private Dictionary<string, string[]> _dataDict = new();
    private Dictionary<int, EnemyDef> _enemyDefs = new();
    public Dictionary<string, EnemyDef> _nameEnemyDefs = new();
    private const string ENEMY_DEF_FOLDER = "Assets/ScriptableObjects/Character/Enemies/";

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        string filePath = "Assets/Editor/CSVs/CQ_MasterData - MonsterUnit.csv";
        CSVReader csvReader = new CSVReader();
        _data = csvReader.ReadCSVFile(filePath);

        _data.RemoveRange(0, 2);
        _dataDict = _data.ToDictionary(x => x[0], x => x);

        _database = AssetDatabase.LoadAssetAtPath<EnemyDatabase>(EnemyDatabaseAssetPath);


        var guids = AssetDatabase.FindAssets("t:EnemyDef");
        foreach (var guid in guids)
        {
            var enemyDef = AssetDatabase.LoadAssetAtPath<EnemyDef>(AssetDatabase.GUIDToAssetPath(guid));
            _enemyDefs.Add(enemyDef.Id, enemyDef);
        }

        foreach (var enemyDef in _enemyDefs)
        {
            _nameEnemyDefs.Add(enemyDef.Value.name, enemyDef.Value);
        }
    }

    [UnityTest, Category("Smokes")]
    public IEnumerator AllEnemyIsCreated_ReturnTrue()
    {
        Assert.IsTrue(_data.Count > 0);
        Assert.IsTrue(_enemyDefs.Count > 0);

        foreach (var data in _data)
        {
            if (int.TryParse(data[0], out int id))
                Assert.IsTrue(_enemyDefs.ContainsKey(id));
        }

        yield return null;
    }

    [UnityTest, Category("Smokes")]
    public IEnumerator AllEnemyIsCreatedAndAddedToDatabase_ReturnTrue()
    {
        var databaseDict = _database.Maps.ToDictionary(x => x.Id, x => x);
        foreach (var data in _data)
        {
            if (int.TryParse(data[0], out int id))
            {
                Debug.Log(id);
                Assert.IsTrue(databaseDict.ContainsKey(id));
                Assert.IsTrue(databaseDict[id].Data != null);
            }
        }

        yield return null;
    }

    [UnityTest, Category("Smokes")]
    public IEnumerator EnemyNameSetUp_IsCorrect([ValueSource("TestEnemy")] string defName)
    {
        string name = defName;
        var enemyDef = _nameEnemyDefs[name];
        Assert.IsNotNull(enemyDef);

        var key = enemyDef.Name.TableEntryReference;

        var table = LocalizationEditorSettings.GetStringTableCollection("Enemies");
        var tableEntry = table.SharedData.GetEntryFromReference(_dataDict[enemyDef.Id.ToString()][1]);
        Debug.Log(key);
        Assert.IsTrue(tableEntry.Id == key);
        yield return null;
    }

    [UnityTest, Category("Smokes")]
    public IEnumerator EnemyElementSetUp_IsCorrect([ValueSource("TestEnemy")] string defName)
    {
        string name = defName;
        var enemyDef = _nameEnemyDefs[name];
        Assert.IsNotNull(enemyDef);
        Assert.IsNotNull(enemyDef.Element);

        Assert.IsTrue(enemyDef.Element.Id == int.Parse(_dataDict[enemyDef.Id.ToString()][7]));
        yield return null;
    }

    [UnityTest, Category("Smokes")]
    public IEnumerator EnemyPrefabSetUp_IsCorrect([ValueSource("TestEnemy")] string defName)
    {
        string name = defName;
        var enemyDef = _nameEnemyDefs[name];
        Assert.IsNotNull(enemyDef);
        Assert.IsNotNull(enemyDef.Model);

        var prefabName = enemyDef.Model.editorAsset.name.ToLower();
        var dataName = _dataDict[enemyDef.Id.ToString()][26].ToLower();
        Debug.Log(prefabName + " " + dataName);
        Assert.IsTrue(prefabName == dataName);

        yield return null;
    }

    [UnityTest, Category("Smokes")]
    public IEnumerator EnemyStatsSetUp_IsCorrect([ValueSource("TestEnemy")] string defName)
    {
        string name = defName;

        var enemyDef = _nameEnemyDefs[name];
        Assert.IsNotNull(enemyDef);
        Assert.IsNotNull(enemyDef.Stats);
        Debug.Log(_dataDict[enemyDef.Id.ToString()][27] + " " + _dataDict[enemyDef.Id.ToString()][28]);
        Assert.IsTrue(enemyDef.Stats.Length == 13);

        for (int i = 0; i < enemyDef.Stats.Length; i++)
        {
            Assert.IsNotNull(enemyDef.Stats[i]);
            Assert.IsNotNull(enemyDef.Stats[i].Attribute);
        }

        yield return null;
    }

    [UnityTest, Category("Smokes")]
    public IEnumerator EnemyNormalAttackProbabilitySetUp_IsCorrect([ValueSource("TestEnemy")] string defName)
    {
        string name = defName;

        var enemyDef = _nameEnemyDefs[name];
        Assert.IsNotNull(enemyDef);
        Assert.IsNotNull(enemyDef.Stats);

        float probabilityData = float.Parse(_dataDict[enemyDef.Id.ToString()][27]) / 100;
        float probabilityDef = enemyDef.NormalAttackProbability;
        Debug.Log(probabilityData + " " + probabilityDef);
        Assert.IsTrue(Math.Abs(probabilityData - probabilityDef) < TOLERANCE);
        yield return null;
    }

    [UnityTest, Category("Smokes")]
    public IEnumerator EnemySkillSetSetUp_IsCorrect([ValueSource("TestEnemy")] string defName)
    {
        string name = defName;
        int passivescolumn1 = 28;

        var enemyDef = _nameEnemyDefs[name];
        Assert.IsNotNull(enemyDef);
        Assert.IsNotNull(enemyDef.Stats);
        int skillCount = 0;
        for (int i = 0; i < 4; i++)
        {
            int currentId = passivescolumn1 + i * 2;
            if (string.IsNullOrEmpty(_dataDict[enemyDef.Id.ToString()][currentId]))
                break;
            Assert.IsNotNull(enemyDef.Skills[i]);
            Assert.IsTrue(enemyDef.Skills[i].SkillDef.Context.SkillInfo.Id ==
                          int.Parse(_dataDict[enemyDef.Id.ToString()][currentId]));
            Assert.IsTrue(Math.Abs(enemyDef.Skills[i].Probability -
                                   float.Parse(_dataDict[enemyDef.Id.ToString()][currentId + 1]) / 100) < TOLERANCE);
            skillCount++;
        }

        Assert.IsTrue(skillCount == enemyDef.Skills.Length);


        yield return null;
    }

    [UnityTest, Category("Smokes")]
    public IEnumerator EnemyDropExpSetUp_IsCorrect([ValueSource("TestEnemy")] string defName)
    {
        string name = defName;

        var enemyDef = _nameEnemyDefs[name];
        Assert.IsNotNull(enemyDef);
        Assert.IsNotNull(enemyDef.Stats);
        float expData = float.Parse(_dataDict[enemyDef.Id.ToString()][20]);
        if (expData == 0)
            yield break;
        Assert.IsTrue(enemyDef.Drops[0].Loot is ExpLoot);
        var expLoot = enemyDef.Drops[0].Loot as ExpLoot;


        float expDef = expLoot.Exp;

        Assert.IsTrue(Math.Abs(expData - expDef) < TOLERANCE);
        yield return null;
    }


    [UnityTest, Category("Smokes")]
    public IEnumerator EnemyDropGoldSetUp_IsCorrect([ValueSource("TestEnemy")] string defName)
    {
        string name = defName;

        var enemyDef = _nameEnemyDefs[name];
        Assert.IsNotNull(enemyDef);
        Assert.IsNotNull(enemyDef.Stats);
        float goldData = float.Parse(_dataDict[enemyDef.Id.ToString()][21]);
        if (goldData == 0)
            yield break;
        Assert.IsTrue(enemyDef.Drops[1].Loot is CurrencyLootInfo);
        var goldLoot = enemyDef.Drops[1].Loot as CurrencyLootInfo;


        float goldDef = goldLoot.Item.Amount;

        Assert.IsTrue(Math.Abs(goldData - goldDef) < TOLERANCE);
        yield return null;
    }


    private const double TOLERANCE = 0.01f;

    static IEnumerable<string> TestEnemy()
    {
        var enemyDefs = Directory.GetFiles(ENEMY_DEF_FOLDER);
        foreach (var def in enemyDefs)
        {
            if (def.Contains(".meta")) continue;
            if (def.Contains("_Database")) continue;
            string nameAsset = def.Split("/")[^1];
            string name = nameAsset.Split(".")[0];
            yield return name;
        }
    }
}