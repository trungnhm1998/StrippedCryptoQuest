using System.Collections;
using System.Threading.Tasks;
using CryptoQuest.System.SaveSystem;
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
        public async Task SaveGame_ShouldReturnTrue()
        {
            Assert.IsTrue(await _saveManagerSO.SaveAsync("{}"), "SaveGame should return true.");
        }

        [Test]
        public async Task LoadSaveGame_ShouldReturnTrue()
        {
            var saveData = await _saveManagerSO.LoadAsync();
            Assert.NotNull(saveData, "LoadSaveGame should return non null value.");
        }
    }
}