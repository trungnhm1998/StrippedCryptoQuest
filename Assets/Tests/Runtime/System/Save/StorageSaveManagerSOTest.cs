using System.Collections;
using CryptoQuest.System.Save;
using IndiGames.Core.SaveSystem;
using IndiGames.Core.SceneManagementSystem;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests.Runtime.System.Save
{
    public class StorageSaveManagerSOTest
    {
        private SaveManagerSO _saveManagerSO;

        [SetUp]
        public void Setup()
        {
            var assetGuid = AssetDatabase.FindAssets("t:StorageSaveManagerSO");

            Assert.AreEqual(1, assetGuid.Length, "There should be exactly one StorageSaveManagerSO in the project.");

            var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid[0]);
            _saveManagerSO = AssetDatabase.LoadAssetAtPath<StorageSaveManagerSO>(assetPath);

            Assert.IsNotNull(_saveManagerSO, "StorageSaveManagerSO should not be null.");
        }

        [Test]
        public void SaveGame_ShouldReturnTrue()
        {
            Assert.IsTrue(_saveManagerSO.Save(new SaveData()), "SaveGame should return true.");
        }

        [Test]
        public void LoadSaveGame_ShouldReturnTrue()
        {
            Assert.True(_saveManagerSO.Load(out var saveData), "LoadSaveGame should return true.");
            Assert.AreEqual(saveData.playerName, SaveData.DEFAULT_PLAYER_NAME, "saveData.playerName should be the default player name.");
        }
    }
}