﻿using CryptoQuest.Core.SaveSystem;
using NUnit.Framework;
using UnityEditor;

namespace Tests.EditMode
{
    [TestFixture]
    public class NullManagerSOTests
    {
        private SaveManagerSO _nullSaveManagerSO;

        [SetUp]
        public void Setup()
        {
            var assetGuid = AssetDatabase.FindAssets("t:NullSaveManagerSO");

            Assert.AreEqual(1, assetGuid.Length, "There should be exactly one NullSaveManagerSO in the project.");

            var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid[0]);
            _nullSaveManagerSO = AssetDatabase.LoadAssetAtPath<NullSaveManagerSO>(assetPath);

            Assert.IsNotNull(_nullSaveManagerSO, "NullSaveManagerSO should not be null.");
        }

        [Test]
        public void SaveGame_ShouldReturnTrue()
        {
            Assert.IsTrue(_nullSaveManagerSO.Save(new SaveData()), "SaveGame should return true.");
        }

        [Test]
        public void LoadSaveGame_ShouldReturnTrue_WithDefaultPlayerName()
        {
            Assert.IsTrue(_nullSaveManagerSO.Load(out var saveData), "LoadSaveGame should return true.");
            Assert.AreEqual(NullSaveManagerSO.DefaultPlayerName, saveData.playerName,
                "saveData.playerName should be the default player name.");
        }
    }
}