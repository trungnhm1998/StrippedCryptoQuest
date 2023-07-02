using IndiGames.Core.SaveSystem;
using NUnit.Framework;
using UnityEditor;

namespace IndiGames.Core.EditorTests.SaveSystem
{
    [TestFixture]
    public class SaveSystemSOTests
    {
        private SaveSystemSO _saveSystemSO;

        [SetUp]
        public void Setup()
        {
            var assetGuid = AssetDatabase.FindAssets("t:SaveSystemSO");

            Assert.AreEqual(1, assetGuid.Length, "There should be exactly one SaveSystemSO in the project.");

            var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid[0]);
            _saveSystemSO = AssetDatabase.LoadAssetAtPath<SaveSystemSO>(assetPath);

            Assert.IsNotNull(_saveSystemSO.SaveManagerSO, "SaveManagerSO should not be null.");
        }

        [Test]
        public void SaveData_ShouldNotBeNull()
        {
            Assert.IsNotNull(_saveSystemSO._saveData, "saveData should not be null.");
        }

        [Test]
        public void SaveData_PlayerName_ShouldBeEmpty()
        {
            Assert.IsEmpty(_saveSystemSO._saveData.playerName, "saveData.playerName should be empty.");
        }

        [Test]
        public void SaveGame_ShouldReturnTrue()
        {
            Assert.IsTrue(_saveSystemSO.SaveGame(), "SaveGame should return true.");
        }

        [Test]
        public void LoadSaveGame_WithMockName_ShouldHaveDifferentName()
        {
            const string mockPlayerName = "Test Player Name";
            _saveSystemSO._saveData = new SaveData { playerName = mockPlayerName };

            Assert.IsTrue(_saveSystemSO.LoadSaveGame(), "LoadSaveGame should return true.");
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