using System;
using System.IO;
using CryptoQuest.SaveSystem;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Tests.EditMode
{
    [TestFixture]
    public class JsonSaveManagerSOTests
    {
        private JsonSaveManagerSO _jsonSaveManagerSO;
        private readonly SaveData _saveData = new SaveData()
        {
            playerName = "TestPlayer"
        };

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var assetsGUID = AssetDatabase.FindAssets("t:JsonSaveManagerSO");

            Assert.AreEqual(1, assetsGUID.Length, "JsonSaveManagerSO asset not found");

            _jsonSaveManagerSO =
                AssetDatabase.LoadAssetAtPath<JsonSaveManagerSO>(AssetDatabase.GUIDToAssetPath(assetsGUID[0]));

            Assert.IsNotNull(_jsonSaveManagerSO);
        }

        [SetUp]
        public void Setup()
        {
            _jsonSaveManagerSO.saveFileName = "testSave.json";
        }

        [TearDown]
        public void TearDown()
        {
            var saveFilePath = Path.Combine(Application.persistentDataPath, _jsonSaveManagerSO.saveFileName);

            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }
        }

        [Test]
        public void JsonSaveManagerSO_HasSaveFileName()
        {
            Assert.IsNotEmpty(_jsonSaveManagerSO.saveFileName, "_jsonSaveManagerSO.saveFileName != null");
        }

        [Test]
        public void Save_CreateNewSaveFile_ReturnTrue()
        {
            var saveFilePath = Path.Combine(Application.persistentDataPath, _jsonSaveManagerSO.saveFileName);

            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }

            Assert.IsTrue(_jsonSaveManagerSO.Save(_saveData));
        }

        [Test]
        public void Save_WithNoFileName_ReturnFalse()
        {
            _jsonSaveManagerSO.saveFileName = "";

            // Assert Save should throw UnauthorizedAccessException and return false
            Assert.Throws<UnauthorizedAccessException>(() => _jsonSaveManagerSO.Save(_saveData));
            Assert.IsFalse(_jsonSaveManagerSO.Save(_saveData));
        }
    }
}