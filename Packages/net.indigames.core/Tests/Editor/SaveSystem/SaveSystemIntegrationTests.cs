using IndiGames.Core.SaveSystem;
using NUnit.Framework;
using UnityEditor;

namespace IndiGames.Core.Tests.Editor.SaveSystem
{
    [TestFixture]
    public class SaveSystemIntegrationTests
    {
        private SaveSystemSO _saveSystemSO;

        [SetUp]
        public void Setup()
        {
            var assetGuid = AssetDatabase.FindAssets("t:SaveSystemSO");

            Assert.AreEqual(1, assetGuid.Length, "There should be exactly one SaveSystemSO in the project.");

            var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid[0]);
            _saveSystemSO = AssetDatabase.LoadAssetAtPath<SaveSystemSO>(assetPath);
        }

        [Test]
        public void SaveData_ShouldNotBeNull()
        {
            Assert.IsNotNull(_saveSystemSO._saveData, "saveData should not be null.");
        }

        [Test]
        public void SaveData_PlayerName_ShouldBeSameAsEditorSetting()
        {
            Assert.AreEqual(_saveSystemSO._saveData.playerName, "New Player", "saveData.playerName should be `New Player`.");
        }

        [Test]
        public void SaveGame_ShouldReturnTrue()
        {
            Assert.IsTrue(_saveSystemSO.SaveGame(), "SaveGame should return true.");
        }

        [Test]
        public void LoadSaveGame_IfHasSaveGame_ShouldHaveDifferentName()
        {
            const string mockPlayerName = "Test Player Name";
            _saveSystemSO._saveData = new SaveData { playerName = mockPlayerName };

            var hasSaveGame = _saveSystemSO.LoadSaveGame();
            if (!hasSaveGame) Assert.Pass();
            Assert.IsTrue(hasSaveGame, "LoadSaveGame should return true.");
            Assert.AreNotEqual(mockPlayerName, _saveSystemSO._saveData.playerName,
                $"saveData.playerName should be {mockPlayerName}.");
        }

        [TearDown]
        public void Teardown()
        {
            _saveSystemSO._saveData = new SaveData();
            _saveSystemSO = null;
        }
    }
}