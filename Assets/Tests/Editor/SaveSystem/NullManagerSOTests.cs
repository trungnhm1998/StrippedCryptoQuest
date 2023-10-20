using CryptoQuest.System.SaveSystem;
using NUnit.Framework;
using System.Threading.Tasks;
using UnityEditor;

namespace CryptoQuest.Tests.Editor.SaveSystem
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
        public async Task SaveGame_ShouldReturnTrue()
        {
            Assert.IsTrue(await _nullSaveManagerSO.SaveAsync("{}"), "SaveGame should return true.");
        }

        [Test]
        public async Task LoadSaveGame_ShouldReturnTrue()
        {
            var saveData = await _nullSaveManagerSO.LoadAsync();
            Assert.NotNull(saveData, "LoadSaveGame should return not null value.");
        }
    }
}