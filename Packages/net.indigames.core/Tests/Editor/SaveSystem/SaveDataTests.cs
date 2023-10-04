using IndiGames.Core.SaveSystem;
using NUnit.Framework;

namespace IndiGames.Core.Tests.Editor.SaveSystem
{
    [TestFixture]
    public class SaveDataTests
    {
        [Test]
        public void playerName_ShouldBeDefaultName()
        {
            var saveData = new SaveData();

            Assert.AreEqual(saveData.playerName, SaveData.DEFAULT_PLAYER_NAME, "saveData.playerName should be the default player name.");
        }

        [Test]
        public void ToJson_ShouldNotBeNull()
        {
            var saveData = new SaveData();

            Assert.IsNotNull(saveData.ToJson(), "saveData.ToJson() should not be null.");
        }

        [Test]
        public void LoadFromJson_ShouldNotBeNull()
        {
            // Arrange
            const string mockPlayerName = "Test Player";
            const string json = "{\"playerName\":\"" + mockPlayerName + "\"}";
            var saveData = new SaveData();

            // Act
            saveData.LoadFromJson(json);

            // Assert
            Assert.AreEqual(mockPlayerName, saveData.playerName, "saveData.LoadFromJson() should not be null.");
        }
    }
}